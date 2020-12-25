namespace Grayscale.Kifuwarazusa.Entities.Features
{

    public class SyElement_Default : SyElement
    {

        public string Word { get { return this.word; } }
        private string word;

        public SyElement_Default(string word)
        {
            this.word = word;
        }

    }
}
