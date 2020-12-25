using System.Text;


namespace Grayscale.Kifuwarazusa.Entities.Features
{
    public abstract class Logger_Masus
    {

        /// <summary>
        /// デバッグ用文字列を作ります。
        /// </summary>
        /// <param name="masus"></param>
        /// <param name="memo"></param>
        /// <returns></returns>
        public static string Log_Masus(SySet<SyElement> masus, string memo)
        {
            StringBuilder sb = new StringBuilder();

            int errorCount = 0;

            // フォルスクリア
            bool[] ban81 = new bool[81];

            // フラグ立て
            foreach (Basho hMasu in masus.Elements)
            {
                if (Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(Masu_Honshogi.Items_All[hMasu.MasuNumber]))
                {
                    ban81[hMasu.MasuNumber] = true;
                }
            }



            sb.AppendLine("...(^▽^)さて、局面は☆？");

            if (null != memo && "" != memo.Trim())
            {
                sb.AppendLine(memo);
            }

            sb.AppendLine("　９　８　７　６　５　４　３　２　１");
            sb.AppendLine("┏━┯━┯━┯━┯━┯━┯━┯━┯━┓");
            for (int dan = 1; dan <= 9; dan++)
            {
                sb.Append("┃");
                for (int suji = 9; suji >= 1; suji--)// 筋は左右逆☆
                {
                    SyElement masu = Util_Masu.OkibaSujiDanToMasu(Okiba.ShogiBan, suji, dan);
                    if (Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(masu))
                    {
                        if (ban81[Util_Masu.AsMasuNumber(masu)])
                        {
                            sb.Append("●");
                        }
                        else
                        {
                            sb.Append("  ");
                        }
                    }
                    else
                    {
                        errorCount++;
                        sb.Append("  ");
                    }


                    if (suji == 1)//１筋が最後だぜ☆
                    {
                        sb.Append("┃");
                        sb.AppendLine(ConverterKnSh.Int_ToKanSuji(dan));
                    }
                    else
                    {
                        sb.Append("│");
                    }
                }

                if (dan == 9)
                {
                    sb.AppendLine("┗━┷━┷━┷━┷━┷━┷━┷━┷━┛");
                }
                else
                {
                    sb.AppendLine("┠─┼─┼─┼─┼─┼─┼─┼─┼─┨");
                }
            }


            // 後手駒台
            sb.Append("エラー数：");
            sb.AppendLine(errorCount.ToString());
            sb.AppendLine("...(^▽^)ﾄﾞｳﾀﾞｯﾀｶﾅ～☆");


            return sb.ToString();
        }

    }
}
