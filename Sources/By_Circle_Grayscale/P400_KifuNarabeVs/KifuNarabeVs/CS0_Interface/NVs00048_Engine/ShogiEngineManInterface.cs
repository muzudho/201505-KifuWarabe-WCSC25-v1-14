
using Grayscale.P025_KifuLarabe.L00012_Atom;

namespace Grayscale.P400_KifuNaraVs.L00048_Engine
{
    public interface ShogiEngineManInterface
    {

        
        /// <summary>
        /// 将棋エンジンに、"usinewgame"を送信します。
        /// </summary>
        void Usinewgame();


        /// <summary>
        /// 将棋エンジンに、"usi"を送信します。
        /// </summary>
        void Usi();


                
        /// <summary>
        /// 将棋エンジンに、終了するように促します。
        /// </summary>
        void Shutdown();


        ShogiServerMessenger ShogiServerMessenger { get; set; }


        
        /// <summary>
        /// 将棋エンジンに、"setoption ～略～"を送信します。
        /// </summary>
        void Setoption(string setoption);


                
        /// <summary>
        /// 将棋エンジンに、"quit"を送信します。
        /// </summary>
        void Quit();


                
        /// <summary>
        /// 将棋エンジンに、"position ～略～"を送信します。
        /// </summary>
        void Position(string position);


        
        /// <summary>
        /// 将棋エンジンに、ログを出すように促します。
        /// </summary>
        void Logdase();


                
        /// <summary>
        /// 将棋エンジンに、"isready"を送信します。
        /// </summary>
        void Isready();


                
        /// <summary>
        /// 将棋エンジンが起動しているか否かです。
        /// </summary>
        /// <returns></returns>
        bool IsLive();



        /// <summary>
        /// 将棋エンジンに、"go"を送信します。
        /// </summary>
        void Go();



        /// <summary>
        /// 将棋エンジンに、"gameover lose"を送信します。
        /// </summary>
        void Gameover_lose();

    }
}
