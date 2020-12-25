using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using System.Collections.Generic;

/*
     /// <summary>
    /// ------------------------------------------------------------------------------------------------------------------------
    /// 枡ハンドルの一覧。
    /// ------------------------------------------------------------------------------------------------------------------------
    /// 
    /// ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐
    /// │72│63│54│45│36│27│18│ 9│ 0│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │73│64│55│46│37│28│19│10│ 1│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │74│65│56│47│38│29│20│11│ 2│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │75│66│57│48│39│30│21│12│ 3│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │76│67│58│49│40│31│22│13│ 4│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │77│68│59│50│41│32│23│14│ 5│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │78│69│60│51│42│33│24│15│ 6│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │79│70│61│52│43│34│25│16│ 7│
    /// ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
    /// │80│71│62│53│44│35│26│17│ 8│
    /// └─┴─┴─┴─┴─┴─┴─┴─┴─┘
    /// 先手駒台：81～120
    /// 後手駒台：121～160
    /// 駒袋：161～200
    /// エラー：201
    /// の、計202。
    /// 
    /// 将棋盤上の枡のリスト。
    /// 
    /// ・Add、Removeといった、データ構造に縛られるメソッドは持たせません。
    ///   変わりに、Minus といった汎用的に操作できるメソッドを持たせます。
    /// 
    /// ・Clearメソッドは持たせません。インスタンスを作り直して親要素にセットし直してください。
    ///   空にすることができないオブジェクト（線分など）があることが理由です。
    /// </summary>

 */

namespace Grayscale.P025_KifuLarabe.L004_StructShogi
{
    public abstract class Util_Masu
    {

        /// <summary>
        /// 先手駒台は40マス、後手駒台は40マス、駒袋は40マスです。
        /// </summary>
        public const int KOMADAI_KOMABUKURO_SPACE_LENGTH = 40;

        public const int KOMADAI_LAST_SUJI = 4;
        public const int KOMADAI_LAST_DAN = 10;

        public const int SHOGIBAN_LAST_SUJI = 9;
        public const int SHOGIBAN_LAST_DAN = 9;


        /// <summary>
        /// 漢字符号→列挙型変換用
        /// 
        /// 将棋盤の８１マスの符号だぜ☆　０～８０の、８１個の連番を振っているぜ☆
        /// </summary>
        public static Dictionary<string, SyElement> kanjiToEnum;




