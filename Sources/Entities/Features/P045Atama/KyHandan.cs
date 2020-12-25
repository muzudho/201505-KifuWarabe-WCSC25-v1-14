namespace Grayscale.Kifuwarazusa.Entities.Features
{

    /// <summary>
    /// 局面評価の判断。
    /// </summary>
    public interface KyHandan
    {

        /// <summary>
        /// 名前。
        /// </summary>
        TenonagareName Name { get; }

        /// <summary>
        /// 0.0d ～100.0d の範囲で、評価値を返します。数字が大きい方がグッドです。
        /// </summary>
        /// <param name="keisanArgs"></param>
        /// <returns></returns>
        void Keisan(out KyHyokaItem kyokumenScore, KyHandanArgs keisanArgs);

    }
}
