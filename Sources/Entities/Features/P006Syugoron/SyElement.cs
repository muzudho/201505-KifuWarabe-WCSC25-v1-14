namespace Grayscale.Kifuwarazusa.Entities.Features
{

    /// <summary>
    /// 素朴集合論の「要素」。
    /// </summary>
    public interface SyElement
    {
        string Word { get; }

        bool Equals(object obj);
    }
}
