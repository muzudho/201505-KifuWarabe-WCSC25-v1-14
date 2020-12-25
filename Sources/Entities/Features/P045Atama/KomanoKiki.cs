using System.Collections.Generic;
//スプライト番号

namespace Grayscale.Kifuwarazusa.Entities.Features
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
