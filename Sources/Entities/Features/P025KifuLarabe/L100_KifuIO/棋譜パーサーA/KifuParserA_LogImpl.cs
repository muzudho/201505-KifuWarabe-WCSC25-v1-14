using Grayscale.P025_KifuLarabe.L00060_KifuParser;

namespace Grayscale.P025_KifuLarabe.L100_KifuIO
{
    public class KifuParserA_LogImpl : KifuParserA_Log
    {
        public string Hint { get { return this.hint; } }
        private string hint;

        public KifuParserA_LogImpl(  string hint)
        {
            this.hint = hint;
        }

    }
}
