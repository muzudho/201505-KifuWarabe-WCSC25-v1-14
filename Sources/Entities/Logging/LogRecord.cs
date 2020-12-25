using Grayscale.Kifuwarazusa.Entities.Configuration;

namespace Grayscale.Kifuwarazusa.Entities.Logging
{
    public class LogRecord : ILogRecord
    {
        public LogRecord(IResFile logFile, bool enabled, bool timeStampPrintable)
        {
            this.LogFile = logFile;
            this.Enabled = enabled;
            this.TimeStampPrintable = timeStampPrintable;
        }

        /// <summary>
        /// 出力先ファイル。
        /// </summary>
        public IResFile LogFile { get; private set; }

        /// <summary>
        /// ログ出力の有無。
        /// </summary>
        public bool Enabled { get; private set; }


        /// <summary>
        /// タイムスタンプ出力の有無。
        /// </summary>
        public bool TimeStampPrintable { get; private set; }
    }
}
