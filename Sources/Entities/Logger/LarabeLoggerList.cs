using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Grayscale.Kifuwarazusa.Entities;

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
        /// <summary>
        /// デフォルトのロガーリスト。
        /// </summary>
        /// <returns></returns>
        public static LarabeLoggerList GetDefaultList()
        {
            if (null == LarabeLoggerList.defaultList)
            {
                LarabeLoggerList.defaultList = new LarabeLoggerList(
                    "../../Logs/#default(" + System.Diagnostics.Process.GetCurrentProcess() + ")",
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
        /// <param name="timeStampPrintable">タイムスタンプ出力のON/OFF</param>
        public LarabeLoggerList(string defaultLogFileStem, bool enable, bool timeStampPrintable)
        {
            this.defaultFile = new LarabeLoggerImpl(defaultLogFileStem, enable, timeStampPrintable);

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
                System.IO.File.Delete(this.defaultFile.LogRecord.FileName);

                foreach (LarabeLoggerable tag in this.tagList)
                {
                    System.IO.File.Delete(tag.LogRecord.FileName);
                }
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので  ログだけ取って　無視します。
                string message = this.GetType().Name + "#RemoveFile：" + ex.Message;
                Logger.Error.WriteLine_Error( message);
            }

        }


    }
}
