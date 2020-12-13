using Grayscale.P027_SasiteSeisei.L0005_SasiteSeisei;
using Grayscale.P025_KifuLarabe.L00012_Atom;

namespace Grayscale.P027_SasiteSeisei.L100_SasiteSeisei
{
    public class SasiteseiseiGenjoImpl : SasiteseiseiGenjo
    {


        public SasiteseiseiArgs Args { get { return this.yomiArgs; } }
        private SasiteseiseiArgs yomiArgs;

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

        public SasiteseiseiGenjoImpl(SasiteseiseiArgs yomiArgs, int yomuDeep, int tesumi_yomiCur, Playerside pside_teban)
        {
            this.yomiArgs = yomiArgs;
            this.YomuDeep = yomuDeep;
            this.Tesumi_yomiCur = tesumi_yomiCur;
            this.Pside_teban = pside_teban;
        }

    }
}
