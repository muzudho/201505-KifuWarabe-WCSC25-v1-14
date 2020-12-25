//スプライト番号

using System;

namespace Grayscale.Kifuwarazusa.Entities.Features
{
    /// <summary>
    /// 近似測定エンジン。
    /// </summary>
    public abstract class Util_KomanoKyori
    {

        /// <summary>
        /// 距離の近さ
        /// </summary>
        /// <returns></returns>
        public static int GetKyori(SyElement mokuhyo, SyElement genzai)
        {
            //
            // とりあえず　おおざっぱに計算します。
            //

            int mokuhyoDan;
            Util_MasuNum.MasuToDan(mokuhyo, out mokuhyoDan);

            int mokuhyoSuji;
            Util_MasuNum.MasuToSuji(mokuhyo, out mokuhyoSuji);

            int genzaiDan;
            Util_MasuNum.MasuToDan(genzai, out genzaiDan);

            int genzaiSuji;
            Util_MasuNum.MasuToSuji(genzai, out genzaiSuji);

            int kyori = Math.Abs(mokuhyoDan - genzaiDan) + Math.Abs(mokuhyoSuji - genzaiSuji);

            return kyori;
        }

    }
}
