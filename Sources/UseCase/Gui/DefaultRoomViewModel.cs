namespace Grayscale.Kifuwarazusa.UseCases.Gui
{
    using Grayscale.Kifuwarazusa.Entities.Features;
    using Grayscale.Kifuwarazusa.Entities.Features.Gui;

    public class DefaultRoomViewModel : IRoomViewModel
    {

        public IGameViewModel GameViewModel { get; set; }

        public DefaultRoomViewModel(KifuTree kifu)
        {
            this.GameViewModel = new GameViewModel(kifu);
        }

    }

}
