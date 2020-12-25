namespace Grayscale.Kifuwarazusa.Engine
{
#if DEBUG
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using Grayscale.Kifuwarazusa.Entities.Logging;
    using Grayscale.Kifuwarazusa.UseCases;
    using Grayscale.Kifuwarazusa.UseCases.Gui;
    using Grayscale.Kifuwarazusa.Entities.Features;
    using Grayscale.Kifuwarazusa.Entities.Features;
    using Grayscale.Kifuwarazusa.Entities.Features;
    using Grayscale.Kifuwarazusa.Entities.Features;
    using Grayscale.Kifuwarazusa.Entities.Features;
    using Grayscale.Kifuwarazusa.Entities.Features;
    using Grayscale.Kifuwarazusa.Entities.Features;
    using Grayscale.Kifuwarazusa.Entities.Features;
    using Grayscale.P050_KifuWarabe.L030_Shogisasi;
    using Grayscale.P050_KifuWarabe.L031_AjimiEngine;
    using Nett;
#else
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using Grayscale.Kifuwarazusa.Entities.Features;
    using Grayscale.Kifuwarazusa.Entities.Logging;
    using Grayscale.Kifuwarazusa.UseCases;
    using Grayscale.Kifuwarazusa.UseCases.Gui;
    using Grayscale.P050_KifuWarabe.L030_Shogisasi;
    using Grayscale.P050_KifuWarabe.L031_AjimiEngine;
    using Nett;
#endif

    /// <summary>
    /// 将棋エンジン　きふわらべ
    /// プログラムのエントリー・ポイントです。
    /// </summary>
    class Program
    {
        /// <summary>
        /// Ｃ＃のプログラムは、
        /// この Main 関数から始まり、 Main 関数を抜けて終わります。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // 
            var playing = new Playing();

            // 思考エンジンの、記憶を読み取ります。
            playing.shogisasi = new ShogisasiImpl(playing);
            playing.shogisasi.Kokoro.ReadTenonagare();

            try
            {
                // 
                // 図.
                // 
                //     プログラムの開始：  ここの先頭行から始まります。
                //     プログラムの実行：  この中で、ずっと無限ループし続けています。
                //     プログラムの終了：  この中の最終行を終えたとき、
                //                         または途中で Environment.Exit(0); が呼ばれたときに終わります。
                //                         また、コンソールウィンドウの[×]ボタンを押して強制終了されたときも  ぶつ切り  で突然終わります。

                //------+-----------------------------------------------------------------------------------------------------------------
                // 準備 |
                //------+-----------------------------------------------------------------------------------------------------------------
                var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
                var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

                // データの読取「道」
                Michi187Array.Load(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Michi187")));

                // データの読取「配役」
                Util_Haiyaku184Array.Load(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("Haiyaku185")), Encoding.UTF8);

                // データの読取「強制転成表」　※駒配役を生成した後で。
                ForcePromotionArray.Load(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("InputForcePromotion")), Encoding.UTF8);
                File.WriteAllText(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("OutputForcePromotion")), ForcePromotionArray.LogHtml());

                // データの読取「配役転換表」
                Data_HaiyakuTransition.Load(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("InputSyuruiToHaiyaku")), Encoding.UTF8);
                File.WriteAllText(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("OutputSyuruiToHaiyaku")), Data_HaiyakuTransition.LogHtml());



                //-------------------+----------------------------------------------------------------------------------------------------
                // ログファイル削除  |
                //-------------------+----------------------------------------------------------------------------------------------------
                //
                // 図.
                //
                //      フォルダー
                //          ├─ Engine.KifuWarabe.exe
                //          └─ log.txt               ←これを削除
                //
                Logger.RemoveAllLogFile();

                {
                    //-------------+----------------------------------------------------------------------------------------------------------
                    // ログ書込み  |  ＜この将棋エンジン＞  製品名、バージョン番号
                    //-------------+----------------------------------------------------------------------------------------------------------
                    //
                    // 図.
                    //
                    //      log.txt
                    //      ┌────────────────────────────────────────
                    //      │2014/08/02 1:04:59> v(^▽^)v ｲｪｰｲ☆ ... fugafuga 1.00.0
                    //      │
                    //      │
                    //
                    //
                    // 製品名とバージョン番号は、次のファイルに書かれているものを使っています。
                    // 場所：  [ソリューション エクスプローラー]-[ソリューション名]-[プロジェクト名]-[Properties]-[AssemblyInfo.cs] の中の、[AssemblyProduct]と[AssemblyVersion] を参照。
                    //
                    // バージョン番号を「1.00.0」形式（メジャー番号.マイナー番号.ビルド番号)で書くのは作者の趣味です。
                    //
                    string versionStr;
                    {

                        // バージョン番号
                        Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                        versionStr = String.Format("{0}.{1}.{2}", version.Major, version.Minor.ToString("00"), version.Build);

                        //seihinName += " " + versionStr;
                    }

                    var engineName = toml.Get<TomlTable>("Engine").Get<string>("Name");
                    Logger.Trace($"v(^▽^)v ｲｪｰｲ☆ ... {engineName} {versionStr}");
                }


                //-----------+------------------------------------------------------------------------------------------------------------
                // 通信開始  |
                //-----------+------------------------------------------------------------------------------------------------------------
                //
                // 図.
                //
                //      無限ループ（全体）
                //          │
                //          ├─無限ループ（１）
                //          │                      将棋エンジンであることが認知されるまで、目で訴え続けます(^▽^)
                //          │                      認知されると、無限ループ（２）に進みます。
                //          │
                //          └─無限ループ（２）
                //                                  対局中、ずっとです。
                //                                  対局が終わると、無限ループ（１）に戻ります。
                //
                // 無限ループの中に、２つの無限ループが入っています。
                //




                // ループ（全体）
                while (true)
                {
#if DEBUG_STOPPABLE
            MessageBox.Show("きふわらべのMainの無限ループでブレイク☆！", "デバッグ");
            System.Diagnostics.Debugger.Break();
#endif
                    // ループ（１つ目）
                    while (true)
                    {
                        // 将棋サーバーから何かメッセージが届いていないか、見てみます。
                        string line = Util_Message.Download_BlockingIO();
                        Logger.WriteLineR(line);

                        if ("usi" == line)
                        {
                            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                            var engineName = $"{toml.Get<TomlTable>("Engine").Get<string>("Name")} { version.Major}.{ version.Minor}.{ version.Build}";
                            var engineAuthor = toml.Get<TomlTable>("Engine").Get<string>("Author");
                            playing.UsiOk(engineName, engineAuthor);
                        }
                        else if (line.StartsWith("setoption"))
                        {
                            Regex regex = new Regex(@"setoption name ([^ ]+)(?: value (.*))?", RegexOptions.Singleline);
                            Match m = regex.Match(line);

                            if (m.Success)
                            {
                                string name = (string)m.Groups[1].Value;
                                string value = "";

                                if (3 <= m.Groups.Count)
                                {
                                    // 「value ★」も省略されずにありました。
                                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                    value = (string)m.Groups[2].Value;
                                }

                                playing.SetOption(name, value);
                            }
                        }
                        else if ("isready" == line)
                        {
                            playing.ReadyOk();
                        }
                        else if ("usinewgame" == line)
                        {
                            playing.UsiNewGame();

                            // 無限ループ（１つ目）を抜けます。無限ループ（２つ目）に進みます。
                            break;
                        }
                        else if ("quit" == line)
                        {
                            playing.Quit();

                            // このプログラムを終了します。
                            goto end_usi;
                        }
                        else
                        {
                            //------------------------------------------------------------
                            // ○△□×！？
                            //------------------------------------------------------------
                            //
                            // ／(＾×＾)＼
                            //

                            // 通信が届いていますが、このプログラムでは  聞かなかったことにします。
                            // USIプロトコルの独習を進め、対応／未対応を選んでください。
                        }
                    }

                    // ループ（２つ目）
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


                    //PerformanceMetrics performanceMetrics = new PerformanceMetrics();//使ってない？

                    while (true)
                    {
                        // 将棋サーバーから何かメッセージが届いていないか、見てみます。
                        string line = Util_Message.Download_BlockingIO();
                        Logger.WriteLineR(line);

                        if (line.StartsWith("position"))
                        {
                            // 手番になったときに、“まず”、将棋所から送られてくる文字が position です。
                            // このメッセージを読むと、駒の配置が分かります。
                            //
                            // “が”、まだ指してはいけません。
                            Logger.Trace("（＾△＾）positionきたｺﾚ！");

                            // 入力行を解析します。
                            KifuParserA_Result result = new KifuParserA_ResultImpl();
                            var roomViewModel = new DefaultRoomViewModel(playing.Game.Kifu);
                            var genjo = new KifuParserA_GenjoImpl(line);
                            var parser = new KifuParserA_Impl();
                            Logger.Trace($@"┏━━━━━━━━━━┓
わたしは　{parser.State.GetType().Name}　の　Execute_All　だぜ☆");

                            KifuParserA_State nextState = parser.State;

                            while (!genjo.ToBreak)
                            {
                                genjo.InputLine = parser.State.Execute(
                                    ref result,
                                    roomViewModel,
                                    out nextState, parser,
                                    genjo);
                                parser.State = nextState;
                            }


                            KifuNode kifuNode = (KifuNode)result.Out_newNode_OrNull;
                            int tesumi_yomiGenTeban_forLog = 0;//ログ用。読み進めている現在の手目済

                            Logger.Trace(
                                Util_Sky.Json_1Sky(playing.Game.Kifu.CurNode.Value.ToKyokumenConst, "現局面になっているのかなんだぜ☆？　line=[" + line + "]　棋譜＝" + KirokuGakari.ToJapaneseKifuText(playing.Game.Kifu),
                                "PgCS",
                                tesumi_yomiGenTeban_forLog//読み進めている現在の手目
                                ));

                            //
                            // 局面画像ﾛｸﾞ
                            //
                            if(kifuNode!=null) // (2020-12-20 sun) なんでヌルになるのか分からないが、避けるぜ☆（＾～＾）
                            {
                                //SFEN文字列と、出力ファイル名を指定することで、局面の画像ログを出力します。
                                var ky = kifuNode.ToRO_Kyokumen1();
                                var fullname = SpecifyLogFiles.LatestPositionLogPng.Name;
                                var env = ShogisasiImpl.ReportEnvironment;
                                KyokumenPngWriterImpl.Write1(ky, fullname, env);
                            }


                            //------------------------------------------------------------
                            // じっとがまん
                            //------------------------------------------------------------
                            //
                            // 応答は無用です。
                            // 多分、将棋所もまだ準備ができていないのではないでしょうか（？）
                            //
                            playing.Position();
                        }
                        else if (line.StartsWith("go ponder"))
                        {
                            playing.GoPonder();
                        }
                        // 「go ponder」「go mate」「go infinite」とは区別します。
                        else if (line.StartsWith("go"))
                        {
                            //------------------------------------------------------------
                            // あなたの手番です
                            //------------------------------------------------------------
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

                            // ｎ手目を 2 増やします。
                            // 相手の手番と、自分の手番の 2つが増えた、という数え方です。
                            playing.Game.TesumiCount += 2;

                            //------------------------------------------------------------
                            // 先手 3:00  後手 0:00  記録係「50秒ぉ～」
                            //------------------------------------------------------------
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
                            Regex regex = new Regex(@"go btime (\d+) wtime (\d+) byoyomi (\d+)", RegexOptions.Singleline);
                            Match m = regex.Match(line);

                            if (m.Success)
                            {
                                playing.Go((string)m.Groups[1].Value, (string)m.Groups[2].Value, (string)m.Groups[3].Value, "", "");
                            }
                            else
                            {
                                // (2020-12-16 wed) フィッシャー・クロック・ルールに対応☆（＾～＾）
                                regex = new Regex(@"go btime (\d+) wtime (\d+) binc (\d+) winc (\d+)", RegexOptions.Singleline);
                                m = regex.Match(line);

                                playing.Go((string)m.Groups[1].Value, (string)m.Groups[2].Value, "", (string)m.Groups[3].Value, (string)m.Groups[4].Value);
                            }
                            //System.C onsole.WriteLine();

                            //throw new Exception("デバッグだぜ☆！　エラーはキャッチできたかな～☆？（＾▽＾）");
                        }
                        else if (line.StartsWith("stop"))
                        {
                            playing.Stop();
                        }
                        else if (line.StartsWith("gameover"))
                        {
                            Regex regex = new Regex(@"gameover (.)", RegexOptions.Singleline);
                            Match m = regex.Match(line);

                            if (m.Success)
                            {
                                playing.GameOver((string)m.Groups[1].Value);
                            }

                            // 無限ループ（２つ目）を抜けます。無限ループ（１つ目）に戻ります。
                            break;
                        }
                        // 独自コマンド「ログ出せ」
                        else if ("logdase" == line)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("ログだせ～（＾▽＾）");

                            playing.Game.Kifu.ForeachZenpuku(
                                playing.Game.Kifu.GetRoot(), (int tesumi, KyokumenWrapper sky, Node<ShootingStarlightable, KyokumenWrapper> node, ref bool toBreak) =>
                                {
                                    //sb.AppendLine("(^-^)");

                                    if (null != node)
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
                        else
                        {
                            //------------------------------------------------------------
                            // ○△□×！？
                            //------------------------------------------------------------
                            //
                            // ／(＾×＾)＼
                            //

                            // 通信が届いていますが、このプログラムでは  聞かなかったことにします。
                            // USIプロトコルの独習を進め、対応／未対応を選んでください。
                        }
                    }

                    //-------------------+----------------------------------------------------------------------------------------------------
                    // スナップショット  |
                    //-------------------+----------------------------------------------------------------------------------------------------
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
                    Logger.Trace("KifuParserA_Impl.LOGGING_BY_ENGINE, ┏━確認━━━━setoptionDictionary ━┓");
                    foreach (KeyValuePair<string, string> pair in playing.SetoptionDictionary)
                    {
                        Logger.Trace(pair.Key + "=" + pair.Value);
                    }
                    Logger.Trace("┗━━━━━━━━━━━━━━━━━━┛");

                    //Dictionary<string, string> goMateProperties = new Dictionary<string, string>();
                    //goMateProperties["mate"] = "";
                    //LarabeLoggerList_Warabe.ENGINE.WriteLine_AddMemo("┏━確認━━━━goMateDictionary━━━┓");
                    //foreach (KeyValuePair<string, string> pair in this.goMateProperties)
                    //{
                    //    LarabeLoggerList_Warabe.ENGINE.WriteLine_AddMemo(pair.Key + "=" + pair.Value);
                    //}

                    Logger.Trace("┗━━━━━━━━━━━━━━━━━━┛");
                }

            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                Logger.Fatal($"(^ー^)「大外枠でキャッチ」{ex}");
                Playing.Send("bestmove resign");
            }

        end_usi:
            // 終了時に、妄想履歴のログを残します。
            playing.shogisasi.Kokoro.WriteTenonagare(playing.shogisasi);
        }
    }
}
