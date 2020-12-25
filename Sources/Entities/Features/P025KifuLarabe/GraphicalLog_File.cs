using System.Collections.Generic;

namespace Grayscale.Kifuwarazusa.Entities.Features
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
