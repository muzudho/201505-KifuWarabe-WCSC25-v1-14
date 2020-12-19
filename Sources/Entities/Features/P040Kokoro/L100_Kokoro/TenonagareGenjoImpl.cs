using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P040_Kokoro.L00050_Kokoro;

namespace Grayscale.P040_Kokoro.L100_Kokoro
{

    /// <summary>
    /// 頭の隅に置いてある、手の流れ。その目標物など。
    /// </summary>
    public class TenonagareGenjoImpl : TenonagareGenjo
    {
        /// <summary>
        /// 評価計算の名前。
        /// </summary>
        public TenonagareName Name { get { return this.name; } }// set;
        private TenonagareName name;

        /// <summary>
        /// スコアの倍率。
        /// </summary>
        public double ScoreKeisu { get { return this.scoreKeisu; } }
        private double scoreKeisu;

        /// <summary>
        /// どの駒が
        /// </summary>
        public RO_Star_Koma Koma1 { get { return this.koma1; } }// set;
        private RO_Star_Koma koma1;

        /// <summary>
        /// どの駒が＜その２＞
        /// </summary>
        public RO_Star_Koma Koma2 { get { return this.koma2; } }
        private RO_Star_Koma koma2;

        /// <summary>
        /// どのマスを
        /// </summary>
        public Basho Masu { get { return this.masu; } }// set;
        private Basho masu;

        public TenonagareGenjoImpl(TenonagareName name, double scoreKeisu, RO_Star_Koma koma1, RO_Star_Koma koma2, Basho masu)
        {
            this.name = name;
            this.scoreKeisu = scoreKeisu;
            this.koma1 = koma1;
            this.koma2 = koma2;
            this.masu = masu;
        }

    }
}
