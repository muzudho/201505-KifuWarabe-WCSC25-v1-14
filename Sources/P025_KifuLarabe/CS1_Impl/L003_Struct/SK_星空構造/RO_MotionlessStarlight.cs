using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P025_KifuLarabe.L00012_Atom;

namespace Grayscale.P025_KifuLarabe.L00025_Struct
{

    /// <summary>
    /// リードオンリー駒位置
    /// 
    /// 動かない星の光。
    /// </summary>
    public class RO_MotionlessStarlight : IMoveHalf
    {
        #region プロパティー類

        //public Finger Finger { get{return this.finger;} }
        //private Finger finger;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 先後、升、配役
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public IMoveSource MoveSource { get { return this.now; } }
        protected IMoveSource now;

        #endregion



        /// <summary>
        /// ************************************************************************************************************************
        /// 駒用。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="masu"></param>
        /// <param name="syurui"></param>
        public RO_MotionlessStarlight(IMoveSource nowStar)//Finger finger,
        {
            //this.finger = finger;
            this.now = nowStar;
        }

    }
}
