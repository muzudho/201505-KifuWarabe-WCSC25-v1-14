using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L007_Random;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P006_Syugoron;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P025_KifuLarabe.L00050_StructShogi;

namespace Grayscale.P025_KifuLarabe.L100_KifuIO
{


    public abstract class KifuIO
    {


        /// <summary>
        /// 一手指します。または、一手戻します。
        /// </summary>
        /// <param name="move"></param>
        /// <param name="kifu"></param>
        /// <param name="isMakimodosi"></param>
        /// <param name="figMovedKoma"></param>
        /// <param name="figFoodKoma">取られた駒</param>
        /// <param name="out_newNode_OrNull"></param>
        /// <param name="logTag"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void Ittesasi(
            ShootingStarlightable move,
            KifuTree kifu,
            bool isMakimodosi,
            out Finger figMovedKoma,
            out Finger figFoodKoma,
            out Node<ShootingStarlightable, KyokumenWrapper> out_newNode_OrNull,
            LarabeLoggerable logTag
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            out_newNode_OrNull = null;
            figFoodKoma = Fingers.Error_1;


            KifuIO.Kifusasi25(
                out figMovedKoma,
                move,
                kifu,
                isMakimodosi,
                logTag
                );


            if (Fingers.Error_1 == figMovedKoma)
            {
                goto gt_EndMethod;
            }


            Ks14 syurui2 = KifuIO.Kifusasi30_Naru(move, isMakimodosi);


            Starlight dst = KifuIO.Kifusasi35_NextMasu(syurui2, move, kifu, isMakimodosi);



            KifuIO.Kifusasi52_WhenKifuRead(
                dst,
                syurui2,
                ref figMovedKoma,
                out figFoodKoma,
                move, 
                kifu,
                isMakimodosi,
                out out_newNode_OrNull,
                logTag
                );



        gt_EndMethod:

            if (isMakimodosi)
            {
                Node<ShootingStarlightable, KyokumenWrapper> removedLeaf = kifu.PopCurrentNode();
            }

            logTag.WriteLine_AddMemo("一手指しが終わったぜ☆　ノードが追加されているんじゃないか☆？　");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="figMovedKoma"></param>
        /// <param name="move">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="kifu"></param>
        /// <param name="isMakimodosi"></param>
        private static void Kifusasi25(
            out Finger figMovedKoma,
            ShootingStarlightable move,
            KifuTree kifu,
            bool isMakimodosi,
            LarabeLoggerable logTag
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            figMovedKoma = Fingers.Error_1;

            SkyConst src_Sky = kifu.CurNode.Value.ToKyokumenConst;

            //------------------------------------------------------------
            // 選択  ：  動かす駒
            //------------------------------------------------------------
            if (isMakimodosi)
            {
                // [巻戻し]のとき
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // 打った駒も、指した駒も、結局は将棋盤の上にあるはず。

                RO_Star_Koma koma = Util_Koma.AsKoma(move.Now);

                // 動かす駒
                figMovedKoma = Util_Sky.Finger_AtMasuNow_Shogiban(
                    src_Sky,
                    koma.Pside,
                    koma.Masu,//[巻戻し]のときは、先位置が　駒の居場所。
                    logTag
                    );
            }
            else
            {
                // 進むとき
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                //------------------------------
                // 符号の追加（一手進む）
                //------------------------------
                KifuNode genKyokumenCopyNode = new KifuNodeImpl(move, new KyokumenWrapper(src_Sky), KifuNodeImpl.GetReverseTebanside(((KifuNode)kifu.CurNode).Tebanside));

                // TODO: ↓？
                ((KifuNode)kifu.CurNode).AppendChildA_New(genKyokumenCopyNode);
                kifu.CurNode = genKyokumenCopyNode;

                if (Util_Sky.IsDaAction(move))
                {
                    //----------
                    // 駒台から “打”
                    //----------

                    RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);
                    RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);


                    figMovedKoma = Util_Sky.FingerNow_BySyuruiIgnoreCase(
                        src_Sky,
                        Util_Masu.GetOkiba(srcKoma.Masu),
                        Haiyaku184Array.Syurui(dstKoma.Haiyaku),
                        logTag
                        );
                }
                else
                {
                    //----------
                    // 将棋盤から
                    //----------

                    RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);
                    RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);


