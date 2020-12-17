using Grayscale.Kifuwarazusa.Entities;

namespace Grayscale.P025_KifuLarabe.L00025_Struct
{
    public interface LarabeLoggerable
    {
        LogRecord LogRecord { get; }

        /// <summary>
        /// メモを、ログ・ファイルの末尾に追記します。
        /// </summary>
        /// <param name="line"></param>
        void WriteLine_AddMemo(string line);

                /// <summary>
        /// エラーを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        void WriteLine_Error(string line);



        /// <summary>
        /// メモを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        void WriteLine_OverMemo(string line);



        /// <summary>
        /// サーバーへ送ったコマンドを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        void WriteLine_S(string line);


        /// <summary>
        /// サーバーから受け取ったコマンドを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        void WriteLine_R(string line);

    }
}
