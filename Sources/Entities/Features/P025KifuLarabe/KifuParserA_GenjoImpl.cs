namespace Grayscale.Kifuwarazusa.Entities.Features
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
