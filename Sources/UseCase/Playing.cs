using Grayscale.Kifuwarazusa.Entities;
using Grayscale.P025_KifuLarabe.L00012_Atom;

namespace Grayscale.Kifuwarazusa.UseCases
{
    public class Playing
    {
        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="line">メッセージ</param>
        public static void Send(string line)
        {
            // 将棋サーバーに向かってメッセージを送り出します。
            Util_Message.Upload(line);

            // 送信記録をつけます。
            Logger.WriteLineS(LogTags.Client, line);
        }

        public void UsiOk(string engineName, string engineAuthor)
        {

        }
    }
}
