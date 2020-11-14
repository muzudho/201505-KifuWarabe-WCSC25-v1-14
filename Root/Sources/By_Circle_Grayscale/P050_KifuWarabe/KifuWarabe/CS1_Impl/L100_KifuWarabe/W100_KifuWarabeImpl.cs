using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P045_Atama.L00025_KyHandan;
using Grayscale.P050_KifuWarabe.L00025_UsiLoop;
using Grayscale.P050_KifuWarabe.L030_Shogisasi;
using Grayscale.P050_KifuWarabe.L050_UsiLoop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Grayscale.P050_KifuWarabe.L003_Kokoro;
using Grayscale.P050_KifuWarabe.L00052_Shogisasi;

namespace Grayscale.P050_KifuWarabe.L100_KifuWarabe
{

    public class W100_KifuWarabeImpl : ShogiEngine
    {

        #region ロガー
        /// <summary>
        /// ログ。将棋エンジンきふわらべで汎用に使います。
        /// </summary>
        private static readonly LarabeLoggerable ENGINE = new LarabeLoggerImpl("../../Logs/_log_将棋ｴﾝｼﾞﾝ_汎用", ".txt", true, false);
        public LarabeLoggerable Log_Engine
        {
            get
            {
                return W100_KifuWarabeImpl.ENGINE;
            }
        }

        /// <summary>
        /// ログ。送受信内容の記録専用です。
        /// </summary>
        private static readonly LarabeLoggerable CLIENT = new LarabeLoggerImpl("../../Logs/_log_将棋ｴﾝｼﾞﾝ_ｸﾗｲｱﾝﾄ", ".txt", true, false);
        public LarabeLoggerable Log_Client
        {
            get
            {
                return W100_KifuWarabeImpl.CLIENT;
            }
        }

        /// <summary>
        /// ログ。思考ルーチン専用です。
        /// </summary>
        private static readonly LarabeLoggerable MOUSOU_RIREKI = new LarabeLoggerImpl("../../Logs/_log_将棋ｴﾝｼﾞﾝ_妄想履歴", ".txt", true, false);
        public LarabeLoggerable Log_MousouRireki
        {
            get
            {
                return W100_KifuWarabeImpl.MOUSOU_RIREKI;
            }
        }
        #endregion

        #region プロパティー
        /// <summary>
        /// きふわらべの作者名です。
        /// </summary>
        public string AuthorName { get { return this.authorName; } }
        private string authorName;


        /// <summary>
        /// 製品名です。
        /// </summary>
        public string SeihinName { get { return this.seihinName; } }
        private string seihinName;


        /// <summary>
        /// USI「setoption」コマンドのリストです。
        /// </summary>
        public Dictionary<string, string> SetoptionDictionary { get; set; }


        /// <summary>
        /// 将棋エンジンの中の一大要素「思考エンジン」です。
        /// 指す１手の答えを出すのが仕事です。
        /// </summary>
        private Shogisasi shogisasi;


        public PlayerInfo PlayerInfo { get { return this.playerInfo; } }
        private PlayerInfo playerInfo;
        #endregion

        #region 送信
        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="line">メッセージ</param>
        public void Send(string line)
        {
            // 将棋サーバーに向かってメッセージを送り出します。
            Util_Message.Upload(line);

            // 送信記録をつけます。
            this.Log_Client.WriteLine_S(line);
        }
        #endregion

        #region コンストラクター
        /// <summary>
        /// コンストラクター
        /// </summary>
        public W100_KifuWarabeImpl()
        {
            // 作者名
            this.authorName = "TAKAHASHI Satoshi"; // むずでょ

            // 製品名
            this.seihinName = ((System.Reflection.AssemblyProductAttribute)Attribute.GetCustomAttribute(System.Reflection.Assembly.GetExecutingAssembly(), typeof(System.Reflection.AssemblyProductAttribute))).Product;

            //-------------+----------------------------------------------------------------------------------------------------------
            // データ設計  |
            //-------------+----------------------------------------------------------------------------------------------------------
            // 将棋所から送られてくるデータを、一覧表に変えたものです。
            this.SetoptionDictionary = new Dictionary<string, string>(); // 不定形

            this.playerInfo = new PlayerInfoImpl();
        }
        #endregion

