using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.P006_Syugoron
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
