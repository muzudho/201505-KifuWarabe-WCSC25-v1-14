using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.P027_SasiteSeisei.L0005_SasiteSeisei;
using Grayscale.P025_KifuLarabe.L00025_Struct;

namespace Grayscale.P027_SasiteSeisei.L100_SasiteSeisei
{
    public class SsssLogGenjoImpl : SsssLogGenjo
    {
        public Boolean EnableLog { get { return this.enableLog; } }
        private bool enableLog;


        public LarabeLoggerable LogTag { get { return this.logTag; } }
        private LarabeLoggerable logTag;

        public SsssLogGenjoImpl(bool enableLog, LarabeLoggerable logTag)
        {
            this.enableLog = enableLog;
            this.logTag = logTag;
        }

    }
}
