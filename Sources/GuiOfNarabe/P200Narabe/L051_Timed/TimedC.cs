using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.P200_KifuNarabe.L00048_ShogiGui;

using Grayscale.P200_KifuNarabe.L050_Scene;
using System.Windows.Forms;


using Grayscale.P025_KifuLarabe.L00025_Struct;
//スプライト番号

using System.Drawing;

using Grayscale.P100_ShogiServer.L100_InServer;
using Grayscale.P200_KifuNarabe.L025_Macro;
using Grayscale.P200_KifuNarabe.L051_Timed;
using Grayscale.P200_KifuNarabe.L015_Sprite;

namespace Grayscale.P200_KifuNarabe.L051_Timed
{

    /// <summary>
    /// [再生]ボタンを押したときの処理。
    /// </summary>
    public class TimedC : TimedAbstract
    {


        private ShogiGui shogiGui;

        /// <summary>
        /// [再生]の状態です。
        /// </summary>
        public Queue<SaiseiEventState> SaiseiEventQueue { get; set; }


        private string restText;


        public TimedC(ShogiGui shogiGui)
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
                            this.SaiseiEventQueue.Enqueue(new SaiseiEventState(SaiseiEventStateName.Step, eventState.Flg_logTag));
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
                            Util_InGui.Komaokuri_Gui(restText, shogiGui, eventState.Flg_logTag);//追加

                            //------------------------------
                            // メナス
                            //------------------------------
                            Util_Menace.Menace(shogiGui, eventState.Flg_logTag);

                            shogiGui.Response("Saisei", eventState.Flg_logTag);// 再描画

                            if (toBreak)
                            {
                                // 終了
                                this.SaiseiEventQueue.Enqueue(new SaiseiEventState(SaiseiEventStateName.Finished, eventState.Flg_logTag));
                            }
                            else
                            {
                                // 続行
                                this.SaiseiEventQueue.Enqueue(new SaiseiEventState(SaiseiEventStateName.Step, eventState.Flg_logTag));
                            }
                            #endregion
                        }
                        break;
                }
            }
        }



    }
}
