using System.Collections.Generic;
using Grayscale.P045_Atama.L00025_KyHandan;

namespace Grayscale.P050_KifuWarabe.L00025_UsiLoop
{
    public interface ShogiEngine
    {
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
