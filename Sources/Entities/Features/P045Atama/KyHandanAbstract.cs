namespace Grayscale.Kifuwarazusa.Entities.Features
{

    /// <summary>
    /// 局面の得点計算。
    /// </summary>
    public abstract class KyHandanAbstract : KyHandan
    {

        public TenonagareName Name
        {
            get
            {
                return this.name;
            }
        }
        private TenonagareName name;

        public KyHandanAbstract(TenonagareName name)
        {
            this.name = name;
        }

        /// <summary>
        /// 0.0d ～100.0d の範囲で、評価値を返します。数字が大きい方がグッドです。
        /// </summary>
        /// <param name="keisanArgs"></param>
        /// <returns></returns>
        abstract public void Keisan(out KyHyokaItem kyokumenScore, KyHandanArgs keisanArgs);

    }
}
