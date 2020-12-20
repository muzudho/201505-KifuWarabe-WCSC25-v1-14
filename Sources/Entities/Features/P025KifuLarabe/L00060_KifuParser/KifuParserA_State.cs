using Grayscale.Kifuwarazusa.Entities.Features.Gui;

namespace Grayscale.P025_KifuLarabe.L00060_KifuParser
{
    public interface KifuParserA_State
    {

        string Execute(
            ref KifuParserA_Result result,
            IRoomViewModel roomViewModel,
            out KifuParserA_State nextState,
            KifuParserA owner,
            KifuParserA_Genjo genjo
            );

    }
}
