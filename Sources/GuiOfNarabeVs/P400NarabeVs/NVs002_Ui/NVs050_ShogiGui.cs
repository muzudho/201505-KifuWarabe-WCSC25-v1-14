
using Grayscale.P200_KifuNarabe.L00048_ShogiGui;
using Grayscale.P400_KifuNaraVs.L00048_Engine;

namespace Grayscale.P400_KifuNaraVs.L002_Ui
{

    public interface NVs_ShogiGui : ShogiGui
    {
        ShogiEngineLive ShogiEnginePrWrapperLauncher { get; }

    }
}
