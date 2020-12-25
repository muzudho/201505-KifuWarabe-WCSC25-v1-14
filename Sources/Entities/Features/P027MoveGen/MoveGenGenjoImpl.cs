using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.P027MoveGen.L0005MoveGen;

namespace Grayscale.P027MoveGen.L100MoveGen
{
    public class MoveGenGenjoImpl : MoveGenGenjo
    {


        public MoveGenArgs Args { get { return this.yomiArgs; } }
        private MoveGenArgs yomiArgs;

        /// <summary>
        /// 脳内読み手数
        /// </summary>
        public int YomuDeep { get; set; }

        /// <summary>
        /// 読み進めている現在の手目
        /// </summary>
        public int Tesumi_yomiCur { get; set; }

        /// <summary>
        /// どちらの手番か。
        /// </summary>
        public Playerside Pside_teban { get; set; }

        public MoveGenGenjoImpl(MoveGenArgs yomiArgs, int yomuDeep, int tesumi_yomiCur, Playerside pside_teban)
        {
            this.yomiArgs = yomiArgs;
            this.YomuDeep = yomuDeep;
            this.Tesumi_yomiCur = tesumi_yomiCur;
            this.Pside_teban = pside_teban;
        }

    }
}
