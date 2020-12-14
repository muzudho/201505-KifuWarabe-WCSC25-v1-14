using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.P025_KifuLarabe.L100_KifuIO
{
    public class KifuReaderB_StateB0 : KifuReaderB_State
    {

        public void Execute(string inputLine, out string nextCommand, out string rest)
        {
            nextCommand = "";
            rest = inputLine;
        }

    }
}
