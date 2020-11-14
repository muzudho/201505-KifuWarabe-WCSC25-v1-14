using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.P012_KnowledgeShogi
{
    public class ConverterKnSh
    {

        #region プロパティ類
        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// アラビア数字。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public static string[] ARABIA_SUJI_ZENKAKU = new string[] { "１", "２", "３", "４", "５", "６", "７", "８", "９" };//, "０"
        //public static string[] ARABIA_SUJI_HANKAKU = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };//, "0"

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 漢数字。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public static string[] KAN_SUJI = new string[] { "一", "二", "三", "四", "五", "六", "七", "八", "九" };//,"〇"

        #endregion





        /// <summary>
        /// ************************************************************************************************************************
        /// 数値を漢数字に変換します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string Int_ToKanSuji(int num)
        {
            string numStr;

            if (1 <= num && num <= 9)
            {
                numStr = ConverterKnSh.KAN_SUJI[num - 1];
            }
            else
            {
                numStr = "×";
            }

            return numStr;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 数値をアラビア数字に変換します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string Int_ToArabiaSuji(int num)
        {
            string numStr;

            if (1 <= num && num <= 9)
            {
                numStr = ConverterKnSh.ARABIA_SUJI_ZENKAKU[num - 1];
            }
            else
            {
                numStr = "×";
            }

            return numStr;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// アラビア数字（全角半角）、漢数字を、int型に変換します。変換できなかった場合、0です。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="suji"></param>
        /// <returns></returns>
        public static int Suji_ToInt(string suji)
        {
            int result;

            switch (suji)
            {
                case "1":
                case "１":
                case "一":
                    result = 1;
                    break;

                case "2":
                case "２":
                case "二":
                    result = 2;
                    break;

                case "3":
                case "３":
                case "三":
                    result = 3;
                    break;

                case "4":
                case "４":
                case "四":
                    result = 4;
                    break;

                case "5":
                case "５":
                case "五":
                    result = 5;
                    break;

                case "6":
                case "６":
                case "六":
                    result = 6;
                    break;

                case "7":
                case "７":
                case "七":
                    result = 7;
                    break;

                case "8":
                case "８":
                case "八":
                    result = 8;
                    break;

                case "9":
                case "９":
                case "九":
                    result = 9;
                    break;

                default:
                    result = 0;
                    break;

            }

            return result;
        }

    }
}
