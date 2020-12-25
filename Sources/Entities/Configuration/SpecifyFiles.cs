namespace Grayscale.Kifuwarazusa.Entities.Configuration
{
    using System.IO;
    using Nett;

    public static class SpecifyFiles
    {
        static SpecifyFiles()
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            var logDirectory = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("LogDirectory"));

            /*
            AddLog(LogTags.NarabeNetwork, new LogRecord("../../Logs/#将棋GUI_ﾈｯﾄﾜｰｸ", true, false));
            // ログ。将棋エンジンきふわらべで汎用に使います。
            AddLog(LogTags.Engine, new LogRecord("../../Logs/#将棋ｴﾝｼﾞﾝ_汎用", true, false));
            // ログ。送受信内容の記録専用です。
            AddLog(LogTags.Client, new LogRecord("../../Logs/#将棋ｴﾝｼﾞﾝ_ｸﾗｲｱﾝﾄ", true, false));
            // ログ。思考ルーチン専用です。
            AddLog(LogTags.MousouRireki, new LogRecord("../../Logs/#将棋ｴﾝｼﾞﾝ_妄想履歴", true, false));
            */

            /*
            OutputForcePromotion = DataEntry(profilePath, toml, "OutputForcePromotion");
            OutputPieceTypeToHaiyaku = DataEntry(profilePath, toml, "OutputPieceTypeToHaiyaku");
            HaichiTenkanHyoOnlyDataLog = DataEntry(profilePath, toml, "HaichiTenkanHyoOnlyDataLog");
            HaichiTenkanHyoAllLog = DataEntry(profilePath, toml, "HaichiTenkanHyoAllLog");
            */


            LatestPositionLogPng = LogEntry(logDirectory, toml, "LatestPositionLogPng");
            MousouRireki = LogEntry(logDirectory, toml, "MousouRireki");
            GuiDefault = LogEntry(logDirectory, toml, "GuiRecordLog");
            LinkedList = LogEntry(logDirectory, toml, "LinkedListLog");
            GuiPaint = LogEntry(logDirectory, toml, "GuiPaint");
            /*
            LegalMove = LogEntry(logDirectory, toml, "LegalMoveLog");
            LegalMoveEvasion = LogEntry(logDirectory, toml, "LegalMoveEvasionLog");
            */
            GenMove = LogEntry(logDirectory, toml, "GenMoveLog");
        }

        static IResFile LogEntry(string logDirectory, TomlTable toml, string resourceKey)
        {
            return ResFile.AsLog(logDirectory, toml.Get<TomlTable>("Logs").Get<string>(resourceKey));
        }
        static IResFile DataEntry(string profilePath, TomlTable toml, string resourceKey)
        {
            return ResFile.AsData(profilePath, toml.Get<TomlTable>("Resources").Get<string>(resourceKey));
        }

        /*
        public static ILogFile OutputForcePromotion { get; private set; }
        public static ILogFile OutputPieceTypeToHaiyaku { get; private set; }
        public static ILogFile HaichiTenkanHyoOnlyDataLog { get; private set; }
        public static ILogFile HaichiTenkanHyoAllLog { get; private set; }
        */



        public static IResFile LatestPositionLogPng { get; private set; }
        public static IResFile MousouRireki { get; private set; }
        public static IResFile GuiDefault { get; private set; }
        public static IResFile LinkedList { get; private set; }
        public static IResFile GuiPaint { get; private set; }

        /*
        public static ILogFile LegalMove { get; private set; }
        public static ILogFile LegalMoveEvasion { get; private set; }
        */
        /// <summary>
        /// 指し手生成だけ別ファイルにログを取りたいとき。
        /// </summary>
        public static IResFile GenMove { get; private set; }
    }
}
