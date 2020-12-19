
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;

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
