using Grayscale.P025_KifuLarabe.L00025_Struct;

namespace Grayscale.Kifuwarazusa.Entities
{
    public static class Logger
    {
        public static readonly LarabeLoggerable NarabePaint  = new LarabeLoggerImpl("../../Logs/#将棋GUI_ﾍﾟｲﾝﾄ", true, false);
        public static readonly LarabeLoggerable NarabeNetwork = new LarabeLoggerImpl("../../Logs/#将棋GUI_ﾈｯﾄﾜｰｸ", true, false);
        public static readonly LarabeLoggerable MoveGenRoutine = new LarabeLoggerImpl("../../Logs/#指し手生成ルーチン", true, false);
        public static readonly LarabeLoggerable Gui = new LarabeLoggerImpl("../../Logs/#将棋GUI_棋譜読取", true, false);
        public static readonly LarabeLoggerable LibStandalone = new LarabeLoggerImpl("../../Logs/#ララベProgram", true, false);
        public static readonly LarabeLoggerable LinkedList = new LarabeLoggerImpl("../../Logs/#リンクトリスト", false, false);
        public static readonly LarabeLoggerable Error = new LarabeLoggerImpl("../../Logs/#エラー", true, false);

        /// <summary>
        /// ログ。将棋エンジンきふわらべで汎用に使います。
        /// </summary>
        public static readonly LarabeLoggerable Engine = new LarabeLoggerImpl("../../Logs/#将棋ｴﾝｼﾞﾝ_汎用", true, false);

        /// <summary>
        /// ログ。送受信内容の記録専用です。
        /// </summary>
        public static readonly LarabeLoggerable Client = new LarabeLoggerImpl("../../Logs/#将棋ｴﾝｼﾞﾝ_ｸﾗｲｱﾝﾄ", true, false);

        /// <summary>
        /// ログ。思考ルーチン専用です。
        /// </summary>
        public static readonly LarabeLoggerable MousouRireki = new LarabeLoggerImpl("../../Logs/#将棋ｴﾝｼﾞﾝ_妄想履歴", true, false);
    }
}
