using Grayscale.P050_KifuWarabe.L100_KifuWarabe;

namespace Grayscale.P050_KifuWarabe
{
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
            W100_KifuWarabeImpl kifuWarabe = new W100_KifuWarabeImpl();
            kifuWarabe.AtBegin();
            kifuWarabe.AtLoop();    // 将棋サーバーからのメッセージの受信や、
                                    // 思考は、ここで行っています。
            kifuWarabe.AtEnd();
        }
    }
}
