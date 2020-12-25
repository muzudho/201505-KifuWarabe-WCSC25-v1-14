namespace Grayscale.Kifuwarazusa.Entities.Configuration
{
    using System.IO;
    using Nett;

    public static class SpecifyFiles
    {
        /// <summary>
        /// このクラスを使う前にセットしてください。
        /// </summary>
        public static void Init(IEngineConf engineConf)
        {
            EngineConf = engineConf;

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


            LatestPositionLogPng = LogEntry("LatestPositionLogPng");
            MousouRireki = LogEntry("MousouRireki");
            GuiDefault = LogEntry("GuiRecordLog");
            LinkedList = LogEntry("LinkedListLog");
            GuiPaint = LogEntry("GuiPaint");
            /*
            LegalMove = LogEntry(engineConf, "LegalMoveLog");
            LegalMoveEvasion = LogEntry(engineConf, "LegalMoveEvasionLog");
            */
            GenMove = LogEntry("GenMoveLog");
        }

        static IResFile LogEntry(string resourceKey)
        {
            return ResFile.AsLog(EngineConf.LogDirectory, EngineConf.GetLogBasename(resourceKey));
        }

        static IEngineConf EngineConf { get; set; }

        /*
        static IResFile DataEntry(string profilePath, TomlTable toml, string resourceKey)
        {
            return ResFile.AsData(profilePath, toml.Get<TomlTable>("Resources").Get<string>(resourceKey));
        }
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
