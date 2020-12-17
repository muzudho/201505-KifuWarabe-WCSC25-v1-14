namespace Grayscale.Kifuwarazusa.Entities
{
    /// <summary>
    /// ログのタグ全集。
    /// </summary>
    public class LogTags
    {
        public static readonly ILogTag Default = new LogTag("Default");
        public static readonly ILogTag NarabePaint = new LogTag("NarabePaint");
        public static readonly ILogTag NarabeNetwork = new LogTag("NarabeNetwork");
        public static readonly ILogTag MoveGenRoutine = new LogTag("MoveGenRoutine");
        public static readonly ILogTag Gui = new LogTag("Gui");
        public static readonly ILogTag LibStandalone = new LogTag("LibStandalone");
        public static readonly ILogTag LinkedList = new LogTag("LinkedList");
        public static readonly ILogTag Error = new LogTag("Error");
        public static readonly ILogTag Engine = new LogTag("Engine");
        public static readonly ILogTag Client = new LogTag("Client");
        public static readonly ILogTag MousouRireki = new LogTag("MousouRireki");
    }
}
