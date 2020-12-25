using System.Text.RegularExpressions;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwarazusa.Entities.Features
{
    public class KifuIO_MovesParsers
    {

        /// <summary>
        /// 将棋盤上での検索
        /// </summary>
        /// <param name="srcAll">候補マス</param>
        /// <param name="komas"></param>
        /// <returns></returns>
        public static bool Hit_JfugoParser(
            Playerside pside, Ks14 syurui, SySet<SyElement> srcAll,
            KifuTree kifu,
            out Finger foundKoma)
        {
            SkyConst src_Sky = kifu.CurNode.Value.ToKyokumenConst;

            bool hit = false;
            foundKoma = Fingers.Error_1;


            foreach (Basho masu1 in srcAll.Elements)//筋・段。（先後、種類は入っていません）
            {
                foreach (Finger koma1 in Finger_Honshogi.Items_KomaOnly)
                {
                    RO_Star_Koma koma2 = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(koma1).Now);


                    if (pside == koma2.Pside
                        && Okiba.ShogiBan == Util_Masu.GetOkiba(koma2.Masu)
                        && KomaSyurui14Array.Matches(syurui, Haiyaku184Array.Syurui(koma2.Haiyaku))
                        && masu1 == koma2.Masu
                        )
                    {
                        // 候補マスにいた
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                        hit = true;
                        foundKoma = koma1;
                        break;
                    }
                }
            }

            return hit;
        }

        /// <summary>
        /// テキスト形式の符号「▲７六歩△３四歩▲２六歩…」の最初の要素を、切り取ってプロセスに変換します。
        /// 
        /// [再生]、[コマ送り]で利用。
        /// </summary>
        /// <returns></returns>
        public static bool ParseJfugo_FromText(
            string inputLine,
            out string str1,
            out string str2,
            out string str3,
            out string str4,
            out string str5,
            out string str6,
            out string str7,
            out string str8,
            out string str9,
            out string rest
            )
        {
            //nextTe = null;
            bool successful = false;
            rest = inputLine;

            str1 = "";
            str2 = "";
            str3 = "";
            str4 = "";
            str5 = "";
            str6 = "";
            str7 = "";
            str8 = "";
            str9 = "";

            //------------------------------------------------------------
            // リスト作成
            //------------------------------------------------------------
            Regex regex = new Regex(
                @"^\s*([▲△]?)(?:([123456789１２３４５６７８９])([123456789１２３４５６７８９一二三四五六七八九]))?(同)?[\s　]*(歩|香|桂|銀|金|飛|角|王|玉|と|成香|成桂|成銀|竜|龍|馬)(右|左|直)?(寄|引|上)?(成|不成)?(打?)",
                RegexOptions.Singleline
            );

            MatchCollection mc = regex.Matches(inputLine);
            foreach (Match m in mc)
            {
                if (0 < m.Groups.Count)
                {
                    successful = true;

                    // 残りのテキスト
                    rest = inputLine.Substring(0, m.Index) + inputLine.Substring(m.Index + m.Length, inputLine.Length - (m.Index + m.Length));

                    str1 = m.Groups[1].Value;
                    str2 = m.Groups[2].Value;
                    str3 = m.Groups[3].Value;
                    str4 = m.Groups[4].Value;
                    str5 = m.Groups[5].Value;
                    str6 = m.Groups[6].Value;
                    str7 = m.Groups[7].Value;
                    str8 = m.Groups[8].Value;
                    str9 = m.Groups[9].Value;
                }

                // 最初の１件だけ処理して終わります。
                break;
            }

            rest = rest.Trim();


            return successful;
        }




        /// <summary>
        /// テキスト形式の符号「7g7f 3c3d 6g6f…」の最初の要素を、切り取ってプロセスに変換します。
        /// 
        /// [再生]、[コマ送り]で利用。
        /// </summary>
        /// <returns></returns>
        public static bool ParseSfen_FromText(
            string inputLine,
            out string str1,
            out string str2,
            out string str3,
            out string str4,
            out string str5,
            out string rest
            )
        {
            bool successful = false;
            //nextTe = null;
            rest = inputLine;
            str1 = "";
            str2 = "";
            str3 = "";
            str4 = "";
            str5 = "";

            //System.C onsole.WriteLine("TuginoItte_Sfen.GetData_FromText:text=[" + text + "]");

            //------------------------------------------------------------
            // リスト作成
            //------------------------------------------------------------
            Regex regex = new Regex(
                @"^\s*([123456789PLNSGKRB])([abcdefghi\*])([123456789])([abcdefghi])(\+)?",
                RegexOptions.Singleline
            );

            MatchCollection mc = regex.Matches(inputLine);
            foreach (Match m in mc)
            {


                if (0 < m.Groups.Count)
                {
                    successful = true;

                    // 残りのテキスト
                    rest = inputLine.Substring(0, m.Index) + inputLine.Substring(m.Index + m.Length, inputLine.Length - (m.Index + m.Length));

                    str1 = m.Groups[1].Value;
                    str2 = m.Groups[2].Value;
                    str3 = m.Groups[3].Value;
                    str4 = m.Groups[4].Value;
                    str5 = m.Groups[5].Value;
                }

                // 最初の１件だけ処理して終わります。
                break;
            }

            rest = rest.Trim();

            return successful;
        }
    }
}
