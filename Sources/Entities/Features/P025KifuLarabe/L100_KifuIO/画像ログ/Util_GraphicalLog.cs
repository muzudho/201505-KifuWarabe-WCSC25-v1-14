using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L002_GraphicLog;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P006_Syugoron;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.Kifuwarazusa.Entities.Logging;

namespace Grayscale.P025_KifuLarabe.L100_KifuIO
{
    public abstract class Util_GraphicalLog
    {

        /// <summary>
        /// ログファイル通し番号。
        /// </summary>
        private static int LogFileCounter{get;set;}

        /// <summary>
        /// 駒画像のファイル名。
        /// </summary>
        /// <param name="pside"></param>
        /// <param name="ks14"></param>
        /// <param name="extentionWithDot"></param>
        /// <returns></returns>
        public static string PsideKs14_ToString(Playerside pside, Ks14 ks14, string extentionWithDot)
        {
            string komaImg;

            if (pside == Playerside.P1)
            {
                switch (ks14)
                {
                    case Ks14.H01_Fu: komaImg = "fu" + extentionWithDot; break;
                    case Ks14.H02_Kyo: komaImg = "kyo" + extentionWithDot; break;
                    case Ks14.H03_Kei: komaImg = "kei" + extentionWithDot; break;
                    case Ks14.H04_Gin: komaImg = "gin" + extentionWithDot; break;
                    case Ks14.H05_Kin: komaImg = "kin" + extentionWithDot; break;
                    case Ks14.H06_Oh: komaImg = "oh" + extentionWithDot; break;
                    case Ks14.H07_Hisya: komaImg = "hi" + extentionWithDot; break;
                    case Ks14.H08_Kaku: komaImg = "kaku" + extentionWithDot; break;
                    case Ks14.H09_Ryu: komaImg = "ryu" + extentionWithDot; break;
                    case Ks14.H10_Uma: komaImg = "uma" + extentionWithDot; break;
                    case Ks14.H11_Tokin: komaImg = "tokin" + extentionWithDot; break;
                    case Ks14.H12_NariKyo: komaImg = "narikyo" + extentionWithDot; break;
                    case Ks14.H13_NariKei: komaImg = "narikei" + extentionWithDot; break;
                    case Ks14.H14_NariGin: komaImg = "narigin" + extentionWithDot; break;
                    default: komaImg = "batu" + extentionWithDot; break;
                }
            }
            else
            {
                switch (ks14)
                {
                    case Ks14.H01_Fu: komaImg = "fuV" + extentionWithDot; break;
                    case Ks14.H02_Kyo: komaImg = "kyoV" + extentionWithDot; break;
                    case Ks14.H03_Kei: komaImg = "keiV" + extentionWithDot; break;
                    case Ks14.H04_Gin: komaImg = "ginV" + extentionWithDot; break;
                    case Ks14.H05_Kin: komaImg = "kinV" + extentionWithDot; break;
                    case Ks14.H06_Oh: komaImg = "ohV" + extentionWithDot; break;
                    case Ks14.H07_Hisya: komaImg = "hiV" + extentionWithDot; break;
                    case Ks14.H08_Kaku: komaImg = "kakuV" + extentionWithDot; break;
                    case Ks14.H09_Ryu: komaImg = "ryuV" + extentionWithDot; break;
                    case Ks14.H10_Uma: komaImg = "umaV" + extentionWithDot; break;
                    case Ks14.H11_Tokin: komaImg = "tokinV" + extentionWithDot; break;
                    case Ks14.H12_NariKyo: komaImg = "narikyoV" + extentionWithDot; break;
                    case Ks14.H13_NariKei: komaImg = "narikeiV" + extentionWithDot; break;
                    case Ks14.H14_NariGin: komaImg = "nariginV" + extentionWithDot; break;
                    default: komaImg = "batu" + extentionWithDot; break;
                }
            }

            return komaImg;
        }

        public static string Finger_ToString(SkyConst src_Sky, Finger finger, string extentionWithDot)
        {
            string komaImg = "";

            if ((int)finger < Finger_Honshogi.Items_KomaOnly.Length)
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(finger).Now);

                    Playerside pside = koma.Pside;
                    Ks14 ks14 = Haiyaku184Array.Syurui(koma.Haiyaku);

                    komaImg = Util_GraphicalLog.PsideKs14_ToString(pside, ks14, extentionWithDot);
            }
            else
            {
                komaImg = Util_GraphicalLog.PsideKs14_ToString(Playerside.Empty, Ks14.H00_Null, extentionWithDot);
            }



