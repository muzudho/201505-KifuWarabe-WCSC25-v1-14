using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Diagnostics;

using Grayscale.P025_KifuLarabe.L00025_Struct;

namespace Grayscale.P025_KifuLarabe.L00025_Struct
{


    /// <summary>
    /// 継承できる列挙型として利用☆
    /// </summary>
    public class LarabeLoggerImpl : LarabeLoggerable
    {

        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName { get { return this.FileNameWoe + this.Extension; } }

        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileNameWoe { get { return this.fileNameWoe; } }
        private string fileNameWoe;

        /// <summary>
        /// 拡張子
        /// </summary>
        public string Extension { get { return this.extension; } }
        private string extension;

        /// <summary>
        /// ログ出力の有無。
        /// </summary>
        public bool Enable { get { return this.enable; } }
        private bool enable;


        /// <summary>
        /// タイムスタンプ出力の有無。
        /// </summary>
        public bool Print_TimeStamp { get { return this.print_TimeStamp; } }
        private bool print_TimeStamp;



        /// <summary>
        /// コンストラクター。
        /// </summary>
        /// <param name="fileNameWoe">拡張子抜きファイル名</param>
        /// <param name="extension">拡張子</param>
        /// <param name="enable">ログ出力の有無</param>
        public LarabeLoggerImpl(string fileNameWoe, string extension, bool enable, bool print_TimeStamp)
        {
            this.fileNameWoe = fileNameWoe;
            this.extension = extension;
            this.enable = enable;
            this.print_TimeStamp = print_TimeStamp;
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
            return (this.FileNameWoe+this.Extension == p.FileNameWoe+p.Extension);
        }












        /// <summary>
        /// ************************************************************************************************************************
        /// メモを、ログ・ファイルの末尾に追記します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine_AddMemo(
            string line
            )
        {
            bool enable = this.Enable;
            bool print_TimeStamp = this.Print_TimeStamp;
            string fileName = this.FileName;

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
                LarabeLoggerList.ERROR.WriteLine_Error(message);
            }

        gt_EndMethod:
            ;
        }








        /// <summary>
        /// ************************************************************************************************************************
        /// エラーを、ログ・ファイルに記録します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine_Error(
            string line
            )
        {
            bool enable = this.Enable;
            bool printTimestamp = this.Print_TimeStamp;
            string fileName = this.FileName;

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
        /// ************************************************************************************************************************
        /// メモを、ログ・ファイルに記録します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine_OverMemo(
            string line
            )
        {
            bool enable = this.Enable;
            bool printTimestamp = this.Print_TimeStamp;
            string fileName = this.FileName;

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
                LarabeLoggerList.ERROR.WriteLine_Error(message);
            }

        gt_EndMethod:
            ;
        }








        /// <summary>
        /// ************************************************************************************************************************
        /// サーバーへ送ったコマンドを、ログ・ファイルに記録します。
        /// ************************************************************************************************************************
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
            bool enable = this.Enable;
            bool print_TimeStamp = this.Print_TimeStamp;
            string fileName = this.FileName;

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
                LarabeLoggerList.ERROR.WriteLine_Error(message);
            }

        gt_EndMethod:
            ;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// サーバーから受け取ったコマンドを、ログ・ファイルに記録します。
        /// ************************************************************************************************************************
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
            bool enable = this.Enable;
            bool print_TimeStamp = this.Print_TimeStamp;
            string fileName = this.FileName;

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
                LarabeLoggerList.ERROR.WriteLine_Error(message);
            }

        gt_EndMethod:
            ;
        }

    }


}