        static Util_Masu()
        {
            Util_Masu.kanjiToEnum = new Dictionary<string, SyElement>();
            Util_Masu.kanjiToEnum.Add("１一", Masu_Honshogi.ban11_１一);
            Util_Masu.kanjiToEnum.Add("１二", Masu_Honshogi.ban12_１二);
            Util_Masu.kanjiToEnum.Add("１三", Masu_Honshogi.ban13_１三);
            Util_Masu.kanjiToEnum.Add("１四", Masu_Honshogi.ban14_１四);
            Util_Masu.kanjiToEnum.Add("１五", Masu_Honshogi.ban15_１五);
            Util_Masu.kanjiToEnum.Add("１六", Masu_Honshogi.ban16_１六);
            Util_Masu.kanjiToEnum.Add("１七", Masu_Honshogi.ban17_１七);
            Util_Masu.kanjiToEnum.Add("１八", Masu_Honshogi.ban18_１八);
            Util_Masu.kanjiToEnum.Add("１九", Masu_Honshogi.ban19_１九);
            Util_Masu.kanjiToEnum.Add("２一", Masu_Honshogi.ban21_２一);
            Util_Masu.kanjiToEnum.Add("２二", Masu_Honshogi.ban22_２二);
            Util_Masu.kanjiToEnum.Add("２三", Masu_Honshogi.ban23_２三);
            Util_Masu.kanjiToEnum.Add("２四", Masu_Honshogi.ban24_２四);
            Util_Masu.kanjiToEnum.Add("２五", Masu_Honshogi.ban25_２五);
            Util_Masu.kanjiToEnum.Add("２六", Masu_Honshogi.ban26_２六);
            Util_Masu.kanjiToEnum.Add("２七", Masu_Honshogi.ban27_２七);
            Util_Masu.kanjiToEnum.Add("２八", Masu_Honshogi.ban28_２八);
            Util_Masu.kanjiToEnum.Add("２九", Masu_Honshogi.ban29_２九);
            Util_Masu.kanjiToEnum.Add("３一", Masu_Honshogi.ban31_３一);
            Util_Masu.kanjiToEnum.Add("３二", Masu_Honshogi.ban32_３二);
            Util_Masu.kanjiToEnum.Add("３三", Masu_Honshogi.ban33_３三);
            Util_Masu.kanjiToEnum.Add("３四", Masu_Honshogi.ban34_３四);
            Util_Masu.kanjiToEnum.Add("３五", Masu_Honshogi.ban35_３五);
            Util_Masu.kanjiToEnum.Add("３六", Masu_Honshogi.ban36_３六);
            Util_Masu.kanjiToEnum.Add("３七", Masu_Honshogi.ban37_３七);
            Util_Masu.kanjiToEnum.Add("３八", Masu_Honshogi.ban38_３八);
            Util_Masu.kanjiToEnum.Add("３九", Masu_Honshogi.ban39_３九);
            Util_Masu.kanjiToEnum.Add("４一", Masu_Honshogi.ban41_４一);
            Util_Masu.kanjiToEnum.Add("４二", Masu_Honshogi.ban42_４二);
            Util_Masu.kanjiToEnum.Add("４三", Masu_Honshogi.ban43_４三);
            Util_Masu.kanjiToEnum.Add("４四", Masu_Honshogi.ban44_４四);
            Util_Masu.kanjiToEnum.Add("４五", Masu_Honshogi.ban45_４五);
            Util_Masu.kanjiToEnum.Add("４六", Masu_Honshogi.ban46_４六);
            Util_Masu.kanjiToEnum.Add("４七", Masu_Honshogi.ban47_４七);
            Util_Masu.kanjiToEnum.Add("４八", Masu_Honshogi.ban48_４八);
            Util_Masu.kanjiToEnum.Add("４九", Masu_Honshogi.ban49_４九);
            Util_Masu.kanjiToEnum.Add("５一", Masu_Honshogi.ban51_５一);
            Util_Masu.kanjiToEnum.Add("５二", Masu_Honshogi.ban52_５二);
            Util_Masu.kanjiToEnum.Add("５三", Masu_Honshogi.ban53_５三);
            Util_Masu.kanjiToEnum.Add("５四", Masu_Honshogi.ban54_５四);
            Util_Masu.kanjiToEnum.Add("５五", Masu_Honshogi.ban55_５五);
            Util_Masu.kanjiToEnum.Add("５六", Masu_Honshogi.ban56_５六);
            Util_Masu.kanjiToEnum.Add("５七", Masu_Honshogi.ban57_５七);
            Util_Masu.kanjiToEnum.Add("５八", Masu_Honshogi.ban58_５八);
            Util_Masu.kanjiToEnum.Add("５九", Masu_Honshogi.ban59_５九);
            Util_Masu.kanjiToEnum.Add("６一", Masu_Honshogi.ban61_６一);
            Util_Masu.kanjiToEnum.Add("６二", Masu_Honshogi.ban62_６二);
            Util_Masu.kanjiToEnum.Add("６三", Masu_Honshogi.ban63_６三);
            Util_Masu.kanjiToEnum.Add("６四", Masu_Honshogi.ban64_６四);
            Util_Masu.kanjiToEnum.Add("６五", Masu_Honshogi.ban65_６五);
            Util_Masu.kanjiToEnum.Add("６六", Masu_Honshogi.ban66_６六);
            Util_Masu.kanjiToEnum.Add("６七", Masu_Honshogi.ban67_６七);
            Util_Masu.kanjiToEnum.Add("６八", Masu_Honshogi.ban68_６八);
            Util_Masu.kanjiToEnum.Add("６九", Masu_Honshogi.ban69_６九);
            Util_Masu.kanjiToEnum.Add("７一", Masu_Honshogi.ban71_７一);
            Util_Masu.kanjiToEnum.Add("７二", Masu_Honshogi.ban72_７二);
            Util_Masu.kanjiToEnum.Add("７三", Masu_Honshogi.ban73_７三);
            Util_Masu.kanjiToEnum.Add("７四", Masu_Honshogi.ban74_７四);
            Util_Masu.kanjiToEnum.Add("７五", Masu_Honshogi.ban75_７五);
            Util_Masu.kanjiToEnum.Add("７六", Masu_Honshogi.ban76_７六);
            Util_Masu.kanjiToEnum.Add("７七", Masu_Honshogi.ban77_７七);
            Util_Masu.kanjiToEnum.Add("７八", Masu_Honshogi.ban78_７八);
            Util_Masu.kanjiToEnum.Add("７九", Masu_Honshogi.ban79_７九);
            Util_Masu.kanjiToEnum.Add("８一", Masu_Honshogi.ban81_８一);
            Util_Masu.kanjiToEnum.Add("８二", Masu_Honshogi.ban82_８二);
            Util_Masu.kanjiToEnum.Add("８三", Masu_Honshogi.ban83_８三);
            Util_Masu.kanjiToEnum.Add("８四", Masu_Honshogi.ban84_８四);
            Util_Masu.kanjiToEnum.Add("８五", Masu_Honshogi.ban85_８五);
            Util_Masu.kanjiToEnum.Add("８六", Masu_Honshogi.ban86_８六);
            Util_Masu.kanjiToEnum.Add("８七", Masu_Honshogi.ban87_８七);
            Util_Masu.kanjiToEnum.Add("８八", Masu_Honshogi.ban88_８八);
            Util_Masu.kanjiToEnum.Add("８九", Masu_Honshogi.ban89_８九);
            Util_Masu.kanjiToEnum.Add("９一", Masu_Honshogi.ban91_９一);
            Util_Masu.kanjiToEnum.Add("９二", Masu_Honshogi.ban92_９二);
            Util_Masu.kanjiToEnum.Add("９三", Masu_Honshogi.ban93_９三);
            Util_Masu.kanjiToEnum.Add("９四", Masu_Honshogi.ban94_９四);
            Util_Masu.kanjiToEnum.Add("９五", Masu_Honshogi.ban95_９五);
            Util_Masu.kanjiToEnum.Add("９六", Masu_Honshogi.ban96_９六);
            Util_Masu.kanjiToEnum.Add("９七", Masu_Honshogi.ban97_９七);
            Util_Masu.kanjiToEnum.Add("９八", Masu_Honshogi.ban98_９八);
            Util_Masu.kanjiToEnum.Add("９九", Masu_Honshogi.ban99_９九);
        }


