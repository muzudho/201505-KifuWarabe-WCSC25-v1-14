using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.P025_KifuLarabe.L00060_KifuParser;

using Grayscale.P025_KifuLarabe;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L007_Random;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;

namespace Grayscale.P025_KifuLarabe.L100_KifuIO
{
    public class KifuParserA_LogImpl : KifuParserA_Log
    {

        public string Hint { get { return this.hint; } }
        private string hint;

        public LarabeLoggerable LogTag { get { return this.logTag; } }
        private LarabeLoggerable logTag;

        public KifuParserA_LogImpl( LarabeLoggerable logTag, string hint)
        {
            this.hint = hint;
            this.logTag = logTag;
        }

    }
}
