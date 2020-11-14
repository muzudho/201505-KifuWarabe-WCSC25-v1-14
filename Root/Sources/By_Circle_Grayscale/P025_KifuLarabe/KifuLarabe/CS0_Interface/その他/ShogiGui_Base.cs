using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.P025_KifuLarabe.L00025_Struct
{
    public interface ShogiGui_Base
    {

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 将棋の状況は全部この中です。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        Model_PnlTaikyoku Model_PnlTaikyoku { get; }


    }
}