                    figMovedKoma = Util_Sky.Finger_AtMasuNow_Shogiban(
                        src_Sky,
                        dstKoma.Pside,
                        Util_Masu.OkibaSujiDanToMasu(
                            Util_Masu.GetOkiba(Masu_Honshogi.Items_All[Util_Masu.AsMasuNumber(dstKoma.Masu)]),
                            Util_Masu.AsMasuNumber(srcKoma.Masu)
                            ),
                            logTag
                            );

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="move">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="back"></param>
        /// <returns></returns>
        private static Ks14 Kifusasi30_Naru(
            ShootingStarlightable move,
            bool back)
        {
            //------------------------------------------------------------
            // 確定  ：  移動先升
            //------------------------------------------------------------
            Ks14 syurui2;
            {
                //----------
                // 成るかどうか
                //----------

                RO_Star_Koma koma = Util_Koma.AsKoma(move.Now);


                if (Util_Sky.IsNatta_Sasite(move))
                {
                    if (back)
                    {
                        // 正順で成ったのなら、巻戻しでは「非成」に戻します。
                        syurui2 = KomaSyurui14Array.NarazuCaseHandle(Haiyaku184Array.Syurui(koma.Haiyaku));
                    }
                    else
                    {
                        syurui2 = Haiyaku184Array.Syurui(koma.Haiyaku);
                    }
                }
                else
                {
                    syurui2 = Haiyaku184Array.Syurui(koma.Haiyaku);
                }
            }

            return syurui2;
        }

