namespace Grayscale.Kifuwarazusa.UseCases
{
    using System.Collections.Generic;
    using Grayscale.Kifuwarazusa.Entities;
    using Grayscale.P025_KifuLarabe.L00012_Atom;
    using Grayscale.P045_Atama.L00025_KyHandan;
    using Grayscale.P050_KifuWarabe.L00025_UsiLoop;
    using Grayscale.P050_KifuWarabe.L00052_Shogisasi;
    using Grayscale.P050_KifuWarabe.L003_Kokoro;

    public class ProgramSupport : ShogiEngine
    {
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
            Logger.Log_Client.WriteLine_S(line);
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
    }
}
