﻿namespace Grayscale.P006Sfen
{
    public abstract class Square
    {
        /// <summary>
        /// 本将棋の将棋盤の筋、段を、升番号へ変換。
        /// </summary>
        /// <param name="okiba"></param>
        /// <param name="suji"></param>
        /// <param name="dan"></param>
        /// <returns></returns>
        public static int From(int suji, int dan)
        {
            int sq = -1;

            if (1 <= suji && suji <= 9 && 1 <= dan && dan <= 9)
            {
                sq = (suji - 1) * 9 + (dan - 1);
            }

            if (sq < 0 || 80 < sq)
            {
                sq = -1;//範囲外が指定されることもあります。
            }

            return sq;
        }
    }


}