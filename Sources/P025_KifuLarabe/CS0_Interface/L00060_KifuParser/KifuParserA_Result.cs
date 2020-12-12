using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L012_Common;

namespace Grayscale.P025_KifuLarabe.L00060_KifuParser
{
    public interface KifuParserA_Result
    {
        Node<IMove, KyokumenWrapper> Out_newNode_OrNull { get; set; }
    }
}
