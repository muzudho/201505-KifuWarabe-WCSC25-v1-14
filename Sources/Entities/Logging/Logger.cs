using System;
using System.Collections.Generic;
using System.Text;
using Grayscale.P025_KifuLarabe.L00025_Struct;

namespace Grayscale.Kifuwarazusa.Entities.Logging
{
    public static class Logger
    {
        static Logger()
        {
            AddLog(LogTags.Default, DefaultLogRecord);
            AddLog(LogTags.NarabePaint, new LogRecord("../../Logs/#将棋GUI_ﾍﾟｲﾝﾄ", true, false));
            AddLog(LogTags.NarabeNetwork, new LogRecord("../../Logs/#将棋GUI_ﾈｯﾄﾜｰｸ", true, false));
            AddLog(LogTags.MoveGenRoutine, new LogRecord("../../Logs/#指し手生成ルーチン", true, false));
            AddLog(LogTags.Gui, new LogRecord("../../Logs/#将棋GUI_棋譜読取", true, false));
            AddLog(LogTags.LibStandalone, new LogRecord("../../Logs/#ララベProgram", true, false));
            AddLog(LogTags.LinkedList, new LogRecord("../../Logs/#リンクトリスト", false, false));
            AddLog(LogTags.Error, new LogRecord("../../Logs/#エラー", true, false));
            // ログ。将棋エンジンきふわらべで汎用に使います。
            AddLog(LogTags.Engine, new LogRecord("../../Logs/#将棋ｴﾝｼﾞﾝ_汎用", true, false));
            // ログ。送受信内容の記録専用です。
            AddLog(LogTags.Client, new LogRecord("../../Logs/#将棋ｴﾝｼﾞﾝ_ｸﾗｲｱﾝﾄ", true, false));
            // ログ。思考ルーチン専用です。
            AddLog(LogTags.MousouRireki, new LogRecord("../../Logs/#将棋ｴﾝｼﾞﾝ_妄想履歴", true, false));
        }

        public static ILogRecord DefaultLogRecord
        {
            get
            {
                if (null == Logger.defaultLogRecord)
                {
                    Logger.defaultLogRecord = new LogRecord(
                    "../../Logs/#default(" + System.Diagnostics.Process.GetCurrentProcess() + ")",
                    false,
                    false
                    );
                }

                return Logger.defaultLogRecord;
            }
        }
        private static ILogRecord defaultLogRecord;

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

        public static ILogRecord GetRecord(ILogTag logTag)
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
                Logger.WriteLineError(LogTags.Error, message);
            }

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// メモを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        public static void WriteLineOverMemo(
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
                System.IO.File.WriteAllText(fileName, sb.ToString());
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。
                // どうにもできないので  ログだけ取って　無視します。
                string message = "Util_Log#WriteLine_OverMemo：" + ex.Message;
                Logger.WriteLineError(LogTags.Error, message);
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
                Logger.WriteLineError(LogTags.Error, message);
            }

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// サーバーから受け取ったコマンドを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        public static void WriteLineR(
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
                sb.Append("  > ");
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
                string message = "Util_Log#WriteLine_R：" + ex.Message;
                Logger.WriteLineError(LogTags.Error, message);
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

        /// <summary>
        /// ログファイルを削除します。(連番がなければ)
        /// </summary>
        public static void RemoveAllLogFile()
        {
            try
            {
                if (Logger.defaultLogRecord != null)
                {
                    System.IO.File.Delete(Logger.defaultLogRecord.FileName);
                }

                foreach (KeyValuePair<ILogTag, ILogRecord> entry in Logger.logMap)
                {
                    System.IO.File.Delete(entry.Value.FileName);
                }
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って　無視します。
                string message = $"#RemoveAllLogFile：{ex.Message}";
                Logger.WriteLineError(LogTags.Error, message);
            }

        }
    }
}
