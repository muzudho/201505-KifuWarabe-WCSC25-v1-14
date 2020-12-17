using System;
using System.Text;
using System.Windows.Forms;
using Grayscale.Kifuwarazusa.Entities;

namespace Grayscale.P025_KifuLarabe.L00025_Struct
{
    /// <summary>
    /// 継承できる列挙型として利用☆
    /// </summary>
    public class LarabeLoggerImpl : LarabeLoggerable
    {
        public LogRecord LogRecord { get; private set; }

        /// <summary>
        /// コンストラクター。
        /// </summary>
        /// <param name="fileNameStem">拡張子抜きファイル名</param>
        /// <param name="enabled">ログ出力の有無</param>
        /// <param name="timeStampPrintable">タイムスタンプの有無</param>
        public LarabeLoggerImpl(string fileNameStem, bool enabled, bool timeStampPrintable)
        {
            this.LogRecord = new LogRecord(fileNameStem, enabled, timeStampPrintable);
        }


        /// <summary>
        /// Equalsをオーバーライドしたので、このメソッドのオーバーライドも必要になります。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            LarabeLoggerable p = obj as LarabeLoggerable;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (this.LogRecord.FileName == p.LogRecord.FileName);
        }

        /// <summary>
        /// メモを、ログ・ファイルの末尾に追記します。
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine_AddMemo(
            string line
            )
        {
            bool enable = this.LogRecord.Enabled;
            bool print_TimeStamp = this.LogRecord.TimeStampPrintable;
            string fileName = this.LogRecord.FileName;

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
        /// エラーを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine_Error(
            string line
            )
        {
            bool enable = this.LogRecord.Enabled;
            bool printTimestamp = this.LogRecord.TimeStampPrintable;
            string fileName = this.LogRecord.FileName;

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
                MessageBox.Show(message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
        /// メモを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine_OverMemo(
            string line
            )
        {
            bool enable = this.LogRecord.Enabled;
            bool printTimestamp = this.LogRecord.TimeStampPrintable;
            string fileName = this.LogRecord.FileName;

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
                Logger.Error.WriteLine_Error(message);
            }

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// サーバーへ送ったコマンドを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine_S(
            string line
            //,
            //[CallerMemberName] string memberName = "",
            //[CallerFilePath] string sourceFilePath = "",
            //[CallerLineNumber] int sourceLineNumber = 0
            )
        {
            bool enable = this.LogRecord.Enabled;
            bool print_TimeStamp = this.LogRecord.TimeStampPrintable;
            string fileName = this.LogRecord.FileName;

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
                string message = "Util_Log#WriteLine_S：" + ex.Message;
                Logger.Error.WriteLine_Error(message);
            }

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// サーバーから受け取ったコマンドを、ログ・ファイルに記録します。
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine_R(
            string line
            //,
            //[CallerMemberName] string memberName = "",
            //[CallerFilePath] string sourceFilePath = "",
            //[CallerLineNumber] int sourceLineNumber = 0
            )
        {
            bool enable = this.LogRecord.Enabled;
            bool print_TimeStamp = this.LogRecord.TimeStampPrintable;
            string fileName = this.LogRecord.FileName;

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
                Logger.Error.WriteLine_Error(message);
            }

        gt_EndMethod:
            ;
        }
    }
}
