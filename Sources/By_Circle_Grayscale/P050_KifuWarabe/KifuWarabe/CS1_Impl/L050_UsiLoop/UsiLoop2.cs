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

namespace Grayscale.P050_KifuWarabe.L050_UsiLoop
{

    /// <summary>
    /// USIの２番目のループです。
    /// </summary>
    public class UsiLoop2
    {
        #region プロパティー
        private ShogiEngine owner;
        private Shogisasi shogisasi;

        /// <summary>
        /// 手目済カウントです。
        /// </summary>
        public int TesumiCount { get; set; }


        /// <summary>
        /// 棋譜です。
        /// </summary>
        public KifuTree Kifu { get; set; }


        /// <summary>
        /// 「go」の属性一覧です。
        /// </summary>
        public Dictionary<string, string> GoProperties { get; set; }


        /// <summary>
        /// 「go ponder」の属性一覧です。
        /// </summary>
        public bool GoPonderNow { get; set; }


        /// <summary>
        /// USIの２番目のループで保持される、「gameover」の一覧です。
        /// </summary>
        public Dictionary<string, string> GameoverProperties { get; set; }

        private AjimiEngine ajimiEngine;
        #endregion

        public UsiLoop2(Shogisasi shogisasi, ShogiEngine owner)
        {
            this.owner = owner;
            this.shogisasi = shogisasi;

            this.ajimiEngine = new AjimiEngine(owner);

            #region ↓詳説  ＜n手目＞
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
            #endregion
            this.TesumiCount = 0;// ｎ手目

            // 棋譜
            {
                this.Kifu = new KifuTreeImpl(
                        new KifuNodeImpl(
                            Util_Sky.NULL_OBJECT_SASITE,
                            new KyokumenWrapper(new SkyConst(Util_Sky.New_Hirate())),// きふわらべ起動時 // FIXME:平手とは限らないが。
                            Playerside.P2
                        )
                );
                this.Kifu.SetProperty(KifuTreeImpl.PropName_FirstPside, Playerside.P1);
                this.Kifu.SetProperty(KifuTreeImpl.PropName_Startpos, "startpos");// 平手 // FIXME:平手とは限らないが。

                Debug.Assert(!Util_MasuNum.OnKomabukuro(
                    Util_Masu.AsMasuNumber(((RO_Star_Koma)this.Kifu.CurNode.Value.ToKyokumenConst.StarlightIndexOf((Finger)0).Now).Masu)
                    ), "駒が駒袋にあった。");
            }

            // goの属性一覧
            {
                this.GoProperties = new Dictionary<string, string>();
                this.GoProperties["btime"] = "";
                this.GoProperties["wtime"] = "";
                this.GoProperties["byoyomi"] = "";
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


        public void AtBegin()
        {
            this.shogisasi.OnTaikyokuKaisi();//対局開始時の処理。
        }

        public void AtLoop()
        {
            //PerformanceMetrics performanceMetrics = new PerformanceMetrics();//使ってない？

            while (true)
            {
                Result_UsiLoop2 result_Usi = Result_UsiLoop2.None;

                // 将棋サーバーから何かメッセージが届いていないか、見てみます。
                string line = Util_Message.Download_NonStop();
                this.owner.Log_Client.WriteLine_AddMemo(line);

                if (null == line)
                {
                    // メッセージは届いていませんでした。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    goto gt_NextTime2;
                }

                // メッセージが届いています！
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                LarabeLoggerList.GetDefaultList().DefaultFile.WriteLine_R(line);


                if (line.StartsWith("position")){this.AtLoop_OnPosition(line,ref result_Usi);}
                else if (line.StartsWith("go ponder")) { this.AtLoop_OnGoponder(line, ref result_Usi); }
                else if (line.StartsWith("go")) { this.AtLoop_OnGo(line, ref result_Usi); }// 「go ponder」「go mate」「go infinite」とは区別します。
                else if (line.StartsWith("stop")) { this.AtLoop_OnStop(line, ref result_Usi); }
                else if (line.StartsWith("gameover")) { this.AtLoop_OnGameover(line, ref result_Usi); }
                else if ("logdase" == line) { this.AtLoop_OnLogdase(line, ref result_Usi); }
                else
                {
                    //------------------------------------------------------------
                    // ○△□×！？
                    //------------------------------------------------------------
                    #region ↓詳説
                    //
                    // ／(＾×＾)＼
                    //

                    // 通信が届いていますが、このプログラムでは  聞かなかったことにします。
                    // USIプロトコルの独習を進め、対応／未対応を選んでください。
                    //
                    // ログだけ取って、スルーします。
                    #endregion
                }

                switch (result_Usi)
                {
                    case Result_UsiLoop2.Break:
                        goto end_loop2;

                    default:
                        break;
                }

            gt_NextTime2:
                ;
            }
        end_loop2:
            ;
        }

        public void AtEnd()
        {
            //-------------------+----------------------------------------------------------------------------------------------------
            // スナップショット  |
            //-------------------+----------------------------------------------------------------------------------------------------
            #region ↓詳説
            // 対局後のタイミングで、データの中身を確認しておきます。
            // Key と Value の表の形をしています。（順不同）
            //
            // 図.
            //      ※内容はサンプルです。実際と異なる場合があります。
            //
            //      setoptionDictionary
            //      ┌──────┬──────┐
            //      │Key         │Value       │
            //      ┝━━━━━━┿━━━━━━┥
            //      │USI_Ponder  │true        │
            //      ├──────┼──────┤
            //      │USI_Hash    │256         │
            //      └──────┴──────┘
            //
            //      goDictionary
            //      ┌──────┬──────┐
            //      │Key         │Value       │
            //      ┝━━━━━━┿━━━━━━┥
            //      │btime       │599000      │
            //      ├──────┼──────┤
            //      │wtime       │600000      │
            //      ├──────┼──────┤
            //      │byoyomi     │60000       │
            //      └──────┴──────┘
            //
            //      goMateDictionary
            //      ┌──────┬──────┐
            //      │Key         │Value       │
            //      ┝━━━━━━┿━━━━━━┥
            //      │mate        │599000      │
            //      └──────┴──────┘
            //
            //      gameoverDictionary
            //      ┌──────┬──────┐
            //      │Key         │Value       │
            //      ┝━━━━━━┿━━━━━━┥
            //      │gameover    │lose        │
            //      └──────┴──────┘
            //
            #endregion
            this.owner.Log_Engine.WriteLine_AddMemo("KifuParserA_Impl.LOGGING_BY_ENGINE, ┏━確認━━━━setoptionDictionary ━┓");
            foreach (KeyValuePair<string, string> pair in this.owner.SetoptionDictionary)
            {
                this.owner.Log_Engine.WriteLine_AddMemo(pair.Key + "=" + pair.Value);
            }
            this.owner.Log_Engine.WriteLine_AddMemo("┗━━━━━━━━━━━━━━━━━━┛");
            this.owner.Log_Engine.WriteLine_AddMemo("┏━確認━━━━goDictionary━━━━━┓");
            foreach (KeyValuePair<string, string> pair in this.GoProperties)
            {
                this.owner.Log_Engine.WriteLine_AddMemo(pair.Key + "=" + pair.Value);
            }

            //Dictionary<string, string> goMateProperties = new Dictionary<string, string>();
            //goMateProperties["mate"] = "";
            //LarabeLoggerList_Warabe.ENGINE.WriteLine_AddMemo("┗━━━━━━━━━━━━━━━━━━┛");
            //LarabeLoggerList_Warabe.ENGINE.WriteLine_AddMemo("┏━確認━━━━goMateDictionary━━━┓");
            //foreach (KeyValuePair<string, string> pair in this.goMateProperties)
            //{
            //    LarabeLoggerList_Warabe.ENGINE.WriteLine_AddMemo(pair.Key + "=" + pair.Value);
            //}

            this.owner.Log_Engine.WriteLine_AddMemo("┗━━━━━━━━━━━━━━━━━━┛");
            this.owner.Log_Engine.WriteLine_AddMemo("┏━確認━━━━gameoverDictionary━━┓");
            foreach (KeyValuePair<string, string> pair in this.GameoverProperties)
            {
                this.owner.Log_Engine.WriteLine_AddMemo(pair.Key + "=" + pair.Value);
            }
            this.owner.Log_Engine.WriteLine_AddMemo("┗━━━━━━━━━━━━━━━━━━┛");
        }


        public void AtLoop_OnPosition(string line, ref Result_UsiLoop2 result_Usi)
        {
            try
            {
                //------------------------------------------------------------
                // これが棋譜です
                //------------------------------------------------------------
                #region ↓詳説
                //
                // 図.
                //
                //      log.txt
                //      ┌────────────────────────────────────────
                //      ～
                //      │2014/08/02 2:03:35> position startpos moves 2g2f
                //      │
                //
                // ↑↓この将棋エンジンは後手で、平手初期局面から、先手が初手  ▲２六歩  を指されたことが分かります。
                //
                //        ９  ８  ７  ６  ５  ４  ３  ２  １                 ９  ８  ７  ６  ５  ４  ３  ２  １
                //      ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐             ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐
                //      │香│桂│銀│金│玉│金│銀│桂│香│一           │ｌ│ｎ│ｓ│ｇ│ｋ│ｇ│ｓ│ｎ│ｌ│ａ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │飛│  │  │  │  │  │角│  │二           │  │ｒ│  │  │  │  │  │ｂ│  │ｂ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │歩│歩│歩│歩│歩│歩│歩│歩│歩│三           │ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｃ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │  │  │  │  │  │  │  │  │四           │  │  │  │  │  │  │  │  │  │ｄ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │  │  │  │  │  │  │  │  │五           │  │  │  │  │  │  │  │  │  │ｅ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │  │  │  │  │  │  │歩│  │六           │  │  │  │  │  │  │  │Ｐ│  │ｆ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │歩│歩│歩│歩│歩│歩│歩│  │歩│七           │Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│  │Ｐ│ｇ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │角│  │  │  │  │  │飛│  │八           │  │Ｂ│  │  │  │  │  │Ｒ│  │ｈ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │香│桂│銀│金│玉│金│銀│桂│香│九           │Ｌ│Ｎ│Ｓ│Ｇ│Ｋ│Ｇ│Ｓ│Ｎ│Ｌ│ｉ
                //      └─┴─┴─┴─┴─┴─┴─┴─┴─┘             └─┴─┴─┴─┴─┴─┴─┴─┴─┘
                //
                // または
                //
                //      log.txt
                //      ┌────────────────────────────────────────
                //      ～
                //      │2014/08/02 2:03:35> position sfen lnsgkgsnl/9/ppppppppp/9/9/9/PPPPPPPPP/1B5R1/LNSGKGSNL w - 1 moves 5a6b 7g7f 3a3b
                //      │
                //
                // ↑↓将棋所のサンプルによると、“２枚落ち初期局面から△６二玉、▲７六歩、△３二銀と進んだ局面”とのことです。
                //
                //                                           ＜初期局面＞    ９  ８  ７  ６  ５  ４  ３  ２  １
                //                                                         ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐
                //                                                         │ｌ│ｎ│ｓ│ｇ│ｋ│ｇ│ｓ│ｎ│ｌ│ａ  ←lnsgkgsnl
                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //                                                         │  │  │  │  │  │  │  │  │  │ｂ  ←9
                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //                                                         │ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｃ  ←ppppppppp
                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //                                                         │  │  │  │  │  │  │  │  │  │ｄ  ←9
                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //                                                         │  │  │  │  │  │  │  │  │  │ｅ  ←9
                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //                                                         │  │  │  │  │  │  │  │  │  │ｆ  ←9
                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //                                                         │Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│ｇ  ←PPPPPPPPP
                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //                                                         │  │Ｂ│  │  │  │  │  │Ｒ│  │ｈ  ←1B5R1
                //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //                                                         │Ｌ│Ｎ│Ｓ│Ｇ│Ｋ│Ｇ│Ｓ│Ｎ│Ｌ│ｉ  ←LNSGKGSNL
                //                                                         └─┴─┴─┴─┴─┴─┴─┴─┴─┘
                //
                //        ９  ８  ７  ６  ５  ４  ３  ２  １   ＜３手目＞    ９  ８  ７  ６  ５  ４  ３  ２  １
                //      ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐             ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐
                //      │香│桂│銀│金│  │金│  │桂│香│一           │ｌ│ｎ│ｓ│ｇ│  │ｇ│  │ｎ│ｌ│ａ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │  │  │玉│  │  │銀│  │  │二           │  │  │  │ｋ│  │  │ｓ│  │  │ｂ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │歩│歩│歩│歩│歩│歩│歩│歩│歩│三           │ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｃ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │  │  │  │  │  │  │  │  │四           │  │  │  │  │  │  │  │  │  │ｄ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │  │  │  │  │  │  │  │  │五           │  │  │  │  │  │  │  │  │  │ｅ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │  │歩│  │  │  │  │  │  │六           │  │  │Ｐ│  │  │  │  │  │  │ｆ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │歩│歩│  │歩│歩│歩│歩│歩│歩│七           │Ｐ│Ｐ│  │Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│ｇ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │  │角│  │  │  │  │  │飛│  │八           │  │Ｂ│  │  │  │  │  │Ｒ│  │ｈ
                //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //      │香│桂│銀│金│玉│金│銀│桂│香│九           │Ｌ│Ｎ│Ｓ│Ｇ│Ｋ│Ｇ│Ｓ│Ｎ│Ｌ│ｉ
                //      └─┴─┴─┴─┴─┴─┴─┴─┴─┘             └─┴─┴─┴─┴─┴─┴─┴─┴─┘
                //

                // 手番になったときに、“まず”、将棋所から送られてくる文字が position です。
                // このメッセージを読むと、駒の配置が分かります。
                //
                // “が”、まだ指してはいけません。
                #endregion
                this.Log1("（＾△＾）positionきたｺﾚ！");

                // 入力行を解析します。
                KifuParserA_Result result = new KifuParserA_ResultImpl();
                new KifuParserA_Impl().Execute_All(
                    ref result,
                    new ShogiGui_Warabe(this.Kifu),
                    new KifuParserA_GenjoImpl(line),
                    new KifuParserA_LogImpl( this.owner.Log_Engine, "Program#Main(Warabe)")
                    );
                this.Log2(line, (KifuNode)result.Out_newNode_OrNull, this.owner.Log_Engine);


                //------------------------------------------------------------
                // じっとがまん
                //------------------------------------------------------------
                #region ↓詳説
                //
                // 応答は無用です。
                // 多分、将棋所もまだ準備ができていないのではないでしょうか（？）
                //
                #endregion

            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                this.owner.Log_Engine.WriteLine_AddMemo("Program「position」：" + ex.GetType().Name + "：" + ex.Message);
            }
        }
        private void Log1(string message)
        {
            this.owner.Log_Engine.WriteLine_AddMemo(message);
        }
        private void Log2(string line, KifuNode kifuNode, LarabeLoggerable logTag)
        {
            int tesumi_yomiGenTeban_forLog = 0;//ログ用。読み進めている現在の手目済

            this.owner.Log_Engine.WriteLine_AddMemo(Util_Sky.Json_1Sky(this.Kifu.CurNode.Value.ToKyokumenConst, "現局面になっているのかなんだぜ☆？　line=[" + line + "]　棋譜＝" + KirokuGakari.ToJapaneseKifuText(this.Kifu, this.owner.Log_Engine),
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
        

        public void AtLoop_OnGoponder(string line, ref Result_UsiLoop2 result_Usi)
        {
            try
            {

                //------------------------------------------------------------
                // 将棋所が次に呼びかけるまで、考えていてください
                //------------------------------------------------------------
                #region ↓詳説
                //
                // 図.
                //
                //      log.txt
                //      ┌────────────────────────────────────────
                //      ～
                //      │2014/08/02 2:03:35> go ponder
                //      │
                //

                // 先読み用です。
                // 今回のプログラムでは対応しません。
                //
                // 将棋エンジンが  将棋所に向かって  「bestmove ★ ponder ★」といったメッセージを送ったとき、
                // 将棋所は「go ponder」というメッセージを返してくると思います。
                //
                // 恐らく  このメッセージを受け取っても、将棋エンジンは気にせず  考え続けていればいいのではないでしょうか。
                #endregion


                //------------------------------------------------------------
                // じっとがまん
                //------------------------------------------------------------
                #region ↓詳説
                //
                // まだ指してはいけません。
                // 指したら反則です。相手はまだ指していないのだ☆ｗ
                //
                #endregion
            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                this.owner.Log_Engine.WriteLine_AddMemo("Program「go ponder」：" + ex.GetType().Name + "：" + ex.Message);
            }
        }


        /// <summary>
        /// 「go ponder」「go mate」「go infinite」とは区別します。
        /// </summary>
        /// <param name="line"></param>
        /// <param name="result_Usi"></param>
        public void AtLoop_OnGo(string line, ref Result_UsiLoop2 result_Usi)
        {
            try
            {

                //------------------------------------------------------------
                // あなたの手番です
                //------------------------------------------------------------
                #region ↓詳説
                //
                // 図.
                //
                //      log.txt
                //      ┌────────────────────────────────────────
                //      ～
                //      │2014/08/02 2:36:19> go btime 599000 wtime 600000 byoyomi 60000
                //      │
                //
                // もう指していいときに、将棋所から送られてくる文字が go です。
                //
                #endregion

                // ｎ手目を 2 増やします。
                // 相手の手番と、自分の手番の 2つが増えた、という数え方です。
                this.TesumiCount += 2;

                //------------------------------------------------------------
                // 先手 3:00  後手 0:00  記録係「50秒ぉ～」
                //------------------------------------------------------------
                #region ↓詳説
                //
                // 上図のメッセージのままだと使いにくいので、
                // あとで使いやすいように Key と Value の表に分けて持ち直します。
                //
                // 図.
                //
                //      goDictionary
                //      ┌──────┬──────┐
                //      │Key         │Value       │
                //      ┝━━━━━━┿━━━━━━┥
                //      │btime       │599000      │
                //      ├──────┼──────┤
                //      │wtime       │600000      │
                //      ├──────┼──────┤
                //      │byoyomi     │60000       │
                //      └──────┴──────┘
                //      単位はミリ秒ですので、599000 は 59.9秒 です。
                //
                #endregion
                Regex regex = new Regex(@"go btime (\d+) wtime (\d+) byoyomi (\d+)", RegexOptions.Singleline);
                Match m = regex.Match(line);

                if (m.Success)
                {
                    this.GoProperties["btime"] = (string)m.Groups[1].Value;
                    this.GoProperties["wtime"] = (string)m.Groups[2].Value;
                    this.GoProperties["byoyomi"] = (string)m.Groups[3].Value;
                }
                else
                {
                    this.GoProperties["btime"] = "";
                    this.GoProperties["wtime"] = "";
                    this.GoProperties["byoyomi"] = "";
                }



                // ┏━━━━サンプル・プログラム━━━━┓

                int latestTesumi = this.Kifu.CountTesumi(this.Kifu.CurNode);//現・手目済
                this.owner.PlayerInfo.Playerside = this.Kifu.CountPside(latestTesumi);// 先後

//#if DEBUG
//                MessageBox.Show("["+latestTesumi+"]手目済　["+this.owner.PlayerInfo.Playerside+"]の手番");
//#endif

                SkyConst src_Sky = this.Kifu.NodeAt(latestTesumi).Value.ToKyokumenConst;//現局面

                this.owner.Log_Engine.WriteLine_AddMemo("将棋サーバー「" + latestTesumi + "手目、きふわらべ　さんの手番ですよ！」　" + line);


                Result_Ajimi result_Ajimi = this.ajimiEngine.Ajimi(src_Sky);


                //------------------------------------------------------------
                // わたしの手番のとき、王様が　将棋盤上からいなくなっていれば、投了します。
                //------------------------------------------------------------
                //
                //      将棋ＧＵＩ『きふならべ』用☆　将棋盤上に王さまがいないときに、本将棋で　go　コマンドが送られてくることは無いのでは☆？
                //
                switch (result_Ajimi)
                {
                    case Result_Ajimi.Lost_SenteOh:// 先手の王さまが将棋盤上にいないとき☆
                    case Result_Ajimi.Lost_GoteOh:// または、後手の王さまが将棋盤上にいないとき☆
                        {
                            //------------------------------------------------------------
                            // 投了
                            //------------------------------------------------------------
                            #region ↓詳説
                            //
                            // 図.
                            //
                            //      log.txt
                            //      ┌────────────────────────────────────────
                            //      ～
                            //      │2014/08/02 2:36:21< bestmove resign
                            //      │
                            //

                            // この将棋エンジンは、後手とします。
                            // ２０手目、投了  を決め打ちで返します。
                            #endregion
                            this.owner.Send("bestmove resign");//投了
                        }
                        break;
                    default:// どちらの王さまも、まだまだ健在だぜ☆！
                        {
                            try
                            {
                                //------------------------------------------------------------
                                // 指し手のチョイス
                                //------------------------------------------------------------
                                bool enableLog = false;
                                bool isHonshogi = true;

                                // 指し手を決めます。
                                ShootingStarlightable bestSasite = this.shogisasi.WA_Bestmove(
                                    enableLog,
                                    isHonshogi,
                                    this.Kifu,
                                    this.owner.PlayerInfo,
                                    this.owner.Log_Engine
                                    );




                                if (Util_Sky.isEnableSfen(bestSasite))
                                {
                                    string sfenText = Util_Sky.ToSfenSasiteText(bestSasite);
                                    this.owner.Log_Engine.WriteLine_AddMemo("(Warabe)指し手のチョイス： bestmove＝[" + sfenText + "]" +
                                        "　棋譜＝" + KirokuGakari.ToJapaneseKifuText(this.Kifu, this.owner.Log_Engine));

                                    this.owner.Send("bestmove " + sfenText);//指し手を送ります。
                                }
                                else // 指し手がないときは、SFENが書けない☆　投了だぜ☆
                                {
                                    this.owner.Log_Engine.WriteLine_AddMemo("(Warabe)指し手のチョイス： 指し手がないときは、SFENが書けない☆　投了だぜ☆ｗｗ（＞＿＜）" +
                                        "　棋譜＝" + KirokuGakari.ToJapaneseKifuText(this.Kifu, this.owner.Log_Engine));

                                    // 投了ｗ！
                                    this.owner.Send("bestmove resign");
                                }
                            }
                            catch (Exception ex)
                            {
                                //>>>>> エラーが起こりました。
                                string message = ex.GetType().Name + " " + ex.Message + "：ベスト指し手のチョイスをしたときです。：";
                                Debug.Fail(message);

                                // どうにもできないので  ログだけ取って無視します。
                                this.owner.Log_Engine.WriteLine_Error(message);
                            }
                        }
                        break;
                }
                // ┗━━━━サンプル・プログラム━━━━┛


            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                this.owner.Log_Engine.WriteLine_AddMemo("Program「go」：" + ex.GetType().Name + " " + ex.Message + "：goを受け取ったときです。：");
            }


            //System.C onsole.WriteLine();

            //throw new Exception("デバッグだぜ☆！　エラーはキャッチできたかな～☆？（＾▽＾）");
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
                    this.owner.Send("bestmove 9a9b");
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
                    this.owner.Send("bestmove 9a9b");
                }

            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                this.owner.Log_Engine.WriteLine_AddMemo("Program「stop」：" + ex.GetType().Name + " " + ex.Message);
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
                this.owner.Log_Engine.WriteLine_AddMemo("Program「gameover」：" + ex.GetType().Name + " " + ex.Message);
            }
        }

        /// <summary>
        /// 独自コマンド「ログ出せ」
        /// </summary>
        /// <param name="line"></param>
        /// <param name="result_Usi"></param>
        public void AtLoop_OnLogdase(string line, ref Result_UsiLoop2 result_Usi)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ログだせ～（＾▽＾）");

            this.Kifu.ForeachZenpuku(
                this.Kifu.GetRoot(), (int tesumi, KyokumenWrapper sky, Node<ShootingStarlightable, KyokumenWrapper> node, ref bool toBreak) =>
                {
                    //sb.AppendLine("(^-^)");

                    if(null!=node)
                    {
                        if (null != node.Key)
                        {
                            string sfenText = Util_Sky.ToSfenSasiteText(node.Key);
                            sb.Append(sfenText);
                            sb.AppendLine();
                        }
                    }
                });

            File.WriteAllText("../../Logs/_log_ログ出せ命令.txt", sb.ToString());
        }
    }
}
