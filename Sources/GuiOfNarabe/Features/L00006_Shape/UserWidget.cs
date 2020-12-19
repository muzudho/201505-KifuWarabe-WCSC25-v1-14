﻿using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using System.Drawing;

namespace Grayscale.P200_KifuNarabe.L00006_Shape
{
    /// <summary>
    /// クリックされたときの動きです。
    /// </summary>
    /// <param name="shape_PnlTaikyoku"></param>
    public delegate void DELEGATE_MouseHitEvent(
         object obj_shogiGui //ShogiGui
        , Shape_BtnKoma btnKoma_Selected
        , ILogTag logTag
    );


    public interface UserWidget
    {
        object Object { get; }
        string Type { get; set; }
        string Name { get; set; }
        void Compile();

        Color BackColor { get; set; }

        DELEGATE_MouseHitEvent Delegate_MouseHitEvent { get; set; }

        bool IsLight_OnFlowB_1TumamitaiKoma { get; set; }

        Rectangle Bounds { get; }
        void SetBounds(Rectangle rect);

        string Text { get; set; }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// フォントサイズ。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        float FontSize { get; set; }

        string Fugo { get; set; }

        /// <summary>
        /// マス用
        /// </summary>
        Okiba Okiba { get; set; }

        /// <summary>
        /// マス用
        /// </summary>
        int Suji { get; set; }

        /// <summary>
        /// マス用
        /// </summary>
        int Dan { get; set; }

        /// <summary>
        /// マス用
        /// </summary>
        int MasuHandle { get; set; }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="g1"></param>
        void Paint(Graphics g1);

        /// <summary>
        /// ************************************************************************************************************************
        /// マウスカーソルに重なっているか否か。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        bool HitByMouse(int x, int y);

        
        /// <summary>
        /// ************************************************************************************************************************
        /// マウスが重なった駒は、光フラグを立てます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void LightByMouse(int x, int y);

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 光
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        bool Light { get; set; }

        
        /// <summary>
        /// ************************************************************************************************************************
        /// 動かしたい駒の解除
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        bool DeselectByMouse(int x, int y, object obj_shogiGui);


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 表示／非表示
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        bool Visible { get; set; }

    }
}