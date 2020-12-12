using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;

namespace Grayscale.P027MoveGen.L00025_MovableMove
{
    public interface MmGenjo_MovableMasu
    {
        bool IsHonshogi { get; }

        SkyConst Src_Sky { get; }

        Playerside Pside_genTeban3 { get; }

        bool IsAiteban { get; }
    }
}
