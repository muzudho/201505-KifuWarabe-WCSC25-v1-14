using System;
using System.Diagnostics;
using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Gui;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P025_KifuLarabe.L100_KifuIO;
using Grayscale.P200_KifuNarabe.L025_Macro;
using Grayscale.P200_KifuNarabe.L100_GUI;
using Grayscale.P400_KifuNaraVs.L00048_Engine;

namespace Grayscale.P400_KifuNaraVs.L025_ShogiEngine
{
    /// <summary>
    ///  プロセスラッパー
    /// 
    ///     １つの将棋エンジンと通信します。１対１の関係になります。
    ///     このクラスを、将棋エンジンのコンソールだ、と想像して使います。
    /// 
    /// </summary>
    public class ShogiEngineLiveImpl : ShogiEngineLive
    {
        /// <summary>
        /// 将棋エンジンと会話できるオブジェクトです。
        /// </summary>
        public ShogiEngineManInterface ShogiEngineManInterface { get; set; }

        public ShogiEngineLiveImpl(NarabeRoomViewModel ownerShogiGui)
        {
            this.ShogiEngineManInterface = new ShogiEngineManInterfaceImpl();
            this.ShogiEngineManInterface.ShogiServerMessenger.Delegate_ShogiServer_ToEngine = (string line) =>
            {
                //
                // USIコマンドを将棋エンジンに送ったタイミングで、なにかすることがあれば、
                // ここに書きます。
                //
                Logger.WriteLineS(line);
            };
        }

        /// <summary>
        /// 将棋エンジンを起動します。
        /// </summary>
        public void Start(string shogiEngineFilePath)
        {
            try
            {
                if (this.ShogiEngineManInterface.IsLive())
                {
                    Util_Message.Show("将棋エンジンサービスは終了していません。");
                    goto gt_EndMethod;
                }

                //------------------------------
                // ログファイルを削除します。
                //------------------------------
                Logger.RemoveAllLogFile();


                ProcessStartInfo startInfo = new ProcessStartInfo();

                startInfo.FileName = shogiEngineFilePath; // 実行するファイル名
                //startInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
                startInfo.UseShellExecute = false; // シェル機能を使用しない
                startInfo.RedirectStandardInput = true;//標準入力をリダイレクト
                startInfo.RedirectStandardOutput = true; // 標準出力をリダイレクト

                this.ShogiEngineManInterface.ShogiServerMessenger.ShogiEngine = Process.Start(startInfo); // アプリの実行開始

                //  OutputDataReceivedイベントハンドラを追加
                this.ShogiEngineManInterface.ShogiServerMessenger.ShogiEngine.OutputDataReceived += this.ListenUpload_Async;
                this.ShogiEngineManInterface.ShogiServerMessenger.ShogiEngine.Exited += this.OnExited;

                // 非同期受信スタート☆！
                this.ShogiEngineManInterface.ShogiServerMessenger.ShogiEngine.BeginOutputReadLine();

                // 「usi」
                this.ShogiEngineManInterface.Usi();


                // エンジン設定
                // 「setoption」
                //Server.shogiEnginePr.StandardInput.WriteLine("setoption");


                //
                // サーバー・スレッドはここで終了しますが、
                // 代わりに、Server.shogiEnginePr プロセスが動き続け、
                // ReceivedData_FromShogiEngine メソッドが非同期に呼び出され続けます。
                //

                //this.thread = new Thread(new ThreadStart(Server.Main2));
                //this.thread.IsBackground = true;
                //this.thread.Start();

            }
            catch (Exception ex)
            {
                Util_Message.Show(ex.GetType().Name + "：" + ex.Message);
            }

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// この将棋サーバーを終了したときにする挙動を、ここに書きます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExited(object sender, System.EventArgs e)
        {
            this.ShogiEngineManInterface.Shutdown();
        }

        /// <summary>
        /// 手番が替わったときの挙動を、ここに書きます。
        /// </summary>
        public void ChangeTurn99(KifuTree kifu)
        {
            if (!this.ShogiEngineManInterface.IsLive())
            {
                goto gt_EndMethod;
            }

            // FIXME:
            switch (kifu.CountPside(KifuNarabe_KifuWrapper.CountCurTesumi1(kifu)))
            {
                case Playerside.P2:
                    // 仮に、コンピューターが後手番とします。

                    //------------------------------------------------------------
                    // とりあえず、コンピューターが後手ということにしておきます。
                    //------------------------------------------------------------

                    // 例：「position startpos moves 7g7f」
                    this.ShogiEngineManInterface.Position(KirokuGakari.ToSfen_PositionString(kifu));

                    this.ShogiEngineManInterface.Go();

                    break;
                default:
                    break;
            }

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// 将棋エンジンから、データを非同期受信(*1)します。
        /// 
        ///         *1…こっちの都合に合わせず、データが飛んできます。
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ListenUpload_Async(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            string line = e.Data;

            if (null == line)
            {
                // 無視
            }
            else
            {
                //>>>>>>>>>> メッセージを受け取りました。
                Logger.WriteLineR(line);

                if (line.StartsWith("option"))
                {

                }
                else if ("usiok" == line)
                {

                    //------------------------------------------------------------
                    // 「私は将棋サーバーですが、USIプロトコルのponderコマンドには対応していませんので、送ってこないでください」
                    //------------------------------------------------------------
                    this.ShogiEngineManInterface.Setoption("setoption name USI_Ponder value false");

                    //------------------------------------------------------------
                    // 「準備はいいですか？」
                    //------------------------------------------------------------
                    this.ShogiEngineManInterface.Isready();
                }
                else if ("readyok" == line)
                {

                    //------------------------------------------------------------
                    // 対局開始！
                    //------------------------------------------------------------
                    this.ShogiEngineManInterface.Usinewgame();

                }
                else if (line.StartsWith("info"))
                {
                }
                else if (line.StartsWith("bestmove resign"))
                {
                    // 将棋エンジンが、投了されました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    //------------------------------------------------------------
                    // あなたの負けです☆
                    //------------------------------------------------------------
                    this.ShogiEngineManInterface.Gameover_lose();

                    //------------------------------------------------------------
                    // 将棋エンジンを終了してください☆
                    //------------------------------------------------------------
                    this.ShogiEngineManInterface.Quit();
                }
                else if (line.StartsWith("bestmove"))
                {
                    // 将棋エンジンが、手を指されました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    Ui_PnlMain.input99 += line.Substring("bestmove".Length + "".Length);

                    Logger.Trace( "USI受信：bestmove input99=[" + Ui_PnlMain.input99 + "]");
                }
                else
                {
                }
            }
        }
    }
}
