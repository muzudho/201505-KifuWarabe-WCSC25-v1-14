using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L002_GraphicLog;
using Grayscale.P027_SasiteSeisei.L0005_SasiteSeisei;

namespace Grayscale.P027_SasiteSeisei.L100_SasiteSeisei
{

    public class SasiteseiseiArgsImpl : SasiteseiseiArgs
    {
        public bool IsHonshogi { get { return this.isHonshogi; } }
        private bool isHonshogi;

        public int[] YomuLimitter { get { return this.yomuLimitter; } }
        private int[] yomuLimitter;

        public GraphicalLog_File LogF_moveKiki { get { return this.logF_moveKiki; } }
        private GraphicalLog_File logF_moveKiki;

        public SasiteseiseiArgsImpl(
            bool isHonshogi,
            int[] yomuLimitter,
            GraphicalLog_File logF_moveKiki
            )
        {
            this.isHonshogi = isHonshogi;
            this.yomuLimitter = yomuLimitter;
            this.logF_moveKiki = logF_moveKiki;
        }
    }

}
