namespace Grayscale.Kifuwarazusa.Entities.Features
{
    public interface MoveGenGenjo
    {

        MoveGenArgs Args { get; }

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
