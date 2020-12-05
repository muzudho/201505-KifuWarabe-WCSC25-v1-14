using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace Grayscale.P025_KifuLarabe.L00012_Atom
{
    public class ShogiServerMessenger
    {

        public Util_Message.DELEGATE_ShogiServer_ToEngine Delegate_ShogiServer_ToEngine { get; set; }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// これが、将棋エンジン（プロセス）です。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Process ShogiEngine { get; set; }

        /// <summary>
        /// 将棋エンジンが起動しているか否かです。
        /// </summary>
        /// <returns></returns>
        public bool IsLive_ShogiEngine()
        {
            return null != this.ShogiEngine && !this.ShogiEngine.HasExited;
        }


        /// <summary>
        /// 生成後、Pr_ofShogiEngine をセットしてください。
        /// </summary>
        public ShogiServerMessenger()
        {
        }

        /// <summary>
        /// 将棋エンジンの標準入力へ、メッセージを送ります。
        /// 
        /// 二度手間なんだが、メソッドを１箇所に集約するためにこれを使う☆
        /// </summary>
        public void Download(string message)//ShogiServer_ToEngine
        {
            this.ShogiEngine.StandardInput.WriteLine(message);

            if (null != this.Delegate_ShogiServer_ToEngine)
            {
                this.Delegate_ShogiServer_ToEngine(message);
            }
        }


    }
}
