namespace Grayscale.Kifuwarazusa.Entities.Features.Gui
{
    public interface IGameViewModel
    {

        void SetGuiSky(SkyConst sky);

        KifuTree Kifu { get; }

        int GuiTesumi { get; set; }

        SkyConst GuiSkyConst { get; }

        Playerside GuiPside { get; set; }

    }
}
