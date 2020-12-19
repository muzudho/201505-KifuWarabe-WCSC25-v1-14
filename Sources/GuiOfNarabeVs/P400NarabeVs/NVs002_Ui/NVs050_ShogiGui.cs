using Grayscale.Kifuwarazusa.GuiOfNarabe.Gui;
using Grayscale.P400_KifuNaraVs.L00048_Engine;

namespace Grayscale.P400_KifuNaraVs.L002_Ui
{

    public interface NVs_ShogiGui : NarabeRoomViewModel
    {
        ShogiEngineLive ShogiEnginePrWrapperLauncher { get; }

    }
}
