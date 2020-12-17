namespace Grayscale.Kifuwarazusa.UseCases
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Grayscale.Kifuwarazusa.Entities;
    using Grayscale.P025_KifuLarabe.L00012_Atom;
    using Grayscale.P025_KifuLarabe.L00025_Struct;
    using Grayscale.P025_KifuLarabe.L00050_StructShogi;
    using Grayscale.P025_KifuLarabe.L012_Common;
    using Grayscale.P025_KifuLarabe.L100_KifuIO;
    using Grayscale.P045_Atama.L00025_KyHandan;
    using Grayscale.P050_KifuWarabe.L00025_UsiLoop;
    using Grayscale.P050_KifuWarabe.L00052_Shogisasi;
    using Grayscale.P050_KifuWarabe.L003_Kokoro;
    using Grayscale.P050_KifuWarabe.L031_AjimiEngine;

    public class Playing : ShogiEngine
    {
        // USI「ponder」の使用の有無です。
        // ポンダーに対応している将棋サーバーなら真です。
        public bool usiPonderEnabled { get; private set; } = false;

        /// <summary>
        /// USI「setoption」コマンドのリストです。
        /// </summary>
        public Dictionary<string, string> SetoptionDictionary { get; set; }


        /// <summary>
        /// 将棋エンジンの中の一大要素「思考エンジン」です。
        /// 指す１手の答えを出すのが仕事です。
        /// </summary>
        public Shogisasi shogisasi;

        public PlayerInfo PlayerInfo { get { return this.playerInfo; } }
        private PlayerInfo playerInfo;

        /// <summary>
        /// 棋譜です。
        /// </summary>
        public KifuTree Kifu { get; set; }

        /// <summary>
        /// 手目済カウントです。
        /// </summary>
        public int TesumiCount { get; set; }

        /// <summary>
        /// 「go」の属性一覧です。
        /// </summary>
        public Dictionary<string, string> GoProperties { get; set; }

        public AjimiEngine AjimiEngine { get; set; }

        /// <summary>
        /// 「go ponder」の属性一覧です。
        /// </summary>
        public bool GoPonderNow { get; set; }

        /// <summary>
        /// コンストラクター
        /// </summary>
        public Playing()
        {
            //-------------+----------------------------------------------------------------------------------------------------------
            // データ設計  |
            //-------------+----------------------------------------------------------------------------------------------------------
            // 将棋所から送られてくるデータを、一覧表に変えたものです。
            this.SetoptionDictionary = new Dictionary<string, string>(); // 不定形

            this.playerInfo = new PlayerInfoImpl();
        }

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="line">メッセージ</param>
        public static void Send(string line)
        {
            // 将棋サーバーに向かってメッセージを送り出します。
            Util_Message.Upload(line);

            // 送信記録をつけます。
            Logger.WriteLineS(LogTags.Client, line);
        }

        public void UsiOk(string engineName, string engineAuthor)
        {
            //------------------------------------------------------------
            // あなたは USI ですか？
            //------------------------------------------------------------
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 1:31:35> usi
            //      │
            //
            //
            // 将棋所で [対局(G)]-[エンジン管理...]-[追加...] でファイルを選んだときに、
            // 送られてくる文字が usi です。


            //------------------------------------------------------------
            // エンジン設定ダイアログボックスを作ります
            //------------------------------------------------------------
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 23:40:15< option name 子 type check default true
            //      │2014/08/02 23:40:15< option name USI type spin default 2 min 1 max 13
            //      │2014/08/02 23:40:15< option name 寅 type combo default tiger var マウス var うし var tiger var ウー var 龍 var へび var 馬 var ひつじ var モンキー var バード var ドッグ var うりぼー
            //      │2014/08/02 23:40:15< option name 卯 type button default うさぎ
            //      │2014/08/02 23:40:15< option name 辰 type string default DRAGON
            //      │2014/08/02 23:40:15< option name 巳 type filename default スネーク.html
            //      │
            //
            //
            // 将棋所で [エンジン設定] ボタンを押したときに出てくるダイアログボックスに、
            //      ・チェックボックス
            //      ・スピン
            //      ・コンボボックス
            //      ・ボタン
            //      ・テキストボックス
            //      ・ファイル選択テキストボックス
            // を置くことができます。
            //
            Playing.Send("option name 子 type check default true");
            Playing.Send("option name USI type spin default 2 min 1 max 13");
            Playing.Send("option name 寅 type combo default tiger var マウス var うし var tiger var ウー var 龍 var へび var 馬 var ひつじ var モンキー var バード var ドッグ var うりぼー");
            Playing.Send("option name 卯 type button default うさぎ");
            Playing.Send("option name 辰 type string default DRAGON");
            Playing.Send("option name 巳 type filename default スネーク.html");


            //------------------------------------------------------------
            // USI です！！
            //------------------------------------------------------------
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 2:03:33< id name fugafuga 1.00.0
            //      │2014/08/02 2:03:33< id author hogehoge
            //      │2014/08/02 2:03:33< usiok
            //      │
            //
            // プログラム名と、作者名を送り返す必要があります。
            // オプションも送り返せば、受け取ってくれます。
            // usi を受け取ってから、5秒以内に usiok を送り返して完了です。

            Playing.Send($"id name {engineName}");
            Playing.Send($"id author {engineAuthor}");
            Playing.Send("usiok");
        }

        public void SetOption(string name, string value)
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
            if (this.SetoptionDictionary.ContainsKey(name))
            {
                // 設定を上書きします。
                this.SetoptionDictionary[name] = value;
            }
            else
            {
                // 設定を追加します。
                this.SetoptionDictionary.Add(name, value);
            }

            if (this.SetoptionDictionary.ContainsKey("USI_ponder"))
            {
                bool result;
                if (Boolean.TryParse(this.SetoptionDictionary["USI_ponder"], out result))
                {
                    usiPonderEnabled = result;
                }
            }
        }

        public void ReadyOk()
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
            foreach (KeyValuePair<string, string> pair in this.SetoptionDictionary)
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

        public void UsiNewGame()
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
        }

        public void Quit()
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
        }

        public void Position()
        {
            //------------------------------------------------------------
            // これが棋譜です
            //------------------------------------------------------------
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 2:03:35> position startpos moves 2g2f
            //      │
            //
            // ↑↓この将棋エンジンは後手で、平手初期局面から、先手が初手  ▲２六歩  を指されたことが分かります。
            //
            //        ９  ８  ７  ６  ５  ４  ３  ２  １                 ９  ８  ７  ６  ５  ４  ３  ２  １
            //      ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐             ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐
            //      │香│桂│銀│金│玉│金│銀│桂│香│一           │ｌ│ｎ│ｓ│ｇ│ｋ│ｇ│ｓ│ｎ│ｌ│ａ
            //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //      │  │飛│  │  │  │  │  │角│  │二           │  │ｒ│  │  │  │  │  │ｂ│  │ｂ
            //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //      │歩│歩│歩│歩│歩│歩│歩│歩│歩│三           │ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｃ
            //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //      │  │  │  │  │  │  │  │  │  │四           │  │  │  │  │  │  │  │  │  │ｄ
            //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //      │  │  │  │  │  │  │  │  │  │五           │  │  │  │  │  │  │  │  │  │ｅ
            //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //      │  │  │  │  │  │  │  │歩│  │六           │  │  │  │  │  │  │  │Ｐ│  │ｆ
            //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //      │歩│歩│歩│歩│歩│歩│歩│  │歩│七           │Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│  │Ｐ│ｇ
            //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //      │  │角│  │  │  │  │  │飛│  │八           │  │Ｂ│  │  │  │  │  │Ｒ│  │ｈ
            //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //      │香│桂│銀│金│玉│金│銀│桂│香│九           │Ｌ│Ｎ│Ｓ│Ｇ│Ｋ│Ｇ│Ｓ│Ｎ│Ｌ│ｉ
            //      └─┴─┴─┴─┴─┴─┴─┴─┴─┘             └─┴─┴─┴─┴─┴─┴─┴─┴─┘
            //
            // または
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 2:03:35> position sfen lnsgkgsnl/9/ppppppppp/9/9/9/PPPPPPPPP/1B5R1/LNSGKGSNL w - 1 moves 5a6b 7g7f 3a3b
            //      │
            //
            // ↑↓将棋所のサンプルによると、“２枚落ち初期局面から△６二玉、▲７六歩、△３二銀と進んだ局面”とのことです。
            //
            //                                           ＜初期局面＞    ９  ８  ７  ６  ５  ４  ３  ２  １
            //                                                         ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐
            //                                                         │ｌ│ｎ│ｓ│ｇ│ｋ│ｇ│ｓ│ｎ│ｌ│ａ  ←lnsgkgsnl
            //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //                                                         │  │  │  │  │  │  │  │  │  │ｂ  ←9
            //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //                                                         │ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｃ  ←ppppppppp
            //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //                                                         │  │  │  │  │  │  │  │  │  │ｄ  ←9
            //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //                                                         │  │  │  │  │  │  │  │  │  │ｅ  ←9
            //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //                                                         │  │  │  │  │  │  │  │  │  │ｆ  ←9
            //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //                                                         │Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│ｇ  ←PPPPPPPPP
            //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //                                                         │  │Ｂ│  │  │  │  │  │Ｒ│  │ｈ  ←1B5R1
            //                                                         ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //                                                         │Ｌ│Ｎ│Ｓ│Ｇ│Ｋ│Ｇ│Ｓ│Ｎ│Ｌ│ｉ  ←LNSGKGSNL
            //                                                         └─┴─┴─┴─┴─┴─┴─┴─┴─┘
            //
            //        ９  ８  ７  ６  ５  ４  ３  ２  １   ＜３手目＞    ９  ８  ７  ６  ５  ４  ３  ２  １
            //      ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐             ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐
            //      │香│桂│銀│金│  │金│  │桂│香│一           │ｌ│ｎ│ｓ│ｇ│  │ｇ│  │ｎ│ｌ│ａ
            //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //      │  │  │  │玉│  │  │銀│  │  │二           │  │  │  │ｋ│  │  │ｓ│  │  │ｂ
            //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //      │歩│歩│歩│歩│歩│歩│歩│歩│歩│三           │ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｐ│ｃ
            //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //      │  │  │  │  │  │  │  │  │  │四           │  │  │  │  │  │  │  │  │  │ｄ
            //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //      │  │  │  │  │  │  │  │  │  │五           │  │  │  │  │  │  │  │  │  │ｅ
            //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //      │  │  │歩│  │  │  │  │  │  │六           │  │  │Ｐ│  │  │  │  │  │  │ｆ
            //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //      │歩│歩│  │歩│歩│歩│歩│歩│歩│七           │Ｐ│Ｐ│  │Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│Ｐ│ｇ
            //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //      │  │角│  │  │  │  │  │飛│  │八           │  │Ｂ│  │  │  │  │  │Ｒ│  │ｈ
            //      ├─┼─┼─┼─┼─┼─┼─┼─┼─┤             ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //      │香│桂│銀│金│玉│金│銀│桂│香│九           │Ｌ│Ｎ│Ｓ│Ｇ│Ｋ│Ｇ│Ｓ│Ｎ│Ｌ│ｉ
            //      └─┴─┴─┴─┴─┴─┴─┴─┴─┘             └─┴─┴─┴─┴─┴─┴─┴─┴─┘
            //
        }

        public void GoPonder()
        {
            try
            {

                //------------------------------------------------------------
                // 将棋所が次に呼びかけるまで、考えていてください
                //------------------------------------------------------------
                //
                // 図.
                //
                //      log.txt
                //      ┌────────────────────────────────────────
                //      ～
                //      │2014/08/02 2:03:35> go ponder
                //      │
                //

                // 先読み用です。
                // 今回のプログラムでは対応しません。
                //
                // 将棋エンジンが  将棋所に向かって  「bestmove ★ ponder ★」といったメッセージを送ったとき、
                // 将棋所は「go ponder」というメッセージを返してくると思います。
                //
                // 恐らく  このメッセージを受け取っても、将棋エンジンは気にせず  考え続けていればいいのではないでしょうか。


                //------------------------------------------------------------
                // じっとがまん
                //------------------------------------------------------------
                //
                // まだ指してはいけません。
                // 指したら反則です。相手はまだ指していないのだ☆ｗ
                //
            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                Logger.WriteLineAddMemo(LogTags.Engine, "Program「go ponder」：" + ex.GetType().Name + "：" + ex.Message);
            }
        }

        public void Go(string btime, string wtime, string byoyomi, string binc, string winc)
        {
            // ┏━━━━サンプル・プログラム━━━━┓

            int latestTesumi = this.Kifu.CountTesumi(this.Kifu.CurNode);//現・手目済
            this.PlayerInfo.Playerside = this.Kifu.CountPside(latestTesumi);// 先後

            //#if DEBUG
            //                MessageBox.Show("["+latestTesumi+"]手目済　["+this.owner.PlayerInfo.Playerside+"]の手番");
            //#endif

            SkyConst src_Sky = this.Kifu.NodeAt(latestTesumi).Value.ToKyokumenConst;//現局面

            // + line
            Logger.WriteLineAddMemo(LogTags.Engine, "将棋サーバー「" + latestTesumi + "手目、きふわらべ　さんの手番ですよ！」　");


            Result_Ajimi result_Ajimi = this.AjimiEngine.Ajimi(src_Sky);


            //------------------------------------------------------------
            // わたしの手番のとき、王様が　将棋盤上からいなくなっていれば、投了します。
            //------------------------------------------------------------
            //
            //      将棋ＧＵＩ『きふならべ』用☆　将棋盤上に王さまがいないときに、本将棋で　go　コマンドが送られてくることは無いのでは☆？
            //
            switch (result_Ajimi)
            {
                case Result_Ajimi.Lost_SenteOh:// 先手の王さまが将棋盤上にいないとき☆
                case Result_Ajimi.Lost_GoteOh:// または、後手の王さまが将棋盤上にいないとき☆
                    {
                        //------------------------------------------------------------
                        // 投了
                        //------------------------------------------------------------
                        //
                        // 図.
                        //
                        //      log.txt
                        //      ┌────────────────────────────────────────
                        //      ～
                        //      │2014/08/02 2:36:21< bestmove resign
                        //      │
                        //

                        // この将棋エンジンは、後手とします。
                        // ２０手目、投了  を決め打ちで返します。
                        Playing.Send("bestmove resign");//投了
                    }
                    break;
                default:// どちらの王さまも、まだまだ健在だぜ☆！
                    {
                        try
                        {
                            //------------------------------------------------------------
                            // 指し手のチョイス
                            //------------------------------------------------------------
                            bool enableLog = false;
                            bool isHonshogi = true;

                            // 指し手を決めます。
                            ShootingStarlightable bestMove = this.shogisasi.WA_Bestmove(
                                enableLog,
                                isHonshogi,
                                this.Kifu,
                                this.PlayerInfo,
                                LogTags.Engine
                                );




                            if (Util_Sky.isEnableSfen(bestMove))
                            {
                                string sfenText = Util_Sky.ToSfenMoveText(bestMove);
                                Logger.WriteLineAddMemo(LogTags.Engine, "(Warabe)指し手のチョイス： bestmove＝[" + sfenText + "]" +
                                    "　棋譜＝" + KirokuGakari.ToJapaneseKifuText(this.Kifu, LogTags.Engine));

                                Playing.Send("bestmove " + sfenText);//指し手を送ります。
                            }
                            else // 指し手がないときは、SFENが書けない☆　投了だぜ☆
                            {
                                Logger.WriteLineAddMemo(LogTags.Engine, "(Warabe)指し手のチョイス： 指し手がないときは、SFENが書けない☆　投了だぜ☆ｗｗ（＞＿＜）" +
                                    "　棋譜＝" + KirokuGakari.ToJapaneseKifuText(this.Kifu, LogTags.Engine));

                                // 投了ｗ！
                                Playing.Send("bestmove resign");
                            }
                        }
                        catch (Exception ex)
                        {
                            //>>>>> エラーが起こりました。
                            string message = ex.GetType().Name + " " + ex.Message + "：ベスト指し手のチョイスをしたときです。：";
                            Debug.Fail(message);

                            // どうにもできないので  ログだけ取って無視します。
                            Logger.WriteLineError(LogTags.Engine, message);
                        }
                    }
                    break;
            }
            // ┗━━━━サンプル・プログラム━━━━┛

        }

        public void Stop()
        {
            try
            {

                //------------------------------------------------------------
                // あなたの手番です  （すぐ指してください！）
                //------------------------------------------------------------
                //
                // 図.
                //
                //      log.txt
                //      ┌────────────────────────────────────────
                //      ～
                //      │2014/08/02 2:03:35> stop
                //      │
                //

                // 何らかの理由で  すぐ指してほしいときに、将棋所から送られてくる文字が stop です。
                //
                // 理由は２つ考えることができます。
                //  （１）１手前に、将棋エンジンが  将棋所に向かって「予想手」付きで指し手を伝えたのだが、
                //        相手の応手が「予想手」とは違ったので、予想手にもとづく思考を  今すぐ変えて欲しいとき。
                //
                //  （２）「急いで指すボタン」が押されたときなどに送られてくるようです？
                //
                // stop するのは思考です。  stop を受け取ったら  すぐに最善手を指してください。

                if (this.GoPonderNow)
                {
                    //------------------------------------------------------------
                    // 将棋エンジン「（予想手が間違っていたって？）  △９二香 を指そうと思っていたんだが」
                    //------------------------------------------------------------
                    //
                    // 図.
                    //
                    //      log.txt
                    //      ┌────────────────────────────────────────
                    //      ～
                    //      │2014/08/02 2:36:21< bestmove 9a9b
                    //      │
                    //
                    //
                    //      １手前の指し手で、将棋エンジンが「bestmove ★ ponder ★」という形で  予想手付きで将棋所にメッセージを送っていたとき、
                    //      その予想手が外れていたならば、将棋所は「stop」を返してきます。
                    //      このとき  思考を打ち切って最善手の指し手をすぐに返信するわけですが、将棋所はこの返信を無視します☆ｗ
                    //      （この指し手は、外れていた予想手について考えていた“最善手”ですからゴミのように捨てられます）
                    //      その後、将棋所から「position」「go」が再送されてくるのだと思います。
                    //
                    //          将棋エンジン「bestmove ★ ponder ★」
                    //              ↓
                    //          将棋所      「stop」
                    //              ↓
                    //          将棋エンジン「うその指し手返信」（無視されます）←今ここ
                    //              ↓
                    //          将棋所      「position」「go」
                    //              ↓
                    //          将棋エンジン「本当の指し手」
                    //
                    //      という流れと思います。

                    // この指し手は、無視されます。（無視されますが、送る必要があります）
                    Playing.Send("bestmove 9a9b");
                }
                else
                {
                    //------------------------------------------------------------
                    // じゃあ、△９二香で
                    //------------------------------------------------------------
                    //
                    // 図.
                    //
                    //      log.txt
                    //      ┌────────────────────────────────────────
                    //      ～
                    //      │2014/08/02 2:36:21< bestmove 9a9b
                    //      │
                    //
                    //
                    // 特に何もなく、すぐ指せというのですから、今考えている最善手をすぐに指します。
                    Playing.Send("bestmove 9a9b");
                }

            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                Logger.WriteLineAddMemo(LogTags.Engine, "Program「stop」：" + ex.GetType().Name + " " + ex.Message);
            }
        }

        public void GameOver(string value)
        {
            //------------------------------------------------------------
            // 対局が終わりました
            //------------------------------------------------------------
            //
            // 図.
            //
            //      log.txt
            //      ┌────────────────────────────────────────
            //      ～
            //      │2014/08/02 3:08:34> gameover lose
            //      │
            //

            // 対局が終わったときに送られてくる文字が gameover です。

            //------------------------------------------------------------
            // 「あ、勝ちました」「あ、引き分けました」「あ、負けました」
            //------------------------------------------------------------
            //
            // 上図のメッセージのままだと使いにくいので、
            // あとで使いやすいように Key と Value の表に分けて持ち直します。
            //
            // 図.
            //
            //      gameoverDictionary
            //      ┌──────┬──────┐
            //      │Key         │Value       │
            //      ┝━━━━━━┿━━━━━━┥
            //      │gameover    │lose        │
            //      └──────┴──────┘
            //
        }

    }
}
