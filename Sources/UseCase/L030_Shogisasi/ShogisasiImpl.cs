using Grayscale.P007_SfenReport.L00025_Report;
using Grayscale.P007_SfenReport.L050_Report;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P025_KifuLarabe.L002_GraphicLog;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P027MoveGen.L0005MoveGen;
using Grayscale.P027MoveGen.L100MoveGen;
using Grayscale.P040_Kokoro.L00050_Kokoro;
using Grayscale.P045_Atama.L00025_KyHandan;
using Grayscale.P050_KifuWarabe.L00025_UsiLoop;
using Grayscale.P050_KifuWarabe.L00049_Kokoro;
using Grayscale.P050_KifuWarabe.L00050_KyHyoka;
using Grayscale.P050_KifuWarabe.L00051_Minimax;
using Grayscale.P050_KifuWarabe.L00052_Shogisasi;
using Grayscale.P050_KifuWarabe.L003_Kokoro;
using Grayscale.P050_KifuWarabe.L009_KyHyoka;
using Grayscale.P050_KifuWarabe.L010_Minimax;
using Grayscale.P050_KifuWarabe.L012_ScoreSibori;
using Grayscale.P050_KifuWarabe.L025_Erabu;
using Grayscale.P025_KifuLarabe.L100_KifuIO;
using System;
using System.Diagnostics;
using Grayscale.Kifuwarazusa.Entities;

namespace Grayscale.P050_KifuWarabe.L030_Shogisasi
{
    /// <summary>
    /// 将棋指しエンジン。
    /// 
    /// 指し手を決めるエンジンです。
    /// </summary>
    public class ShogisasiImpl : Shogisasi
    {
        private ShogiEngine Owner { get { return this.owner; } }
        private ShogiEngine owner;

        public static ReportEnvironment REPORT_ENVIRONMENT;
        static ShogisasiImpl()
        {
            ShogisasiImpl.REPORT_ENVIRONMENT = new ReportEnvironmentImpl(
                        "../../Logs/局面画像ﾛｸﾞ/",//argsDic["outFolder"],
                        "../../Profile/Data/img/gkLog/",//argsDic["imgFolder"],
                        "koma1.png",//argsDic["kmFile"],
                        "suji1.png",//argsDic["sjFile"],
                        "20",//argsDic["kmW"],
                        "20",//argsDic["kmH"],
                        "8",//argsDic["sjW"],
                        "12"//argsDic["sjH"]
                        );
        }

        /// <summary>
        /// 心エンジン。
        /// </summary>
        public Kokoro Kokoro { get; set; }

        /// <summary>
        /// 点数付けエンジン。
        /// </summary>
        public MinimaxEngine MinimaxEngine { get; set; }

        /// <summary>
        /// 枝狩りエンジン。
        /// </summary>
        public ScoreSiboriEngine EdagariEngine { get; set; }

        /// <summary>
        /// 指し手を１つに決めるエンジン。
        /// </summary>
        public ErabuEngine ErabuEngine { get; set; }

        /// <summary>
        /// 思考エンジンの性格。
        /// </summary>
        public Seikaku Seikaku { get; set; }

        private KyHyokaWriter kyHyokaWriter;


        public ShogisasiImpl(ShogiEngine owner)
        {
            this.owner = owner;
            this.Kokoro = new KokoroImpl(this.owner);
            this.MinimaxEngine = new MinimaxEngineImpl(this.owner);
            this.EdagariEngine = new ScoreSiboriEngine();
            this.ErabuEngine = new ErabuEngine();

            this.Seikaku = new Seikaku();
            this.kyHyokaWriter = new KyHyokaWriterImpl();
        }

        /// <summary>
        /// 対局開始のとき。
        /// </summary>
        public void OnTaikyokuKaisi()
        {
            this.Kokoro.ClearTenonagare();

            // 作った妄想は履歴に追加。

            // 重要さ
            double ordKmdk = 1000.0d;//駒得
            double ordHmdk = 510.0d;//紐付き
            double ordKmgr = 170.0d;//きまぐれ（紐付きの1/3ぐらいで）
            double ordGm = 100.0d;//玉の守り
            double ord7 =  50.0d;
            double ord8 =  45.0d;
            double ordKahd = 10.0d;//角頭の紐付き（うまく働いていないので下げる）


            //--------------------------------------------------------------------------------------------------------------
            //
            // 優先しても悪さしないもの。
            //

            // 「駒得」タイプの狙いを作ります。
            this.Kokoro.AddTenonagare(new TenonagareImpl(null, TenonagareName.KomaDoku, ordKmdk, null, null, new Basho(0), Logger.Log_Engine));

            //--------------------------------------------------------------------------------------------------------------
            //
            // 強さの頼みの綱。
            //

            // 「紐付き」タイプの狙いを作ります。
            this.Kokoro.AddTenonagare(new TenonagareImpl(null, TenonagareName.Himoduki, ordHmdk, null, null, new Basho(0), Logger.Log_Engine));

            //--------------------------------------------------------------------------------------------------------------
            //
            // 面白くするための苦肉の策。
            //

            // 「気まぐれ」タイプの狙いを作ります。
            this.Kokoro.AddTenonagare(new TenonagareImpl(null, TenonagareName.Kimagure, ordKmgr, null, null, new Basho(0), Logger.Log_Engine));

            //--------------------------------------------------------------------------------------------------------------

            // 「玉の守り」タイプの狙いを作ります。
            this.Kokoro.AddTenonagare(new TenonagareImpl(null, TenonagareName.GyokuNoMamori, ordGm, null, null, new Basho(0), Logger.Log_Engine));

            // 「飛車道が通っている」タイプの狙いを作ります。
            this.Kokoro.AddTenonagare(new TenonagareImpl(null, TenonagareName.Toosi, ord7, null, null, new Basho(0), Logger.Log_Engine));

            // 「目の前の歩を取れ」タイプの狙いを作ります。
            this.Kokoro.AddTenonagare(new TenonagareImpl(null, TenonagareName.MenomaenoFuWoTore, ord8, null, null, new Basho(0), Logger.Log_Engine));

            //--------------------------------------------------------------------------------------------------------------
            //
            // 故障部品
            //

            // 「角頭の紐付き」タイプの狙いを作ります。
            this.Kokoro.AddTenonagare(new TenonagareImpl(null, TenonagareName.KakuTouNoHimoduki, ordKahd, null, null, new Basho(0), Logger.Log_Engine));

        }

