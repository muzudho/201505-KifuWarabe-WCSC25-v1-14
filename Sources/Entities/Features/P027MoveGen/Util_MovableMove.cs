using System;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwarazusa.Entities.Features
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
            ShootingStarlightable move_forLog
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
                        tebanKurau
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
                    Converter04.MoveToStringForLog(move_forLog, pside_genTeban3)
                    );// 盤上の駒の移動できる場所

                // 持ち駒を置ける場所
                List_OneAndMulti<Finger, SySet<SyElement>> sMsMove_seme_MOTI = Util_Things.Get_Move_Moti(
                    fingers_seme_MOTI,
                    masus_seme_IKUSA,
                    masus_kurau_IKUSA,
                    src_Sky,
                    Converter04.MoveToStringForLog(move_forLog, pside_genTeban3)
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


                // 《１》　＝　《１．４》の戦駒＋持駒

                // 盤上の駒の移動できる場所を足します。
                sMs_move.AddRange_New(kmMove_seme_IKUSA);

                // 持ち駒の置ける場所を足します。
                sMs_move.AddRange_New(sMsMove_seme_MOTI);
            }

            return sMs_move;
        }

        public static void SplitGroup_Teban(
            out Playerside tebanSeme,//手番（利きを調べる側）
            out Playerside tebanKurau,//手番（喰らう側）
            bool isAiteban,
            Playerside pside_genTeban3
            )
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
        }

        public static void SplitGroup_Ikusa(
            out SySet<SyElement> masus_seme_IKUSA,
            out SySet<SyElement> masus_kurau_IKUSA,
            SkyConst src_Sky,
            Fingers fingers_kurau_IKUSA,//戦駒（喰らう側）
            Fingers fingers_seme_IKUSA//戦駒（利きを調べる側）
            )
        {
            masus_seme_IKUSA = Converter04.Fingers_ToMasus(fingers_seme_IKUSA, src_Sky);// 盤上のマス（利きを調べる側の駒）
            masus_kurau_IKUSA = Converter04.Fingers_ToMasus(fingers_kurau_IKUSA, src_Sky);// 盤上のマス（喰らう側の駒）
        }

        /// <summary>
        /// 駒別の進めるマスを返します。
        /// 
        /// 指定された局面で、指定された手番の駒の、移動可能マスを算出します。
        /// 利きを調べる目的ではありません。
        /// 
        /// 「手目」は判定できません。
        /// 
        /// </summary>
        /// <param name="kouho"></param>
        /// <param name="sbGohosyu"></param>
        /// <param name="logger"></param>
        public static void LA_Get_KomaBETUSusumeruMasus(
            out List_OneAndMulti<Finger, SySet<SyElement>> komaBETUSusumeruMasus,
            MmGenjo_MovableMasu mmGenjo,
            MmLogGenjo log_orNull
            )
        {
            if (null != log_orNull)
            {
                log_orNull.Log1(mmGenjo.Pside_genTeban3);
            }


            // 《１》 移動可能場所
            komaBETUSusumeruMasus = new List_OneAndMulti<Finger, SySet<SyElement>>();
            {
                //
                // 《１．１》 手番を２つのグループに分類します。
                //
                //  ┌─手番──────┬──────┐
                //  │                  │            │
                //  │  利きを調べる側  │  喰らう側  │
                //  └─────────┴──────┘
                Playerside tebanSeme;   //手番（利きを調べる側）
                Playerside tebanKurau;  //手番（喰らう側）
                {
                    Util_MovableMove.SplitGroup_Teban(out tebanSeme, out tebanKurau, mmGenjo.IsAiteban, mmGenjo.Pside_genTeban3);
                    if (null != log_orNull)
                    {
                        log_orNull.Log2(tebanSeme, tebanKurau);
                    }
                }


                //
                // 《１．２》 駒を４つのグループに分類します。
                //
                //  ┌─駒──────────┬─────────┐
                //  │                        │                  │
                //  │  利きを調べる側の戦駒  │  喰らう側の戦駒  │
                //  ├────────────┼─────────┤
                //  │                        │                  │
                //  │  利きを調べる側の持駒  │  喰らう側の持駒  │
                //  └────────────┴─────────┘
                //
                Fingers fingers_seme_IKUSA;//戦駒（利きを調べる側）
                Fingers fingers_kurau_IKUSA;//戦駒（喰らう側）
                Fingers fingers_seme_MOTI;// 持駒（利きを調べる側）
                Fingers fingers_kurau_MOTI;// 持駒（喰らう側）
                {
                    Util_Things.AAABAAAA_SplitGroup(out fingers_seme_IKUSA, out fingers_kurau_IKUSA, out fingers_seme_MOTI, out fingers_kurau_MOTI, mmGenjo.Src_Sky, tebanSeme, tebanKurau);
                    if (null != log_orNull)
                    {
                        log_orNull.Log3(mmGenjo.Src_Sky, tebanKurau, tebanSeme, fingers_kurau_IKUSA, fingers_kurau_MOTI, fingers_seme_IKUSA, fingers_seme_MOTI);
                    }
                }





                //
                // 《１．３》 駒を２つのグループに分類します。
                //
                //  ┌─盤上のマス───┬──────┐
                //  │                  │            │
                //  │  利きを調べる側  │  喰らう側  │
                //  └─────────┴──────┘
                //
                SySet<SyElement> masus_seme_IKUSA;// 盤上のマス（利きを調べる側の駒）
                SySet<SyElement> masus_kurau_IKUSA;// 盤上のマス（喰らう側の駒）
                {
                    Util_MovableMove.SplitGroup_Ikusa(out masus_seme_IKUSA, out masus_kurau_IKUSA, mmGenjo.Src_Sky, fingers_kurau_IKUSA, fingers_seme_IKUSA);
                    // 駒のマスの位置は、特にログに取らない。
                }



                // 《１．４》
                Maps_OneAndOne<Finger, SySet<SyElement>> kmSusumeruMasus_seme_IKUSA;
                if (null != log_orNull)
                {
                    kmSusumeruMasus_seme_IKUSA = Util_Things.Get_KmEffect_seme_IKUSA(
                        fingers_seme_IKUSA,
                        masus_seme_IKUSA,
                        masus_kurau_IKUSA,
                        mmGenjo.Src_Sky,
                        log_orNull.Enable,
                        Converter04.MoveToStringForLog(log_orNull.Move, mmGenjo.Pside_genTeban3)
                        );// 盤上の駒の移動できる場所
                }
                else
                {
                    kmSusumeruMasus_seme_IKUSA = Util_Things.Get_KmEffect_seme_IKUSA(
                        fingers_seme_IKUSA,
                        masus_seme_IKUSA,
                        masus_kurau_IKUSA,
                        mmGenjo.Src_Sky,
                        false,//log.Enable,
                        ""
                        );// 盤上の駒の移動できる場所
                }

                // 持ち駒を置ける場所
                List_OneAndMulti<Finger, SySet<SyElement>> sMsSusumeruMasus_seme_MOTI;
                if (null != log_orNull)
                {
                    sMsSusumeruMasus_seme_MOTI = Util_Things.Get_Move_Moti(
                        fingers_seme_MOTI,
                        masus_seme_IKUSA,
                        masus_kurau_IKUSA,
                        mmGenjo.Src_Sky,
                        Converter04.MoveToStringForLog(log_orNull.Move, mmGenjo.Pside_genTeban3)
                        );
                }
                else
                {
                    sMsSusumeruMasus_seme_MOTI = Util_Things.Get_Move_Moti(
                        fingers_seme_MOTI,
                        masus_seme_IKUSA,
                        masus_kurau_IKUSA,
                        mmGenjo.Src_Sky,
                        ""
                        );
                }

                if (null != log_orNull)
                {
                    log_orNull.Log4(mmGenjo.Src_Sky, tebanSeme, kmSusumeruMasus_seme_IKUSA);
                }



                try
                {
                    // 《１》　＝　《１．４》の戦駒＋持駒

                    // 盤上の駒の移動できる場所を足します。
                    komaBETUSusumeruMasus.AddRange_New(kmSusumeruMasus_seme_IKUSA);

                    // 持ち駒の置ける場所を足します。
                    komaBETUSusumeruMasus.AddRange_New(sMsSusumeruMasus_seme_MOTI);
                }
                catch (Exception ex)
                {
                    //>>>>> エラーが起こりました。
                    throw new Exception($"{ex.GetType().Name} {ex.Message}：ランダムチョイス(50)：");
                }

            }
        }

    }
}
