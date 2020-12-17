using System;
using System.Collections.Generic;
using System.Text;
using Grayscale.P025_KifuLarabe.L00025_Struct;

namespace Grayscale.Kifuwarazusa.Entities
{
    public static class Logger
    {
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

        static Logger()
        {
            AddLog(LogTags.NarabePaint, new LogRecord("../../Logs/#将棋GUI_ﾍﾟｲﾝﾄ", true, false));
            AddLog(LogTags.NarabeNetwork, new LogRecord("../../Logs/#将棋GUI_ﾈｯﾄﾜｰｸ", true, false));
            AddLog(LogTags.MoveGenRoutine, new LogRecord("../../Logs/#指し手生成ルーチン", true, false));
            AddLog(LogTags.Gui, new LogRecord("../../Logs/#将棋GUI_棋譜読取", true, false));
        }

        /// <summary>
        /// アドレスの登録。ログ・ファイルのリムーブに使用。
        /// </summary>
        public static Dictionary<ILogTag, ILogRecord> LogMap
        {
            get
            {
                if (Logger.logMap == null)
                {
                    Logger.logMap = new Dictionary<ILogTag, ILogRecord>();
                }
                return Logger.logMap;
            }
        }
        private static Dictionary<ILogTag, ILogRecord> logMap;

        public static void AddLog(ILogTag key, ILogRecord value)
        {
            Logger.LogMap.Add(key, value);
        }

        static ILogRecord GetRecord(ILogTag logTag)
        {
            try
            {
                return LogMap[logTag];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラー: GetRecord(). [{logTag.Name}] {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// メモを、ログ・ファイルの末尾に追記します。
        /// </summary>
        /// <param name="line"></param>
        public static void WriteLineAddMemo(
            ILogTag logTag,
            string line
            )
        {
            ILogRecord record = GetRecord(logTag);

            bool enable = record.Enabled;
            bool print_TimeStamp = record.TimeStampPrintable;
            string fileName = record.FileName;

            if (!enable)
            {
                // ログ出力オフ
                goto gt_EndMethod;
            }

            // ログ追記 TODO:非同期
            try
            {
                StringBuilder sb = new StringBuilder();

                // タイムスタンプ
                if (print_TimeStamp)
                {
                    sb.Append(DateTime.Now.ToString());
                    sb.Append(" : ");
                }

                sb.Append(line);
                sb.AppendLine();

                System.IO.File.AppendAllText(fileName, sb.ToString());
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って　無視します。
                string message = "Util_Log#WriteLine_AddMemo：" + ex.Message;
                Logger.Error.WriteLine_Error(message);
            }

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// サーバーへ送ったコマンドを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        public static void WriteLineS(
            ILogTag logTag,
            string line
            //,
            //[CallerMemberName] string memberName = "",
            //[CallerFilePath] string sourceFilePath = "",
            //[CallerLineNumber] int sourceLineNumber = 0
            )
        {
            ILogRecord record = GetRecord(logTag);

            bool enable = record.Enabled;
            bool print_TimeStamp = record.TimeStampPrintable;
            string fileName = record.FileName;

            if (!enable)
            {
                // ログ出力オフ
                goto gt_EndMethod;
            }

            // ログ追記 TODO:非同期
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now.ToString());
                sb.Append("<   ");
                sb.Append(line);
                //sb.Append("：");
                //sb.Append(memberName);
                //sb.Append("：");
                //sb.Append(sourceFilePath);
                //sb.Append("：");
                //sb.Append(sourceLineNumber);
                sb.AppendLine();

                System.IO.File.AppendAllText(fileName, sb.ToString());
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って　無視します。
                string message = "Logger#WriteLineS：" + ex.Message;
                Logger.Error.WriteLine_Error(message);
            }

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// エラーを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        public static void WriteLineError(
            ILogTag logTag,
            string line
            )
        {
            ILogRecord record = GetRecord(logTag);

            bool enable = record.Enabled;
            bool printTimestamp = record.TimeStampPrintable;
            string fileName = record.FileName;

            if (!enable)
            {
                // ログ出力オフ
                goto gt_EndMethod;
            }

            // ログ追記 TODO:非同期
            try
            {
                StringBuilder sb = new StringBuilder();

                // タイムスタンプ
                if (printTimestamp)
                {
                    sb.Append(DateTime.Now.ToString());
                    sb.Append(" : ");
                }

                sb.Append(line);
                sb.AppendLine();

                string message = sb.ToString();
                // MessageBox.Show(message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

                System.IO.File.AppendAllText(fileName, message);
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って　無視します。
                string message = "Util_Log#WriteLine_Error：" + ex.Message;
                System.IO.File.AppendAllText("../../Logs/_log_致命的ｴﾗｰ.txt", message);
            }

        gt_EndMethod:
            ;
        }
    }
}
