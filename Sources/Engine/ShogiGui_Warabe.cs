using Grayscale.Kifuwarazusa.Entities.Features.Gui;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;

namespace Grayscale.P050_KifuWarabe.CS1_Impl.W050_UsiLoop
{

    public class ShogiGui_Warabe : IRoomViewModel
    {

        public IGameViewModel GameViewModel { get; set; }

        public ShogiGui_Warabe(KifuTree kifu)
        {
            this.GameViewModel = new Model_PnlTaikyokuImpl(kifu);
        }

    }

}
