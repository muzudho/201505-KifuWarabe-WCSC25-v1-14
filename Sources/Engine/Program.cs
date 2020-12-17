namespace Grayscale.P050_KifuWarabe
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Grayscale.P025_KifuLarabe.L00012_Atom;
    using Grayscale.P025_KifuLarabe.L00025_Struct;
    using Grayscale.P025_KifuLarabe.L004_StructShogi;
    using Grayscale.P025_KifuLarabe.L012_Common;
    using Grayscale.P050_KifuWarabe.L030_Shogisasi;
    using Grayscale.P050_KifuWarabe.L050_UsiLoop;
    using Grayscale.Kifuwarazusa.UseCases;
    using Nett;
    using Grayscale.Kifuwarazusa.Entities;

    /// <summary>
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
            // 将棋エンジン　きふわらべ
            ProgramSupport programSupport = new ProgramSupport();
            // 思考エンジンの、記憶を読み取ります。
            programSupport.shogisasi = new ShogisasiImpl(programSupport);
            programSupport.shogisasi.Kokoro.ReadTenonagare();

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
                LarabeLoggerList.GetDefaultList().RemoveFile();


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
                Logger.Log_Engine.WriteLine_AddMemo($"v(^▽^)v ｲｪｰｲ☆ ... {engineName} {versionStr}");


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




                //************************************************************************************************************************
                // ループ（全体）
                //************************************************************************************************************************
                while (true)
                {
#if DEBUG_STOPPABLE
            MessageBox.Show("きふわらべのMainの無限ループでブレイク☆！", "デバッグ");
            System.Diagnostics.Debugger.Break();
#endif
                    //************************************************************************************************************************
                    // ループ（１つ目）
                    //************************************************************************************************************************
                    UsiLoop1 usiLoop1 = new UsiLoop1(programSupport);
                    Result_UsiLoop1 result_UsiLoop1;

                    while (true)
                    {
                        result_UsiLoop1 = Result_UsiLoop1.None;

                        // 将棋サーバーから何かメッセージが届いていないか、見てみます。
                        string line = Util_Message.Download_BlockingIO();
                        Logger.Log_Client.WriteLine_AddMemo(line);
                        LarabeLoggerList.GetDefaultList().DefaultFile.WriteLine_R(line);


                        if ("usi" == line) { usiLoop1.AtLoop_OnUsi(line, ref result_UsiLoop1); }
                        else if (line.StartsWith("setoption")) { usiLoop1.AtLoop_OnSetoption(line, ref result_UsiLoop1); }
                        else if ("isready" == line) { usiLoop1.AtLoop_OnIsready(line, ref result_UsiLoop1); }
                        else if ("usinewgame" == line) { usiLoop1.AtLoop_OnUsinewgame(line, ref result_UsiLoop1); }
                        else if ("quit" == line) { usiLoop1.AtLoop_OnQuit(line, ref result_UsiLoop1); }
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
                            //
                            // ログだけ取って、スルーします。
                        }

                        switch (result_UsiLoop1)
                        {
                            case Result_UsiLoop1.Break:
                                goto end_loop1;

                            case Result_UsiLoop1.Quit:
                                goto end_loop1;

                            default:
                                break;
                        }

                    //gt_NextTime1:
                    //    ;
                    }
                end_loop1:

                    if (result_UsiLoop1 == Result_UsiLoop1.Quit)
                    {
                        break;//全体ループを抜けます。
                    }

                    //************************************************************************************************************************
                    // ループ（２つ目）
                    //************************************************************************************************************************
                    UsiLoop2 usiLoop2 = new UsiLoop2(programSupport.shogisasi, programSupport);
                    programSupport.shogisasi.OnTaikyokuKaisi();//対局開始時の処理。

                    //PerformanceMetrics performanceMetrics = new PerformanceMetrics();//使ってない？

                    while (true)
                    {
                        Result_UsiLoop2 result_Usi = Result_UsiLoop2.None;

                        // 将棋サーバーから何かメッセージが届いていないか、見てみます。
                        string line = Util_Message.Download_BlockingIO();
                        Logger.Log_Client.WriteLine_AddMemo(line);
                        LarabeLoggerList.GetDefaultList().DefaultFile.WriteLine_R(line);


                        if (line.StartsWith("position")) {
                            usiLoop2.AtLoop_OnPosition(line, ref result_Usi);
                        }
                        else if (line.StartsWith("go ponder")) { usiLoop2.AtLoop_OnGoponder(line, ref result_Usi); }
                        else if (line.StartsWith("go")) { usiLoop2.AtLoop_OnGo(line, ref result_Usi); }// 「go ponder」「go mate」「go infinite」とは区別します。
                        else if (line.StartsWith("stop")) { usiLoop2.AtLoop_OnStop(line, ref result_Usi); }
                        else if (line.StartsWith("gameover")) { usiLoop2.AtLoop_OnGameover(line, ref result_Usi); }
                        else if ("logdase" == line) { usiLoop2.AtLoop_OnLogdase(line, ref result_Usi); }
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
                            //
                            // ログだけ取って、スルーします。
                        }

                        switch (result_Usi)
                        {
                            case Result_UsiLoop2.Break:
                                goto end_loop2;

                            default:
                                break;
                        }

                    //gt_NextTime2:
                    //    ;
                    }
                end_loop2:
                    ;

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
                    Logger.Log_Engine.WriteLine_AddMemo("KifuParserA_Impl.LOGGING_BY_ENGINE, ┏━確認━━━━setoptionDictionary ━┓");
                    foreach (KeyValuePair<string, string> pair in usiLoop2.owner.SetoptionDictionary)
                    {
                        Logger.Log_Engine.WriteLine_AddMemo(pair.Key + "=" + pair.Value);
                    }
                    Logger.Log_Engine.WriteLine_AddMemo("┗━━━━━━━━━━━━━━━━━━┛");
                    Logger.Log_Engine.WriteLine_AddMemo("┏━確認━━━━goDictionary━━━━━┓");
                    foreach (KeyValuePair<string, string> pair in usiLoop2.GoProperties)
                    {
                        Logger.Log_Engine.WriteLine_AddMemo(pair.Key + "=" + pair.Value);
                    }

                    //Dictionary<string, string> goMateProperties = new Dictionary<string, string>();
                    //goMateProperties["mate"] = "";
                    //LarabeLoggerList_Warabe.ENGINE.WriteLine_AddMemo("┗━━━━━━━━━━━━━━━━━━┛");
                    //LarabeLoggerList_Warabe.ENGINE.WriteLine_AddMemo("┏━確認━━━━goMateDictionary━━━┓");
                    //foreach (KeyValuePair<string, string> pair in this.goMateProperties)
                    //{
                    //    LarabeLoggerList_Warabe.ENGINE.WriteLine_AddMemo(pair.Key + "=" + pair.Value);
                    //}

                    Logger.Log_Engine.WriteLine_AddMemo("┗━━━━━━━━━━━━━━━━━━┛");
                    Logger.Log_Engine.WriteLine_AddMemo("┏━確認━━━━gameoverDictionary━━┓");
                    foreach (KeyValuePair<string, string> pair in usiLoop2.GameoverProperties)
                    {
                        Logger.Log_Engine.WriteLine_AddMemo(pair.Key + "=" + pair.Value);
                    }
                    Logger.Log_Engine.WriteLine_AddMemo("┗━━━━━━━━━━━━━━━━━━┛");
                }

            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                Logger.Log_Engine.WriteLine_AddMemo("Program「大外枠でキャッチ」：" + ex.GetType().Name + " " + ex.Message);
            }

            // 終了時に、妄想履歴のログを残します。
            programSupport.shogisasi.Kokoro.WriteTenonagare(programSupport.shogisasi, Logger.Log_Engine);
        }
    }
}
