using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.P007_SfenReport.L00025_Report;
using Grayscale.P007_SfenReport.L050_Report;
using System.Drawing;


namespace Grayscale.P007_SfenReport.L00050_Write
{
    public interface KyokumenPngWriter
    {
        /// <summary>
        /// 局面を描きます。
        /// </summary>
        void Paint(Graphics g, ReportArgs args);

    }
}
