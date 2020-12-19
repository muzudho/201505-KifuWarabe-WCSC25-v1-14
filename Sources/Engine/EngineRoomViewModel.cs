using Grayscale.Kifuwarazusa.Entities.Features.Gui;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;

namespace Grayscale.P050_KifuWarabe.CS1_Impl.W050_UsiLoop
{

    public class EngineRoomViewModel : IRoomViewModel
    {

        public IGameViewModel GameViewModel { get; set; }

        public EngineRoomViewModel(KifuTree kifu)
        {
            this.GameViewModel = new GameViewModel(kifu);
        }

    }

}
