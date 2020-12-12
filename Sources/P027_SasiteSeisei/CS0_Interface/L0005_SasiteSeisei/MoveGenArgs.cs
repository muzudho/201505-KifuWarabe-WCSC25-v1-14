using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L002_GraphicLog;

namespace Grayscale.P027MoveGen.L0005MoveGen
{
    public interface MoveGenArgs
    {

        bool IsHonshogi { get; }
        int[] YomuLimitter { get; }

        GraphicalLog_File LogF_moveKiki { get; }

    }
}
