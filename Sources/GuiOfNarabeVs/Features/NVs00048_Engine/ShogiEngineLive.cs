using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.Kifuwarazusa.GuiOfNarabeVs.Features.Features
{
    public interface ShogiEngineLive
    {

        void Start(string shogiEngineFilePath);

        ShogiEngineManInterface ShogiEngineManInterface { get; set; }

        void ChangeTurn99(KifuTree kifu);

    }
}
