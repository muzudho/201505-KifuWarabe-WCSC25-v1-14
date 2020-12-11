using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grayscale.P025_KifuLarabe.L00012_Atom
{
    /// <summary>
    /// ************************************************************************************************************************
    /// 駒の動作(*1)です。
    /// ************************************************************************************************************************
    /// 
    ///         *1…寄る、引く、上がる、非表記。
    /// 
    /// </summary>
    public enum AgaruHiku
    {
        /// <summary>
        /// 寄
        /// </summary>
        Yoru,

        /// <summary>
        /// 引く
        /// </summary>
        Hiku,

        /// <summary>
        /// 上がる
        /// </summary>
        Agaru,

        /// <summary>
        /// 非表記。
        /// </summary>
        No_Print
    }
}