        public static int AsMasuNumber(SyElement syElm)
        {
            int result;

            if (syElm is Basho)
            {
                result = ((Basho)syElm).MasuNumber;
            }
            else
            {
                result = Masu_Honshogi.Error.MasuNumber;
            }

            return result;
        }

        public static SyElement HandleToMasu(int masuHandle)
        {
            SyElement masu;

            if (
                !Util_Masu.Yuko(masuHandle)
            )
            {
                masu = Masu_Honshogi.Error;
            }
            else
            {
                masu = Masu_Honshogi.Items_All[masuHandle];
            }

            return masu;
        }


        public static SyElement OkibaSujiDanToMasu(Okiba okiba, int suji, int dan)
        {
            int masuHandle = -1;

            switch(okiba)
            {
                case Okiba.ShogiBan:
                    if (1 <= suji && suji <= Util_Masu.SHOGIBAN_LAST_SUJI && 1 <= dan && dan <= Util_Masu.SHOGIBAN_LAST_DAN)
                    {
                        masuHandle = (suji - 1) * Util_Masu.SHOGIBAN_LAST_DAN + (dan - 1);
                    }
                    break;

                case Okiba.Sente_Komadai:
                case Okiba.Gote_Komadai:
                case Okiba.KomaBukuro:
                    if (1 <= suji && suji <= Util_Masu.KOMADAI_LAST_SUJI && 1 <= dan && dan <= Util_Masu.KOMADAI_LAST_DAN)
                    {
                        masuHandle = (suji - 1) * Util_Masu.KOMADAI_LAST_DAN + (dan - 1);
                        masuHandle += Util_Masu.AsMasuNumber(Util_Masu.GetFirstMasuFromOkiba(okiba));
                    }
                    break;

                default:
                    break;
            }


            SyElement masu = Masu_Honshogi.Error;//範囲外が指定されることもあります。

            if (Util_Masu.Yuko(masuHandle))
            {
                masu = Masu_Honshogi.Items_All[masuHandle];
            }


            return masu;
        }

