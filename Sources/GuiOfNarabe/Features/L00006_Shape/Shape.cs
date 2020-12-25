using System.Drawing;

namespace Grayscale.Kifuwarazusa.GuiOfNarabe.Features
{
    public interface Shape
    {

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 位置とサイズ
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        Rectangle Bounds { get; }

        void SetBounds(Rectangle rect);

    }
}
