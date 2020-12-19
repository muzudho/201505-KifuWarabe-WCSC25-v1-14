using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;

namespace Grayscale.P400_KifuNaraVs.L00048_Engine
{
    public interface ShogiEngineLive
    {

        void Start(string shogiEngineFilePath);

        ShogiEngineManInterface ShogiEngineManInterface { get; set; }

        void ChangeTurn99(KifuTree kifu, ILogTag logTag);

    }
}