        #region 処理の流れ
        public void AtBegin()
        {
            // 思考エンジンの、記憶を読み取ります。
            this.shogisasi = new ShogisasiImpl(this);
            this.shogisasi.Kokoro.ReadTenonagare();
        }

        public void AtLoop()
        {
            try
            {

                #region ↑詳説
                // 
                // 図.
                // 
                //     プログラムの開始：  ここの先頭行から始まります。
                //     プログラムの実行：  この中で、ずっと無限ループし続けています。
                //     プログラムの終了：  この中の最終行を終えたとき、
                //                         または途中で Environment.Exit(0); が呼ばれたときに終わります。
                //                         また、コンソールウィンドウの[×]ボタンを押して強制終了されたときも  ぶつ切り  で突然終わります。
                #endregion

                //------+-----------------------------------------------------------------------------------------------------------------
                // 準備 |
                //------+-----------------------------------------------------------------------------------------------------------------

                // データの読取「道」
                Michi187Array.Load("../../Data/data_michi187.csv");

                // データの読取「配役」
                Util_Haiyaku184Array.Load("../../Data/data_haiyaku185_UTF-8.csv", Encoding.UTF8);

                // データの読取「強制転成表」　※駒配役を生成した後で。
                ForcePromotionArray.Load("../../Data/data_forcePromotion_UTF-8.csv", Encoding.UTF8);
                File.WriteAllText("../../Logs/_log_強制転成表.html", ForcePromotionArray.LogHtml());

                // データの読取「配役転換表」
                Data_HaiyakuTransition.Load("../../Data/data_syuruiToHaiyaku.csv", Encoding.UTF8);
                File.WriteAllText("../../Logs/_log_配役転換表.html", Data_HaiyakuTransition.LogHtml());



                //-------------------+----------------------------------------------------------------------------------------------------
                // ログファイル削除  |
                //-------------------+----------------------------------------------------------------------------------------------------
                #region ↓詳説
                //
                // 図.
                //
                //      フォルダー
                //          ├─ Engine.KifuWarabe.exe
                //          └─ log.txt               ←これを削除
                //
                #endregion
                LarabeLoggerList.GetDefaultList().RemoveFile();


                //-------------+----------------------------------------------------------------------------------------------------------
                // ログ書込み  |  ＜この将棋エンジン＞  製品名、バージョン番号
                //-------------+----------------------------------------------------------------------------------------------------------
                #region ↓詳説
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
                #endregion
                string versionStr;
                {

                    // バージョン番号
                    Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                    versionStr = String.Format("{0}.{1}.{2}", version.Major, version.Minor.ToString("00"), version.Build);

                    //seihinName += " " + versionStr;
                }
                this.Log_Engine.WriteLine_AddMemo("v(^▽^)v ｲｪｰｲ☆ ... " + this.SeihinName + " " + versionStr);


                //-----------+------------------------------------------------------------------------------------------------------------
                // 通信開始  |
                //-----------+------------------------------------------------------------------------------------------------------------
                #region ↓詳説
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
                #endregion




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
                    UsiLoop1 usiLoop1 = new UsiLoop1(this);
                    usiLoop1.AtStart();
                    Result_UsiLoop1 result_UsiLoop1 = usiLoop1.AtLoop();
                    usiLoop1.AtEnd();

                    if (result_UsiLoop1 == Result_UsiLoop1.Quit)
                    {
                        break;//全体ループを抜けます。
                    }

                    //************************************************************************************************************************
                    // ループ（２つ目）
                    //************************************************************************************************************************
                    UsiLoop2 usiLoop2 = new UsiLoop2(this.shogisasi, this);
                    usiLoop2.AtBegin();
                    usiLoop2.AtLoop();
                    usiLoop2.AtEnd();
                }

            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                this.Log_Engine.WriteLine_AddMemo("Program「大外枠でキャッチ」：" + ex.GetType().Name + " " + ex.Message);
            }
        }

        public void AtEnd()
        {
            //
            // 終了時に、妄想履歴のログを残します。
            //
            this.shogisasi.Kokoro.WriteTenonagare(this.shogisasi, this.Log_Engine);
        }
        #endregion

    }
}
