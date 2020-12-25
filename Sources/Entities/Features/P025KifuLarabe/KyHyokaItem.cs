namespace Grayscale.Kifuwarazusa.Entities.Features
{

    /// <summary>
    /// 指定局面限定での評価項目値の明細。
    /// </summary>
    public interface KyHyokaItem
    {

        double Score { get; }

        string Text { get; }
    }
}
