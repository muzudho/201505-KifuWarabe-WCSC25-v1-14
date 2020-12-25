using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.P027MoveGen.L0005MoveGen;

namespace Grayscale.P027MoveGen.L100MoveGen
{

    public class MoveGenArgsImpl : MoveGenArgs
    {
        public bool IsHonshogi { get { return this.isHonshogi; } }
        private bool isHonshogi;

        public int[] YomuLimitter { get { return this.yomuLimitter; } }
        private int[] yomuLimitter;

        public GraphicalLog_File LogF_moveKiki { get { return this.logF_moveKiki; } }
        private GraphicalLog_File logF_moveKiki;

        public MoveGenArgsImpl(
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
