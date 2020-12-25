//スプライト番号
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.P050_KifuWarabe.L003_Kokoro;

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
