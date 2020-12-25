namespace Grayscale.Kifuwarazusa.Entities.Features
{
    public interface MoveGenArgs
    {

        bool IsHonshogi { get; }
        int[] YomuLimitter { get; }

        GraphicalLog_File LogF_moveKiki { get; }

    }
}