        public static SyElement OkibaSujiDanToMasu(Okiba okiba, int masuHandle)
        {
            switch (Util_Masu.GetOkiba(Masu_Honshogi.Items_All[masuHandle]))
            {
                case Okiba.Sente_Komadai:
                    masuHandle -= Util_Masu.AsMasuNumber(Util_Masu.GetFirstMasuFromOkiba(Okiba.Sente_Komadai));
                    break;

                case Okiba.Gote_Komadai:
                    masuHandle -= Util_Masu.AsMasuNumber(Util_Masu.GetFirstMasuFromOkiba(Okiba.Gote_Komadai));
                    break;

                case Okiba.KomaBukuro:
                    masuHandle -= Util_Masu.AsMasuNumber(Util_Masu.GetFirstMasuFromOkiba(Okiba.KomaBukuro));
                    break;

                case Okiba.ShogiBan:
                    // そのんまま
                    break;

                default:
                    // エラー
                    break;
            }

            masuHandle = masuHandle + Util_Masu.AsMasuNumber(Util_Masu.GetFirstMasuFromOkiba(okiba));

            return Util_Masu.HandleToMasu( masuHandle);
        }


        #region 列挙型変換

        public static SyElement GetFirstMasuFromOkiba(Okiba okiba)
        {
            SyElement firstMasu;

            switch (okiba)
            {
                case Okiba.ShogiBan:
                    firstMasu = Masu_Honshogi.ban11_１一;//[0]
                    break;

                case Okiba.Sente_Komadai:
                    firstMasu = Masu_Honshogi.sen01;//[81]
                    break;

                case Okiba.Gote_Komadai:
                    firstMasu = Masu_Honshogi.go01;//[121]
                    break;

                case Okiba.KomaBukuro:
                    firstMasu = Masu_Honshogi.fukuro01;//[161];
                    break;

                default:
                    //エラー
                    firstMasu = Masu_Honshogi.Error;// -1→[201];
                    break;
            }

            return firstMasu;
        }
        #endregion



        #region 範囲妥当性チェック

        public static bool Yuko(int masuHandle)
        {
            return 0 <= masuHandle && masuHandle <= 201;
        }

        #endregion



        #region 一致判定(性質判定)

        public static Okiba GetOkiba(SyElement masu)
        {
            Okiba okiba;

            int masuHandle = Util_Masu.AsMasuNumber(masu);

            if (0 <= masuHandle && masuHandle <= 80)
            {
                // 将棋盤
                okiba = Okiba.ShogiBan;
            }
            else if (81 <= masuHandle && masuHandle <= 120)
            {
                // 先手駒台
                okiba = Okiba.Sente_Komadai;
            }
            else if (121 <= masuHandle && masuHandle <= 160)
            {
                // 後手駒台
                okiba = Okiba.Gote_Komadai;
            }
            else if (161 <= masuHandle && masuHandle <= 200)
            {
                // 駒袋
                okiba = Okiba.KomaBukuro;
            }
            else
            {
                // エラー
                okiba = Okiba.Empty;
            }

            return okiba;
        }

        #endregion








        /// <summary>
        /// ************************************************************************************************************************
        /// 升一致判定。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool MatchSujiDan(int masuHandle, int masuHandle_m2)
        {
            bool result = false;

            result = masuHandle == masuHandle_m2
                && masuHandle == masuHandle_m2;

            return result;
        }






        /// <summary>
        /// ************************************************************************************************************************
        /// １マス上、のように指定して、マスを取得します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="offsetSuji"></param>
        /// <param name="offsetDan"></param>
        /// <returns></returns>
        public static SyElement Offset(Okiba okiba, SyElement masu, Playerside pside, Hogaku muki)
        {
            int offsetSuji;
            int offsetDan;
            Util_Muki.MukiToOffsetSujiDan(muki, pside, out offsetSuji, out offsetDan);

            int suji;
            int dan;
            Util_MasuNum.MasuToSuji(masu, out suji);
            Util_MasuNum.MasuToDan(masu, out dan);

            return Util_Masu.OkibaSujiDanToMasu(
                okiba,
                suji + offsetSuji,
                dan + offsetDan);
        }




