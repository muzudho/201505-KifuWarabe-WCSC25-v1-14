using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;

namespace Grayscale.P050_KifuWarabe.CS1_Impl.W050_UsiLoop
{

    public class ShogiGui_Warabe : ShogiGui_Base
    {

        public Model_PnlTaikyoku Model_PnlTaikyoku { get; set; }

        public ShogiGui_Warabe(KifuTree kifu)
        {
            this.Model_PnlTaikyoku = new Model_PnlTaikyokuImpl(kifu);
        }

    }

}
