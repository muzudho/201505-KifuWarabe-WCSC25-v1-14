using System;
using System.Diagnostics;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwarazusa.Entities.Features
{
    public abstract class KifuIO_MovesExecuter
    {
        /// <summary>
        /// 符号１「7g7f」を元に、move を作ります。
        /// 
        /// ＜[再生]、[コマ送り]で呼び出されます＞
        /// </summary>
        /// <returns></returns>
        public static void ExecuteSfenMoves_FromTextSub(
            bool isHonshogi,
            string str1, //123456789 か、 PLNSGKRB
            string str2, //abcdefghi か、 *
            string str3, //123456789
            string str4, //abcdefghi
            string strNari, //+
            out ShootingStarlightable move,
            KifuTree kifu,
            string hint,
            int tesumi_yomiGenTeban//読み進めている現在の手目済
            )
        {
            move = Util_Sky.NullObjectMove;

            SkyConst src_Sky = kifu.NodeAt(tesumi_yomiGenTeban).Value.ToKyokumenConst;

            Debug.Assert(!Util_MasuNum.OnKomabukuro(Util_Masu.AsMasuNumber(((RO_Star_Koma)src_Sky.StarlightIndexOf((Finger)0).Now).Masu)), "[" + tesumi_yomiGenTeban + "]手目、駒が駒袋にあった。");

            try
            {
                PieceType uttaSyurui; // 打った駒の種類

                int srcSuji = Util_Koma.CTRL_NOTHING_PROPERTY_SUJI;
                int srcDan = Util_Koma.CTRL_NOTHING_PROPERTY_DAN;

                if ("*" == str2)
                {
                    //>>>>>>>>>> 「打」でした。

                    Converter04.SfenUttaSyurui(str1, out uttaSyurui);
                }
                else
                {
                    //>>>>>>>>>> 指しました。
                    uttaSyurui = PieceType.None;//打った駒はない☆

                    //------------------------------
                    // 1
                    //------------------------------
                    if (!int.TryParse(str1, out srcSuji))
                    {
                    }

                    //------------------------------
                    // 2
                    //------------------------------
                    srcDan = Converter04.Alphabet_ToInt(str2);
                }

                //------------------------------
                // 3
                //------------------------------
                int suji;
                if (!int.TryParse(str3, out suji))
                {
                }

                //------------------------------
                // 4
                //------------------------------
                int dan;
                dan = Converter04.Alphabet_ToInt(str4);

                Finger koma;

                if ("*" == str2)
                {
                    //>>>>> 「打」でした。
                    Node<ShootingStarlightable, KyokumenWrapper> siteiNode = kifu.CurNode;

                    // 駒台から、打った種類の駒を取得
                    koma = Util_Sky.FingerNow_BySyuruiIgnoreCase(
                        siteiNode.Value.ToKyokumenConst,
                        Converter04.Pside_ToKomadai(kifu.CountPside(kifu.CurNode)),//Okiba.Sente_Komadai,//FIXME:
                        uttaSyurui);
                    if (Fingers.Error_1 == koma)
                    {
                        throw new Exception($"TuginoItte_Sfen#GetData_FromTextSub：駒台から種類[{uttaSyurui}]の駒を掴もうとしましたが、エラーでした。");
                    }

                    //// FIXME: 打のとき、srcSuji、srcDan が Int.Min
                }
                else
                {
                    //>>>>> 打ではないとき
                    koma = Util_Sky.Fingers_AtMasuNow(
                        src_Sky, Util_Masu.OkibaSujiDanToMasu(Okiba.ShogiBan, srcSuji, srcDan)
                        ).ToFirst();

                    if (Fingers.Error_1 == koma)
                    {
                        //
                        // エラーの理由：
                        // 0手目、平手局面を想定していたが、駒がすべて駒袋に入っているときなど
                        //

                        var sky2 = Util_Sky.Json_1Sky(src_Sky, "エラー駒になったとき", $"{hint}_SF解3",
                            tesumi_yomiGenTeban);

                        throw new Exception($@"TuginoItte_Sfen#GetData_FromTextSub：SFEN解析中の失敗： 将棋盤から [{srcSuji}]筋、[{srcDan}]段 にある駒を掴もうとしましたが、駒がありませんでした。
isHonshogi=[{isHonshogi}]
str1=[{str1}]
str2=[{str2}]
str3=[{str3}]
str4=[{str4}]
strNari=[{strNari}]
hint=[{hint}]
tesumi_yomiGenTeban=[{tesumi_yomiGenTeban}]
局面=sfen {Util_SfenStartposWriter.CreateSfenstring(new StartposExporter(src_Sky), true)}
{sky2}");
                    }
                }


                PieceType dstSyurui;
                PieceType srcSyurui;
                Okiba srcOkiba;
                SyElement srcMasu;


                if ("*" == str2)
                {
                    //>>>>> 打った駒の場合

                    dstSyurui = uttaSyurui;
                    srcSyurui = uttaSyurui;
                    switch (kifu.CountPside(kifu.CurNode))
                    {
                        case Playerside.P2:
                            srcOkiba = Okiba.Gote_Komadai;
                            break;
                        case Playerside.P1:
                            srcOkiba = Okiba.Sente_Komadai;
                            break;
                        default:
                            srcOkiba = Okiba.Empty;
                            break;
                    }

                    int lastTesumi = kifu.CountTesumi(kifu.CurNode);
                    Node<ShootingStarlightable, KyokumenWrapper> siteiNode = kifu.NodeAt(lastTesumi);

                    Finger srcKoma = Util_Sky.FingerNow_BySyuruiIgnoreCase(siteiNode.Value.ToKyokumenConst, srcOkiba, srcSyurui);

                    RO_Star_Koma dstKoma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(srcKoma).Now);

                    srcMasu = dstKoma.Masu;
                }
                else
                {
                    //>>>>> 盤上の駒を指した場合

                    RO_Star_Koma dstKoma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(koma).Now);


                    dstSyurui = Haiyaku184Array.Syurui(dstKoma.Haiyaku);
                    srcSyurui = dstSyurui; //駒は「元・種類」を記憶していませんので、「現・種類」を指定します。
                    srcOkiba = Okiba.ShogiBan;
                    srcMasu = Util_Masu.OkibaSujiDanToMasu(srcOkiba, srcSuji, srcDan);
                }


                //------------------------------
                // 5
                //------------------------------
                if ("+" == strNari)
                {
                    // 成りました
                    dstSyurui = KomaSyurui14Array.NariCaseHandle[(int)dstSyurui];
                }


                //------------------------------
                // 結果
                //------------------------------
                // 棋譜
                move = new RO_ShootingStarlight(
                    //koma,//TODO:

                    new RO_Star_Koma(
                        kifu.CountPside(kifu.CurNode),
                        srcMasu,//FIXME:升ハンドルにしたい
                        srcSyurui
                    ),

                    new RO_Star_Koma(
                        kifu.CountPside(kifu.CurNode),
                        Util_Masu.OkibaSujiDanToMasu(Okiba.ShogiBan, suji, dan),//符号は将棋盤の升目です。 FIXME:升ハンドルにしたい
                        dstSyurui
                        ),

                    PieceType.None//符号からは、取った駒は分からない
                );
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。

                // どうにもできないので　経路と情報を付け足して　更に外側に投げます。
                throw new Exception($"{ex.GetType().Name}：{ex.Message}　in　TuginoItte_Sfen.GetData_FromTextSub（A）　str1=「{str1}」　str2=「{str2}」　str3=「{str3}」　str4=「{str4}」　strNari=「{strNari}」　");
            }
        }



        /// <summary>
        /// 次の１手データを作ります(*1)
        /// 
        ///         *1…符号１「▲６８銀上」を元に、「7968」を作ります。
        /// 
        /// ＜[再生]、[コマ送り]で呼び出されます＞
        /// </summary>
        /// <returns></returns>
        public static void ExecuteJfugo_FromTextSub(
            string strPside, //▲△
            string strSuji, //123…9、１２３…９、一二三…九
            string strDan, //123…9、１２３…９、一二三…九
            string strDou, // “同”
            string strSrcSyurui, //(歩|香|桂|…
            string strMigiHidari,           // 右|左…
            string strAgaruHiku, // 上|引
            string strNariNarazu, //成|不成
            string strDaHyoji, //打
            out ShootingStarlightable move,
            KifuTree kifu
            )
        {
            Node<ShootingStarlightable, KyokumenWrapper> siteiNode = kifu.CurNode;
            SkyConst src_Sky = siteiNode.Value.ToKyokumenConst;

            //------------------------------
            // 符号確定
            //------------------------------
            MigiHidari migiHidari = Converter04.Str_ToMigiHidari(strMigiHidari);
            AgaruHiku agaruHiku = Converter04.Str_ToAgaruHiku(strAgaruHiku);            // 上|引
            NariNarazu nariNarazu = Converter04.Nari_ToBool(strNariNarazu);//成
            DaHyoji daHyoji = Converter04.Str_ToDaHyoji(strDaHyoji);             //打

            PieceType srcSyurui = Converter04.Str_ToSyurui(strSrcSyurui);


            //------------------------------
            // 
            //------------------------------
            Playerside pside = Converter04.Pside_ToEnum(strPside);


            SyElement dstMasu;

            if ("同" == strDou)
            {
                // 1手前の筋、段を求めるのに使います。

                RO_Star_Koma koma = Util_Koma.AsKoma(siteiNode.Key.Now);

                dstMasu = koma.Masu;
            }
            else
            {
                dstMasu = Util_Masu.OkibaSujiDanToMasu(
                    Okiba.ShogiBan,
                    ConverterKnSh.Suji_ToInt(strSuji),
                    ConverterKnSh.Suji_ToInt(strDan)
                    );
            }

            // TODO: 駒台にあるかもしれない。
            Okiba srcOkiba1 = Okiba.ShogiBan; //Okiba.FUMEI_FUGO_YOMI_CHOKUGO;// Okiba.SHOGIBAN;
            if (DaHyoji.Visible == daHyoji)
            {
                if (Playerside.P2 == pside)
                {
                    srcOkiba1 = Okiba.Gote_Komadai;
                }
                else
                {
                    srcOkiba1 = Okiba.Sente_Komadai;
                }
            }

            // 
            SyElement dst1 = dstMasu;

            Finger foundKoma = Fingers.Error_1;

            //----------
            // 駒台の駒を(明示的に)打つなら
            //----------
            bool utsu = false;//駒台の駒を打つなら真
            if (DaHyoji.Visible == daHyoji)
            {
                utsu = true;
                goto gt_EndShogiban;
            }

            if (PieceType.P == srcSyurui)
            {
                //************************************************************
                // 歩
                //************************************************************

                //----------
                // 候補マス
                //----------
                //┌─┬─┬─┐
                //│  │  │  │
                //├─┼─┼─┤
                //│  │至│  │
                //├─┼─┼─┤
                //│  │Ｅ│  │
                //└─┴─┴─┘
                bool isE = true;

                SySet<SyElement> srcAll = new SySet_Default<SyElement>("J符号");
                if (isE) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻引(pside, dst1)); }

                if (KifuIO_MovesParsers.Hit_JfugoParser(pside, srcSyurui, srcAll, kifu, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
            }
            else if (PieceType.R == srcSyurui)
            {
                #region 飛
                //************************************************************
                // 飛
                //************************************************************
                //----------
                // 候補マス
                //----------
                //  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┐
                //  │  │  │  │  │  │  │  │  │A7│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A6│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A5│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A4│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //　│  │  │  │  │  │  │  │  │A3│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A2│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A1│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A0│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │G7│G6│G5│G4│G3│G2│G1│G0│至│C0│C1│C2│C3│C4│C5│C6│C7│
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E0│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E1│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E2│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E3│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //　│  │  │  │  │  │  │  │  │E4│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E5│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E6│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E7│  │  │  │  │  │  │  │  │
                //  └─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┘
                bool isA = true;
                bool isC = true;
                bool isE = true;
                bool isG = true;

                switch (agaruHiku)
                {
                    case AgaruHiku.Yoru:
                        isA = false;
                        isE = false;
                        break;
                    case AgaruHiku.Agaru:
                        isA = false;
                        isC = false;
                        isG = false;
                        break;
                    case AgaruHiku.Hiku:
                        isC = false;
                        isE = false;
                        isG = false;
                        break;
                }

                switch (migiHidari)
                {
                    case MigiHidari.Migi:
                        isA = false;
                        isE = false;
                        isG = false;
                        break;
                    case MigiHidari.Hidari:
                        isA = false;
                        isC = false;
                        isE = false;
                        break;
                    case MigiHidari.Sugu:
                        isA = false;
                        isC = false;
                        isG = false;
                        break;
                }

                SySet<SyElement> srcAll = new SySet_Default<SyElement>("J符号");
                if (isA) { srcAll.AddSupersets(KomanoKidou.SrcKantu_戻上(pside, dst1)); }
                if (isC) { srcAll.AddSupersets(KomanoKidou.SrcKantu_戻射(pside, dst1)); }
                if (isE) { srcAll.AddSupersets(KomanoKidou.SrcKantu_戻引(pside, dst1)); }
                if (isG) { srcAll.AddSupersets(KomanoKidou.SrcKantu_戻滑(pside, dst1)); }

                if (KifuIO_MovesParsers.Hit_JfugoParser(pside, srcSyurui, srcAll, kifu, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else if (PieceType.B == srcSyurui)
            {
                #region 角
                //************************************************************
                // 角
                //************************************************************
                //----------
                // 候補マス
                //----------
                //  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┐
                //  │H7│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │B7│
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │H6│  │  │  │  │  │  │  │  │  │  │  │  │  │B6│  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │H5│  │  │  │  │  │  │  │  │  │  │  │B5│  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │H4│  │  │  │  │  │  │  │  │  │B4│  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //　│  │  │  │  │H3│  │  │  │  │  │  │  │B3│  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │H2│  │  │  │  │  │B2│  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │H1│  │  │  │B1│  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │H0│  │B0│  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │至│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │F0│  │D0│  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │F1│  │  │  │D1│  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │F2│  │  │  │  │  │D2│  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │F3│  │  │  │  │  │  │  │D3│  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //　│  │  │  │F4│  │  │  │  │  │  │  │  │  │D4│  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │F5│  │  │  │  │  │  │  │  │  │  │  │D5│  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │F6│  │  │  │  │  │  │  │  │  │  │  │  │  │D6│  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │F7│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │D7│
                //  └─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┘
                bool isB = true;
                bool isD = true;
                bool isF = true;
                bool isH = true;

                switch (agaruHiku)
                {
                    case AgaruHiku.Yoru:
                        isB = false;
                        isD = false;
                        isF = false;
                        isH = false;
                        break;
                    case AgaruHiku.Agaru:
                        isB = false;
                        isH = false;
                        break;
                    case AgaruHiku.Hiku:
                        isB = false;
                        isH = false;
                        break;
                }

                switch (migiHidari)
                {
                    case MigiHidari.Migi:
                        isF = false;
                        isH = false;
                        break;
                    case MigiHidari.Hidari:
                        isB = false;
                        isD = false;
                        break;
                    case MigiHidari.Sugu:
                        isD = false;
                        isF = false;
                        break;
                }

                SySet_Default<SyElement> srcAll = new SySet_Default<SyElement>("J符号");
                if (isB) { srcAll.AddSupersets(KomanoKidou.SrcKantu_戻昇(pside, dst1)); }
                if (isD) { srcAll.AddSupersets(KomanoKidou.SrcKantu_戻沈(pside, dst1)); }
                if (isF) { srcAll.AddSupersets(KomanoKidou.SrcKantu_戻降(pside, dst1)); }
                if (isH) { srcAll.AddSupersets(KomanoKidou.SrcKantu_戻浮(pside, dst1)); }

                //----------
                // 候補マスＢ
                //----------
                if (KifuIO_MovesParsers.Hit_JfugoParser(pside, srcSyurui, srcAll, kifu, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else if (PieceType.L == srcSyurui)
            {
                #region 香
                //************************************************************
                // 香
                //************************************************************
                //----------
                // 候補マス
                //----------
                //  ┌─┬─┬─┐
                //  │  │至│  │
                //  ├─┼─┼─┤
                //  │  │E0│  │
                //  ├─┼─┼─┤
                //  │  │E1│  │
                //  ├─┼─┼─┤
                //  │  │E2│  │
                //  ├─┼─┼─┤
                //  │  │E3│  │
                //  ├─┼─┼─┤
                //  │  │E4│  │
                //  ├─┼─┼─┤
                //  │  │E5│  │
                //  ├─┼─┼─┤
                //  │  │E6│  │
                //  ├─┼─┼─┤
                //  │  │E7│  │
                //  └─┴─┴─┘
                bool isE = true;

                SySet<SyElement> srcAll = new SySet_Default<SyElement>("J符号");
                if (isE) { srcAll.AddSupersets(KomanoKidou.SrcKantu_戻引(pside, dst1)); }

                if (KifuIO_MovesParsers.Hit_JfugoParser(pside, srcSyurui, srcAll, kifu, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else if (PieceType.N == srcSyurui)
            {
                #region 桂
                //************************************************************
                // 桂
                //************************************************************
                //----------
                // 候補マス
                //----------
                //┌─┐　┌─┐
                //│  │　│  │
                //├─┼─┼─┤
                //│　│  │  │
                //├─┼─┼─┤
                //│　│至│  │先手から見た図
                //├─┼─┼─┤
                //│　│  │  │
                //├─┼─┼─┤
                //│Ｊ│　│Ｉ│
                //└─┘　└─┘
                bool isI = true;
                bool isJ = true;

                switch (agaruHiku)
                {
                    case AgaruHiku.Yoru:
                        isI = false;
                        isJ = false;
                        break;
                    case AgaruHiku.Agaru:
                        break;
                    case AgaruHiku.Hiku:
                        isI = false;
                        isJ = false;
                        break;
                }

                switch (migiHidari)
                {
                    case MigiHidari.Migi:
                        isJ = false;
                        break;
                    case MigiHidari.Hidari:
                        isI = false;
                        break;
                    case MigiHidari.Sugu:
                        isI = false;
                        isJ = false;
                        break;
                }

                SySet<SyElement> srcAll = new SySet_Default<SyElement>("J符号");
                if (isI) { srcAll.AddSupersets(KomanoKidou.SrcKeimatobi_戻跳(pside, dst1)); }
                if (isJ) { srcAll.AddSupersets(KomanoKidou.SrcKeimatobi_戻駆(pside, dst1)); }

                if (KifuIO_MovesParsers.Hit_JfugoParser(pside, srcSyurui, srcAll, kifu, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else if (PieceType.S == srcSyurui)
            {
                #region 銀
                //************************************************************
                // 銀
                //************************************************************
                //----------
                // 候補マス
                //----------
                //┌─┬─┬─┐
                //│Ｈ│  │Ｂ│
                //├─┼─┼─┤
                //│　│至│  │先手から見た図
                //├─┼─┼─┤
                //│Ｆ│Ｅ│Ｄ│
                //└─┴─┴─┘
                bool isB = true;
                bool isD = true;
                bool isE = true;
                bool isF = true;
                bool isH = true;

                switch (agaruHiku)
                {
                    case AgaruHiku.Yoru:
                        isB = false;
                        isD = false;
                        isE = false;
                        isF = false;
                        isH = false;
                        break;
                    case AgaruHiku.Agaru:
                        isB = false;
                        isH = false;
                        break;
                    case AgaruHiku.Hiku:
                        isD = false;
                        isE = false;
                        isF = false;
                        break;
                }

                switch (migiHidari)
                {
                    case MigiHidari.Migi:
                        isE = false;
                        isF = false;
                        isH = false;
                        break;
                    case MigiHidari.Hidari:
                        isB = false;
                        isD = false;
                        isE = false;
                        break;
                    case MigiHidari.Sugu:
                        isB = false;
                        isD = false;
                        isF = false;
                        isH = false;
                        break;
                }

                SySet<SyElement> srcAll = new SySet_Default<SyElement>("J符号");
                if (isB) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻昇(pside, dst1)); }
                if (isD) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻沈(pside, dst1)); }
                if (isE) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻引(pside, dst1)); }
                if (isF) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻降(pside, dst1)); }
                if (isH) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻浮(pside, dst1)); }

                if (KifuIO_MovesParsers.Hit_JfugoParser(pside, srcSyurui, srcAll, kifu, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else if (
                PieceType.G == srcSyurui
                || PieceType.PP == srcSyurui
                || PieceType.PL == srcSyurui
                || PieceType.PN == srcSyurui
                || PieceType.PS == srcSyurui
                )
            {
                #region △金、△と金、△成香、△成桂、△成銀
                //************************************************************
                // △金、△と金、△成香、△成桂、△成銀
                //************************************************************
                //----------
                // 候補マス
                //----------
                //┌─┬─┬─┐
                //│  │Ａ│  │
                //├─┼─┼─┤
                //│Ｇ│至│Ｃ│先手から見た図
                //├─┼─┼─┤
                //│Ｆ│Ｅ│Ｄ│
                //└─┴─┴─┘
                bool isA = true;
                bool isC = true;
                bool isD = true;
                bool isE = true;
                bool isF = true;
                bool isG = true;

                switch (agaruHiku)
                {
                    case AgaruHiku.Yoru:
                        isA = false;
                        isD = false;
                        isE = false;
                        isF = false;
                        break;
                    case AgaruHiku.Agaru:
                        isA = false;
                        isC = false;
                        isG = false;
                        break;
                    case AgaruHiku.Hiku:
                        isC = false;
                        isD = false;
                        isE = false;
                        isF = false;
                        isG = false;
                        break;
                }

                switch (migiHidari)
                {
                    case MigiHidari.Migi:
                        isA = false;
                        isE = false;
                        isF = false;
                        isG = false;
                        break;
                    case MigiHidari.Hidari:
                        isA = false;
                        isC = false;
                        isD = false;
                        isE = false;
                        break;
                    case MigiHidari.Sugu:
                        isA = false;
                        isC = false;
                        isD = false;
                        isF = false;
                        isG = false;
                        break;
                }

                SySet<SyElement> srcAll = new SySet_Default<SyElement>("J符号");
                if (isA) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻上(pside, dst1)); }
                if (isC) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻射(pside, dst1)); }
                if (isD) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻沈(pside, dst1)); }
                if (isE) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻引(pside, dst1)); }
                if (isF) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻降(pside, dst1)); }
                if (isG) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻滑(pside, dst1)); }

                if (KifuIO_MovesParsers.Hit_JfugoParser(pside, srcSyurui, srcAll, kifu, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else if (PieceType.K == srcSyurui)
            {
                #region 王
                //************************************************************
                // 王
                //************************************************************

                //----------
                // 候補マス
                //----------
                //┌─┬─┬─┐
                //│Ｈ│Ａ│Ｂ│
                //├─┼─┼─┤
                //│Ｇ│至│Ｃ│先手から見た図
                //├─┼─┼─┤
                //│Ｆ│Ｅ│Ｄ│
                //└─┴─┴─┘
                bool isA = true;
                bool isB = true;
                bool isC = true;
                bool isD = true;
                bool isE = true;
                bool isF = true;
                bool isG = true;
                bool isH = true;

                SySet<SyElement> srcAll = new SySet_Default<SyElement>("J符号");
                if (isA) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻上(pside, dst1)); }
                if (isB) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻昇(pside, dst1)); }
                if (isC) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻射(pside, dst1)); }
                if (isD) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻沈(pside, dst1)); }
                if (isE) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻引(pside, dst1)); }
                if (isF) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻降(pside, dst1)); }
                if (isG) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻滑(pside, dst1)); }
                if (isH) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻浮(pside, dst1)); }

                // 王は１つです。
                if (KifuIO_MovesParsers.Hit_JfugoParser(pside, srcSyurui, srcAll, kifu, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else if (PieceType.PR == srcSyurui)
            {
                #region 竜
                //************************************************************
                // 竜
                //************************************************************

                //----------
                // 候補マス
                //----------
                //  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┐
                //  │  │  │  │  │  │  │  │  │A7│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A6│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A5│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A4│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //　│  │  │  │  │  │  │  │  │A3│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A2│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │A1│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │Ｈ│A0│Ｂ│  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │G7│G6│G5│G4│G3│G2│G1│G0│至│C0│C1│C2│C3│C4│C5│C6│C7│
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │Ｆ│E0│Ｄ│  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E1│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E2│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E3│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //　│  │  │  │  │  │  │  │  │E4│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E5│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E6│  │  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │  │E7│  │  │  │  │  │  │  │  │
                //  └─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┘
                bool isA = true;
                bool isB = true;
                bool isC = true;
                bool isD = true;
                bool isE = true;
                bool isF = true;
                bool isG = true;
                bool isH = true;

                switch (agaruHiku)
                {
                    case AgaruHiku.Yoru:
                        isA = false;
                        isB = false;
                        isD = false;
                        isE = false;
                        isF = false;
                        isH = false;
                        break;
                    case AgaruHiku.Agaru:
                        isA = false;
                        isB = false;
                        isC = false;
                        isG = false;
                        isH = false;
                        break;
                    case AgaruHiku.Hiku:
                        isC = false;
                        isD = false;
                        isE = false;
                        isF = false;
                        isG = false;
                        break;
                }

                switch (migiHidari)
                {
                    case MigiHidari.Migi:
                        isA = false;
                        isE = false;
                        isF = false;
                        isG = false;
                        isH = false;
                        break;
                    case MigiHidari.Hidari:
                        isA = false;
                        isB = false;
                        isC = false;
                        isD = false;
                        isE = false;
                        break;
                    case MigiHidari.Sugu:
                        isA = false;
                        isB = false;
                        isC = false;
                        isD = false;
                        isF = false;
                        isG = false;
                        isH = false;
                        break;
                }

                SySet<SyElement> srcAll = new SySet_Default<SyElement>("J符号");
                if (isA) { srcAll.AddSupersets(KomanoKidou.SrcKantu_戻上(pside, dst1)); }
                if (isB) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻昇(pside, dst1)); }
                if (isC) { srcAll.AddSupersets(KomanoKidou.SrcKantu_戻射(pside, dst1)); }
                if (isD) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻沈(pside, dst1)); }
                if (isE) { srcAll.AddSupersets(KomanoKidou.SrcKantu_戻引(pside, dst1)); }
                if (isF) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻降(pside, dst1)); }
                if (isG) { srcAll.AddSupersets(KomanoKidou.SrcKantu_戻滑(pside, dst1)); }
                if (isH) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻浮(pside, dst1)); }

                if (KifuIO_MovesParsers.Hit_JfugoParser(pside, srcSyurui, srcAll, kifu, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else if (PieceType.PB == srcSyurui)
            {
                #region 馬
                //************************************************************
                // 馬
                //************************************************************
                //----------
                // 候補マス
                //----------
                //  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┐
                //  │H7│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │B7│
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │H6│  │  │  │  │  │  │  │  │  │  │  │  │  │B6│  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │H5│  │  │  │  │  │  │  │  │  │  │  │B5│  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │H4│  │  │  │  │  │  │  │  │  │B4│  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //　│  │  │  │  │H3│  │  │  │  │  │  │  │B3│  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │H2│  │  │  │  │  │B2│  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │H1│  │  │  │B1│  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │H0│Ａ│B0│  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │Ｇ│至│Ｃ│  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │  │F0│Ｅ│D0│  │  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │  │F1│  │  │  │D1│  │  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │  │F2│  │  │  │  │  │D2│  │  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │  │  │F3│  │  │  │  │  │  │  │D3│  │  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //　│  │  │  │F4│  │  │  │  │  │  │  │  │  │D4│  │  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │  │F5│  │  │  │  │  │  │  │  │  │  │  │D5│  │  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │  │F6│  │  │  │  │  │  │  │  │  │  │  │  │  │D6│  │
                //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
                //  │F7│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │D7│
                //  └─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┘
                bool isA = true;
                bool isB = true;
                bool isC = true;
                bool isD = true;
                bool isE = true;
                bool isF = true;
                bool isG = true;
                bool isH = true;

                switch (agaruHiku)
                {
                    case AgaruHiku.Yoru:
                        isA = false;
                        isB = false;
                        isD = false;
                        isE = false;
                        isF = false;
                        isH = false;
                        break;
                    case AgaruHiku.Agaru:
                        isA = false;
                        isB = false;
                        isC = false;
                        isG = false;
                        isH = false;
                        break;
                    case AgaruHiku.Hiku:
                        isC = false;
                        isD = false;
                        isE = false;
                        isF = false;
                        isG = false;
                        break;
                }

                switch (migiHidari)
                {
                    case MigiHidari.Migi:
                        isA = false;
                        isE = false;
                        isF = false;
                        isG = false;
                        isH = false;
                        break;
                    case MigiHidari.Hidari:
                        isA = false;
                        isB = false;
                        isC = false;
                        isD = false;
                        isE = false;
                        break;
                    case MigiHidari.Sugu:
                        isA = false;
                        isB = false;
                        isC = false;
                        isD = false;
                        isF = false;
                        isG = false;
                        isH = false;
                        break;
                }

                SySet<SyElement> srcAll = new SySet_Default<SyElement>("J符号");
                if (isA) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻上(pside, dst1)); }
                if (isB) { srcAll.AddSupersets(KomanoKidou.SrcKantu_戻昇(pside, dst1)); }
                if (isC) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻射(pside, dst1)); }
                if (isD) { srcAll.AddSupersets(KomanoKidou.SrcKantu_戻沈(pside, dst1)); }
                if (isE) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻引(pside, dst1)); }
                if (isF) { srcAll.AddSupersets(KomanoKidou.SrcKantu_戻降(pside, dst1)); }
                if (isG) { srcAll.AddSupersets(KomanoKidou.SrcIppo_戻滑(pside, dst1)); }
                if (isH) { srcAll.AddSupersets(KomanoKidou.SrcKantu_戻浮(pside, dst1)); }

                if (KifuIO_MovesParsers.Hit_JfugoParser(pside, srcSyurui, srcAll, kifu, out foundKoma))
                {
                    srcOkiba1 = Okiba.ShogiBan;
                    goto gt_EndSyurui;
                }
                #endregion
            }
            else
            {
                #region エラー
                //************************************************************
                // エラー
                //************************************************************

                #endregion
            }

        gt_EndShogiban:

            if (Fingers.Error_1 == foundKoma && utsu)
            {
                // 駒台の駒を(明示的に)打ちます。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


                Fingers komas = Util_Sky.Fingers_ByOkibaPsideSyuruiNow(
                    siteiNode.Value.ToKyokumenConst, srcOkiba1, pside, srcSyurui);

                if (0 < komas.Count)
                {
                    switch (pside)
                    {
                        case Playerside.P2:
                            srcOkiba1 = Okiba.Gote_Komadai;
                            break;
                        case Playerside.P1:
                            srcOkiba1 = Okiba.Sente_Komadai;
                            break;
                        default:
                            srcOkiba1 = Okiba.Empty;
                            break;
                    }

                    foundKoma = komas[0];
                    goto gt_EndSyurui;
                }
            }

        gt_EndSyurui:


            int srcMasuHandle1;

            if (Fingers.Error_1 != foundKoma)
            {
                // 将棋盤の上に駒がありました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(foundKoma).Now);

                srcMasuHandle1 = Util_Masu.AsMasuNumber(koma.Masu);
            }
            else
            {
                // （符号に書かれていませんが）「打」のとき。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                switch (pside)
                {
                    case Playerside.P2:
                        srcOkiba1 = Okiba.Gote_Komadai;
                        break;
                    case Playerside.P1:
                        srcOkiba1 = Okiba.Sente_Komadai;
                        break;
                    default:
                        srcOkiba1 = Okiba.Empty;
                        break;
                }


                Debug.Assert(0 < siteiNode.Value.ToKyokumenConst.Count, "星の光が 1個未満。");

                // 駒台から、該当する駒を探します。
                Fingers daiKomaFgs = Util_Sky.Fingers_ByOkibaPsideSyuruiNow(
                    siteiNode.Value.ToKyokumenConst, srcOkiba1, pside, srcSyurui);//(2014-10-04 12:46)変更


                Debug.Assert(0 < daiKomaFgs.Count, "フィンガーズが 1個未満。 srcOkiba1=[" + srcOkiba1 + "] pside=[" + pside + "] srcSyurui=[" + srcSyurui + "]");

                // 1個はヒットするはず
                Finger hitKoma = daiKomaFgs[0];//▲！[コマ送り]ボタンを連打すると、エラーになります。



                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(hitKoma).Now);

                srcMasuHandle1 = Util_Masu.AsMasuNumber(koma.Masu);
            }


            PieceType dstSyurui;
            if (NariNarazu.Nari == nariNarazu)
            {
                // 成ります
                dstSyurui = KomaSyurui14Array.NariCaseHandle[(int)srcSyurui];
            }
            else
            {
                // そのままです。
                dstSyurui = srcSyurui;
            }


            // １手を、データにします。
            move = new RO_ShootingStarlight(
                //foundKoma,//TODO:

                new RO_Star_Koma(
                    pside,
                    Util_Masu.HandleToMasu(srcMasuHandle1),
                    srcSyurui
                ),

                new RO_Star_Koma(
                    pside,
                    dstMasu,//符号は将棋盤の升目です。
                    dstSyurui
                ),

                PieceType.None // 符号からは、取った駒の種類は分からないんだぜ☆　だがバグではない☆　あとで調べる☆
            );
        }

    }
}
