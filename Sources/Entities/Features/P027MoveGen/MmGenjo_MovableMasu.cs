using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.P027MoveGen.L00025_MovableMove
{
    public interface MmGenjo_MovableMasu
    {
        bool IsHonshogi { get; }

        SkyConst Src_Sky { get; }

        Playerside Pside_genTeban3 { get; }

        bool IsAiteban { get; }
    }
}
