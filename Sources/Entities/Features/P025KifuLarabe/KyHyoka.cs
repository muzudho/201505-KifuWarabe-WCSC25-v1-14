using System.Collections.Generic;

namespace Grayscale.Kifuwarazusa.Entities.Features
{

    /// <summary>
    /// 局面評価の明細。
    /// </summary>
    public interface KyHyoka
    {

        /// <summary>
        /// 全項目。
        /// </summary>
        Dictionary<string, KyHyokaItem> Items { get; }

        void Add(string name, KyHyokaItem item);
        KyHyokaItem Get(string name);
        void Clear();
        double Total();

    }
}