        /// <summary>
        /// [巻戻し]時の、駒台にもどる動きを吸収。
        /// </summary>
        /// <param name="syurui2"></param>
        /// <param name="move">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="kifu"></param>
        /// <param name="isMakimodosi"></param>
        /// <returns></returns>
        private static Starlight Kifusasi35_NextMasu(
            Ks14 syurui2,
            ShootingStarlightable move,
            KifuTree kifu,
            bool isMakimodosi)
        {
            Starlight dst;

            RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);//移動元
            RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);//移動先


            if (isMakimodosi)
            {
                SyElement masu;

                if (
                    Okiba.Gote_Komadai == Util_Masu.GetOkiba(srcKoma.Masu)
                    || Okiba.Sente_Komadai == Util_Masu.GetOkiba(srcKoma.Masu)
                    )
                {
                    //>>>>> １手前が駒台なら

                    // 駒台の空いている場所

                    Node<ShootingStarlightable, KyokumenWrapper> node6 = kifu.CurNode;

                    masu = KifuIO.GetKomadaiKomabukuroSpace(Util_Masu.GetOkiba(srcKoma.Masu), node6.Value.ToKyokumenConst);
                    // 必ず空いている場所があるものとします。
                }
                else
                {
                    //>>>>> １手前が将棋盤上なら

                    // その位置
                    masu = srcKoma.Masu;//戻し先
                }



                dst = new RO_MotionlessStarlight(
                    //move.Finger,
                    new RO_Star_Koma(dstKoma.Pside,
                    masu,//戻し先
                    syurui2)
                    );
            }
            else
            {
                // 次の位置


                dst = new RO_MotionlessStarlight(
                    //move.Finger,
                    new RO_Star_Koma(dstKoma.Pside,
                    dstKoma.Masu,
                    syurui2)
                    );
            }

            return dst;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜[再生]時用（マウス操作のときは関係ない）
        /// ************************************************************************************************************************
        /// 
        ///         一手進む、一手[巻戻し]に対応。
        /// 
        /// </summary>
        /// <param name="move">棋譜に記録するために「指す前／指した後」を含めた手。</param>
        /// <param name="kifu"></param>
        /// <param name="isMakimodosi"></param>
        private static void Kifusasi52_WhenKifuRead(
            Starlight dst,
            Ks14 syurui2,
            ref Finger figMovedKoma,
            out Finger out_figFoodKoma,
            ShootingStarlightable move,
            KifuTree kifu,
            bool isMakimodosi,
            out Node<ShootingStarlightable, KyokumenWrapper> out_newNode_OrNull,
            LarabeLoggerable logTag
            )
        {
            out_figFoodKoma = Fingers.Error_1;

            // Sky 局面データは、この関数の途中で何回か変更されます。ローカル変数に退避しておくと、同期が取れなくなります。

            //------------------------------------------------------------
            // 駒を取る
            //------------------------------------------------------------
            if (!isMakimodosi)
            {

                RO_Star_Koma dstKoma = Util_Koma.AsKoma(dst.Now);


                Ks14 foodKomaSyurui;//取られた駒の種類

                //----------
                // 将棋盤上のその場所に駒はあるか
                //----------
                foodKomaSyurui = Ks14.H00_Null;//ひとまずクリアー
                out_figFoodKoma = Util_Sky.Fingers_AtMasuNow(kifu.CurNode.Value.ToKyokumenConst, dstKoma.Masu).ToFirst();//盤上

                if (Fingers.Error_1 != out_figFoodKoma)
                {
                    //>>>>> 指した先に駒があったなら

                    //
                    // 取られる駒
                    //
                    foodKomaSyurui = Util_Koma.AsKoma(kifu.CurNode.Value.ToKyokumenConst.StarlightIndexOf(out_figFoodKoma).Now).Syurui;

                    //
                    // 取られる駒は、駒置き場の空きマスに移動させます。
                    //
                    Okiba okiba;
                    Playerside pside;
                    switch (dstKoma.Pside)
                    {
                        case Playerside.P1:
                            {
                                okiba = Okiba.Sente_Komadai;
                                pside = Playerside.P1;
                            }
                            break;
                        case Playerside.P2:
                            {
                                okiba = Okiba.Gote_Komadai;
                                pside = Playerside.P2;
                            }
                            break;
                        default:
                            {
                                //>>>>> エラー：　先後がおかしいです。

                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine("エラー：　先後がおかしいです。");
                                sb.AppendLine("dst.Pside=" + dstKoma.Pside);
                                throw new Exception(sb.ToString());
                            }
                    }

                    // 駒台の空きスペース
                    SyElement akiMasu = akiMasu = KifuIO.GetKomadaiKomabukuroSpace(okiba, kifu.CurNode.Value.ToKyokumenConst);

                    if (Masu_Honshogi.Error == akiMasu)
                    {
                        //>>>>> エラー：　駒台に空きスペースがありませんでした。

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("エラー：　駒台に空きスペースがありませんでした。");
                        sb.AppendLine("駒台=" + Okiba.Gote_Komadai);
                        throw new Exception(sb.ToString());
                    }

                    //>>>>> 駒台に空きスペースがありました。

                    //
                    // 取られる動き
                    //
                    {
                        SkyBuffer buffer_Sky = new SkyBuffer(kifu.CurNode.Value.ToKyokumenConst);
                        buffer_Sky.AddOverwriteStarlight(
                            out_figFoodKoma,
                            new RO_MotionlessStarlight(
                                //out_figFoodKoma,
                                new RO_Star_Koma(
                                    pside,
                                    akiMasu,//駒台の空きマスへ
                                    Util_Koma.AsKoma(kifu.CurNode.Value.ToKyokumenConst.StarlightIndexOf(out_figFoodKoma).Now).Syurui//駒の種類
                                )
                            )
                        );
                        kifu.CurNode.Value.SetKyokumen(new SkyConst(buffer_Sky));
                        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                        // この時点で局面データに変更あり
                        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                    }




                    //------------------------------
                    // 成っていれば、「成り」は解除。
                    //------------------------------
                    RO_Star_Koma koma3 = Util_Koma.AsKoma(kifu.CurNode.Value.ToKyokumenConst.StarlightIndexOf(out_figFoodKoma).Now);


                    switch (Util_Masu.GetOkiba(koma3.Masu))
                    {
                        case Okiba.Sente_Komadai://thru
                        case Okiba.Gote_Komadai:
                            // 駒台へ移動しました
                            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                            SkyBuffer buffer_Sky = new SkyBuffer(kifu.CurNode.Value.ToKyokumenConst);
                            buffer_Sky.AddOverwriteStarlight(out_figFoodKoma, new RO_MotionlessStarlight(
                                //out_figFoodKoma,
                                    new RO_Star_Koma(koma3.Pside,
                                    koma3.Masu,
                                    koma3.ToNarazuCase())
                                )
                            );
                            kifu.CurNode.Value.SetKyokumen(new SkyConst(buffer_Sky));
                            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                            // この時点で局面データに変更あり
                            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

                            break;
                    }


                    //------------------------------
                    // 取った駒を棋譜に覚えさせます。（差替え）
                    //------------------------------
                    kifu.AppendChildB_Swap(
                        foodKomaSyurui,
                        kifu.CurNode.Value.ToKyokumenConst,
                        "KifuIO_Kifusasi52",
                        logTag
                    );
                    //}
                }
            }

            //------------------------------------------------------------
            // 駒の移動
            //------------------------------------------------------------
            {
                SkyBuffer buffer_Sky = new SkyBuffer(kifu.CurNode.Value.ToKyokumenConst);
                buffer_Sky.AddOverwriteStarlight(figMovedKoma, dst);
                kifu.CurNode.Value.SetKyokumen(new SkyConst(buffer_Sky));
            }
            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            // この時点で局面データに変更あり
            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■


            //------------------------------------------------------------
            // 取った駒を戻す
            //------------------------------------------------------------
            if (isMakimodosi)
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(move.Now);


                if (Ks14.H00_Null != (Ks14)move.FoodKomaSyurui)
                {
                    // 駒台から、駒を検索します。
                    Okiba okiba;
                    if (Playerside.P2 == koma.Pside)
                    {
                        okiba = Okiba.Gote_Komadai;
                    }
                    else
                    {
                        okiba = Okiba.Sente_Komadai;
                    }


                    // 取った駒は、種類が同じなら、駒台のどの駒でも同じです。
                    Finger temp_figFoodKoma = Util_Sky.FingerNow_BySyuruiIgnoreCase(kifu.CurNode.Value.ToKyokumenConst, okiba, (Ks14)move.FoodKomaSyurui, logTag);
                    if (Fingers.Error_1 != temp_figFoodKoma)
                    {
                        // 取った駒のデータをセットし直します。
                        SkyBuffer buffer_Sky = new SkyBuffer(kifu.CurNode.Value.ToKyokumenConst);

                        buffer_Sky.AddOverwriteStarlight(
                            temp_figFoodKoma,
                            new RO_MotionlessStarlight(
                                //temp_figFoodKoma,
                                new RO_Star_Koma(
                                    Converter04.AlternatePside(koma.Pside),//先後を逆にして駒台に置きます。
                                    koma.Masu,// マス
                                    (Ks14)move.FoodKomaSyurui
                                )
                            )
                        );

                        kifu.CurNode.Value.SetKyokumen(new SkyConst(buffer_Sky));
                        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                        // この時点で局面データに変更あり
                        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

                        out_figFoodKoma = temp_figFoodKoma;
                    }

                }
            }


            //
            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            // 局面データに変更があったものとして進めます。
            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            //
            out_newNode_OrNull = kifu.CurNode;// この変数を返すのがポイント。
            {
                out_newNode_OrNull.Value.SetKyokumen(kifu.CurNode.Value.ToKyokumenConst);

                kifu.CurNode.Value.SetKyokumen(kifu.CurNode.Value.ToKyokumenConst);
            }
        }



        /// <summary>
        /// ************************************************************************************************************************
        /// 駒台の空いている升を返します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="okiba">先手駒台、または後手駒台</param>
        /// <param name="uc_Main">メインパネル</param>
        /// <returns>置ける場所。無ければヌル。</returns>
        public static SyElement GetKomadaiKomabukuroSpace(Okiba okiba, SkyConst src_Sky)
        {
            SyElement akiMasu = Masu_Honshogi.Error;

            // 先手駒台または後手駒台の、各マスの駒がある場所を調べます。
            bool[] exists = new bool[Util_Masu.KOMADAI_KOMABUKURO_SPACE_LENGTH];//駒台スペースは40マスです。


            src_Sky.Foreach_Starlights((Finger finger, Starlight komaP, ref bool toBreak) =>
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(komaP.Now);

                    if (Util_Masu.GetOkiba(koma.Masu) == okiba)
                    {
                        exists[
                            Util_Masu.AsMasuNumber(koma.Masu) - Util_Masu.AsMasuNumber(Util_Masu.GetFirstMasuFromOkiba(okiba))
                            ] = true;
                    }
            });


            //駒台スペースは40マスです。
            for (int i = 0; i < Util_Masu.KOMADAI_KOMABUKURO_SPACE_LENGTH;i++ )
            {
                if (!exists[i])
                {
                    akiMasu = Masu_Honshogi.Items_All[i + Util_Masu.AsMasuNumber(Util_Masu.GetFirstMasuFromOkiba(okiba))];
                    goto gt_EndMethod;
                }
            }

        gt_EndMethod:

            //System.C onsole.WriteLine("ゲット駒台駒袋スペース＝" + akiMasu);

            return akiMasu;
        }


    }


}
