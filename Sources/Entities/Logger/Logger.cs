using Grayscale.P025_KifuLarabe.L00025_Struct;

namespace Grayscale.Kifuwarazusa.Entities
{
    public static class Logger
    {
        /// <summary>
        /// ログ。将棋エンジンきふわらべで汎用に使います。
        /// </summary>
        private static readonly LarabeLoggerable ENGINE = new LarabeLoggerImpl("../../Logs/_log_将棋ｴﾝｼﾞﾝ_汎用", ".txt", true, false);
        public static LarabeLoggerable Log_Engine
        {
            get
            {
                return Logger.ENGINE;
            }
        }

        /// <summary>
        /// ログ。送受信内容の記録専用です。
        /// </summary>
        private static readonly LarabeLoggerable CLIENT = new LarabeLoggerImpl("../../Logs/_log_将棋ｴﾝｼﾞﾝ_ｸﾗｲｱﾝﾄ", ".txt", true, false);
        public static LarabeLoggerable Log_Client
        {
            get
            {
                return Logger.CLIENT;
            }
        }

        /// <summary>
        /// ログ。思考ルーチン専用です。
        /// </summary>
        private static readonly LarabeLoggerable MOUSOU_RIREKI = new LarabeLoggerImpl("../../Logs/_log_将棋ｴﾝｼﾞﾝ_妄想履歴", ".txt", true, false);
        public static LarabeLoggerable Log_MousouRireki
        {
            get
            {
                return Logger.MOUSOU_RIREKI;
            }
        }
    }
}
