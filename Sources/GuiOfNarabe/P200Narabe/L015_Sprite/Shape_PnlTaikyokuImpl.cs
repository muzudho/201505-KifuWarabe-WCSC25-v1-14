using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P100_ShogiServer.L100_InServer;
using Grayscale.P200_KifuNarabe.L00006_Shape;
using Grayscale.P200_KifuNarabe.L00048_ShogiGui;
using Grayscale.P200_KifuNarabe.L015_Sprite;
using System.Collections.Generic;
using System.Windows.Forms;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P200_KifuNarabe.L015_Sprite
{


    /// <summary>
    /// ************************************************************************************************************************
    /// 描かれる図画です。１つの対局で描かれるものは、ここにまとめて入れられています。
    /// ************************************************************************************************************************
    /// </summary>
    public class Shape_PnlTaikyokuImpl : Shape_Abstract, Shape_PnlTaikyoku
    {
        public Dictionary<string, UserWidget> Widgets { get; set; }
        public void SetWidget(string name, UserWidget widget)
        {
            this.Widgets[name] = widget;
        }
        public UserWidget GetWidget(string name)
        {
            UserWidget widget;

            if (this.Widgets.ContainsKey(name))
            {
                widget = this.Widgets[name];
            }
            else
            {
                widget = UserButtonImpl.NULL_OBJECT;
            }

            return widget;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [出力切替]
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public SyuturyokuKirikae SyuturyokuKirikae
        {
            get
            {
                return this.syuturyokuKirikae;
            }
        }

        public void SetSyuturyokuKirikae(SyuturyokuKirikae value)
        {
            this.syuturyokuKirikae = value;
        }

        private SyuturyokuKirikae syuturyokuKirikae;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// つまんでいる駒
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public int FigTumandeiruKoma
        {
            get
            {
                return this.figTumandeiruKoma;
            }
        }

        public void SetFigTumandeiruKoma(int value)
        {
            this.figTumandeiruKoma = value;
        }
        private int figTumandeiruKoma;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 動かし終わった駒。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Finger MovedKoma
        {
            get
            {
                return this.movedKoma;
            }
        }

        public void SetHMovedKoma(Finger value)
        {
            this.movedKoma = value;
        }

        private Finger movedKoma;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 動かしたい駒。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public bool SelectFirstTouch
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 成るフラグ
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        ///         マウスボタン押下時にセットされ、
        ///         マウスボタンを放したときに読み取られます。
        /// 
        /// </summary>
        public bool Naru
        {
            get
            {
                return this.naruFlag;
            }
        }

        public void SetNaruFlag(bool naru)
        {
            this.naruFlag = naru;
        }
        private bool naruFlag;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 要求：　成る／成らないダイアログボックス
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        ///     0: なし
        ///     1: 成るか成らないかボタンを表示して決定待ち中。
        /// 
        /// </summary>
        public bool Requested_NaruDialogToShow
        {
            get
            {
                return this.requested_NaruDialogToShow;
            }
        }

        public void Request_NaruDialogToShow(bool show)
        {
            this.requested_NaruDialogToShow = show;
        }
        private bool requested_NaruDialogToShow;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 成る、で移動先
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_BtnMasu NaruBtnMasu
        {
            get
            {
                return this.naruBtnMasu;
            }
        }

        public void SetNaruMasu(Shape_BtnMasu naruMasu)
        {
            this.naruBtnMasu = naruMasu;
        }
        private Shape_BtnMasu naruBtnMasu;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// マウスで駒を動かしたときに使います。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        /// 棋譜[再生]中は使いません。
        /// 
        /// </summary>
        public Starlight MouseStarlightOrNull2 { get { return this.mouseStarlightOrNull2; } }
        public void SetMouseStarlightOrNull2(Starlight mouseDd) { this.mouseStarlightOrNull2 = mouseDd; }
        private Starlight mouseStarlightOrNull2;

        /// <summary>
        /// 「取った駒_巻戻し用」
        /// </summary>
        public RO_Star_Koma MousePos_FoodKoma { get; set; }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 駒ボタンの配列。局面のKomaDoorsと同じ添え字で一対一対応。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        ///     *Doors…名前の由来：ハンドル１つに対応するから。
        /// 
        /// </summary>
        public Shape_BtnKoma[] BtnKomaDoors
        {
            get
            {
                return this.btnKomaDoors;
            }
        }
        public void SetBtnKomaDoors(Shape_BtnKomaImpl[] btnKomaDoors)
        {
            this.btnKomaDoors = btnKomaDoors;
        }
        private Shape_BtnKomaImpl[] btnKomaDoors;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 将棋盤
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_PnlShogiban Shogiban
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 駒置き。[0]先手、[1]後手、[2]駒袋。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Shape_PnlKomadai[] KomadaiArr
        {
            get;
            set;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 差し手符号。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        /// <param name="fugo"></param>
        public void SetFugo(string fugo)
        {
            this.lblFugo.Text = fugo;
        }
        private Shape_LblBoxImpl lblFugo;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 先後表示。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        private Shape_LblBoxImpl lblPside;

        //------------------------------------------------------------

        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクターです。
        /// ************************************************************************************************************************
        /// </summary>
        public Shape_PnlTaikyokuImpl():base(0,0,0,0)
        {

            this.Widgets = new Dictionary<string, UserWidget>();

            // 初期化
            //System.C onsole.WriteLine("つまんでいる駒を放します。(1)");
            this.SetFigTumandeiruKoma(-1);
            this.SetHMovedKoma(Fingers.Error_1);
            
            //----------
            // [出力切替]初期値
            //----------
            this.syuturyokuKirikae = SyuturyokuKirikae.Japanese;


            //----------
            // 成る成らないダイアログ
            //----------
            this.Request_NaruDialogToShow(false);

            //----------
            // 将ボタン
            //----------
            this.SetBtnKomaDoors( new Shape_BtnKomaImpl[]{

                new Shape_BtnKomaImpl(Finger_Honshogi.SenteOh),//[0]
                new Shape_BtnKomaImpl(Finger_Honshogi.GoteOh),

                new Shape_BtnKomaImpl(Finger_Honshogi.Hi1),
                new Shape_BtnKomaImpl(Finger_Honshogi.Hi2),

                new Shape_BtnKomaImpl(Finger_Honshogi.Kaku1),
                new Shape_BtnKomaImpl(Finger_Honshogi.Kaku2),//[5]

                new Shape_BtnKomaImpl(Finger_Honshogi.Kin1),
                new Shape_BtnKomaImpl(Finger_Honshogi.Kin2),
                new Shape_BtnKomaImpl(Finger_Honshogi.Kin3),
                new Shape_BtnKomaImpl(Finger_Honshogi.Kin4),

                new Shape_BtnKomaImpl(Finger_Honshogi.Gin1),//[10]
                new Shape_BtnKomaImpl(Finger_Honshogi.Gin2),
                new Shape_BtnKomaImpl(Finger_Honshogi.Gin3),
                new Shape_BtnKomaImpl(Finger_Honshogi.Gin4),

                new Shape_BtnKomaImpl(Finger_Honshogi.Kei1),
                new Shape_BtnKomaImpl(Finger_Honshogi.Kei2),//[15]
                new Shape_BtnKomaImpl(Finger_Honshogi.Kei3),
                new Shape_BtnKomaImpl(Finger_Honshogi.Kei4),

                new Shape_BtnKomaImpl(Finger_Honshogi.Kyo1),
                new Shape_BtnKomaImpl(Finger_Honshogi.Kyo2),
                new Shape_BtnKomaImpl(Finger_Honshogi.Kyo3),//[20]
                new Shape_BtnKomaImpl(Finger_Honshogi.Kyo4),

                new Shape_BtnKomaImpl(Finger_Honshogi.Fu1),
                new Shape_BtnKomaImpl(Finger_Honshogi.Fu2),
                new Shape_BtnKomaImpl(Finger_Honshogi.Fu3),
                new Shape_BtnKomaImpl(Finger_Honshogi.Fu4),//[25]
                new Shape_BtnKomaImpl(Finger_Honshogi.Fu5),
                new Shape_BtnKomaImpl(Finger_Honshogi.Fu6),
                new Shape_BtnKomaImpl(Finger_Honshogi.Fu7),
                new Shape_BtnKomaImpl(Finger_Honshogi.Fu8),
                new Shape_BtnKomaImpl(Finger_Honshogi.Fu9),//[30]

                new Shape_BtnKomaImpl(Finger_Honshogi.Fu10),
                new Shape_BtnKomaImpl(Finger_Honshogi.Fu11),
                new Shape_BtnKomaImpl(Finger_Honshogi.Fu12),
                new Shape_BtnKomaImpl(Finger_Honshogi.Fu13),
                new Shape_BtnKomaImpl(Finger_Honshogi.Fu14),//[35]
                new Shape_BtnKomaImpl(Finger_Honshogi.Fu15),
                new Shape_BtnKomaImpl(Finger_Honshogi.Fu16),
                new Shape_BtnKomaImpl(Finger_Honshogi.Fu17),
                new Shape_BtnKomaImpl(Finger_Honshogi.Fu18)//[39]
            });

            //----------
            // 将棋盤
            //----------
            this.Shogiban = new Shape_PnlShogibanImpl(200, 220, this);

            //----------
            // 駒置き
            //----------
            this.KomadaiArr = new Shape_PnlKomadaiImpl[3];
            this.KomadaiArr[0] = new Shape_PnlKomadaiImpl( Okiba.Sente_Komadai, 610, 220, 81,this);
            this.KomadaiArr[1] = new Shape_PnlKomadaiImpl(Okiba.Gote_Komadai, 10, 220, 121,this);
            this.KomadaiArr[2] = new Shape_PnlKomadaiImpl(Okiba.KomaBukuro, 810, 220, 161,this);

            //----------
            // 符号表示
            //----------
            this.lblFugo = new Shape_LblBoxImpl("符号", 480, 145);

            //----------
            // 先後表示
            //----------
            this.lblPside = new Shape_LblBoxImpl("－－", 350, 145);
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 対局の描画の一式は、ここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Paint(
            object sender, PaintEventArgs e,
            ShogiGui shogiGui,
            ILogTag logTag
            )
        {
            if (!this.Visible)
            {
                goto gt_EndMethod;
            }

            //----------
            // 将棋盤
            //----------
            this.Shogiban.Paint(e.Graphics);

            //----------
            // 駒置き、駒袋
            //----------
            for (int i = 0; i < this.KomadaiArr.Length;i++ )
            {
                Shape_PnlKomadai k = this.KomadaiArr[i];
                k.Paint( e.Graphics);
            }

            //----------
            // 駒
            //----------
            foreach (Shape_BtnKomaImpl koma in this.BtnKomaDoors)
            {
                koma.Paint(e.Graphics, shogiGui, logTag);
            }

            //----------
            // 符号表示
            //----------
            this.lblFugo.Paint(e.Graphics);

            //----------
            // 先後表示
            //----------
            this.lblPside.Text = Converter04.Pside_ToKanji(shogiGui.Model_PnlTaikyoku.Kifu.CountPside(Util_InServer.CountCurTesumi2(shogiGui)));
            this.lblPside.Paint(e.Graphics);


            foreach(UserWidget widget in this.Widgets.Values)
            {
                widget.Paint(e.Graphics);
            }

        gt_EndMethod:
            ;
        }


        /// <summary>
        /// 移動直後の駒を取得。
        /// </summary>
        /// <returns>移動直後の駒。なければヌル</returns>
        public Shape_BtnKoma Btn_MovedKoma()
        {
            Shape_BtnKoma btn = null;

            if (Fingers.Error_1 != this.MovedKoma)
            {
                btn = this.BtnKomaDoors[(int)this.MovedKoma];
            }

            return btn;
        }

        /// <summary>
        /// つまんでいる駒。
        /// </summary>
        /// <returns>つまんでいる駒。なければヌル</returns>
        public Shape_BtnKoma Btn_TumandeiruKoma(object obj_shogiGui)
        {
            ShogiGui shogiGui = (ShogiGui)obj_shogiGui;
            Shape_BtnKoma found = null;

            if (-1 != shogiGui.Shape_PnlTaikyoku.FigTumandeiruKoma)
            {
                found = this.BtnKomaDoors[shogiGui.Shape_PnlTaikyoku.FigTumandeiruKoma];
            }

            return found;
        }

    }


}
