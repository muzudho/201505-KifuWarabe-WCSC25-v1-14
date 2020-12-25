namespace Grayscale.Kifuwarazusa.Entities.Features
{

    /// <summary>
    /// 指定局面限定での評価項目値。
    /// 
    /// 0.0～100.0 の間で、評価を付けます。
    /// </summary>
    public class KyHyoka100limitItemImpl : KyHyokaItem
    {

        public double Score
        {
            get
            {
                double sc = this.srcScore;
                // 上限、下限があります。
                if (100.0d < sc)
                {
                    sc = 100.0d;
                }
                else if (sc < 0.0d)
                {
                    sc = 0.0d;
                }

                return this.keisu * sc;
            }
        }

        /// <summary>
        /// 0.0～100.0 の得点。
        /// </summary>
        private double srcScore;


        /// <summary>
        /// スコアの倍率
        /// </summary>
        private double keisu;

        public string Text { get { return this.text; } }
        private string text;

        public KyHyoka100limitItemImpl(double keisu, double srcScore, string text)
        {
            this.keisu = keisu;
            this.srcScore = srcScore;
            this.text = text;
        }

    }
}
