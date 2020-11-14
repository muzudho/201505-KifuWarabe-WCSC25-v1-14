using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.P025_KifuLarabe.L00050_StructShogi
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
