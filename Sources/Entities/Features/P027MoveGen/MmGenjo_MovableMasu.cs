namespace Grayscale.Kifuwarazusa.Entities.Features
{
    public interface MmGenjo_MovableMasu
    {
        bool IsHonshogi { get; }

        SkyConst Src_Sky { get; }

        Playerside Pside_genTeban3 { get; }

        bool IsAiteban { get; }
    }
}
