namespace Grayscale.Kifuwarazusa.UseCases.Gui
{
    using Grayscale.Kifuwarazusa.Entities.Features.Gui;
    using Grayscale.P025_KifuLarabe.L00050_StructShogi;

    public class DefaultRoomViewModel : IRoomViewModel
    {

        public IGameViewModel GameViewModel { get; set; }

        public DefaultRoomViewModel(KifuTree kifu)
        {
            this.GameViewModel = new GameViewModel(kifu);
        }

    }

}
