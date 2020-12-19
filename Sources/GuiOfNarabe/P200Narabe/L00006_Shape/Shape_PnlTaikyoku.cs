using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P200_KifuNarabe.L00048_ShogiGui;
using System.Collections.Generic;
using System.Windows.Forms;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P200_KifuNarabe.L00006_Shape
{
    public interface Shape_PnlTaikyoku : Shape
    {

        Dictionary<string, UserWidget> Widgets { get; set; }
        UserWidget GetWidget(string name);

        Shape_BtnKoma Btn_MovedKoma();
        Shape_BtnKoma Btn_TumandeiruKoma(object obj_shogiGui);//ShogiGui
        Shape_BtnKoma[] BtnKomaDoors { get; }

        int FigTumandeiruKoma { get; }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 駒置き。[0]先手、[1]後手、[2]駒袋。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        Shape_PnlKomadai[] KomadaiArr { get; set; }

        /// <summary>
        /// 「取った駒_巻戻し用」
        /// </summary>
        RO_Star_Koma MousePos_FoodKoma { get; set; }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// マウスで駒を動かしたときに使います。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        /// 棋譜[再生]中は使いません。
        /// 
        /// </summary>
        Starlight MouseStarlightOrNull2 { get; }


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 動かし終わった駒。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        Finger MovedKoma { get; }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 成るフラグ
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        ///         マウスボタン押下時にセットされ、
        ///         マウスボタンを放したときに読み取られます。
        /// 
        /// </summary>
        bool Naru { get; }


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 成る、で移動先
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        Shape_BtnMasu NaruBtnMasu { get; }

        void Paint(
            object sender, PaintEventArgs e,
            ShogiGui shogiGui,
            ILogTag logTag
        );

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 要求：　成る／成らないダイアログボックス
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        ///     0: なし
        ///     1: 成るか成らないかボタンを表示して決定待ち中。
        /// 
        /// </summary>
        bool Requested_NaruDialogToShow { get; }
        void Request_NaruDialogToShow(bool show);

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 動かしたい駒。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        bool SelectFirstTouch { get; set; }

        void SetFigTumandeiruKoma(int value);

        
        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 差し手符号。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        /// <param name="fugo"></param>
        void SetFugo(string fugo);

        void SetHMovedKoma(Finger value);

        void SetMouseStarlightOrNull2(Starlight mouseDd);

        void SetNaruFlag(bool naru);

        void SetNaruMasu(Shape_BtnMasu naruMasu);

        void SetWidget(string name, UserWidget widget);

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 将棋盤
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        Shape_PnlShogiban Shogiban { get; set; }

        void SetSyuturyokuKirikae(SyuturyokuKirikae value);

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [出力切替]
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        SyuturyokuKirikae SyuturyokuKirikae { get; }

        
        
    }
}
