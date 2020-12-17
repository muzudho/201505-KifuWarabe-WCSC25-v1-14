namespace Grayscale.Kifuwarazusa.Entities
{
    public interface ILogRecord
    {
        /// <summary>
        /// ファイル名。
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// 拡張子抜きのファイル名。
        /// </summary>
        string FileStem { get; }

        /// <summary>
        /// ドット付きの拡張子。
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// ログ出力の有無。
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// タイムスタンプの有無。
        /// </summary>
        bool TimeStampPrintable { get; }
    }
}