        /// <summary>
        /// ************************************************************************************************************************
        /// １マス上、のように指定して、マスを取得します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="offsetSuji"></param>
        /// <param name="offsetDan"></param>
        /// <returns></returns>
        public static SyElement Offset(Okiba okiba, SyElement masu, int offsetSuji, int offsetDan)
        {
            int suji;
            int dan;
            Util_MasuNum.MasuToSuji(masu, out suji);
            Util_MasuNum.MasuToDan(masu, out dan);

            return Util_Masu.OkibaSujiDanToMasu(
                    okiba,
                    suji + offsetSuji,
                    dan + offsetDan
                );
        }


        /// <summary>
        /// 後手からも、先手のような座標で指示できるように変換します。
        /// </summary>
        /// <param name="masu"></param>
        /// <param name="pside"></param>
        /// <returns></returns>
        public static SyElement BothSenteView(SyElement masu, Playerside pside)
        {
            SyElement result = masu;

            // 将棋盤上で後手なら、180°回転します。
            if (Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(masu) && pside == Playerside.P2)
            {
                result = Masu_Honshogi.Items_All[80 - Util_Masu.AsMasuNumber(masu)];
            }

            // 将棋盤で先手、または　駒台か　駒袋なら、指定されたマスにそのまま入れます。

            return result;
        }




        /// <summary>
        /// ************************************************************************************************************************
        /// 相手陣に入っていれば真。
        /// ************************************************************************************************************************
        /// 
        ///         後手は 7,8,9 段。
        ///         先手は 1,2,3 段。
        /// </summary>
        /// <returns></returns>
        public static bool InAitejin(SyElement masu, Playerside pside)
        {
            int dan;
            Util_MasuNum.MasuToDan(masu, out dan);

            return (Playerside.P2 == pside && 7 <= dan)
                || (Playerside.P1 == pside && dan <= 3);
        }


        #region 定数
        //------------------------------------------------------------
        /// <summary>
        /// 筋は 1～9 だけ有効です。
        /// </summary>
        public const int YUKO_SUJI_MIN = 1;
        public const int YUKO_SUJI_MAX = 9;

        /// <summary>
        /// 段は 1～9 だけ有効です。
        /// </summary>
        public const int YUKO_DAN_MIN = 1;
        public const int YUKO_DAN_MAX = 9;
        //------------------------------------------------------------
        #endregion




        public static Okiba Masu_ToOkiba(SyElement masu)
        {
            Okiba result;

            if ((int)Masu_Honshogi.ban11_１一.MasuNumber <= Util_Masu.AsMasuNumber(masu) && Util_Masu.AsMasuNumber(masu) <= (int)Masu_Honshogi.ban99_９九.MasuNumber)
            {
                // 将棋盤
                result = Okiba.ShogiBan;
            }
            else if ((int)Masu_Honshogi.sen01.MasuNumber <= Util_Masu.AsMasuNumber(masu) && Util_Masu.AsMasuNumber(masu) <= (int)Masu_Honshogi.sen40.MasuNumber)
            {
                // 先手駒台
                result = Okiba.Sente_Komadai;
            }
            else if ((int)Masu_Honshogi.go01.MasuNumber <= Util_Masu.AsMasuNumber(masu) && Util_Masu.AsMasuNumber(masu) <= (int)Masu_Honshogi.go40.MasuNumber)
            {
                // 後手駒台
                result = Okiba.Gote_Komadai;
            }
            else if ((int)Masu_Honshogi.fukuro01.MasuNumber <= Util_Masu.AsMasuNumber(masu) && Util_Masu.AsMasuNumber(masu) <= (int)Masu_Honshogi.fukuro40.MasuNumber)
            {
                // 駒袋
                result = Okiba.KomaBukuro;
            }
            else
            {
                // 該当なし
                result = Okiba.Empty;
            }

            return result;
        }


    }
}
