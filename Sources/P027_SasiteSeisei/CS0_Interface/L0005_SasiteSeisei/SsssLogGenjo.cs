using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.P025_KifuLarabe.L00025_Struct;

namespace Grayscale.P027_SasiteSeisei.L0005_SasiteSeisei
{
    public interface SsssLogGenjo
    {

        Boolean EnableLog { get; }
        LarabeLoggerable LogTag { get; }

    }
}
