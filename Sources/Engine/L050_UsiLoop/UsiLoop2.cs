﻿namespace Grayscale.P050_KifuWarabe.L050_UsiLoop
{
    using Grayscale.P025_KifuLarabe.L00012_Atom;
    using Grayscale.P025_KifuLarabe.L00025_Struct;
    using Grayscale.P025_KifuLarabe.L00050_StructShogi;
    using Grayscale.P025_KifuLarabe.L004_StructShogi;
    using Grayscale.P025_KifuLarabe.L012_Common;
    using Grayscale.P025_KifuLarabe.L100_KifuIO;
    using Grayscale.P050_KifuWarabe.CS1_Impl.W050_UsiLoop;
    using Grayscale.P050_KifuWarabe.L00025_UsiLoop;
    using Grayscale.P050_KifuWarabe.L030_Shogisasi;
    using Grayscale.P050_KifuWarabe.L031_AjimiEngine;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
    using Grayscale.P025_KifuLarabe.L00060_KifuParser;
    using Grayscale.P007_SfenReport.L100_Write;
    using Grayscale.P050_KifuWarabe.L00052_Shogisasi;
    using Grayscale.Kifuwarazusa.Entities;
    using Grayscale.Kifuwarazusa.UseCases;

    /// <summary>
    /// USIの２番目のループです。
    /// </summary>
    public class UsiLoop2
    {
        public ShogiEngine playing;
        private Shogisasi shogisasi;

        /// <summary>
        /// 「go ponder」の属性一覧です。
        /// </summary>
        public bool GoPonderNow { get; set; }


        /// <summary>
        /// USIの２番目のループで保持される、「gameover」の一覧です。
        /// </summary>
        public Dictionary<string, string> GameoverProperties { get; set; }


        public UsiLoop2(Playing playing, Shogisasi shogisasi)
        {
            this.playing = playing;
            this.shogisasi = shogisasi;

            playing.AjimiEngine = new AjimiEngine(playing);

            //
            // 図.
            //
            //      この将棋エンジンが後手とします。
            //
            //      ┌──┬─────────────┬──────┬──────┬────────────────────────────────────┐
            //      │順番│                          │計算        │tesumiCount │解説                                                                    │
            //      ┝━━┿━━━━━━━━━━━━━┿━━━━━━┿━━━━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┥
            //      │   1│初回                      │            │            │相手が先手、この将棋エンジンが後手とします。                            │
            //      │    │                          │            │0           │もし、この将棋エンジンが先手なら、初回は tesumiCount = -1 とします。    │
            //      ├──┼─────────────┼──────┼──────┼────────────────────────────────────┤
            //      │   2│position                  │+-0         │            │                                                                        │
            //      │    │    (相手が指しても、     │            │            │                                                                        │
            //      │    │     指していないときでも │            │            │                                                                        │
            //      │    │     送られてきます)      │            │0           │                                                                        │
            //      ├──┼─────────────┼──────┼──────┼────────────────────────────────────┤
            //      │   3│go                        │+2          │            │+2 します                                                               │
            //      │    │    (相手が指した)        │            │2           │    ※「go」は、「go ponder」「go mate」「go infinite」とは区別します。 │
            //      ├──┼─────────────┼──────┼──────┼────────────────────────────────────┤
            //      │   4│go ponder                 │+-0         │            │                                                                        │
            //      │    │    (相手はまだ指してない)│            │2           │                                                                        │
            //      ├──┼─────────────┼──────┼──────┼────────────────────────────────────┤
            //      │   5│自分が指した              │+-0         │            │相手が指してから +2 すると決めたので、                                  │
            //      │    │                          │            │2           │自分が指したときにはカウントを変えません。                              │
            //      └──┴─────────────┴──────┴──────┴────────────────────────────────────┘
            //
            playing.TesumiCount = 0;// ｎ手目

            // 棋譜
            {
                playing.Kifu = new KifuTreeImpl(
                        new KifuNodeImpl(
                            Util_Sky.NullObjectMove,
                            new KyokumenWrapper(new SkyConst(Util_Sky.New_Hirate())),// きふわらべ起動時 // FIXME:平手とは限らないが。
                            Playerside.P2
                        )
                );
                playing.Kifu.SetProperty(KifuTreeImpl.PropName_FirstPside, Playerside.P1);
                playing.Kifu.SetProperty(KifuTreeImpl.PropName_Startpos, "startpos");// 平手 // FIXME:平手とは限らないが。

                Debug.Assert(!Util_MasuNum.OnKomabukuro(
                    Util_Masu.AsMasuNumber(((RO_Star_Koma)playing.Kifu.CurNode.Value.ToKyokumenConst.StarlightIndexOf((Finger)0).Now).Masu)
                    ), "駒が駒袋にあった。");
            }

            // goの属性一覧
            {
                playing.GoProperties = new Dictionary<string, string>();
                playing.GoProperties["btime"] = "";
                playing.GoProperties["wtime"] = "";
                playing.GoProperties["byoyomi"] = "";
            }

            // go ponderの属性一覧
            {
                this.GoPonderNow = false;   // go ponderを将棋所に伝えたなら真
            }

            // gameoverの属性一覧
            {
                this.GameoverProperties = new Dictionary<string, string>();
                this.GameoverProperties["gameover"] = "";
            }
        }

        public void Log1(string message)
        {
            Logger.WriteLineAddMemo(LogTags.Engine, message);
        }
        public void Log2(string line, KifuNode kifuNode, ILogTag logTag, Playing playing)
        {
            int tesumi_yomiGenTeban_forLog = 0;//ログ用。読み進めている現在の手目済

            Logger.WriteLineAddMemo(
                LogTags.Engine,
                Util_Sky.Json_1Sky(playing.Kifu.CurNode.Value.ToKyokumenConst, "現局面になっているのかなんだぜ☆？　line=[" + line + "]　棋譜＝" + KirokuGakari.ToJapaneseKifuText(playing.Kifu, LogTags.Engine),
                "PgCS",
                tesumi_yomiGenTeban_forLog//読み進めている現在の手目
                ));

            //
            // 局面画像ﾛｸﾞ
            //
            {
                // 出力先
                string fileName = "_log_ベストムーブ_最後の.png";

                //SFEN文字列と、出力ファイル名を指定することで、局面の画像ログを出力します。
                KyokumenPngWriterImpl.Write1(
                    kifuNode.ToRO_Kyokumen1(logTag),
                    "",
                    fileName,
                    ShogisasiImpl.REPORT_ENVIRONMENT
                    );
            }
        }
        
        public void AtLoop_OnStop(string line, ref Result_UsiLoop2 result_Usi)
        {
            try
            {

                //------------------------------------------------------------
                // あなたの手番です  （すぐ指してください！）
                //------------------------------------------------------------
                #region ↓詳説
                //
                // 図.
                //
                //      log.txt
                //      ┌────────────────────────────────────────
                //      ～
                //      │2014/08/02 2:03:35> stop
                //      │
                //

                // 何らかの理由で  すぐ指してほしいときに、将棋所から送られてくる文字が stop です。
                //
                // 理由は２つ考えることができます。
                //  （１）１手前に、将棋エンジンが  将棋所に向かって「予想手」付きで指し手を伝えたのだが、
                //        相手の応手が「予想手」とは違ったので、予想手にもとづく思考を  今すぐ変えて欲しいとき。
                //
                //  （２）「急いで指すボタン」が押されたときなどに送られてくるようです？
                //
                // stop するのは思考です。  stop を受け取ったら  すぐに最善手を指してください。
                #endregion

                if (this.GoPonderNow)
                {
                    //------------------------------------------------------------
                    // 将棋エンジン「（予想手が間違っていたって？）  △９二香 を指そうと思っていたんだが」
                    //------------------------------------------------------------
                    #region ↓詳説
                    //
                    // 図.
                    //
                    //      log.txt
                    //      ┌────────────────────────────────────────
                    //      ～
                    //      │2014/08/02 2:36:21< bestmove 9a9b
                    //      │
                    //
                    //
                    //      １手前の指し手で、将棋エンジンが「bestmove ★ ponder ★」という形で  予想手付きで将棋所にメッセージを送っていたとき、
                    //      その予想手が外れていたならば、将棋所は「stop」を返してきます。
                    //      このとき  思考を打ち切って最善手の指し手をすぐに返信するわけですが、将棋所はこの返信を無視します☆ｗ
                    //      （この指し手は、外れていた予想手について考えていた“最善手”ですからゴミのように捨てられます）
                    //      その後、将棋所から「position」「go」が再送されてくるのだと思います。
                    //
                    //          将棋エンジン「bestmove ★ ponder ★」
                    //              ↓
                    //          将棋所      「stop」
                    //              ↓
                    //          将棋エンジン「うその指し手返信」（無視されます）←今ここ
                    //              ↓
                    //          将棋所      「position」「go」
                    //              ↓
                    //          将棋エンジン「本当の指し手」
                    //
                    //      という流れと思います。
                    #endregion
                    // この指し手は、無視されます。（無視されますが、送る必要があります）
                    Playing.Send("bestmove 9a9b");
                }
                else
                {
                    //------------------------------------------------------------
                    // じゃあ、△９二香で
                    //------------------------------------------------------------
                    #region ↓詳説
                    //
                    // 図.
                    //
                    //      log.txt
                    //      ┌────────────────────────────────────────
                    //      ～
                    //      │2014/08/02 2:36:21< bestmove 9a9b
                    //      │
                    //
                    //
                    // 特に何もなく、すぐ指せというのですから、今考えている最善手をすぐに指します。
                    #endregion
                    Playing.Send("bestmove 9a9b");
                }

            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                Logger.WriteLineAddMemo(LogTags.Engine,"Program「stop」：" + ex.GetType().Name + " " + ex.Message);
            }
        }

        public void AtLoop_OnGameover(string line, ref Result_UsiLoop2 result_Usi)
        {
            try
            {
                //------------------------------------------------------------
                // 対局が終わりました
                //------------------------------------------------------------
                #region ↓詳説
                //
                // 図.
                //
                //      log.txt
                //      ┌────────────────────────────────────────
                //      ～
                //      │2014/08/02 3:08:34> gameover lose
                //      │
                //

                // 対局が終わったときに送られてくる文字が gameover です。
                #endregion

                //------------------------------------------------------------
                // 「あ、勝ちました」「あ、引き分けました」「あ、負けました」
                //------------------------------------------------------------
                #region ↓詳説
                //
                // 上図のメッセージのままだと使いにくいので、
                // あとで使いやすいように Key と Value の表に分けて持ち直します。
                //
                // 図.
                //
                //      gameoverDictionary
                //      ┌──────┬──────┐
                //      │Key         │Value       │
                //      ┝━━━━━━┿━━━━━━┥
                //      │gameover    │lose        │
                //      └──────┴──────┘
                //
                #endregion
                Regex regex = new Regex(@"gameover (.)", RegexOptions.Singleline);
                Match m = regex.Match(line);

                if (m.Success)
                {
                    this.GameoverProperties["gameover"] = (string)m.Groups[1].Value;
                }
                else
                {
                    this.GameoverProperties["gameover"] = "";
                }


                // 無限ループ（２つ目）を抜けます。無限ループ（１つ目）に戻ります。
                result_Usi = Result_UsiLoop2.Break;
            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                Logger.WriteLineAddMemo(LogTags.Engine,"Program「gameover」：" + ex.GetType().Name + " " + ex.Message);
            }
        }

        /// <summary>
        /// 独自コマンド「ログ出せ」
        /// </summary>
        /// <param name="line"></param>
        /// <param name="result_Usi"></param>
        public void AtLoop_OnLogdase(string line, ref Result_UsiLoop2 result_Usi, Playing playing)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ログだせ～（＾▽＾）");

            playing.Kifu.ForeachZenpuku(
                playing.Kifu.GetRoot(), (int tesumi, KyokumenWrapper sky, Node<ShootingStarlightable, KyokumenWrapper> node, ref bool toBreak) =>
                {
                    //sb.AppendLine("(^-^)");

                    if(null!=node)
                    {
                        if (null != node.Key)
                        {
                            string sfenText = Util_Sky.ToSfenMoveText(node.Key);
                            sb.Append(sfenText);
                            sb.AppendLine();
                        }
                    }
                });

            File.WriteAllText("../../Logs/_log_ログ出せ命令.txt", sb.ToString());
        }
    }
}
