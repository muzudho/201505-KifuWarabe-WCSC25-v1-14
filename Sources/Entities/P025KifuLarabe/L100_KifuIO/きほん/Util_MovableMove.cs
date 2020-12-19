using Grayscale.P006_Syugoron;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L002_GraphicLog;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P025_KifuLarabe.L050_Things;
using System;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P025_KifuLarabe.L100_KifuIO;
using Grayscale.Kifuwarazusa.Entities.Logging;

namespace Grayscale.P025_KifuLarabe.L200_KifuIO
{
    public abstract class Util_MovableMove
    {
        /// <summary>
        /// 指定された局面で、指定された手番の駒の、移動可能マスを算出します。
        /// 利きを調べる目的ではありません。
        /// 
        /// 「手目」は判定できません。
        /// 
        /// </summary>
        /// <param name="kouho"></param>
        /// <param name="sbGohosyu"></param>
        /// <param name="logger"></param>
        public static List_OneAndMulti<Finger, SySet<SyElement>> LA_GetMove(
            bool enableLog,
            bool isHonshogi,
            SkyConst src_Sky,
            Playerside pside_genTeban3,
            bool isAiteban,
            GraphicalLog_Board logBrd_move,
            int yomuDeep_forLog,//脳内読み手数
            int tesumi_yomiCur_forLog,
            ShootingStarlightable move_forLog,
            ILogTag logTag
            )
        {
            logBrd_move.Caption = "移動可能_" + Converter04.MoveToStringForLog(move_forLog, pside_genTeban3);
            logBrd_move.Tesumi = tesumi_yomiCur_forLog;
            logBrd_move.NounaiYomiDeep = yomuDeep_forLog;
            //logBrd_move.Score = 0.0d;
            logBrd_move.GenTeban = pside_genTeban3;// 現手番


            // 《１》 移動可能場所
            List_OneAndMulti<Finger, SySet<SyElement>> sMs_move = new List_OneAndMulti<Finger, SySet<SyElement>>();
            {
                // 《１．１》
                Playerside tebanSeme;//手番（利きを調べる側）
                Playerside tebanKurau;//手番（喰らう側）
                {
                    if (isAiteban)
                    {
                        tebanSeme = Converter04.AlternatePside(pside_genTeban3);
                        tebanKurau = pside_genTeban3;
                    }
                    else
                    {
                        tebanSeme = pside_genTeban3;
                        tebanKurau = Converter04.AlternatePside(pside_genTeban3);
                    }

                    if (Playerside.P1 == tebanSeme)
                    {
                        logBrd_move.NounaiSeme = Gkl_NounaiSeme.Sente;
                    }
                    else if (Playerside.P2 == tebanSeme)
                    {
                        logBrd_move.NounaiSeme = Gkl_NounaiSeme.Gote;
                    }
                }


                // 《１．２》
                Fingers fingers_seme_IKUSA;//戦駒（利きを調べる側）
                Fingers fingers_kurau_IKUSA;//戦駒（喰らう側）
                Fingers fingers_seme_MOTI;// 持駒（利きを調べる側）
                Fingers fingers_kurau_MOTI;// 持駒（喰らう側）

                Util_Things.AAABAAAA_SplitGroup(
                        out fingers_seme_IKUSA,
                        out fingers_kurau_IKUSA,
                        out fingers_seme_MOTI,
                        out fingers_kurau_MOTI,
                        src_Sky,
                        tebanSeme,
                        tebanKurau,
                        logTag
                    );


                // 攻め手の駒の位置
                GraphicalLog_Board boardLog_clone = new GraphicalLog_Board(logBrd_move);
                foreach (Finger finger in fingers_seme_IKUSA.Items)
                {
                    RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(finger).Now);

                    Gkl_KomaMasu km = new Gkl_KomaMasu(
                        Util_GraphicalLog.PsideKs14_ToString(tebanSeme, Haiyaku184Array.Syurui(koma.Haiyaku), ""),
                        Util_Masu.AsMasuNumber(koma.Masu)
                        );
                    boardLog_clone.KomaMasu1.Add(km);
                }

                foreach (Finger finger in fingers_kurau_IKUSA.Items)
                {
                    RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(finger).Now);

                    logBrd_move.KomaMasu2.Add(new Gkl_KomaMasu(
                        Util_GraphicalLog.PsideKs14_ToString(tebanKurau, Haiyaku184Array.Syurui(koma.Haiyaku), ""),
                        Util_Masu.AsMasuNumber(koma.Masu)
                        ));
                }

