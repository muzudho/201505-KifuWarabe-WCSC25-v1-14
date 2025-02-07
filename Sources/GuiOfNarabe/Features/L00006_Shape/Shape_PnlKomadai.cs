﻿using System.Drawing;

namespace Grayscale.Kifuwarazusa.GuiOfNarabe.Features
{
    public interface Shape_PnlKomadai : Shape
    {


        /// <summary>
        /// ************************************************************************************************************************
        /// 段を指定すると、ｙ座標を返します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="dan"></param>
        /// <returns></returns>
        int DanToY(int dan);

        /// <summary>
        /// ************************************************************************************************************************
        /// 筋を指定すると、ｘ座標を返します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="suji"></param>
        /// <returns></returns>
        int SujiToX(int suji);


        /// <summary>
        /// ************************************************************************************************************************
        /// 駒台の描画は、ここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="g"></param>
        void Paint(Graphics g);

    }
}
