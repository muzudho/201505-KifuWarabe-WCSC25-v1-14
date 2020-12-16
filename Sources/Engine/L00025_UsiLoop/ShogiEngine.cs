using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P045_Atama.L00025_KyHandan;
using System.Collections.Generic;

namespace Grayscale.P050_KifuWarabe.L00025_UsiLoop
{
    public interface ShogiEngine
    {
        /// <summary>
        /// 将棋エンジンきふわらべの汎用ログ。
        /// </summary>
        LarabeLoggerable Log_Engine { get; }

        /// <summary>
        /// 通信内容用のログ。
        /// </summary>
        LarabeLoggerable Log_Client { get; }

        /// <summary>
        /// 思考内容（妄想履歴）用ログ。
        /// </summary>
        LarabeLoggerable Log_MousouRireki { get; }

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="line">メッセージ</param>
        void Send(string line);

        /// <summary>
        /// USI「setoption」コマンドのリストです。
        /// </summary>
        Dictionary<string, string> SetoptionDictionary { get; set; }


        PlayerInfo PlayerInfo { get; }

    }
}
