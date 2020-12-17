namespace Grayscale.Kifuwarazusa.UseCases
{
    using System;
    using System.Collections.Generic;
    using Grayscale.Kifuwarazusa.Entities;
    using Grayscale.P025_KifuLarabe.L00012_Atom;
    using Grayscale.P045_Atama.L00025_KyHandan;
    using Grayscale.P050_KifuWarabe.L00025_UsiLoop;
    using Grayscale.P050_KifuWarabe.L00052_Shogisasi;
    using Grayscale.P050_KifuWarabe.L003_Kokoro;

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

    }
}
