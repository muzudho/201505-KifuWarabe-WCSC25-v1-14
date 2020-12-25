using System;

namespace Grayscale.Kifuwarazusa.Entities.Features
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
