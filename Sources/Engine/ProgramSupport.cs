namespace Grayscale.P050_KifuWarabe.L100_KifuWarabe
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Grayscale.P025_KifuLarabe.L00012_Atom;
    using Grayscale.P025_KifuLarabe.L00025_Struct;
    using Grayscale.P025_KifuLarabe.L004_StructShogi;
    using Grayscale.P025_KifuLarabe.L012_Common;
    using Grayscale.P045_Atama.L00025_KyHandan;
    using Grayscale.P050_KifuWarabe.L00025_UsiLoop;
    using Grayscale.P050_KifuWarabe.L00052_Shogisasi;
    using Grayscale.P050_KifuWarabe.L003_Kokoro;
    using Grayscale.P050_KifuWarabe.L030_Shogisasi;
    using Grayscale.P050_KifuWarabe.L050_UsiLoop;
    using Nett;

    public class ProgramSupport : ShogiEngine
    {
        /// <summary>
        /// ログ。将棋エンジンきふわらべで汎用に使います。
        /// </summary>
        private static readonly LarabeLoggerable ENGINE = new LarabeLoggerImpl("../../Logs/_log_将棋ｴﾝｼﾞﾝ_汎用", ".txt", true, false);
        public LarabeLoggerable Log_Engine
        {
            get
            {
                return ProgramSupport.ENGINE;
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
                return ProgramSupport.CLIENT;
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
                return ProgramSupport.MOUSOU_RIREKI;
            }
        }

        /// <summary>
        /// USI「setoption」コマンドのリストです。
        /// </summary>
        public Dictionary<string, string> SetoptionDictionary { get; set; }


        /// <summary>
        /// 将棋エンジンの中の一大要素「思考エンジン」です。
        /// 指す１手の答えを出すのが仕事です。
        /// </summary>
        public Shogisasi shogisasi;


        public PlayerInfo PlayerInfo { get { return this.playerInfo; } }
        private PlayerInfo playerInfo;

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

        /// <summary>
        /// コンストラクター
        /// </summary>
        public ProgramSupport()
        {
            //-------------+----------------------------------------------------------------------------------------------------------
            // データ設計  |
            //-------------+----------------------------------------------------------------------------------------------------------
            // 将棋所から送られてくるデータを、一覧表に変えたものです。
            this.SetoptionDictionary = new Dictionary<string, string>(); // 不定形

            this.playerInfo = new PlayerInfoImpl();
        }

        public void AtBegin()
        {
            // 思考エンジンの、記憶を読み取ります。
            this.shogisasi = new ShogisasiImpl(this);
            this.shogisasi.Kokoro.ReadTenonagare();
        }

        public void AtEnd()
        {
            //
            // 終了時に、妄想履歴のログを残します。
            //
            this.shogisasi.Kokoro.WriteTenonagare(this.shogisasi, this.Log_Engine);
        }

    }
}
