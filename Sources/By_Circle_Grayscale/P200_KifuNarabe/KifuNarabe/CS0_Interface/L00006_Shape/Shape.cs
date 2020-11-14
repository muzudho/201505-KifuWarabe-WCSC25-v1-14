using System.Drawing;

namespace Grayscale.P200_KifuNarabe.L00006_Shape
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
