using Grayscale.Kifuwarazusa.Entities.Features.Gui;

namespace Grayscale.P025_KifuLarabe.L00060_KifuParser
{
    public interface KifuParserA_State
    {

        string Execute(
            ref KifuParserA_Result result,
            IRoomViewModel obj_shogiGui_Base,
            out KifuParserA_State nextState,
            KifuParserA owner,
            KifuParserA_Genjo genjo,
            KifuParserA_Log log
            );

    }
}
