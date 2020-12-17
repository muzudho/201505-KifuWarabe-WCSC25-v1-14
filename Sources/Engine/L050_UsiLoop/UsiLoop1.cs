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


        /// <summary>
        /// USI「ponder」の使用の有無です。
        /// </summary>
        public bool Enable_usiPonder { get; set; }


        public UsiLoop1(ShogiEngine owner)
        {
            this.owner = owner;
            this.Enable_usiPonder = false; // ポンダーに対応している将棋サーバーなら真です。
        }

        public void AtLoop_OnUsi(string line, ref Result_UsiLoop1 result_Usi)
        {
        }


        public void AtLoop_OnSetoption(string line, ref Result_UsiLoop1 result_Usi)
        {
            //------------------------------------------------------------
            // 設定してください
            //------------------------------------------------------------
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 8:19:36> setoption name USI_Ponder value true
            //      │2014/08/02 8:19:36> setoption name USI_Hash value 256
            //      │
            //
            // ↑ゲーム開始時には、[対局]ダイアログボックスの[エンジン共通設定]の２つの内容が送られてきます。
            //      ・[相手の手番中に先読み] チェックボックス
            //      ・[ハッシュメモリ  ★　MB] スピン
            //
            // または
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 23:47:35> setoption name 卯
            //      │2014/08/02 23:47:35> setoption name 卯
            //      │2014/08/02 23:48:29> setoption name 子 value true
            //      │2014/08/02 23:48:29> setoption name USI value 6
            //      │2014/08/02 23:48:29> setoption name 寅 value 馬
            //      │2014/08/02 23:48:29> setoption name 辰 value DRAGONabcde
            //      │2014/08/02 23:48:29> setoption name 巳 value C:\Users\Takahashi\Documents\新しいビットマップ イメージ.bmp
            //      │
            //
            //
            // 将棋所から、[エンジン設定] ダイアログボックスの内容が送られてきます。
            // このダイアログボックスは、将棋エンジンから将棋所に  ダイアログボックスを作るようにメッセージを送って作ったものです。
            //

            //------------------------------------------------------------
            // 設定を一覧表に変えます
            //------------------------------------------------------------
            //
            // 上図のメッセージのままだと使いにくいので、
            // あとで使いやすいように Key と Value の表に分けて持ち直します。
            //
            // 図.
            //
            //      setoptionDictionary
            //      ┌──────┬──────┐
            //      │Key         │Value       │
            //      ┝━━━━━━┿━━━━━━┥
            //      │USI_Ponder  │true        │
            //      ├──────┼──────┤
            //      │USI_Hash    │256         │
            //      └──────┴──────┘
            //
            Regex regex = new Regex(@"setoption name ([^ ]+)(?: value (.*))?", RegexOptions.Singleline);
            Match m = regex.Match(line);

            if (m.Success)
            {
                string name = (string)m.Groups[1].Value;
                string value = "";

                if (3 <= m.Groups.Count)
                {
                    // 「value ★」も省略されずにありました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    value = (string)m.Groups[2].Value;
                }

                if (this.Owner.SetoptionDictionary.ContainsKey(name))
                {
                    // 設定を上書きします。
                    this.Owner.SetoptionDictionary[name] = value;
                }
                else
                {
                    // 設定を追加します。
                    this.Owner.SetoptionDictionary.Add(name, value);
                }
            }

            if (this.Owner.SetoptionDictionary.ContainsKey("USI_ponder"))
            {
                string value = this.Owner.SetoptionDictionary["USI_ponder"];

                bool result;
                if (Boolean.TryParse(value, out result))
                {
                    this.Enable_usiPonder = result;
                }
            }
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
