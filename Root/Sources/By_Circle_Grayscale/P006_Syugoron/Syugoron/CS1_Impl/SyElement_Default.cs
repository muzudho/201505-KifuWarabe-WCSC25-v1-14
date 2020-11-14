using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.P006_Syugoron
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
