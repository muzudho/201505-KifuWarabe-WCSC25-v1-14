using Grayscale.P025_KifuLarabe;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P045_Atama.L000125_Sokutei;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P040_Kokoro.L00050_Kokoro;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;

namespace Grayscale.P045_Atama.L00025_KyHandan
{

    /// <summary>
    /// 
    /// </summary>
    public interface KyHandanArgs
    {
        TenonagareGenjo TenonagareGenjo { get; }

        KifuNode Node { get; }

        PlayerInfo PlayerInfo { get; }

    }
}
