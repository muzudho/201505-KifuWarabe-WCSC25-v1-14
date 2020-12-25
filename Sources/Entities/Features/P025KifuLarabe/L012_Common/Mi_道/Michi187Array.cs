using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L007_Random;
using Grayscale.Kifuwarazusa.Entities.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.P025_KifuLarabe.L00012_Atom;

namespace Grayscale.P025_KifuLarabe.L012_Common
{


    /// <summary>
    /// 「1,上一,１九,１八,１七,１六,１五,１四,１三,１二,１一」を、
    /// 「[1]={0,1,2,3,4,5,6,7,8}」に変換して持ちます。
    /// </summary>
    public abstract class Michi187Array
    {

                
        #region 静的プロパティー類

        public static List<SySet<SyElement>> Items
        {
            get
            {
                return Michi187Array.items;
            }
        }
        private static List<SySet<SyElement>> items;

        static Michi187Array()
        {

            //----------
            // 筋１８７
            //----------
            Michi187Array.items = new List<SySet<SyElement>>();
        }
        #endregion



        public static List<List<string>> Load(string path)
        {
            List<List<string>> rows = Util_Csv.ReadCsv(path);



            // 最初の１行は削除。
            rows.RemoveRange(0, 1);



            Michi187Array.Items.Clear();

            // 構文解析は大雑把です。
            // （１）空セルは無視します。
            // （２）「@DEFINE」セルが処理開始の合図です。
            // （３）次のセルには集合の名前です。「味方陣」「平野部」「敵陣」のいずれかです。
            // （４）次のセルは「=」です。
            // （５）次のセルは「{」です。
            // （６）次に「}」セルが出てくるまで、符号のセルが連続します。「１九」「１八」など。
            // （７）「}」セルで、@DEFINEの処理は終了です。
            foreach (List<string> row in rows)
            {
                // ２列目は、道名。
                SySet<SyElement> michi187 = new SySet_Ordered<SyElement>(row[1].Trim());
                SySet<SyElement> michiPart = null;

                // 各行の先頭１列目（連番）と２列目（道名）は削除。
                row.RemoveRange(0, 2);

                bool isPart_Define = false;//@DEFINEパート
                bool isPart_Define_Member = false;//符号パート

                foreach (string cell1 in row)
                {
                    string cell = cell1.Trim();

                    if(cell=="")
                    {
                        goto gt_Next1;
                    }

                    if (isPart_Define)
                    {
                        if (cell == "=")
                        {
                            goto gt_Next1;
                        }

                        if (cell == "{")
                        {
                            isPart_Define_Member = true;
                            goto gt_Next1;
                        }

                        if (cell == "}")
                        {
                            isPart_Define_Member = false;
                            isPart_Define = false;
                            goto gt_Next1;
                        }

                        if (isPart_Define_Member)
                        {
                            // 「１一」を「1」に変換します。
                            SyElement masu81 = Util_Masu.kanjiToEnum[cell];
                            michiPart.AddElement(masu81);
                        }
                        else
                        {
                            switch (cell)
                            {
                                case "味方陣": michiPart = new SySet_Ordered<SyElement>("味方陣"); michi187.AddSupersets(michiPart); goto gt_Next1;
                                case "平野部": michiPart = new SySet_Ordered<SyElement>("平野部"); michi187.AddSupersets(michiPart); goto gt_Next1;
                                case "敵陣": michiPart = new SySet_Ordered<SyElement>("敵陣"); michi187.AddSupersets(michiPart); goto gt_Next1;
                                default: throw new Exception($"未定義のキーワードです。[{cell}]");
                            }
                        }
                    }
                    else
                    {
                        if (cell == "@DEFINE")
                        {
                            isPart_Define = true;
                            goto gt_Next1;
                        }
                    }

                    gt_Next1:
                        ;
                }

                Michi187Array.Items.Add(michi187);
            }

            return rows;
        }

        /// <summary>
        /// ロードした内容を確認するときに使います。
        /// </summary>
        /// <returns></returns>
        public static string LogHtml()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("    <title>道表</title>");
            sb.AppendLine("    <style type=\"text/css\">");
            sb.AppendLine("            /* 将棋盤 */");
            sb.AppendLine("            table{");
            sb.AppendLine("                border-collapse:collapse;");
            sb.AppendLine("                border:2px #2b2b2b solid;");
            sb.AppendLine("            }");
            sb.AppendLine("            td{");
            sb.AppendLine("                border:1px #2b2b2b solid;");
            sb.AppendLine("                background-color:#ffcc55;");
            sb.AppendLine("                width:48px; height:48px;");
            sb.AppendLine("            }");
            sb.AppendLine("    </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");


            foreach (SySet<SyElement> michi in Michi187Array.Items)
            {
                sb.Append("<h1>「");
                sb.Append(michi.Word);
                sb.AppendLine("」</h1>");

                // そのマスに振る、順番の番号。「M 味方陣」「H 平野部」「T 敵陣」で分けてあります。
                Dictionary<SyElement, int> orderOnBanM = new Dictionary<SyElement, int>();
                Dictionary<SyElement, int> orderOnBanH = new Dictionary<SyElement, int>();
                Dictionary<SyElement, int> orderOnBanT = new Dictionary<SyElement, int>();

                int order = 1;
                foreach (SySet<SyElement>superset in michi.Supersets)
                {
                    switch(superset.Word)
                    {
                        case "味方陣":
                            {
                                foreach (SyElement masu in superset.Elements)
                                {
                                    orderOnBanM.Add(masu, order);
                                    order++;
                                }
                            }
                            break;
                        case "平野部":
                            {
                                foreach (SyElement masu in superset.Elements)
                                {
                                    orderOnBanH.Add(masu, order);
                                    order++;
                                }
                            }
                            break;
                        case "敵陣":
                            {
                                foreach (SyElement masu in superset.Elements)
                                {
                                    orderOnBanT.Add(masu, order);
                                    order++;
                                }
                            }
                            break;
                        default:
                            throw new Exception($"未定義の集合名です。[{superset.Word}]");
                    }
                }


                

                sb.Append("<table>");
                // ９一～１一、９二～１二、…９九～１九の順だぜ☆
                for (int dan = 1; dan <= 9; dan++)
                {
                    sb.AppendLine("<tr>");

                    sb.Append("    ");
                    for (int suji = 9; suji >= 1; suji--)
                    {

                        SyElement masu = Util_Masu.OkibaSujiDanToMasu(Okiba.ShogiBan, suji, dan);

                        if (orderOnBanM.ContainsKey(masu))
                        {
                            // 順番が記されている味方陣マス
                            sb.Append("<td style=\"text-align:center; background-color:blue;\">");
                            sb.Append(orderOnBanM[masu]);
                            sb.Append("</td>");
                        }
                        else if (orderOnBanH.ContainsKey(masu))
                        {
                            // 順番が記されている平野部マス
                            sb.Append("<td style=\"text-align:center; background-color:green;\">");
                            sb.Append(orderOnBanH[masu]);
                            sb.Append("</td>");
                        }
                        else if (orderOnBanT.ContainsKey(masu))
                        {
                            // 順番が記されている敵陣マス
                            sb.Append("<td style=\"text-align:center; background-color:red;\">");
                            sb.Append(orderOnBanT[masu]);
                            sb.Append("</td>");
                        }
                        else
                        {
                            // 特に指定のないマス。
                            sb.Append("<td></td>");
                        }

                    }
                    sb.AppendLine();
                    sb.AppendLine("</tr>");

                }
                sb.AppendLine("</table>");
            }


            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }

    }
}
