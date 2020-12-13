using Grayscale.P025_KifuLarabe.L00012_Atom;

namespace Grayscale.P027_SasiteSeisei.L0005_SasiteSeisei
{
    public interface SasiteseiseiGenjo
    {

        SasiteseiseiArgs Args { get; }

        /// <summary>
        /// 読み進めている現在の手目
        /// </summary>
        int YomuDeep { get; set; }

        /// <summary>
        /// 読み進めている現在の手目
        /// </summary>
        int Tesumi_yomiCur { get; set; }

        /// <summary>
        /// どちらの手番か。
        /// </summary>
        Playerside Pside_teban { get; set; }

    }
}
