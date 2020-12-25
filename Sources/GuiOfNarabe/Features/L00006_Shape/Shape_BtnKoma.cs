using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwarazusa.GuiOfNarabe.Features
{
    public interface Shape_BtnKoma : Shape
    {

        Finger Finger { get; set; }
        Finger Koma { get; set; }

    }
}
