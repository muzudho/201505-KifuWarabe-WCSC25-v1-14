namespace Grayscale.P006_Sfen.L100_Sfen
{

    /// <summary>
    /// SFENのstartpos文字列を入れているという明示をします。
    /// </summary>
    public class SfenstringImpl
    {
        public string ValueStr { get { return this.valueStr; } }
        private string valueStr;

        public SfenstringImpl()
        {
            this.valueStr = "";
        }

        public SfenstringImpl(string src)
        {
            this.valueStr = src;
        }

        public override string ToString()
        {
            return this.ValueStr;
        }
    }
}
