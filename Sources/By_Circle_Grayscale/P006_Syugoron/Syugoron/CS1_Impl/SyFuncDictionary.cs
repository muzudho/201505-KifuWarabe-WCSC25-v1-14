using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace Grayscale.P006_Syugoron
{
    public class SyFuncDictionary
    {
        public delegate string Delegate_Func(string prm1);

        private Dictionary<string, SyFuncDictionary.Delegate_Func> dictionary;

        public void Add(string word, SyFuncDictionary.Delegate_Func func)
        {
            this.dictionary.Add(word.Trim(), func);
        }

        public bool ContainsKey(string word)
        {
            bool result = false;

            if (this.dictionary.ContainsKey(word.Trim()))
            {
                result = true;
            }

            return result;
        }

        public SyFuncDictionary.Delegate_Func GetFunc(string word)
        {
            SyFuncDictionary.Delegate_Func func = null;// Sample_Func.funcφ;

            if (this.dictionary.ContainsKey(word.Trim()))
            {
                func = this.dictionary[word.Trim()];
            }

            return func;
        }


        public SyFuncDictionary()
        {
            this.dictionary = new Dictionary<string, Delegate_Func>();
        }


    }
}
