//スプライト番号
using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.Kifuwarazusa.UseCases.Features
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
