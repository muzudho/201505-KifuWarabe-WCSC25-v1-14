using System.Collections.Generic;
using System.Windows.Forms;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Gui;
using Grayscale.P100_ShogiServer.L100_InServer;
using Grayscale.P200_KifuNarabe.L015_Sprite;
using Grayscale.P200_KifuNarabe.L025_Macro;
using Grayscale.P200_KifuNarabe.L050_Scene;

namespace Grayscale.P200_KifuNarabe.L051_Timed
{

    /// <summary>
    /// [再生]ボタンを押したときの処理。
    /// </summary>
    public class TimedC : TimedAbstract
    {


        private NarabeRoomViewModel shogiGui;

        /// <summary>
        /// [再生]の状態です。
        /// </summary>
        public Queue<SaiseiEventState> SaiseiEventQueue { get; set; }


        private string restText;


        public TimedC(NarabeRoomViewModel shogiGui)
        {
            this.shogiGui = shogiGui;
            this.SaiseiEventQueue = new Queue<SaiseiEventState>();
        }

        public override void Step()
        {

            // 入っているマウス操作イベントは、全部捨てていきます。
            SaiseiEventState[] queue = this.SaiseiEventQueue.ToArray();
            this.SaiseiEventQueue.Clear();
            foreach (SaiseiEventState eventState in queue)
            {
                switch (eventState.Name2)
                {
                    case SaiseiEventStateName.Start:
                        {
                            #region スタート
                            //MessageBox.Show("再生を実行します2。");

                            shogiGui.ResponseData = new ResponseImpl();

                            this.restText = Util_InGui.ReadLine_FromTextbox();
                            this.SaiseiEventQueue.Enqueue(new SaiseiEventState(SaiseiEventStateName.Step));
                            #endregion
                        }
                        break;

                    case SaiseiEventStateName.Step:
                        {
                            #region ステップ

                            // [コマ送り]に成功している間、コマ送りし続けます。
                            bool toBreak = false;
                            Util_InServer.ReadLine_TuginoItteSusumu_Srv(ref restText, shogiGui, out toBreak, "再生ボタン");

                            //TimedC.Saisei_Step(restText, shogiGui, eventState.Flg_logTag);// 再描画（ループが１回も実行されなかったとき用）
                            // 他のアプリが固まらないようにします。
                            Application.DoEvents();

                            // 早すぎると描画されないので、ウェイトを入れます。
                            System.Threading.Thread.Sleep(90);//45


                            //------------------------------
                            // 再描画
                            //------------------------------
                            Util_InGui.Komaokuri_Gui(restText, shogiGui);//追加

                            //------------------------------
                            // メナス
                            //------------------------------
                            Util_Menace.Menace(shogiGui);

                            shogiGui.Response("Saisei");// 再描画

                            if (toBreak)
                            {
                                // 終了
                                this.SaiseiEventQueue.Enqueue(new SaiseiEventState(SaiseiEventStateName.Finished));
                            }
                            else
                            {
                                // 続行
                                this.SaiseiEventQueue.Enqueue(new SaiseiEventState(SaiseiEventStateName.Step));
                            }
                            #endregion
                        }
                        break;
                }
            }
        }



    }
}
