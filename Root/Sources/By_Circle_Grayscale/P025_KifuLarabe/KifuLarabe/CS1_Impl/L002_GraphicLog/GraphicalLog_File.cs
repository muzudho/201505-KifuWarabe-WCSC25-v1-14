using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.P025_KifuLarabe.L002_GraphicLog
{

    /// <summary>
    /// 複数の盤をもつログ・ファイルです。
    /// </summary>
    public class GraphicalLog_File
    {

        public List<GraphicalLog_Board> boards { get; set; }

        public GraphicalLog_File()
        {
            this.boards = new List<GraphicalLog_Board>();
        }

    }
}
