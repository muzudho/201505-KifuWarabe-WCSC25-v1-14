using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.P027MoveGen.L0005MoveGen
{
    public interface MoveGenArgs
    {

        bool IsHonshogi { get; }
        int[] YomuLimitter { get; }

        GraphicalLog_File LogF_moveKiki { get; }

    }
}
