using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.P400_KifuNaraVs.L00048_Engine
{
    public interface ShogiEngineLive
    {

        void Start(string shogiEngineFilePath);

        ShogiEngineManInterface ShogiEngineManInterface { get; set; }

        void ChangeTurn99(KifuTree kifu);

    }
}
