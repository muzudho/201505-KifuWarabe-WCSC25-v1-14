using System.Diagnostics;




namespace Grayscale.Kifuwarazusa.Entities.Features
{


    /// <summary>
    /// 足し算、引き算をしたいときなどに使います。
    /// </summary>
    public abstract class Util_MasuNum
    {
        public const int SHOGIBAN_SIZE_81 = 81;

        #region 整数変換(基礎)

        /// <summary>
        /// 将棋盤、駒台に筋があります。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns>該当なければ-1</returns>
        public static bool MasuToSuji(SyElement masu, out int result)
        {
            bool successful = true;

            Okiba okiba = Util_Masu.GetOkiba(masu);

            switch (okiba)
            {
                case Okiba.ShogiBan:
                    result = (Util_Masu.AsMasuNumber(masu) - Util_Masu.AsMasuNumber(Util_Masu.GetFirstMasuFromOkiba(okiba))) / 9 + 1;
                    break;

                case Okiba.Sente_Komadai:
                case Okiba.Gote_Komadai:
                    result = (Util_Masu.AsMasuNumber(masu) - Util_Masu.AsMasuNumber(Util_Masu.GetFirstMasuFromOkiba(okiba))) / 10 + 1;
                    break;

                case Okiba.KomaBukuro:
                    result = (Util_Masu.AsMasuNumber(masu) - Util_Masu.AsMasuNumber(Util_Masu.GetFirstMasuFromOkiba(okiba))) / 10 + 1;
                    break;

                default:
                    // エラー
                    result = -1;
                    successful = false;
                    goto gt_EndMethod;
            }

        gt_EndMethod:
            return successful;
        }

        /// <summary>
        /// 将棋盤、駒台に筋があります。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns>該当なければ-1</returns>
        public static bool MasuToDan(SyElement masu, out int result)
        {
            bool successful = true;

            Okiba okiba = Util_Masu.GetOkiba(masu);

            switch (okiba)
            {
                case Okiba.ShogiBan:
                    result = (Util_Masu.AsMasuNumber(masu) - Util_Masu.AsMasuNumber(Util_Masu.GetFirstMasuFromOkiba(okiba))) % 9 + 1;
                    break;

                case Okiba.Sente_Komadai:
                case Okiba.Gote_Komadai:
                    result = (Util_Masu.AsMasuNumber(masu) - Util_Masu.AsMasuNumber(Util_Masu.GetFirstMasuFromOkiba(okiba))) % 10 + 1;
                    break;

                case Okiba.KomaBukuro:
                    result = (Util_Masu.AsMasuNumber(masu) - Util_Masu.AsMasuNumber(Util_Masu.GetFirstMasuFromOkiba(okiba))) % 10 + 1;
                    break;

                default:
                    // エラー
                    result = -1;
                    successful = false;
                    goto gt_EndMethod;
            }

        gt_EndMethod:
            return successful;
        }

        public static bool OnAll(int masuHandle)
        {
            return (int)Masu_Honshogi.ban11_１一.MasuNumber <= masuHandle && masuHandle <= (int)Masu_Honshogi.Error.MasuNumber;
        }

        public static bool OnShogiban(int masuHandle)
        {
            return (int)Masu_Honshogi.ban11_１一.MasuNumber <= masuHandle && masuHandle <= (int)Masu_Honshogi.ban99_９九.MasuNumber;
        }

        /// <summary>
        /// 駒台の上なら真。
        /// </summary>
        /// <param name="masuHandle"></param>
        /// <returns></returns>
        public static bool OnKomadai(int masuHandle)
        {
            return (int)Masu_Honshogi.sen01.MasuNumber <= masuHandle && masuHandle <= (int)Masu_Honshogi.go40.MasuNumber;
        }

        public static bool OnSenteKomadai(int masuHandle)
        {
            return (int)Masu_Honshogi.sen01.MasuNumber <= masuHandle && masuHandle <= (int)Masu_Honshogi.sen40.MasuNumber;
        }

        public static bool OnGoteKomadai(int masuHandle)
        {
            return (int)Masu_Honshogi.go01.MasuNumber <= masuHandle && masuHandle <= (int)Masu_Honshogi.go40.MasuNumber;
        }

        public static bool OnKomabukuro(int masuHandle)
        {
            Debug.Assert(Masu_Honshogi.fukuro01.MasuNumber == 161, "fukuro01=[" + Masu_Honshogi.fukuro01 + "]");

            return (int)Masu_Honshogi.fukuro01.MasuNumber <= masuHandle && masuHandle <= (int)Masu_Honshogi.fukuro40.MasuNumber;
        }

        #endregion

    }
}