                foreach (Finger finger in fingers_seme_MOTI.Items)
                {
                    RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(finger).Now);

                    Gkl_KomaMasu km = new Gkl_KomaMasu(
                        Util_GraphicalLog.PsideKs14_ToString(tebanSeme, Haiyaku184Array.Syurui(koma.Haiyaku), ""),
                        Util_Masu.AsMasuNumber(koma.Masu)
                        );
                    logBrd_move.KomaMasu3.Add(km);
                }

                foreach (Finger finger in fingers_kurau_MOTI.Items)
                {
                    RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(finger).Now);

                    logBrd_move.KomaMasu4.Add(new Gkl_KomaMasu(
                        Util_GraphicalLog.PsideKs14_ToString(tebanKurau, Haiyaku184Array.Syurui(koma.Haiyaku), ""),
                        Util_Masu.AsMasuNumber(koma.Masu)
                        ));
                }
                logBrd_move = boardLog_clone;




                // 《１．３》
                SySet<SyElement> masus_seme_IKUSA = Converter04.Fingers_ToMasus(fingers_seme_IKUSA, src_Sky);// 盤上のマス（利きを調べる側の駒）
                SySet<SyElement> masus_kurau_IKUSA = Converter04.Fingers_ToMasus(fingers_kurau_IKUSA, src_Sky);// 盤上のマス（喰らう側の駒）

                // 駒のマスの位置は、特にログに取らない。



                // 《１．４》
                Maps_OneAndOne<Finger, SySet<SyElement>> kmMove_seme_IKUSA = Util_Things.Get_KmEffect_seme_IKUSA(
                    fingers_seme_IKUSA,
                    masus_seme_IKUSA,
                    masus_kurau_IKUSA,
                    src_Sky,
                    enableLog,
                    Converter04.MoveToStringForLog(move_forLog, pside_genTeban3),
                    logTag
                    );// 盤上の駒の移動できる場所

                // 持ち駒を置ける場所
                List_OneAndMulti<Finger, SySet<SyElement>> sMsMove_seme_MOTI = Util_Things.Get_Move_Moti(
                    fingers_seme_MOTI,
                    masus_seme_IKUSA,
                    masus_kurau_IKUSA,
                    src_Sky,
                    Converter04.MoveToStringForLog(move_forLog, pside_genTeban3),
                    logTag
                    );

                // 戦駒の移動可能場所
                boardLog_clone = new GraphicalLog_Board(logBrd_move);
                kmMove_seme_IKUSA.Foreach_Entry((Finger key, SySet<SyElement> value, ref bool toBreak) =>
                {
                    RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(key).Now);

                    string komaImg = Util_GraphicalLog.PsideKs14_ToString(tebanSeme, Haiyaku184Array.Syurui(koma.Haiyaku), "");

                    foreach (Basho masu in value.Elements)
                    {
                        boardLog_clone.Masu_theMove.Add((int)masu.MasuNumber);
                    }
                });

                logBrd_move = boardLog_clone;


                try
                {
                    // 《１》　＝　《１．４》の戦駒＋持駒

                    // 盤上の駒の移動できる場所を足します。
                    sMs_move.AddRange_New(kmMove_seme_IKUSA);

                    // 持ち駒の置ける場所を足します。
                    sMs_move.AddRange_New(sMsMove_seme_MOTI);
                }
                catch (Exception ex)
                {
                    //>>>>> エラーが起こりました。

                    // どうにもできないので  ログだけ取って無視します。
                    Logger.WriteLineError(logTag, ex.GetType().Name + " " + ex.Message + "：ランダムチョイス(50)：");
                    throw ;
                }
            }

            return sMs_move;
        }
    }
}
