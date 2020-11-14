using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L00060_KifuParser;
using Grayscale.P025_KifuLarabe.L012_Common;

namespace Grayscale.P025_KifuLarabe.L100_KifuIO
{
    public class KifuParserA_GenjoImpl : KifuParserA_Genjo
    {

        public string InputLine { get; set; }
        public bool ToBreak { get; set; }


        public KifuParserA_GenjoImpl(string inputLine)
        {
            this.InputLine = inputLine;
            //this.ToBreak = toBreak;
        }

    }
}
