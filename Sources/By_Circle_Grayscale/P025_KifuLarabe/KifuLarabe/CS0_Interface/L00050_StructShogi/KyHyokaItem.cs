using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.P025_KifuLarabe.L00050_StructShogi
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
