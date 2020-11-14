using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P025_KifuLarabe.L012_Common
{
    public abstract class Util_Finger
    {

        /// <summary>
        /// スプライト番号が、本将棋用か否かを判定します。
        /// </summary>
        /// <param name="finger_honshogi"></param>
        /// <returns></returns>
        public static bool ForHonshogi(Finger finger_honshogi)
        {
            return 0 <= (int)finger_honshogi && (int)finger_honshogi <= 39;
        }

    }
}
