using System;
using System.Reflection;
using System.Windows.Forms;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Gui;

namespace Grayscale.P200_KifuNarabe.L100_GUI
{
    [Serializable]
    public partial class Ui_ShogiForm1 : Form
    {
        private NarabeRoomViewModel owner;

        public Ui_PnlMain Ui_PnlMain1
        {
            get
            {
                return this.ui_PnlMain1;
            }
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクターです。
        /// ************************************************************************************************************************
        /// </summary>
        public Ui_ShogiForm1(NarabeRoomViewModel owner)
        {
            this.owner = owner;
            InitializeComponent();
            this.ui_PnlMain1.ShogiGui = this.owner;
        }


        public delegate void DELEGATE_Form1_Load(NarabeRoomViewModel shogiGui, object sender, EventArgs e);
        public DELEGATE_Form1_Load Delegate_Form1_Load { get; set; }


        /// <summary>
        /// ************************************************************************************************************************
        /// ウィンドウが表示される直前にしておく準備をここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ui_Form1_Load(object sender, EventArgs e)
        {
            //------------------------------
            // タイトルバーに表示する、「タイトル 1.00.0」といった文字を設定します。
            //------------------------------
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = String.Format("{0} {1}.{2}.{3}", this.Text, version.Major, version.Minor.ToString("00"), version.Build);

            if (null != this.Delegate_Form1_Load)
            {
                this.Delegate_Form1_Load(this.Ui_PnlMain1.ShogiGui, sender, e);
            }
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// ウィンドウが閉じられる直前にしておくことを、ここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ui_Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.owner.Shutdown();
        }

        private void Ui_Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.owner.Shutdown();
        }
    }
}
