using Grayscale.Kifuwarazusa.Entities.Features.Gui;

namespace Grayscale.Kifuwarazusa.Entities.Features
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
