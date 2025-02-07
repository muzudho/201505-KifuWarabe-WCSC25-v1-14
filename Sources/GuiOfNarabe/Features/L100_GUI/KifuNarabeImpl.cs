﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Codeplex.Data;//DynamicJson
using Grayscale.Kifuwarazusa.Entities.Configuration;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features.Gui;
using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Gui;
using Nett;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwarazusa.GuiOfNarabe.Features
{

    /// <summary>
    /// きふならべ
    /// </summary>
    public class KifuNarabeImpl : NarabeRoomViewModel
    {
        /// <summary>
        /// 生成後、OwnerFormをセットしてください。
        /// </summary>
        public KifuNarabeImpl(IEngineConf engineConf)
        {
            this.EngineConf = engineConf;

            this.TimedA = new TimedA(this);
            this.TimedB = new TimedB(this);
            this.TimedC = new TimedC(this);

            this.WidgetLoaders = new List<WidgetsLoader>();
            this.ResponseData = new ResponseImpl();

            //----------
            // モデル
            //----------
            this.model_PnlTaikyoku = new GameViewModel();


            //----------
            // ブラシ
            //----------
            //
            //      ボタンや将棋盤などを描画するツールを、事前準備しておきます。
            //
            this.shape_PnlTaikyoku = new Shape_PnlTaikyokuImpl();
        }

        public IEngineConf EngineConf { get; set; }

        public Timed TimedA { get; set; }
        public Timed TimedB { get; set; }
        public Timed TimedC { get; set; }



        public Response ResponseData { get; set; }


        /// <summary>
        /// 使い方：((Ui_Form1)this.OwnerForm)
        /// </summary>
        public Form OwnerForm { get; set; }




        public List<WidgetsLoader> WidgetLoaders { get; set; }


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 将棋の状況は全部この中です。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public IGameViewModel GameViewModel
        {
            get
            {
                return this.model_PnlTaikyoku;
            }
        }
        public GameViewModel model_PnlTaikyoku;


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// グラフィックを描くツールは全部この中です。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_PnlTaikyoku Shape_PnlTaikyoku
        {
            get
            {
                return this.shape_PnlTaikyoku;
            }
        }
        private Shape_PnlTaikyoku shape_PnlTaikyoku;




        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// ゲームの流れの状態遷移図はこれです。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public SceneName Scene
        {
            get
            {
                return this.scene1;
            }
        }

        public void SetScene(SceneName scene)
        {
            if (SceneName.Ignore != scene)
            {
                this.scene1 = scene;
            }
        }
        private SceneName scene1;



        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 駒を動かす状態遷移図はこれです。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public SceneName FlowB
        {
            get
            {
                return this.flowB;
            }
        }

        public void SetFlowB(SceneName name1)
        {
            this.flowB = name1;

            //アライブ
            {
                TimedB timeB = ((TimedB)this.TimedB);
                timeB.MouseEventQueue.Enqueue(new MouseEventState(name1, MouseEventStateName.Arive, Point.Empty));
            }
        }

        private SceneName flowB;



        public void Timer_Tick()
        {
            this.TimedA.Step();
            this.TimedB.Step();
            this.TimedC.Step();
        }




        /// <summary>
        /// 見た目の設定を読み込みます。
        /// </summary>
        public void ReadStyle_ToForm(Ui_ShogiForm1 ui_Form1)
        {
            try
            {
                //string path = Application.StartupPath + @"\Profile\Data\data_style.txt";
                string path = Application.StartupPath + "/../../Profile/Data/data_style.txt";
#if DEBUG
                MessageBox.Show("独自スタイルシート　path=" + path);
#endif

                if (File.Exists(path))
                {
                    string styleText = System.IO.File.ReadAllText(path, Encoding.UTF8);

                    try
                    {
                        var jsonMousou_arr = DynamicJson.Parse(styleText);

                        var bodyElm = jsonMousou_arr["body"];

                        if (null != bodyElm)
                        {
                            var backColor = bodyElm["backColor"];

                            if (null != backColor)
                            {
                                var var_alpha = backColor["alpha"];

                                int red = (int)backColor["red"];

                                int green = (int)backColor["green"];

                                int blue = (int)backColor["blue"];

                                if (null != var_alpha)
                                {
                                    ui_Form1.Ui_PnlMain1.BackColor = Color.FromArgb((int)var_alpha, red, green, blue);
                                }
                                else
                                {
                                    ui_Form1.Ui_PnlMain1.BackColor = Color.FromArgb(red, green, blue);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("JSONのパース時にエラーか？：" + ex.GetType().Name + "：" + ex.Message);
                        throw;
                    }



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("JSONのパース時にエラーか？：" + ex.GetType().Name + "：" + ex.Message);
            }
        }


        public void Load_AsStart()
        {
            Logger.Trace("乱数のたね＝[" + LarabeRandom.Seed + "]");

            //----------
            // 道１８７
            //----------
            Michi187Array.Load(this.EngineConf.GetResourceFullPath("Michi187"));
            File.WriteAllText(this.EngineConf.GetResourceFullPath("MichiTableHtml"), Michi187Array.LogHtml());

            //----------
            // 駒の配役１８１
            //----------
            Util_Haiyaku184Array.Load(this.EngineConf.GetResourceFullPath("Haiyaku185"), Encoding.UTF8);

            {
                List<List<string>> rows = ForcePromotionArray.Load(this.EngineConf.GetResourceFullPath("InputForcePromotion"), Encoding.UTF8);
                File.WriteAllText(this.EngineConf.GetResourceFullPath("OutputForcePromotion"), ForcePromotionArray.LogHtml());
            }

            {
                // 配役転換表
                List<List<string>> rows = Data_HaiyakuTransition.Load(this.EngineConf.GetResourceFullPath("InputSyuruiToHaiyaku"), Encoding.UTF8);
                File.WriteAllText(this.EngineConf.GetResourceFullPath("OutputSyuruiToHaiyaku"), Data_HaiyakuTransition.LogHtml());
            }
        }

        public void LaunchForm_AsBody()
        {
            ((Ui_ShogiForm1)this.OwnerForm).Delegate_Form1_Load = (NarabeRoomViewModel shogiGui, object sender, EventArgs e) =>
            {

                //
                // ボタンのプロパティを外部ファイルから設定
                //
                foreach (WidgetsLoader widgetsLoader in this.WidgetLoaders)
                {
                    widgetsLoader.Step1_ReadFile(shogiGui);
                }

                foreach (WidgetsLoader widgetsLoader in this.WidgetLoaders)
                {
                    widgetsLoader.Step2_Compile_AllWidget(shogiGui);
                }

                foreach (WidgetsLoader widgetsLoader in this.WidgetLoaders)
                {
                    widgetsLoader.Step3_SetEvent(shogiGui);
                }

            };

            this.ReadStyle_ToForm((Ui_ShogiForm1)this.OwnerForm);

            //
            // FIXME: [初期配置]を１回やっておかないと、[コマ送り]ボタン等で不具合が出てしまう。
            //
            {
                //ResponseImpl dammy_response = new ResponseImpl();
                WidgetsLoader_KifuNarabe.Perform_SyokiHaichi(
                    //ref dammy_response,
                    ((Ui_ShogiForm1)this.OwnerForm).Ui_PnlMain1
                );
            }


            Application.Run((Ui_ShogiForm1)this.OwnerForm);
        }


        /// <summary>
        /// 手番が替わったときの挙動を、ここに書きます。
        /// </summary>
        public virtual void ChangeTurn()
        {
        }


        /// <summary>
        /// 将棋エンジンに、終了するように促します。
        /// </summary>
        public virtual void Shutdown()
        {
        }


        /// <summary>
        /// 将棋エンジンに、ログを出すように促します。
        /// </summary>
        public virtual void Logdase()
        {
        }

        /// <summary>
        /// 将棋エンジンを起動します。
        /// </summary>
        public virtual void Start(string shogiEngineFilePath)
        {
        }

        public void Response(string mutexString)
        {
            Ui_PnlMain ui_PnlMain = ((Ui_ShogiForm1)this.OwnerForm).Ui_PnlMain1;

            Ui_PnlMain.Mutex mutex2;
            switch (mutexString)
            {
                case "Timer": mutex2 = Ui_PnlMain.Mutex.Timer; break;
                case "MouseOperation": mutex2 = Ui_PnlMain.Mutex.MouseOperation; break;
                case "Saisei": mutex2 = Ui_PnlMain.Mutex.Saisei; break;
                case "Launch": mutex2 = Ui_PnlMain.Mutex.Launch; break;
                default: mutex2 = Ui_PnlMain.Mutex.Empty; break;
            }


            switch (ui_PnlMain.MutexOwner)
            {
                case Ui_PnlMain.Mutex.Launch:   // 他全部無視
                    goto gt_EndMethod;
                case Ui_PnlMain.Mutex.Saisei:   // マウスとタイマーは無視
                    switch (mutex2)
                    {
                        case Ui_PnlMain.Mutex.MouseOperation:
                        case Ui_PnlMain.Mutex.Timer:
                            goto gt_EndMethod;
                    }
                    break;
                case Ui_PnlMain.Mutex.MouseOperation:
                case Ui_PnlMain.Mutex.Timer:   // タイマーは無視
                    switch (mutex2)
                    {
                        case Ui_PnlMain.Mutex.Timer:
                            goto gt_EndMethod;
                    }
                    break;
                default: break;
            }

            ui_PnlMain.Response(mutex2, this);// 再描画

        gt_EndMethod:
            ;
        }



        public string Input99
        {
            get
            {
                return Ui_PnlMain.input99;
            }
            set
            {
                Ui_PnlMain.input99 = value;
            }
        }

        public RO_Star_Koma GetKoma(Finger finger)
        {
            return Util_Koma.AsKoma(this.GameViewModel.GuiSkyConst.StarlightIndexOf(finger).Now);
        }
    }

}
