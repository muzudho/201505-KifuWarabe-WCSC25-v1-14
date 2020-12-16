
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
//スプライト番号
using Grayscale.P045_Atama.L000125_Sokutei;
using Grayscale.P050_KifuWarabe.L003_Kokoro;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P040_Kokoro.L00050_Kokoro;

namespace Grayscale.P050_KifuWarabe.L00049_Kokoro
{


    /// <summary>
    /// 頭の隅に置いてある、手の流れ。
    /// </summary>
    public interface Tenonagare : TenonagareGenjo
    {
        /// <summary>
        /// 妄想ログ。
        /// </summary>
        ResultKioku ResultKioku { get; set; }


        Json_Val ToJsonVal();
    }
}
