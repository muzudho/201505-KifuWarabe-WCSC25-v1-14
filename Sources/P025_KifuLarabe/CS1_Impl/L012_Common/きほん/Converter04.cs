using Grayscale.P025_KifuLarabe;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L002_GraphicLog;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L007_Random;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P025_KifuLarabe.L100_KifuIO;
using Grayscale.P012_KnowledgeShogi;
using Grayscale.P006_Syugoron;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using System.Diagnostics;

namespace Grayscale.P025_KifuLarabe.L012_Common
{

    /// <summary>
    /// ************************************************************************************************************************
    /// あるデータを、別のデータに変換します。
    /// ************************************************************************************************************************
    /// </summary>
    public abstract class Converter04
    {





        /// <summary>
        /// 「２八」といった表記にして返します。
        /// </summary>
        /// <param name="masu"></param>
        /// <returns></returns>
        public static string Masu_ToKanji(SyElement masu)
        {
            StringBuilder sb = new StringBuilder();

            int suji;
            int dan;
            Util_MasuNum.MasuToSuji(Masu_Honshogi.Items_All[Util_Masu.AsMasuNumber(masu)], out suji);
            Util_MasuNum.MasuToDan(Masu_Honshogi.Items_All[Util_Masu.AsMasuNumber(masu)], out dan);

            sb.Append(ConverterKnSh.Int_ToArabiaSuji(suji));
            sb.Append(ConverterKnSh.Int_ToKanSuji(dan));

            return sb.ToString();
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// a～i を、1～9 に変換します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static int Alphabet_ToInt(string alphabet)
        {
            int num;

            switch (alphabet)
            {
                case "a":
                    num = 1;
                    break;
                case "b":
                    num = 2;
                    break;
                case "c":
                    num = 3;
                    break;
                case "d":
                    num = 4;
                    break;
                case "e":
                    num = 5;
                    break;
                case "f":
                    num = 6;
                    break;
                case "g":
                    num = 7;
                    break;
                case "h":
                    num = 8;
                    break;
                case "i":
                    num = 9;
                    break;
                default:
                    num = -1;
                    break;
            }

            return num;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 1～9 を、a～i に変換します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string Int_ToAlphabet(int num)
        {
            string alphabet;

            switch (num)
            {
                case 1:
                    alphabet = "a";
                    break;
                case 2:
                    alphabet = "b";
                    break;
                case 3:
                    alphabet = "c";
                    break;
                case 4:
                    alphabet = "d";
                    break;
                case 5:
                    alphabet = "e";
                    break;
                case 6:
                    alphabet = "f";
                    break;
                case 7:
                    alphabet = "g";
                    break;
                case 8:
                    alphabet = "h";
                    break;
                case 9:
                    alphabet = "i";
                    break;
                default:
                    string message = "筋[" + num + "]をアルファベットに変えることはできませんでした。";
                    LarabeLoggerList.ERROR.WriteLine_Error( message);
                    throw new Exception(message);
            }

            return alphabet;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 駒の種類。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="syurui"></param>
        /// <returns></returns>
        public static void SfenSyokihaichi_ToSyurui(string sfen, out Playerside pside, out Ks14 syurui)
        {
            switch (sfen)
            {
                case "P":
                    pside = Playerside.P1;
                    syurui = Ks14.H01_Fu;
                    break;

                case "p":
                    pside = Playerside.P2;
                    syurui = Ks14.H01_Fu;
                    break;

                case "L":
                    pside = Playerside.P1;
                    syurui = Ks14.H02_Kyo;
                    break;

                case "l":
                    pside = Playerside.P2;
                    syurui = Ks14.H02_Kyo;
                    break;

                case "N":
                    pside = Playerside.P1;
                    syurui = Ks14.H03_Kei;
                    break;

                case "n":
                    pside = Playerside.P2;
                    syurui = Ks14.H03_Kei;
                    break;

                case "S":
                    pside = Playerside.P1;
                    syurui = Ks14.H04_Gin;
                    break;

                case "s":
                    pside = Playerside.P2;
                    syurui = Ks14.H04_Gin;
                    break;

                case "G":
                    pside = Playerside.P1;
                    syurui = Ks14.H05_Kin;
                    break;

                case "g":
                    pside = Playerside.P2;
                    syurui = Ks14.H05_Kin;
                    break;

                case "R":
                    pside = Playerside.P1;
                    syurui = Ks14.H07_Hisya;
                    break;

                case "r":
                    pside = Playerside.P2;
                    syurui = Ks14.H07_Hisya;
                    break;

                case "B":
                    pside = Playerside.P1;
                    syurui = Ks14.H08_Kaku;
                    break;

                case "b":
                    pside = Playerside.P2;
                    syurui = Ks14.H08_Kaku;
                    break;

                case "K":
                    pside = Playerside.P1;
                    syurui = Ks14.H06_Oh;
                    break;

                case "k":
                    pside = Playerside.P2;
                    syurui = Ks14.H06_Oh;
                    break;

                case "+P":
                    pside = Playerside.P1;
                    syurui = Ks14.H11_Tokin;
                    break;

                case "+p":
                    pside = Playerside.P2;
                    syurui = Ks14.H11_Tokin;
                    break;

                case "+L":
                    pside = Playerside.P1;
                    syurui = Ks14.H12_NariKyo;
                    break;

                case "+l":
                    pside = Playerside.P2;
                    syurui = Ks14.H12_NariKyo;
                    break;

                case "+N":
                    pside = Playerside.P1;
                    syurui = Ks14.H13_NariKei;
                    break;

                case "+n":
                    pside = Playerside.P2;
                    syurui = Ks14.H13_NariKei;
                    break;

                case "+S":
                    pside = Playerside.P1;
                    syurui = Ks14.H14_NariGin;
                    break;

                case "+s":
                    pside = Playerside.P2;
                    syurui = Ks14.H14_NariGin;
                    break;

                case "+R":
                    pside = Playerside.P1;
                    syurui = Ks14.H07_Hisya;
                    break;

                case "+r":
                    pside = Playerside.P2;
                    syurui = Ks14.H07_Hisya;
                    break;

                case "+B":
                    pside = Playerside.P1;
                    syurui = Ks14.H08_Kaku;
                    break;

                case "+b":
                    pside = Playerside.P2;
                    syurui = Ks14.H08_Kaku;
                    break;

                default:
                    pside = Playerside.P2;
                    syurui = Ks14.H00_Null;
                    break;
            }
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 打った駒の種類。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="syurui"></param>
        /// <returns></returns>
        public static void SfenUttaSyurui(string sfen, out Ks14 syurui)
        {
            switch (sfen)
            {
                case "P":
                    syurui = Ks14.H01_Fu;
                    break;

                case "L":
                    syurui = Ks14.H02_Kyo;
                    break;

                case "N":
                    syurui = Ks14.H03_Kei;
                    break;

                case "S":
                    syurui = Ks14.H04_Gin;
                    break;

                case "G":
                    syurui = Ks14.H05_Kin;
                    break;

                case "R":
                    syurui = Ks14.H07_Hisya;
                    break;

                case "B":
                    syurui = Ks14.H08_Kaku;
                    break;

                case "K":
                    syurui = Ks14.H06_Oh;
                    break;

                case "+P":
                    syurui = Ks14.H11_Tokin;
                    break;

                case "+L":
                    syurui = Ks14.H12_NariKyo;
                    break;

                case "+N":
                    syurui = Ks14.H13_NariKei;
                    break;

                case "+S":
                    syurui = Ks14.H14_NariGin;
                    break;

                case "+R":
                    syurui = Ks14.H07_Hisya;
                    break;

                case "+B":
                    syurui = Ks14.H08_Kaku;
                    break;

                default:
                    Util_Message.Show("▲バグ【駒種類】Sfen=[" + sfen + "]");
                    syurui = Ks14.H00_Null;
                    break;
            }
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 駒の文字を、列挙型へ変換。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="moji"></param>
        /// <returns></returns>
        public static Ks14 Str_ToSyurui(string moji)
        {
            Ks14 syurui;

            switch (moji)
            {
                case "歩":
                    syurui = Ks14.H01_Fu;
                    break;

                case "香":
                    syurui = Ks14.H02_Kyo;
                    break;

                case "桂":
                    syurui = Ks14.H03_Kei;
                    break;

                case "銀":
                    syurui = Ks14.H04_Gin;
                    break;

                case "金":
                    syurui = Ks14.H05_Kin;
                    break;

                case "飛":
                    syurui = Ks14.H07_Hisya;
                    break;

                case "角":
                    syurui = Ks14.H08_Kaku;
                    break;

                case "王"://thru
                case "玉":
                    syurui = Ks14.H06_Oh;
                    break;

                case "と":
                    syurui = Ks14.H11_Tokin;
                    break;

                case "成香":
                    syurui = Ks14.H12_NariKyo;
                    break;

                case "成桂":
                    syurui = Ks14.H13_NariKei;
                    break;

                case "成銀":
                    syurui = Ks14.H14_NariGin;
                    break;

                case "竜"://thru
                case "龍":
                    syurui = Ks14.H09_Ryu;
                    break;

                case "馬":
                    syurui = Ks14.H10_Uma;
                    break;

                default:
                    syurui = Ks14.H00_Null;
                    break;
            }

            return syurui;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 駒の文字を、列挙型へ変換。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="moji"></param>
        /// <returns></returns>
        public static string Syurui_ToStrIchimoji(Ks14 ks14)
        {
            string syurui;

            switch (ks14)
            {
                case Ks14.H01_Fu:
                    syurui = "歩";
                    break;

                case Ks14.H02_Kyo:
                    syurui = "香";
                    break;

                case Ks14.H03_Kei:
                    syurui = "桂";
                    break;

                case Ks14.H04_Gin:
                    syurui = "銀";
                    break;

                case Ks14.H05_Kin:
                    syurui = "金";
                    break;

                case Ks14.H07_Hisya:
                    syurui = "飛";
                    break;

                case Ks14.H08_Kaku:
                    syurui = "角";
                    break;

                case Ks14.H06_Oh:
                    syurui = "玉";
                    break;

                case Ks14.H11_Tokin:
                    syurui = "と";
                    break;

                case Ks14.H12_NariKyo:
                    syurui = "杏";
                    break;

                case Ks14.H13_NariKei:
                    syurui = "圭";
                    break;

                case Ks14.H14_NariGin:
                    syurui = "全";
                    break;

                case Ks14.H09_Ryu:
                    syurui = "竜";
                    break;

                case Ks14.H10_Uma:
                    syurui = "馬";
                    break;

                default:
                    syurui = "×";
                    break;
            }

            return syurui;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 打。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="da"></param>
        /// <returns></returns>
        public static string Bool_ToDa(DaHyoji da)
        {
            string daStr = "";

            if (DaHyoji.Visible == da)
            {
                daStr = "打";
            }

            return daStr;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 打表示。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="daStr"></param>
        /// <returns></returns>
        public static DaHyoji Str_ToDaHyoji(string daStr)
        {
            DaHyoji daHyoji = DaHyoji.No_Print;

            if (daStr == "打")
            {
                daHyoji = DaHyoji.Visible;
            }

            return daHyoji;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 成り
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="nari"></param>
        /// <returns></returns>
        public static string Nari_ToStr(NariNarazu nari)
        {
            string nariStr = "";

            switch (nari)
            {
                case NariNarazu.Nari:
                    nariStr = "成";
                    break;
                case NariNarazu.Narazu:
                    nariStr = "不成";
                    break;
                default:
                    break;
            }

            return nariStr;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 成り。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="nariStr"></param>
        /// <returns></returns>
        public static NariNarazu Nari_ToBool(string nariStr)
        {
            NariNarazu nari;

            if ("成" == nariStr)
            {
                nari = NariNarazu.Nari;
            }
            else if ("不成" == nariStr)
            {
                nari = NariNarazu.Narazu;
            }
            else
            {
                nari = NariNarazu.CTRL_SONOMAMA;
            }

            return nari;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 先後。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="psideStr"></param>
        /// <returns></returns>
        public static Playerside Pside_ToEnum(string psideStr)
        {
            Playerside pside;

            switch (psideStr)
            {
                case "△":
                    pside = Playerside.P2;
                    break;

                case "▲":
                default:
                    pside = Playerside.P1;
                    break;
            }

            return pside;
        }

        public static Okiba Pside_ToKomadai(Playerside pside)
        {
            Okiba okiba;

            switch(pside)
            {
                case Playerside.P1:
                    okiba = Okiba.Sente_Komadai;
                    break;
                case Playerside.P2:
                    okiba = Okiba.Gote_Komadai;
                    break;
                default:
                    okiba = Okiba.Empty;
                    break;
            }

            return okiba;
        }

        public static Playerside Okiba_ToPside(Okiba okiba)
        {
            Playerside pside;
            switch (okiba)
            {
                case Okiba.Gote_Komadai:
                    pside = Playerside.P2;
                    break;
                case Okiba.Sente_Komadai:
                    pside = Playerside.P1;
                    break;
                default:
                    pside = Playerside.Empty;
                    break;
            }

            return pside;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 先後。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="pside"></param>
        /// <returns></returns>
        public static string Pside_ToStr(Playerside pside)
        {
            string psideStr;

            switch (pside)
            {
                case Playerside.P2:
                    psideStr = "△";
                    break;

                case Playerside.P1:
                default:
                    psideStr = "▲";
                    break;
            }

            return psideStr;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 先後。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="pside"></param>
        /// <returns></returns>
        public static string Pside_ToKanji(Playerside pside)
        {
            string psideStr;

            switch (pside)
            {
                case Playerside.P1:
                    psideStr = "先手";
                    break;
                case Playerside.P2:
                    psideStr = "後手";
                    break;
                default:
                    psideStr = "×";
                    break;
            }

            return psideStr;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 右左。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="migiHidari"></param>
        /// <returns></returns>
        public static string MigiHidari_ToStr(MigiHidari migiHidari)
        {
            string str;

            switch (migiHidari)
            {
                case MigiHidari.Migi:
                    str = "右";
                    break;

                case MigiHidari.Hidari:
                    str = "左";
                    break;

                case MigiHidari.Sugu:
                    str = "直";
                    break;

                default:
                    str = "";
                    break;
            }

            return str;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 寄、右、左、直、なし
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="migiHidariStr"></param>
        /// <returns></returns>
        public static MigiHidari Str_ToMigiHidari(string migiHidariStr)
        {
            MigiHidari migiHidari;

            switch (migiHidariStr)
            {
                case "右":
                    migiHidari = MigiHidari.Migi;
                    break;

                case "左":
                    migiHidari = MigiHidari.Hidari;
                    break;

                case "直":
                    migiHidari = MigiHidari.Sugu;
                    break;

                default:
                    migiHidari = MigiHidari.No_Print;
                    break;
            }

            return migiHidari;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 上がる、引く
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="agaruHiku"></param>
        /// <returns></returns>
        public static string AgaruHiku_ToStr(AgaruHiku agaruHiku)
        {
            string str;

            switch (agaruHiku)
            {
                case AgaruHiku.Yoru:
                    str = "寄";
                    break;

                case AgaruHiku.Hiku:
                    str = "引";
                    break;

                case AgaruHiku.Agaru:
                    str = "上";
                    break;

                default:
                    str = "";
                    break;
            }

            return str;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 上がる、引く。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="agaruHikuStr"></param>
        /// <returns></returns>
        public static AgaruHiku Str_ToAgaruHiku(string agaruHikuStr)
        {
            AgaruHiku agaruHiku;

            switch (agaruHikuStr)
            {
                case "寄":
                    agaruHiku = AgaruHiku.Yoru;
                    break;

                case "引":
                    agaruHiku = AgaruHiku.Hiku;
                    break;

                case "上":
                    agaruHiku = AgaruHiku.Agaru;
                    break;

                default:
                    agaruHiku = AgaruHiku.No_Print;
                    break;
            }

            return agaruHiku;
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 先後の交代
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="pside">先後</param>
        /// <returns>ひっくりかえった先後</returns>
        public static Playerside AlternatePside(Playerside pside)
        {
            Playerside result;

            switch (pside)
            {
                case Playerside.P1:
                    result = Playerside.P2;
                    break;

                case Playerside.P2:
                    result = Playerside.P1;
                    break;

                default:
                    result = pside;
                    break;
            }

            return result;
        }


        /// <summary>
        /// 変換『「駒→手」のコレクション』→『「駒、指し手」のペアのリスト』
        /// </summary>
        public static List<Couple<Finger, SyElement>> KomabetuMasus_ToKamList(
            Maps_OneAndOne<Finger, SySet<SyElement>> km
            )
        {
            List<Couple<Finger, SyElement>> kmList = new List<Couple<Finger, SyElement>>();

            foreach (Finger koma in km.ToKeyList())
            {
                SySet<SyElement> masus = km.ElementAt(koma);

                foreach (SyElement masu in masus.Elements)
                {
                    // セットとして作っているので、重複エレメントは無いはず……☆
                    kmList.Add(new Couple<Finger, SyElement>(koma, masu));
                }
            }

            return kmList;
        }


        ///// <summary>
        ///// 変換『「指し手→局面」のコレクション』→『「駒、指し手」のペアのリスト』
        ///// </summary>
        //public static List<Couple<Finger,Masu>> MovebetuSky_ToKamList(
        //    SkyConst src_Sky_genzai,
        //    Dictionary<ShootingStarlightable, SkyBuffer> ss,
        //    LarabeLoggerTag logTag
        //    )
        //{
        //    List<Couple<Finger, Masu>> kmList = new List<Couple<Finger, Masu>>();

        //    // TODO:
        //    foreach(KeyValuePair<ShootingStarlightable,SkyBuffer> entry in ss)
        //    {
        //        RO_Star_Koma srcKoma = Util_Koma.AsKoma(entry.Key.LongTimeAgo);
        //        RO_Star_Koma dstKoma = Util_Koma.AsKoma(entry.Key.Now);


        //            Masu srcMasu = srcKoma.Masu;
        //            Masu dstMasu = dstKoma.Masu;

        //            Finger figKoma = Util_Sky.Fingers_AtMasuNow(src_Sky_genzai,srcMasu).ToFirst();

        //            kmList.Add(new Couple<Finger, Masu>(figKoma, dstMasu));
        //    }

        //    return kmList;
        //}


        /// <summary>
        /// 変換『「指し手→局面」のコレクション』→『「駒、指し手」のペアのリスト』
        /// </summary>
        public static List<Couple<Finger, SyElement>> NextNodes_ToKamList(
            SkyConst src_Sky_genzai,
            Node<ShootingStarlightable,KyokumenWrapper> hubNode,
            LarabeLoggerable logTag
            )
        {
            List<Couple<Finger, SyElement>> kmList = new List<Couple<Finger, SyElement>>();

            // TODO:
            hubNode.Foreach_NextNodes((string key, Node<ShootingStarlightable, KyokumenWrapper> nextNode, ref bool toBreak) =>
            {
                RO_Star_Koma srcKoma = Util_Koma.AsKoma(nextNode.Key.LongTimeAgo);
                RO_Star_Koma dstKoma = Util_Koma.AsKoma(nextNode.Key.Now);


                SyElement srcMasu = srcKoma.Masu;
                SyElement dstMasu = dstKoma.Masu;

                Finger figKoma = Util_Sky.Fingers_AtMasuNow(src_Sky_genzai, srcMasu).ToFirst();

                kmList.Add(new Couple<Finger, SyElement>(figKoma, dstMasu));
            });

            return kmList;
        }

        /// <summary>
        /// 変換『「指し手→局面」のコレクション』→『「「指し手→局面」のリスト』
        /// </summary>
        public static List<Node<ShootingStarlightable, KyokumenWrapper>> NextNodes_ToList(
            Node<ShootingStarlightable, KyokumenWrapper> hubNode,
            LarabeLoggerable logTag
            )
        {
            List<Node<ShootingStarlightable, KyokumenWrapper>> list = new List<Node<ShootingStarlightable, KyokumenWrapper>>();

            // TODO:
            hubNode.Foreach_NextNodes((string key, Node<ShootingStarlightable, KyokumenWrapper> node, ref bool toBreak) =>
            {
                list.Add( node);
            });

            return list;
        }


        /// <summary>
        /// 変換「自駒が動ける升」→「自駒が動ける手」
        /// </summary>
        /// <param name="kmDic_Self"></param>
        /// <returns></returns>
        public static Maps_OneAndMulti<Finger, ShootingStarlightable> KomabetuMasusToKomabetuMove(
            Maps_OneAndOne<Finger, SySet<SyElement>> kmDic_Self,
            Node<ShootingStarlightable, KyokumenWrapper> siteiNode_genzai
            )
        {

            Maps_OneAndMulti<Finger, ShootingStarlightable> komaTe = new Maps_OneAndMulti<Finger, ShootingStarlightable>();

            //
            //
            kmDic_Self.Foreach_Entry((Finger key, SySet<SyElement> value, ref bool toBreak) =>
            {
                foreach (SyElement masuHandle in value.Elements)
                {
                    RO_Star_Koma koma = Util_Koma.AsKoma(siteiNode_genzai.Value.ToKyokumenConst.StarlightIndexOf(key).Now);


                    ShootingStarlightable move = new RO_ShootingStarlight(
                            //key,
                            // 元
                            koma,
                            // 先
                            new RO_Star_Koma(
                                koma.Pside,
                                Masu_Honshogi.Items_All[Util_Masu.AsMasuNumber(masuHandle)],
                                koma.Haiyaku//TODO:成るとか考えたい
                            ),

                            Ks14.H00_Null//取った駒不明
                        );
                        //sbSfen.Append(sbSfen.ToString());

                        if (komaTe.ContainsKey(key))
                        {
                            // すでに登録されている駒
                            komaTe.AddExists( key, move);
                        }
                        else
                        {
                            // まだ登録されていない駒
                            komaTe.AddNew(key,move);
                        }

                }
            });

            return komaTe;
        }


        /// <summary>
        /// 変換「各（自駒が動ける升）」→「各（自駒が動ける手）」
        /// </summary>
        /// <param name="komaBETUSusumeruMasus"></param>
        /// <returns></returns>
        public static Maps_OneAndMulti<Finger, ShootingStarlightable> KomaBETUSusumeruMasusToKomaBETUAllMoves(
            List_OneAndMulti<Finger, SySet<SyElement>> komaBETUSusumeruMasus,
            Node<ShootingStarlightable, KyokumenWrapper> siteiNode_genzai
            )
        {
            Maps_OneAndMulti<Finger, ShootingStarlightable> komabetuAllMove = new Maps_OneAndMulti<Finger, ShootingStarlightable>();

            komaBETUSusumeruMasus.Foreach_Entry((Finger figKoma, SySet<SyElement> susumuMasuSet, ref bool toBreak) =>
            {
                RO_Star_Koma srcKoma = Util_Koma.AsKoma(siteiNode_genzai.Value.ToKyokumenConst.StarlightIndexOf(figKoma).Now);

                foreach (SyElement susumuMasu in susumuMasuSet.Elements)
                {
                    // 移動先の駒
                    RO_Star_Koma dstKoma = new RO_Star_Koma(
                        srcKoma.Pside,
                        Masu_Honshogi.Items_All[Util_Masu.AsMasuNumber(susumuMasu)],
                        srcKoma.Haiyaku//TODO:ここで、駒の種類が「成り」に上書きされているバージョンも考えたい
                    );

                    ShootingStarlightable move = new RO_ShootingStarlight(
                            //figKoma,//駒
                            srcKoma,// 移動元
                            dstKoma,// 移動先
                            Ks14.H00_Null//取った駒不明
                        );
                    komabetuAllMove.AddOverwrite(figKoma, move);

                    // これが通称【水際のいんちきプログラム】なんだぜ☆
                    // 必要により、【成り】の指し手を追加します。
                    Converter04.AddKomaBETUAllNariMoves(
                        komabetuAllMove,
                        figKoma,
                        srcKoma,
                        dstKoma
                        );
                }
            });

            //Converter04.AssertNariMove(komabetuAllMove, "#KomabetuMasus_ToKomabetuAllMove");//ここはOK

            return komabetuAllMove;
        }
        /// <summary>
        /// 「成り」ができるなら真。
        /// </summary>
        /// <returns></returns>
        private static bool IsPromotionable(
            out bool isPromotionable,
            RO_Star_Koma srcKoma,
            RO_Star_Koma dstKoma
            )
        {
            bool successful = true;
            isPromotionable = false;

            if (Okiba.ShogiBan != Util_Masu.Masu_ToOkiba(srcKoma.Masu))
            {
                successful = false;
                goto gt_EndMethod;
            }

            if(KomaSyurui14Array.IsNari(srcKoma.Syurui))
            {
                // 既に成っている駒は、「成り」の指し手を追加すると重複エラーになります。
                // 成りになれない、で正常終了します。
                goto gt_EndMethod;
            }

            int srcDan;
            if (!Util_MasuNum.MasuToDan(srcKoma.Masu, out srcDan))
            {
                throw new Exception("段に変換失敗");
            }

            int dstDan;
            if (!Util_MasuNum.MasuToDan(dstKoma.Masu, out dstDan))
            {
                throw new Exception("段に変換失敗");
            }

            // 先手か、後手かで大きく処理を分けます。
            switch (dstKoma.Pside)
            {
                case Playerside.P1:
                    {
                        if (srcDan <= 3)
                        {
                            // 3段目から上にあった駒が移動したのなら、成りの資格ありです。
                            isPromotionable = true;
                            goto gt_EndMethod;
                        }

                        if (dstDan <= 3)
                        {
                            // 3段目から上に駒が移動したのなら、成りの資格ありです。
                            isPromotionable = true;
                            goto gt_EndMethod;
                        }
                    }
                    break;
                case Playerside.P2:
                    {
                        if (7 <= srcDan)
                        {
                            // 7段目から下にあった駒が移動したのなら、成りの資格ありです。
                            isPromotionable = true;
                            goto gt_EndMethod;
                        }

                        if (7 <= dstDan)
                        {
                            // 7段目から下に駒が移動したのなら、成りの資格ありです。
                            isPromotionable = true;
                            goto gt_EndMethod;
                        }
                    }
                    break;
                default: throw new Exception("未定義のプレイヤーサイドです。");
            }
        gt_EndMethod:
            ;
            return successful;
        }
        /// <summary>
        /// これが通称【水際のいんちきプログラム】なんだぜ☆
        /// 必要により、【成り】の指し手を追加します。
        /// </summary>
        private static void AddKomaBETUAllNariMoves(
            Maps_OneAndMulti<Finger, ShootingStarlightable> komaBETUAllMoves,
            Finger figKoma,
            RO_Star_Koma srcKoma,
            RO_Star_Koma dstKoma
            )
        {
            try
            {
                bool isPromotionable;
                if (!Converter04.IsPromotionable(out isPromotionable, srcKoma, dstKoma))
                {
                    goto gt_EndMethod;
                }

                // 成りの資格があれば、成りの指し手を作ります。
                if (isPromotionable)
                {
                    //MessageBox.Show("成りの資格がある駒がありました。 src=["+srcKoma.Masu.Word+"]["+srcKoma.Syurui+"]");

                    ShootingStarlightable move = new RO_ShootingStarlight(
                        //figKoma,//駒
                        srcKoma,// 移動元
                        new RO_Star_Koma(
                            dstKoma.Pside,
                            dstKoma.Masu,
                            KomaSyurui14Array.ToNariCase(dstKoma.Syurui)//強制的に【成り】に駒の種類を変更
                        ),// 移動先
                        Ks14.H00_Null//取った駒不明
                    );

                    // TODO: 一段目の香車のように、既に駒は成っている場合があります。無い指し手だけ追加するようにします。
                    komaBETUAllMoves.AddNotOverwrite(figKoma, move);
                }

            gt_EndMethod:
                ;
            }
            catch (Exception ex)
            {
                throw new Exception("Convert04.cs#AddNariMoveでｴﾗｰ。:"+ex.GetType().Name+":"+ex.Message);
            }
        }
        /// <summary>
        /// これが通称【水際のいんちきプログラム】なんだぜ☆
        /// 必要により、【成り】の指し手を追加します。
        /// </summary>
        public static void AddNariMove(
            KifuNode node_yomiCur,
            KifuNode hubNode,
            LarabeLoggerable logTag
            )
        {
            try
            {
                Dictionary<string, ShootingStarlightable> newMoveList = new Dictionary<string,ShootingStarlightable>();

                hubNode.Foreach_NextNodes((string key, Node<ShootingStarlightable, KyokumenWrapper> nextNode, ref bool toBreak) =>
                {
                    RO_Star_Koma srcKoma = Util_Koma.AsKoma(nextNode.Key.LongTimeAgo);
                    RO_Star_Koma dstKoma = Util_Koma.AsKoma(nextNode.Key.Now);

                    bool isPromotionable;
                    if (!Converter04.IsPromotionable(out isPromotionable, srcKoma, dstKoma))
                    {
                        // ｴﾗｰ
                        goto gt_Next1;
                    }

                    if (isPromotionable)
                    {
                        ShootingStarlightable move = new RO_ShootingStarlight(
                            //figKoma,//駒
                            srcKoma,// 移動元
                            new RO_Star_Koma(
                                dstKoma.Pside,
                                dstKoma.Masu,
                                KomaSyurui14Array.ToNariCase(dstKoma.Syurui)//強制的に【成り】に駒の種類を変更
                            ),// 移動先
                            Ks14.H00_Null//取った駒不明
                        );

                        // TODO: 一段目の香車のように、既に駒は成っている場合があります。無い指し手だけ追加するようにします。
                        string sasiteStr = Util_Sky.ToSfenMoveText(move);//重複防止用のキー
                        if (!newMoveList.ContainsKey(sasiteStr))
                        {
                            newMoveList.Add(sasiteStr, move);
                        }
                    }

                gt_Next1:
                    ;
                });

                
                // 新しく作った【成り】の指し手を追加します。
                foreach(ShootingStarlightable newMove in newMoveList.Values)
                {
                    // 指す前の駒
                    RO_Star_Koma sasumaenoKoma = Util_Koma.AsKoma(newMove.LongTimeAgo);

                    // 指した駒
                    RO_Star_Koma sasitaKoma = Util_Koma.AsKoma(newMove.Now);

                    // 現局面
                    SkyConst src_Sky = node_yomiCur.Value.ToKyokumenConst;

                    // 指す前の駒を、盤上のマス目で指定
                    Finger figSasumaenoKoma = Util_Sky.Fingers_AtMasuNow(src_Sky, sasumaenoKoma.Masu).ToFirst();

                    // 新たな局面
                    KyokumenWrapper kyokumenWrapper = new KyokumenWrapper(Util_Sasu.Sasu(src_Sky, figSasumaenoKoma, sasitaKoma.Masu, KifuNodeImpl.GetReverseTebanside(node_yomiCur.Tebanside), logTag));



                    try
                    {
                        string sasiteStr = Util_Sky.ToSfenMoveText(newMove);

                        if (!hubNode.ContainsKey_NextNodes(sasiteStr))
                        {
                            // 指し手が既存でない局面だけを追加します。

                            hubNode.Add_NextNode(
                                sasiteStr,
                                new KifuNodeImpl(
                                    newMove,
                                    kyokumenWrapper,//node_yomiCur.Value,//FIXME: 成りの手を指した局面を作りたい。
                                    KifuNodeImpl.GetReverseTebanside(node_yomiCur.Tebanside)
                                )
                                );
                        }

                    }
                    catch (Exception ex)
                    {
                        // 既存の指し手
                        StringBuilder sb = new StringBuilder();
                        {
                            hubNode.Foreach_NextNodes((string key, Node<ShootingStarlightable, KyokumenWrapper> nextNode, ref bool toBreak) =>
                            {
                                sb.Append("「");
                                sb.Append(Util_Sky.ToSfenMoveText(nextNode.Key));
                                sb.Append("」");
                            });
                        }

                        //>>>>> エラーが起こりました。
                        string message = ex.GetType().Name + " " + ex.Message + "：新しく作った「成りの指し手」を既存ノードに追加していた時です。：追加したい指し手=「" + Util_Sky.ToSfenMoveText(newMove) + "」既存の手="+sb.ToString();
                        Debug.Fail(message);

                        // どうにもできないので  ログだけ取って、上に投げます。
                        logTag.WriteLine_Error(message);
                        throw ;
                    }

                }

            // gt_EndMethod:
            // ;
            }
            catch (Exception ex)
            {
                throw new Exception("Convert04.cs#AddNariMoveでｴﾗｰ。:" + ex.GetType().Name + ":" + ex.Message);
            }
        }
        public static void AssertNariMove(Maps_OneAndMulti<Finger, ShootingStarlightable> komabetuAllMove, string hint)
        {
            /*
            foreach(KeyValuePair<Finger, List<ShootingStarlightable>> komaAllMove in komabetuAllMove.Items)
            {
                foreach(ShootingStarlightable move in komaAllMove.Value)
                {
                    Starlightable lightable = move.Now;
                    RO_Star_Koma koma = Util_Koma.AsKoma(lightable);

                    if (KomaSyurui14Array.IsNari(koma.Syurui))
                    {
                        MessageBox.Show("指し手に成りが含まれています。[" + koma.Masu.Word + "][" + koma.Syurui + "]", "デバッグ:" + hint);
                        //System.Console.WriteLine("指し手に成りが含まれています。");
                        goto gt_EndMethod;
                    }
                }
            }
        gt_EndMethod:
            ;
             */
        }
        public static void AssertNariMove(Node<ShootingStarlightable, KyokumenWrapper> hubNode, string hint)
        {
            /*
            hubNode.Foreach_NextNodes((string key, Node<ShootingStarlightable, KyokumenWrapper> nextNode, ref bool toBreak) =>
            {
                ShootingStarlightable move = nextNode.Key;
                Starlightable lightable = move.Now;
                RO_Star_Koma koma = Util_Koma.AsKoma(lightable);

                if (KomaSyurui14Array.IsNari(koma.Syurui))
                {
                    MessageBox.Show("指し手に成りが含まれています。[" + koma.Masu.Word + "][" + koma.Syurui + "]", "デバッグ:" + hint);
                    //System.Console.WriteLine("指し手に成りが含まれています。");
                    toBreak = true;
                    goto gt_EndMethod;
                }
            gt_EndMethod:
                ;
            });
             */
        }



        public static Dictionary<ShootingStarlightable, KyokumenWrapper> KomabetuMasus_ToMovebetuSky(
            Maps_OneAndOne<Finger, SySet<SyElement>> komabetuSusumuMasus,
            SkyConst src_Sky,
            Playerside pside,
            LarabeLoggerable logTag
            )
        {
            Dictionary<ShootingStarlightable, KyokumenWrapper> resultMovebetuSky = new Dictionary<ShootingStarlightable, KyokumenWrapper>();


            komabetuSusumuMasus.Foreach_Entry((Finger key, SySet<SyElement> value, ref bool toBreak) =>
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(key).Now);


                foreach (SyElement dstMasu in value.Elements)
                    {

                        ShootingStarlightable move = Util_Sky.New(
                            //key,
                            new RO_Star_Koma(pside, koma.Masu, koma.Haiyaku),
                            new RO_Star_Koma(pside, dstMasu, koma.Haiyaku),//FIXME:配役は適当。
                            Ks14.H00_Null
                            );

                        resultMovebetuSky.Add(move, new KyokumenWrapper( Util_Sasu.Sasu(src_Sky, key, dstMasu, pside, logTag)));
                    }
            });


            return resultMovebetuSky;
        }

        public static Dictionary<ShootingStarlightable, KyokumenWrapper> KomabetuMasusToMovebetuSky(
            List_OneAndMulti<Finger, SySet<SyElement>> sMs, SkyConst src_Sky, Playerside pside, LarabeLoggerable logTag)
        {
            Dictionary<ShootingStarlightable, KyokumenWrapper> result = new Dictionary<ShootingStarlightable, KyokumenWrapper>();


            sMs.Foreach_Entry((Finger key, SySet<SyElement> value, ref bool toBreak) =>
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(key).Now);


                foreach (SyElement dstMasu in value.Elements)
                    {
                        ShootingStarlightable move = Util_Sky.New(
                            //key,
                            new RO_Star_Koma(pside, koma.Masu, koma.Haiyaku),
                            new RO_Star_Koma(pside, dstMasu, koma.Haiyaku),//FIXME:配役は適当。
                            Ks14.H00_Null
                            );

                        result.Add(move, new KyokumenWrapper( Util_Sasu.Sasu(src_Sky, key, dstMasu, pside, logTag)));
                    }
            });


            return result;
        }


        public static KifuNode MovebetuSky_ToHubNode(Dictionary<ShootingStarlightable, KyokumenWrapper> sasitebetuSkys, Playerside nextTebanside)
        {
            KifuNode hubNode = new KifuNodeImpl(null,null,Playerside.Empty);

            foreach (KeyValuePair<ShootingStarlightable, KyokumenWrapper> nextNode in sasitebetuSkys)
            {
                hubNode.Add_NextNode(
                    Util_Sky.ToSfenMoveText(nextNode.Key),
                    new KifuNodeImpl(nextNode.Key, nextNode.Value, nextTebanside)
                    );
            }

            return hubNode;
        }

        /// <summary>
        /// スプライト→駒
        /// </summary>
        /// <param name="fingers"></param>
        /// <param name="src_Sky"></param>
        /// <returns></returns>
        public static SySet<SyElement> Fingers_ToMasus(Fingers fingers, SkyConst src_Sky)
        {
            SySet<SyElement> masus = new SySet_Default<SyElement>("何かの升");

            foreach (Finger finger in fingers.Items)
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(finger).Now);


                    masus.AddElement(koma.Masu);
            }

            return masus;
        }

        public static string MoveToStringForLog(ShootingStarlightable move)
        {
            string sasiteInfo;

            RO_Star_Koma koma = Util_Koma.AsKoma(move.Now);

                sasiteInfo = KomaSyurui14Array.ToIchimoji(Haiyaku184Array.Syurui(koma.Haiyaku));

            return sasiteInfo;
        }

        public static string MoveToStringForLog(ShootingStarlightable move, Playerside pside_genTeban)
        {
            string result;

            if (null == move)
            {
                result = "合法手はありません。";
                goto gt_EndMethod;
            }

            RO_Star_Koma koma = Util_Koma.AsKoma(move.Now);

                // 指し手を「△歩」といった形で。
                result = KomaSyurui14Array.ToNimoji(Haiyaku184Array.Syurui(koma.Haiyaku), pside_genTeban);

        gt_EndMethod:
            return result;
        }

    }
}
