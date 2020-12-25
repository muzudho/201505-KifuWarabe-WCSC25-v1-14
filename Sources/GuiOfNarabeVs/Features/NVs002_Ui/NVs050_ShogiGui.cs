using Grayscale.Kifuwarazusa.GuiOfNarabe.Gui;

namespace Grayscale.Kifuwarazusa.GuiOfNarabeVs.Features.Features
{

    public interface NVs_ShogiGui : NarabeRoomViewModel
    {
        ShogiEngineLive ShogiEnginePrWrapperLauncher { get; }

    }
}
