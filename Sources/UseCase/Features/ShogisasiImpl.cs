using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.Kifuwarazusa.UseCases.Features
{
    /// <summary>
    /// 将棋指しエンジン。
    /// 
    /// 指し手を決めるエンジンです。
    /// </summary>
    public class ShogisasiImpl : Shogisasi
    {
        private ShogiEngine Owner { get; set; }

        public static ReportEnvironment ReportEnvironment;
        static ShogisasiImpl()
        {
            ShogisasiImpl.ReportEnvironment = new ReportEnvironmentImpl(
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
            this.Owner = owner;
            this.Kokoro = new KokoroImpl(this.Owner);
            this.MinimaxEngine = new MinimaxEngineImpl(this.Owner);
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
            double ord7 = 50.0d;
            double ord8 = 45.0d;
            double ordKahd = 10.0d;//角頭の紐付き（うまく働いていないので下げる）


            //--------------------------------------------------------------------------------------------------------------
            //
            // 優先しても悪さしないもの。
            //

            // 「駒得」タイプの狙いを作ります。
            this.Kokoro.AddTenonagare(new TenonagareImpl(null, TenonagareName.KomaDoku, ordKmdk, null, null, new Basho(0)));

            //--------------------------------------------------------------------------------------------------------------
            //
            // 強さの頼みの綱。
            //

            // 「紐付き」タイプの狙いを作ります。
            this.Kokoro.AddTenonagare(new TenonagareImpl(null, TenonagareName.Himoduki, ordHmdk, null, null, new Basho(0)));

            //--------------------------------------------------------------------------------------------------------------
            //
            // 面白くするための苦肉の策。
            //

            // 「気まぐれ」タイプの狙いを作ります。
            this.Kokoro.AddTenonagare(new TenonagareImpl(null, TenonagareName.Kimagure, ordKmgr, null, null, new Basho(0)));

            //--------------------------------------------------------------------------------------------------------------

            // 「玉の守り」タイプの狙いを作ります。
            this.Kokoro.AddTenonagare(new TenonagareImpl(null, TenonagareName.GyokuNoMamori, ordGm, null, null, new Basho(0)));

            // 「飛車道が通っている」タイプの狙いを作ります。
            this.Kokoro.AddTenonagare(new TenonagareImpl(null, TenonagareName.Toosi, ord7, null, null, new Basho(0)));

            // 「目の前の歩を取れ」タイプの狙いを作ります。
            this.Kokoro.AddTenonagare(new TenonagareImpl(null, TenonagareName.MenomaenoFuWoTore, ord8, null, null, new Basho(0)));

            //--------------------------------------------------------------------------------------------------------------
            //
            // 故障部品
            //

            // 「角頭の紐付き」タイプの狙いを作ります。
            this.Kokoro.AddTenonagare(new TenonagareImpl(null, TenonagareName.KakuTouNoHimoduki, ordKahd, null, null, new Basho(0)));

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
            PlayerInfo playerInfo
            )
        {

            // 「移動」タイプの狙いを、どんどん付け足します。
            this.Kokoro.Omoituki(
                playerInfo.Playerside, (KifuNode)kifu.CurNode, this.Seikaku);

            //
            // 指し手生成ルーチンで、棋譜ツリーを作ります。
            //
            SsssLogGenjo ssssLog = new SsssLogGenjoImpl(enableLog);
            MoveGenRoutine.WAA_Yomu_Start(kifu, isHonshogi, ssssLog);



            // デバッグ用だが、メソッドはこのオブジェクトを必要としてしまう。
            GraphicalLog_File logF_kiki = new GraphicalLog_File();

            // 点数を付ける（葉ノードに点数を付け、途中のノードの点数も出します）
            this.MinimaxEngine.Tensuduke_ForeachLeafs(
                Util_Sky.ToSfenMoveText(kifu.CurNode.Key),
                (KifuNode)kifu.CurNode,
                kifu,
                this.Kokoro,
                playerInfo,
                ShogisasiImpl.ReportEnvironment,
                logF_kiki
            );

#if DEBUG
            // 評価明細のログ出力。
            this.kyHyokaWriter.Write_ForeachLeafs(
                Util_Sky.ToSfenMoveText(kifu.CurNode.Key),
                (KifuNode)kifu.CurNode,
                kifu,
                playerInfo,
                "" + Util_Sky.ToSfenMoveText(kifu.CurNode.Key) + "/",
                ShogisasiImpl.ReportEnvironment
                );

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
            ShootingStarlightable bestMove = this.ErabuEngine.ChoiceBestMove(kifu, enableLog, isHonshogi);

            return bestMove;
        }

    }
}
