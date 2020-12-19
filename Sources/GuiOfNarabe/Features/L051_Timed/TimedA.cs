using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Gui;
using Grayscale.P100_ShogiServer.L100_InServer;
using Grayscale.P200_KifuNarabe.L015_Sprite;
using Grayscale.P200_KifuNarabe.L025_Macro;

namespace Grayscale.P200_KifuNarabe.L051_Timed
{
    /// <summary>
    /// ▲人間vs△コンピューター対局のやりとりです。
    /// </summary>
    public class TimedA : TimedAbstract
    {
        private NarabeRoomViewModel shogiGui;

        public TimedA(NarabeRoomViewModel shogiGui)
        {
            this.shogiGui = shogiGui;
        }

        public override void Step()
        {
            ILogTag logTag = LogTags.Gui;

            // 将棋エンジンからの入力が、input99 に溜まるものとします。
            if (0 < shogiGui.Input99.Length)
            {

                string message = "timer入力 input99=[" + shogiGui.Input99 + "]";
                Logger.WriteLineAddMemo(logTag,message);

                //
                // 棋譜入力テキストボックスに、指し手「（例）6a6b」を入力するための一連の流れです。
                //
                {
                    shogiGui.ResponseData = new ResponseImpl();
                    shogiGui.ResponseData.SetAppendInputTextString(shogiGui.Input99);// 受信文字列を、上部テキストボックスに入れるよう、依頼します。
                    shogiGui.Response("Timer", logTag);// テキストボックスに、受信文字列を入れます。
                    shogiGui.Input99 = "";// 受信文字列の要求を空っぽにします。
                }

                //
                // コマ送り
                //
                {
                    string restText = Util_InGui.ReadLine_FromTextbox();
                    Util_InServer.Komaokuri_Srv(ref restText, shogiGui, logTag);// 棋譜の[コマ送り]を実行します。
                    Util_InGui.Komaokuri_Gui(restText, shogiGui, logTag);//追加
                    // ↑チェンジターン済み
                    Util_Menace.Menace(shogiGui, logTag);// メナス
                }

                //
                // ここで、テキストボックスには「（例）6a6b」が入っています。
                //

                //
                // 駒を動かす一連の流れです。
                //
                {
                    //this.ShogiGui.ResponseData.InputTextString = "";//空っぽにすることを要求する。
                    shogiGui.Response("Timer", logTag);// GUIに反映させます。
                }

            }
        }

    }
}