            return komaImg;
        }




        /// <summary>
        /// 駒別マスをJSON化します。
        /// </summary>
        /// <param name="enableLog"></param>
        /// <param name="src_Sky_base"></param>
        /// <param name="km_move"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static string JsonKyokumens_MultiKomabetuMasus(bool enableLog, SkyConst src_Sky_base, Maps_OneAndOne<Finger, SySet<SyElement>> km_move, string comment)
        {
            StringBuilder sb = new StringBuilder();

            if (!enableLog)
            {
                goto gt_EndMethod;
            }

            km_move.Foreach_Entry((Finger key, SySet<SyElement> value, ref bool toBreak) =>
            {
                // 駒１つ
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky_base.StarlightIndexOf(key).Now);

                    Ks14 ks14 = Haiyaku184Array.Syurui(koma.Haiyaku);

                    sb.AppendLine("            [");

                    // マスの色
                    sb.AppendLine("                { act:\"colorMasu\", style:\"rgba(100,240,100,0.5)\" },");

                    // 全マス
                    foreach (Basho masu in value.Elements)
                    {
                        sb.AppendLine("                { act:\"drawMasu\" , masu:" + Util_Masu.AsMasuNumber(masu) + " },");
                    }


                    string komaImg = Util_GraphicalLog.Finger_ToString(src_Sky_base, key, "");
                    sb.AppendLine("                { act:\"drawImg\", img:\"" + komaImg + "\", masu: " + Util_Masu.AsMasuNumber(koma.Masu) + " },");//FIXME:おかしい？

                    // コメント
                    sb.AppendLine("                { act:\"drawText\", text:\"" + comment + "\"  , x:0, y:20 },");

                    sb.AppendLine("            ],");

            });

        gt_EndMethod:
            return sb.ToString();
        }


        /// <summary>
        /// ノードをJSON化します。
        /// </summary>
        /// <param name="enableLog"></param>
        /// <param name="src_Sky_base"></param>
        /// <param name="thisNode"></param>
        /// <param name="comment"></param>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public static string JsonElements_Node(bool enableLog, SkyConst src_Sky_base, Node<ShootingStarlightable, KyokumenWrapper> thisNode, string comment)
        {
            StringBuilder sb = new StringBuilder();

            if (!enableLog)
            {
                goto gt_EndMethod;
            }

            {
                ShootingStarlightable move = thisNode.Key;

                RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);
                RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);


                    Finger finger = Util_Sky.Fingers_AtMasuNow(src_Sky_base,srcKoma.Masu).ToFirst();

                    // 駒１つ
                    Ks14 ks14 = Haiyaku184Array.Syurui(dstKoma.Haiyaku);

                    //sb.AppendLine("            [");

                    // マスの色
                    sb.AppendLine("                { act:\"colorMasu\", style:\"rgba(100,240,100,0.5)\" },");

                    // マス
                    sb.AppendLine("                { act:\"drawMasu\" , masu:" + Util_Masu.AsMasuNumber(dstKoma.Masu) + " },");


                    string komaImg = Util_GraphicalLog.Finger_ToString(src_Sky_base, finger, "");
                    sb.AppendLine("                { act:\"drawImg\", img:\"" + komaImg + "\", masu: " + Util_Masu.AsMasuNumber(dstKoma.Masu) + " },");//FIXME:おかしい？

                    // コメント
                    sb.AppendLine("                { act:\"drawText\", text:\"" + comment + "\"  , x:0, y:20 },");

                    //sb.AppendLine("            ],");
            }

        gt_EndMethod:
            return sb.ToString();
        }


        /// <summary>
        /// ハブ･ノードの次ノード・リストをJSON化します。
        /// </summary>
        /// <param name="enableLog"></param>
        /// <param name="src_Sky_base"></param>
        /// <param name="hubNode"></param>
        /// <param name="comment"></param>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public static string JsonKyokumens_NextNodes(bool enableLog, SkyConst src_Sky_base, Node<ShootingStarlightable,KyokumenWrapper> hubNode, string comment)
        {
            StringBuilder sb = new StringBuilder();

            if (!enableLog)
            {
                goto gt_EndMethod;
            }

            hubNode.Foreach_NextNodes((string key, Node<ShootingStarlightable, KyokumenWrapper> node, ref bool toBreak) =>
            {
                ShootingStarlightable move = node.Key;

                RO_Star_Koma srcKoma1 = Util_Koma.AsKoma(move.LongTimeAgo);
                RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);


                Finger srcKoma2 = Util_Sky.Fingers_AtMasuNow(src_Sky_base, srcKoma1.Masu).ToFirst();

                // 駒１つ
                Ks14 ks14 = Haiyaku184Array.Syurui(dstKoma.Haiyaku);

                sb.AppendLine("            [");

                // マスの色
                sb.AppendLine("                { act:\"colorMasu\", style:\"rgba(100,240,100,0.5)\" },");

                // マス
                sb.AppendLine("                { act:\"drawMasu\" , masu:" + Util_Masu.AsMasuNumber(dstKoma.Masu) + " },");


                string komaImg = Util_GraphicalLog.Finger_ToString(src_Sky_base, srcKoma2, "");
                sb.AppendLine("                { act:\"drawImg\", img:\"" + komaImg + "\", masu: " + Util_Masu.AsMasuNumber(dstKoma.Masu) + " },");//FIXME:おかしい？

                // コメント
                sb.AppendLine("                { act:\"drawText\", text:\"" + comment + "\"  , x:0, y:20 },");

                sb.AppendLine("            ],");
            });

        gt_EndMethod:
            return sb.ToString();
        }


        /// <summary>
        /// 用途例：持ち駒を確認するために使います。
        /// </summary>
        /// <param name="hkomas_gen_MOTI"></param>
        /// <returns></returns>
        public static string JsonElements_KomaHandles(bool enableLog, SkyConst src_Sky, List<int> hKomas, string comment)
        {
            StringBuilder sb = new StringBuilder();

            if (!enableLog)
            {
                goto gt_EndMethod;
            }

            //sb.AppendLine("            [");
            sb.AppendLine("                { act:\"colorMasu\", style:\"rgba(100,240,100,0.5)\" },");

            
            foreach (int hKoma in hKomas)
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(hKoma).Now);


                string komaImg = Util_GraphicalLog.Finger_ToString(src_Sky, hKoma, "");
                sb.AppendLine("                { act:\"drawImg\", img:\"" + komaImg + "\", masu: " + Util_Masu.AsMasuNumber(koma.Masu) + " },");//FIXME:おかしい？
            }



            sb.AppendLine("                { act:\"drawText\", text:\"" + comment + "\"  , x:0, y:20 },");

            //sb.AppendLine("            ],");

        gt_EndMethod:
            return sb.ToString();
        }

        public static string JsonElements_Masus(bool enableLog, SySet<SyElement> masus, string comment)
        {
            StringBuilder sb = new StringBuilder();

            if (!enableLog)
            {
                goto gt_EndMethod;
            }

            sb.AppendLine("                { act:\"colorMasu\", style:\"rgba(100,240,100,0.5)\" },\n");

            foreach (Basho masu in masus.Elements)
            {
                sb.AppendLine("                { act:\"drawMasu\" , masu:" + ((int)masu.MasuNumber) + " },\n");
            }



            sb.AppendLine("                { act:\"drawText\", text:\"" + comment + "\"  , x:0, y:20 },\n");

        gt_EndMethod:
            return sb.ToString();
        }

        /// <summary>
        /// Masus版。
        /// </summary>
        /// <param name="masus"></param>
        /// <param name="fileNameMemo"></param>
        /// <param name="comment"></param>
        public static void Log(bool enableLog, string fileNameMemo, string json)
        {
#if DEBUG
            if (!enableLog)
            {
                goto gt_EndMethod;
            }

            StringBuilder sb = new StringBuilder();


            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"ja\">");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset=\"UTF-8\">");
            sb.AppendLine("    <title>グラフィカル局面ログ</title>");
            sb.AppendLine("    ");
            sb.AppendLine("    <script type=\"text/javascript\" src=\"../Profile/Data/graphicalKyokumenLog.js\">");
            sb.AppendLine("    </script>");
            sb.AppendLine("");
            sb.AppendLine("    <script type=\"text/javascript\">");
            sb.AppendLine("        //");
            sb.AppendLine("        // boardsData は、局面を複数件　持つ配列です。");
            sb.AppendLine("        //");
            sb.AppendLine("        var boardsData =");


            sb.Append(json);
            sb.AppendLine(";");


            sb.AppendLine("");
            sb.AppendLine("    </script>");
            sb.AppendLine("");
            sb.AppendLine("</head>");
            sb.AppendLine("<body onLoad=\"drawBoards(boardsData);\">");
            sb.AppendLine("");
            sb.AppendLine("    ｖ（＾▽＾）ｖ　将棋盤のログを　視覚化したいんだぜ☆<br/>");
            sb.AppendLine("");

            sb.AppendLine("    <div id=\"announce1\" style=\"");
            sb.AppendLine("        visibility:hidden;");
            sb.AppendLine("        color:white;");
            sb.AppendLine("        background-color:gray;");
            sb.AppendLine("        border:solid 2px black;");
            sb.AppendLine("        padding:5px;");
            sb.AppendLine("        margin:5px;");
            sb.AppendLine("        \"");
            sb.AppendLine("    >アナウンス：　別のブラウザに変えて見てくれ☆　このブラウザでは使えない機能があって、もっと色が付くかも☆ｗｗ</div>");

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            File.WriteAllText("../../Logs/_log" + Util_GraphicalLog.LogFileCounter + "_" + fileNameMemo + ".html", sb.ToString());
            Util_GraphicalLog.LogFileCounter++;

        gt_EndMethod:
            ;
#endif
        }


        public static string BoardFileLog_ToJsonStr(GraphicalLog_File boardFileLog1)
        {
            StringBuilder sb_json_boardsLog = new StringBuilder();

            foreach (GraphicalLog_Board boardLog1 in boardFileLog1.boards)
            {
                // 指し手。分かれば。
                string moveStr = Converter04.MoveToStringForLog(boardLog1.moveOrNull, boardLog1.GenTeban);

                //string oldCaption = boardLog1.Caption;
                //boardLog1.Caption += "_" + moveStr;
                sb_json_boardsLog.Append(boardLog1.ToJsonStr());
                //boardLog1.Caption = oldCaption;
            }

            return sb_json_boardsLog.ToString();
        }

    }
}
