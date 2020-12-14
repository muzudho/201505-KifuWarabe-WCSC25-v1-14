
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P400_KifuNaraVs.L00048_Engine;


namespace Grayscale.P400_KifuNaraVs.L025_ShogiEngine
{
    /// <summary>
    /// 将棋エンジンの、マン・インターフェースです。
    /// プロセスをラッピングしています。
    /// </summary>
    public class ShogiEngineManInterfaceImpl : ShogiEngineManInterface
    {

        #region プロパティ類

        public ShogiServerMessenger ShogiServerMessenger { get; set; }

        #endregion


        public ShogiEngineManInterfaceImpl()
        {
            this.ShogiServerMessenger = new ShogiServerMessenger();

            this.ShogiServerMessenger.Delegate_ShogiServer_ToEngine = (string line) =>
            {
                // デフォルトでは何もしません。
            };
        }


        /// <summary>
        /// 将棋エンジンが起動しているか否かです。
        /// </summary>
        /// <returns></returns>
        public bool IsLive()
        {
            return this.ShogiServerMessenger.IsLive_ShogiEngine();
        }


        /// <summary>
        /// 将棋エンジンに、"position ～略～"を送信します。
        /// </summary>
        public void Position(string position)
        {
            // 将棋エンジンの標準入力へ、メッセージを送ります。
            this.ShogiServerMessenger.Download(position);
        }


        /// <summary>
        /// 将棋エンジンに、"setoption ～略～"を送信します。
        /// </summary>
        public void Setoption(string setoption)
        {
            // 将棋エンジンの標準入力へ、メッセージを送ります。
            this.ShogiServerMessenger.Download(setoption);
        }


        /// <summary>
        /// 将棋エンジンに、"usi"を送信します。
        /// </summary>
        public void Usi()
        {
            // 将棋エンジンの標準入力へ、メッセージを送ります。
            this.ShogiServerMessenger.Download("usi");
        }

        /// <summary>
        /// 将棋エンジンに、"isready"を送信します。
        /// </summary>
        public void Isready()
        {
            this.ShogiServerMessenger.Download("isready");
        }

        /// <summary>
        /// 将棋エンジンに、"usinewgame"を送信します。
        /// </summary>
        public void Usinewgame()
        {
            // 将棋エンジンの標準入力へ、メッセージを送ります。
            this.ShogiServerMessenger.Download("usinewgame");
        }

        /// <summary>
        /// 将棋エンジンに、"gameover lose"を送信します。
        /// </summary>
        public void Gameover_lose()
        {
            // 将棋エンジンの標準入力へ、メッセージを送ります。
            this.ShogiServerMessenger.Download("gameover lose");
        }

        /// <summary>
        /// 将棋エンジンに、"quit"を送信します。
        /// </summary>
        public void Quit()
        {
            // 将棋エンジンの標準入力へ、メッセージを送ります。
            this.ShogiServerMessenger.Download("quit");
        }

        /// <summary>
        /// 将棋エンジンに、"go"を送信します。
        /// </summary>
        public void Go()
        {
            // 将棋エンジンの標準入力へ、メッセージを送ります。
            this.ShogiServerMessenger.Download("go");
        }

        /// <summary>
        /// 将棋エンジンに、終了するように促します。
        /// </summary>
        public void Shutdown()
        {
            if (this.IsLive())
            {
                // 将棋エンジンの標準入力へ、メッセージを送ります。
                this.ShogiServerMessenger.Download("quit");
            }
        }

        /// <summary>
        /// 将棋エンジンに、ログを出すように促します。
        /// </summary>
        public void Logdase()
        {
            if (this.IsLive())
            {
                // 将棋エンジンの標準入力へ、メッセージを送ります。
                this.ShogiServerMessenger.Download("logdase");
            }
        }

    }
}
