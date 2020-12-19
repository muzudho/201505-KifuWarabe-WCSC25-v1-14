namespace Grayscale.Kifuwarazusa.Entities.Logging
{
    public class LogRecord : ILogRecord
    {
        public LogRecord(string fileStem, bool enabled, bool timeStampPrintable)
        {
            this.FileStem = fileStem;
            this.Enabled = enabled;
            this.TimeStampPrintable = timeStampPrintable;
        }

        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName { get { return $"{this.FileStem}{this.Extension}"; } }

        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileStem { get; private set; }

        /// <summary>
        /// 拡張子は .log 固定。ファイル削除の目印にします。
        /// </summary>
        public string Extension { get { return ".log"; } }

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
