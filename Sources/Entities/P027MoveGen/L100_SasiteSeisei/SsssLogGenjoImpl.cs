using System;
using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.P027MoveGen.L0005MoveGen;

namespace Grayscale.P027MoveGen.L100MoveGen
{
    public class SsssLogGenjoImpl : SsssLogGenjo
    {
        public Boolean EnableLog { get { return this.enableLog; } }
        private bool enableLog;


        public ILogTag LogTag { get { return this.logTag; } }
        private ILogTag logTag;

        public SsssLogGenjoImpl(bool enableLog, ILogTag logTag)
        {
            this.enableLog = enableLog;
            this.logTag = logTag;
        }

    }
}
