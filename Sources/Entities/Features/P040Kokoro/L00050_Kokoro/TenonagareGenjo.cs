
using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.P040_Kokoro.L00050_Kokoro
{

    /// <summary>
    /// 頭の隅に置いてある、手の流れ。
    /// 
    /// 引数のようなもの。計測や、点数付けに渡されます。
    /// </summary>
    public interface TenonagareGenjo
    {
        /// <summary>
        /// 狙っている「手の流れ」の名前。
        /// 例えば、「KakutouNoHimoduki」とか。
        /// </summary>
        TenonagareName Name { get; }

        /// <summary>
        /// スコアの倍率。
        /// </summary>
        double ScoreKeisu { get; }

        /// <summary>
        /// どのマスを
        /// </summary>
        Basho Masu { get; }

        /// <summary>
        /// どの駒が
        /// </summary>
        RO_Star_Koma Koma1 { get; }

        /// <summary>
        /// どの駒が＜その２＞
        /// </summary>
        RO_Star_Koma Koma2 { get; }// set;

    }


}