        /// <summary>
        /// 指し手を決めます。
        /// </summary>
        /// <param name="enableLog"></param>
        /// <param name="isHonshogi"></param>
        /// <param name="kifu"></param>
        /// <param name="playerInfo"></param>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public ShootingStarlightable WA_Bestmove(
            bool enableLog,
            bool isHonshogi,
            KifuTree kifu,
            PlayerInfo playerInfo,
            LarabeLoggerable logTag
            )
        {

            // 「移動」タイプの狙いを、どんどん付け足します。
            this.Kokoro.Omoituki(
                playerInfo.Playerside, (KifuNode)kifu.CurNode, this.Seikaku);

            try
            {
                //
                // 指し手生成ルーチンで、棋譜ツリーを作ります。
                //
                SsssLogGenjo ssssLog = new SsssLogGenjoImpl(enableLog, logTag);
                MoveGenRoutine.WAA_Yomu_Start(kifu, isHonshogi, ssssLog);
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。
                string message = ex.GetType().Name + " " + ex.Message + "：棋譜ツリーを作っていたときです。：";
                Debug.Fail(message);

                // どうにもできないので  ログだけ取って、上に投げます。
                Logger.Log_Engine.WriteLine_Error(message);
                throw ;
            }



            // デバッグ用だが、メソッドはこのオブジェクトを必要としてしまう。
            GraphicalLog_File logF_kiki = new GraphicalLog_File();

            try
            {
                // 点数を付ける（葉ノードに点数を付け、途中のノードの点数も出します）
                this.MinimaxEngine.Tensuduke_ForeachLeafs(
                    Util_Sky.ToSfenMoveText(kifu.CurNode.Key),
                    (KifuNode)kifu.CurNode,
                    kifu,
                    this.Kokoro,
                    playerInfo,
                    ShogisasiImpl.REPORT_ENVIRONMENT,
                    logF_kiki,
                    logTag
                );
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。
                string message = ex.GetType().Name + " " + ex.Message + "：ノードに点数を付けたときです。：";
                Debug.Fail(message);

                // どうにもできないので  ログだけ取って、上に投げます。
                Logger.Log_Engine.WriteLine_Error(message);
                throw ;
            }

#if DEBUG
            try
            {
                // 評価明細のログ出力。
                this.kyHyokaWriter.Write_ForeachLeafs(
                    Util_Sky.ToSfenMoveText(kifu.CurNode.Key),
                    (KifuNode)kifu.CurNode,
                    kifu,
                    playerInfo,
                    "" + Util_Sky.ToSfenMoveText(kifu.CurNode.Key) + "/",
                    ShogisasiImpl.REPORT_ENVIRONMENT,
                    logTag
                    );
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。
                string message = ex.GetType().Name + " " + ex.Message + "：評価明細付きのログ出力をしていたときです。：";
                Debug.Fail(message);

                // どうにもできないので  ログだけ取って、上に投げます。
                Logger.Log_Engine.WriteLine_Error(message);
                throw ex;
            }
#endif

#if DEBUG
            if (0 < logF_kiki.boards.Count)//ﾛｸﾞが残っているなら
            {
                //
                // ログの書き出し
                //
                Util_GraphicalLog.Log(
                    true,//enableLog,
                    "#評価ログ",
                    "[" + Util_GraphicalLog.BoardFileLog_ToJsonStr(logF_kiki) + "]"
                );

                // 書き出した分はクリアーします。
                logF_kiki.boards.Clear();
            }
#endif

            // 枝狩りする。
            this.EdagariEngine.ScoreSibori(kifu, this.Kokoro);

            // 1手に決める
            ShootingStarlightable bestMove = this.ErabuEngine.ChoiceBestMove(kifu, enableLog, isHonshogi, logTag);

            return bestMove;
        }

    }
}
