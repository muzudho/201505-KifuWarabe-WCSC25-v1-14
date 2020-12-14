using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P200_KifuNarabe.L00006_Shape
{
    public interface Shape_BtnKoma : Shape
    {

        Finger Finger { get; set; }
        Finger Koma { get; set; }

    }
}
