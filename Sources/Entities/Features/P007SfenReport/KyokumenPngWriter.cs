using System.Drawing;


namespace Grayscale.Kifuwarazusa.Entities.Features
{
    public interface KyokumenPngWriter
    {
        /// <summary>
        /// 局面を描きます。
        /// </summary>
        void Paint(Graphics g, ReportArgs args);

    }
}
