using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using System.Collections.Generic;
//スプライト番号

namespace Grayscale.P045_Atama.L000125_Sokutei
{
    public interface KomanoKiki
    {
        /// <summary>
        /// 枡毎の、利き数。
        /// </summary>
        Dictionary<int, int> Kikisu_AtMasu_Mikata { get; set; }
        Dictionary<int, int> Kikisu_AtMasu_Teki { get; set; }

    }
}
