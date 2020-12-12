

using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号


namespace Grayscale.P025_KifuLarabe.L00012_Atom
{

    /// <summary>
    /// 星の光。
    /// 
    /// 通常、今の星の位置を示していますが、拡張バージョンでは、前の星の位置も示すようになります。
    /// </summary>
    public interface IMoveHalf
    {

        //Finger Finger { get; }


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 先後、升、配役
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        IMoveSource MoveSource { get; }

    }
}
