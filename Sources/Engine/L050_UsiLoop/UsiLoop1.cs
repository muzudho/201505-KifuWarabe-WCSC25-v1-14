using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Grayscale.Kifuwarazusa.Entities;
using Grayscale.Kifuwarazusa.UseCases;
using Grayscale.P050_KifuWarabe.L00025_UsiLoop;
using Nett;

//スプライト番号
namespace Grayscale.P050_KifuWarabe.L050_UsiLoop
{

    /// <summary>
    /// USIの１番目のループです。
    /// </summary>
    public class UsiLoop1
    {
        public ShogiEngine Owner { get { return this.owner; } }
        private ShogiEngine owner;

        public UsiLoop1(ShogiEngine owner)
        {
            this.owner = owner;
        }

        public void AtLoop_OnIsready(string line, ref Result_UsiLoop1 result_Usi)
        {
            //------------------------------------------------------------
            // それでは定刻になりましたので……
            //------------------------------------------------------------
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 1:31:35> isready
            //      │
            //
            //
            // 対局開始前に、将棋所から送られてくる文字が isready です。

            //------------------------------------------------------------
            // 将棋エンジン「おっおっ、設定を終わらせておかなければ（汗、汗…）」
            //------------------------------------------------------------
            Logger.WriteLineAddMemo(LogTags.Engine, "┏━━━━━設定━━━━━┓");
            foreach (KeyValuePair<string, string> pair in this.Owner.SetoptionDictionary)
            {
                // ここで将棋エンジンの設定を済ませておいてください。
                Logger.WriteLineAddMemo(LogTags.Engine, pair.Key + "=" + pair.Value);
            }
            Logger.WriteLineAddMemo(LogTags.Engine, "┗━━━━━━━━━━━━┛");

            //------------------------------------------------------------
            // よろしくお願いします(^▽^)！
            //------------------------------------------------------------
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 2:03:33< readyok
            //      │
            //
            //
            // いつでも対局する準備が整っていましたら、 readyok を送り返します。
            Playing.Send("readyok");
        }


        public void AtLoop_OnUsinewgame(string line, ref Result_UsiLoop1 result_Usi)
        {
            //------------------------------------------------------------
            // 対局時計が ポチッ とされました
            //------------------------------------------------------------
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 2:03:33> usinewgame
            //      │
            //
            //
            // 対局が始まったときに送られてくる文字が usinewgame です。

            // 無限ループ（１つ目）を抜けます。無限ループ（２つ目）に進みます。
            result_Usi = Result_UsiLoop1.Break;
            return;
        }


        public void AtLoop_OnQuit(string line, ref Result_UsiLoop1 result_Usi)
        {
            //------------------------------------------------------------
            // おつかれさまでした
            //------------------------------------------------------------
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 1:31:38> quit
            //      │
            //
            //
            // 将棋エンジンを止めるときに送られてくる文字が quit です。

            //------------------------------------------------------------
            // ﾉｼ
            //------------------------------------------------------------
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 3:08:34> (^-^)ﾉｼ
            //      │
            //
            //
            Logger.WriteLineAddMemo(LogTags.Engine, "(^-^)ﾉｼ");

            // このプログラムを終了します。
            result_Usi = Result_UsiLoop1.Quit;
        }


    }
}
