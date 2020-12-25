namespace Grayscale.Kifuwarazusa.Entities.Features
{

    /// <summary>
    /// リードオンリー駒位置
    /// 
    /// 動かない星の光。
    /// </summary>
    public class RO_MotionlessStarlight : Starlight
    {
        #region プロパティー類

        //public Finger Finger { get{return this.finger;} }
        //private Finger finger;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 先後、升、配役
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Starlightable Now { get { return this.now; } }
        protected Starlightable now;

        #endregion



        /// <summary>
        /// ************************************************************************************************************************
        /// 駒用。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="masu"></param>
        /// <param name="syurui"></param>
        public RO_MotionlessStarlight(Starlightable nowStar)//Finger finger,
        {
            //this.finger = finger;
            this.now = nowStar;
        }

    }
}
