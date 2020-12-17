using System.Collections.Generic;
using Grayscale.P045_Atama.L00025_KyHandan;

namespace Grayscale.P050_KifuWarabe.L00025_UsiLoop
{
    public interface ShogiEngine
    {
        /// <summary>
        /// USI「setoption」コマンドのリストです。
        /// </summary>
        Dictionary<string, string> SetoptionDictionary { get; set; }


        PlayerInfo PlayerInfo { get; }
    }
}
