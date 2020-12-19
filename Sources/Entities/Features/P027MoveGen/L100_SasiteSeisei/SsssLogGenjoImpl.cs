using System;
using Grayscale.P027MoveGen.L0005MoveGen;

namespace Grayscale.P027MoveGen.L100MoveGen
{
    public class SsssLogGenjoImpl : SsssLogGenjo
    {
        public Boolean EnableLog { get { return this.enableLog; } }
        private bool enableLog;

        public SsssLogGenjoImpl(bool enableLog)
        {
            this.enableLog = enableLog;
        }

    }
}
