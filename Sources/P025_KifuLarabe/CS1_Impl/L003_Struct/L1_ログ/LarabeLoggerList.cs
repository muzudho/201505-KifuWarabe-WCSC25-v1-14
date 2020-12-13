using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Grayscale.P025_KifuLarabe.L00025_Struct
{
    /// <summary>
    /// ************************************************************************************************************************
    /// ロガー
    /// ************************************************************************************************************************
    /// 
    /// partial … ロガー定数を拡張できるクラスとして開放。
    /// </summary>
    public partial class LarabeLoggerList
    {
        public static readonly LarabeLoggerable SASITE_SEISEI_ROUTINE = new LarabeLoggerImpl("../../Logs/_log_指し手生成ルーチン", ".txt", true, false);
        public static readonly LarabeLoggerable LOGGING_BY_GUI = new LarabeLoggerImpl("../../Logs/_log_将棋GUI_棋譜読取", ".txt", true, false);
        public static readonly LarabeLoggerable LOGGING_BY_LARABE_STANDALONE = new LarabeLoggerImpl("../../Logs/_log_ララベProgram", ".txt", true, false);
        public static readonly LarabeLoggerable LINKED_LIST = new LarabeLoggerImpl("../../Logs/_log_リンクトリスト", ".txt", false, false);
        public static readonly LarabeLoggerable ERROR = new LarabeLoggerImpl("../../Logs/_log_エラー", ".txt", true, false);


        /// <summary>
        /// デフォルトのロガーリスト。
        /// </summary>
        /// <returns></returns>
        public static LarabeLoggerList GetDefaultList()
        {
            if (null == LarabeLoggerList.defaultList)
            {
                LarabeLoggerList.defaultList = new LarabeLoggerList(
                    "../../Logs/_log_default_false_(" + System.Diagnostics.Process.GetCurrentProcess() + ")",
                    ".txt",
                    false,
                    false
                    );
            }

            return LarabeLoggerList.defaultList;
        }
        private static LarabeLoggerList defaultList;






        /// <summary>
        /// デフォルト・ログ・ファイル
        /// </summary>
        public LarabeLoggerable DefaultFile { get { return this.defaultFile; } }
        private LarabeLoggerable defaultFile;


        /// <summary>
        /// タグの登録。リムーブに使用。
        /// </summary>
        private List<LarabeLoggerable> tagList;


        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="fileNameWoe">拡張子抜きのファイル名。(with out extension)</param>
        /// <param name="extensionWd">ドット付き拡張子。(with dot)</param>
        /// <param name="enable">ログ出力のON/OFF</param>
        /// <param name="print_TimeStamp">タイムスタンプ出力のON/OFF</param>
        public LarabeLoggerList(string defaultLogFileNameWoe, string extensionWd, bool enable, bool print_TimeStamp)
        {
            this.defaultFile = new LarabeLoggerImpl(defaultLogFileNameWoe, extensionWd, enable, print_TimeStamp);

            this.tagList = new List<LarabeLoggerable>();
        }


        public void AddLogFile(LarabeLoggerable tag)
        {
            this.tagList.Add(tag);
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// ログファイルを削除します。(連番がなければ)
        /// ************************************************************************************************************************
        /// </summary>
        public void RemoveFile()
        {
            try
            {
                System.IO.File.Delete(this.defaultFile.FileName);

                foreach (LarabeLoggerable tag in this.tagList)
                {
                    System.IO.File.Delete(tag.FileNameWoe+tag.Extension);
                }
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って　無視します。
                string message = this.GetType().Name + "#RemoveFile：" + ex.Message;
                LarabeLoggerList.ERROR.WriteLine_Error( message);
            }

        }


    }
}
