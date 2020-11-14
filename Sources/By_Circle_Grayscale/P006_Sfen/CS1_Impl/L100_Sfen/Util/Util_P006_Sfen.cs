using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.P006_Sfen.L100_Sfen.Util
{


    public abstract class Util_P006_Sfen
    {
        /// <summary>
        /// 本将棋の将棋盤の筋、段を、升番号へ変換。
        /// </summary>
        /// <param name="okiba"></param>
        /// <param name="suji"></param>
        /// <param name="dan"></param>
        /// <returns></returns>
        public static int SujiDanToMasu(int suji, int dan)
        {
            int masuHandle = -1;

            if (1 <= suji && suji <= 9 && 1 <= dan && dan <= 9)
            {
                masuHandle = (suji - 1) * 9 + (dan - 1);
            }

            if (masuHandle < 0 || 80 < masuHandle)
            {
                masuHandle = -1;//範囲外が指定されることもあります。
            }

            return masuHandle;
        }
    }


}
