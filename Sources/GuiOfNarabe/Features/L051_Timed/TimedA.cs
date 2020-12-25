using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Gui;
using Grayscale.Kifuwarazusa.UseCases.Features;

namespace Grayscale.Kifuwarazusa.GuiOfNarabe.Features
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
            // 将棋エンジンからの入力が、input99 に溜まるものとします。
            if (0 < shogiGui.Input99.Length)
            {

                string message = "timer入力 input99=[" + shogiGui.Input99 + "]";
                Logger.Trace(message);

                //
                // 棋譜入力テキストボックスに、指し手「（例）6a6b」を入力するための一連の流れです。
                //
                {
                    shogiGui.ResponseData = new ResponseImpl();
                    shogiGui.ResponseData.SetAppendInputTextString(shogiGui.Input99);// 受信文字列を、上部テキストボックスに入れるよう、依頼します。
                    shogiGui.Response("Timer");// テキストボックスに、受信文字列を入れます。
                    shogiGui.Input99 = "";// 受信文字列の要求を空っぽにします。
                }

                //
                // コマ送り
                //
                {
                    string restText = Util_InGui.ReadLine_FromTextbox();
                    Util_InServer.Komaokuri_Srv(ref restText, shogiGui);// 棋譜の[コマ送り]を実行します。
                    Util_InGui.Komaokuri_Gui(restText, shogiGui);//追加
                    // ↑チェンジターン済み
                    Util_Menace.Menace(shogiGui);// メナス
                }

                //
                // ここで、テキストボックスには「（例）6a6b」が入っています。
                //

                //
                // 駒を動かす一連の流れです。
                //
                {
                    //this.ShogiGui.ResponseData.InputTextString = "";//空っぽにすることを要求する。
                    shogiGui.Response("Timer");// GUIに反映させます。
                }

            }
        }

    }
}
