using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L002_GraphicLog;

namespace Grayscale.P027_SasiteSeisei.L0005_SasiteSeisei
{
    public interface SasiteseiseiArgs
    {

        bool IsHonshogi { get; }
        int[] YomuLimitter { get; }

        GraphicalLog_File LogF_moveKiki { get; }

    }
}
