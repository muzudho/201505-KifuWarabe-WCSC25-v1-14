using Grayscale.P006_Syugoron;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P025_KifuLarabe.L002_GraphicLog;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L007_Random;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P025_KifuLarabe.L100_KifuIO;
using Grayscale.P027MoveGen.L0005MoveGen;
using Grayscale.P027MoveGen.L050_MovableMove;
using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using System;
using System.Diagnostics;

namespace Grayscale.P027MoveGen.L100MoveGen
{

    /// <summary>
    /// 指し手生成ルーチン
    /// </summary>
    public class MoveGenRoutine
    {

        /// <summary>
        /// 読む。
        /// 
        /// 棋譜ツリーを作成します。
        /// </summary>
        /// <param name="kifu">この棋譜ツリーの現局面に、次局面をぶら下げて行きます。</param>
        /// <param name="enableLog"></param>
        /// <param name="isHonshogi"></param>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public static void WAA_Yomu_Start(
            KifuTree kifu,
            bool isHonshogi,
            SsssLogGenjo log
            )
        {

            //------------------------------------------------------------
            // （＞＿＜）次の１手の合法手の中からランダムに選ぶぜ☆！
            //------------------------------------------------------------
            //
            // バグ探し：
            //          ①次の１手の合法手のリスト作成
            //          ②ランダムに１手選ぶ
            //
            //          の２つしかやっていないんだが、合法手ではない手を返してくるんだぜ☆

#if DEBUG_STOPPABLE
            MessageBox.Show("ここでブレイク☆！", "デバッグ");
            System.Diagnostics.Debugger.Break();
            //throw new Exception("デバッグだぜ☆！　エラーはキャッチできたかな～☆？（＾▽＾）");
#endif


            KifuNode node_yomiNext = (KifuNode)kifu.CurNode;// このノードに、ツリーをぶら下げていきます。
            int yomuIndex = 0;



            // TODO:ここではログを出力せずに、ツリーの先端で出力したい。
            GraphicalLog_File logF_moveKiki = new GraphicalLog_File();

            // TODO:「読む」と、ツリー構造が作成されます。
            //int[] yomuLimitter = new int[]{
            //    600, // 読みの1手目の横幅   // 王手回避漏れのために、合法手全読み(約600)は必要です。
            //    100, // 読みの2手目の横幅
            //    100, // 読みの3手目の横幅
            //    //2, // 読みの4手目の横幅
            //    //1 // 読みの5手目の横幅
            //};

            //// ↓これなら１手１秒で指せる☆
            //int[] yomuLimitter = new int[]{
            //    600, // 読みの1手目の横幅   // 王手回避漏れのために、合法手全読み(約600)は必要です。
            //    150, // 読みの2手目の横幅
            //    150, // 読みの3手目の横幅
            //    //2 // 読みの4手目の横幅
            //    //1 // 読みの5手目の横幅
            //};

            //int[] yomuLimitter = new int[]{
            //    600, // 読みの1手目の横幅   // 王手回避漏れのために、合法手全読み(約600)は必要です。
            //    600, // 読みの2手目の横幅
            //    600, // 読みの3手目の横幅
            //};

            //ok
            //int[] yomuLimitter = new int[]{
            //    0,   // 現局面は無視します。
            //    600, // 読みの1手目の横幅   // 王手回避漏れのために、合法手全読み(約600)は必要です。
            //    600, // 読みの2手目の横幅
            //};

            int[] yomuLimitter = new int[]{
                0,   // 現局面は無視します。
                600, // 読みの1手目の横幅   // 王手回避漏れのために、１手目は、合法手全読み(約600)は必要です。
                600, // 読みの2手目の横幅
                //600, // 読みの3手目の横幅
            };

#if DEBUG
            // ログの出力数を減らすために、読みを弱くします。

            //yomuLimitter = new int[]{
            //    0,  // 現局面は無視します。
            //    600, // 読みの1手目の横幅   // 王手回避漏れのために、合法手全読み(約600)は必要です。
            //    100, // 読みの2手目の横幅
            //    100 // 読みの3手目の横幅
            //};

            yomuLimitter = new int[]{
                0,  // 現局面は無視します。
                600, // 読みの1手目の横幅   // 王手回避漏れのために、１手目は、合法手全読み(約600)は必要です。
                600, // 読みの2手目の横幅
                //600 // 読みの3手目の横幅
            };

            //yomuLimitter = new int[]{
            //    600, // 読みの1手目の横幅   // 王手回避漏れのために、合法手全読み(約600)は必要です。
            //    600, // 読みの2手目の横幅
            //    600, // 読みの3手目の横幅
            //    100,
            //    100
            //};

#endif
            MoveGenArgs yomiArgs = new MoveGenArgsImpl(isHonshogi, yomuLimitter, logF_moveKiki);
            MoveGenGenjo yomiGenjo = new MoveGenGenjoImpl(yomiArgs, yomuIndex + 1, kifu.CurrentTesumi(), kifu.CountPside(node_yomiNext));
            MoveGenRoutine.WAAA_Yomu_Loop(node_yomiNext, yomiGenjo, log);

            if (0<logF_moveKiki.boards.Count)//ﾛｸﾞが残っているなら
            {
                //
                // ログの書き出し
                //
                Util_GraphicalLog.Log(
                    true,//enableLog,
                    "MoveRoutine#Yomi_NextNodes(00)新ログ",
                    "[" + Util_GraphicalLog.BoardFileLog_ToJsonStr(logF_moveKiki) + "]"
                );

                // 書き出した分はクリアーします。
                logF_moveKiki.boards.Clear();
            }
        }



        /// <summary>
        /// 棋譜ツリーに、ノードを追加していきます。再帰します。
        /// </summary>
        /// <param name="node_yomiCur"></param>
        /// <param name="yomiGenjo"></param>
        /// <param name="isAiteban"></param>
        /// <param name="log"></param>
        private static void WAAA_Yomu_Loop(
            KifuNode node_yomiCur,
            MoveGenGenjo yomiGenjo,
            SsssLogGenjo log
            )
        {
            //
            // まず前提として、
            // 現手番の「被王手の局面」だけがピックアップされます。
            // これはつまり、次の局面がないときは、その枝は投了ということです。
            //

            // このハブ・ノード自身は空っぽで、ハブ・ノードの次ノードが、次局面のリストになっています。
            KifuNode hub_NextNodes = MoveGenRoutine.WAAAA_CreateNextNodes(yomiGenjo, node_yomiCur, log);

            //
            // （２）現在の局面に、読んだ局面を継ぎ足します。
            //
            node_yomiCur.AppdendNextNodes( hub_NextNodes);


            MoveGenRoutine.WAAAA_Read_NextBranch(yomiGenjo, hub_NextNodes, log);

        }

        /// <summary>
        /// まず前提として、
        /// 現手番の「被王手の局面」だけがピックアップされます。
        /// これはつまり、次の局面がないときは、その枝は投了ということです。
        /// </summary>
        /// <param name="enableLog"></param>
        /// <param name="isHonshogi"></param>
        /// <param name="yomuDeep"></param>
        /// <param name="tesumi_yomiCur"></param>
        /// <param name="pside_yomiCur"></param>
        /// <param name="node_yomiCur"></param>
        /// <param name="logF_moveKiki"></param>
        /// <param name="logTag"></param>
        /// <returns>複数のノードを持つハブ・ノード</returns>
        private static KifuNode WAAAA_CreateNextNodes(
            MoveGenGenjo genjo,
            KifuNode node_yomiCur,
            SsssLogGenjo log
            )
        {
            // 利きから、被王手の局面を除いたハブノード
            // このハブ・ノード自身は空っぽで、ハブ・ノードの次ノードが、次局面のリストになっています。
            KifuNode hubNode;
            {
                // ①現手番の駒の移動可能場所_被王手含む
                List_OneAndMulti<Finger, SySet<SyElement>> komaBETUSusumeruMasus;
                {
                    GraphicalLog_Board logBrd_move1 = new GraphicalLog_Board();// 盤１個分のログの準備

                    Util_MovableMove.LA_Get_KomaBETUSusumeruMasus(
                        out komaBETUSusumeruMasus,//進めるマス
                        new MmGenjo_MovableMasuImpl(
                            genjo.Args.IsHonshogi,//本将棋か
                            node_yomiCur.Value.ToKyokumenConst,//現在の局面
                            genjo.Pside_teban,//手番
                            false//相手番か
                            ),
                        new MmLogGenjoImpl(
                            log.EnableLog,//ログを出力するかfalse,
                            logBrd_move1,//ログ？
                            genjo.YomuDeep,//読みの深さ
                            genjo.Tesumi_yomiCur,//手済み
                            node_yomiCur.Key,//指し手
                            log.LogTag//ログ
                        )
                    );

                    MoveGenRoutine.Log1(genjo,node_yomiCur, logBrd_move1);//ログ試し
                }


                // ②利きから、被王手の局面を除いたハブノード
                if (genjo.Args.IsHonshogi)
                {
                    Maps_OneAndMulti<Finger, IMove> komaBetuAllMoves = Converter04.KomaBETUSusumeruMasus_ToKomaBETUAllMoves(komaBETUSusumeruMasus, node_yomiCur);
                    Converter04.AssertNariMove(komaBetuAllMoves, "#WAAAA_CreateNextNodes(1)");

                    // 本将棋の場合、王手されている局面は削除します。
                    hubNode = Util_LegalMove.LA_RemoveMate(
                        genjo.Args.IsHonshogi,
                        komaBetuAllMoves,
                        genjo.YomuDeep,
                        genjo.Tesumi_yomiCur,
                        genjo.Pside_teban,
                        node_yomiCur,
                        log.EnableLog,
                        genjo.Args.LogF_moveKiki,//利き用
                        "読みNextルーチン",
                        log.LogTag);

                    try
                    {
                        Converter04.AddNariMove(node_yomiCur, hubNode, log.LogTag);
                    }
                    catch (Exception ex)
                    {
                        //>>>>> エラーが起こりました。
                        string message = ex.GetType().Name + " " + ex.Message + "：王手局面除去後に成りの指し手を追加していた時です。：";
                        Debug.Fail(message);

                        // どうにもできないので  ログだけ取って、上に投げます。
                        log.LogTag.WriteLine_Error(message);
                        throw ;
                    }


                    Converter04.AssertNariMove(hubNode, "#WAAAA_CreateNextNodes(2)");//ここで消えていた☆
                }
                else
                {
                    //そのまま変換
                    Dictionary<IMove, KyokumenWrapper> ss = Converter04.KomabetuMasusToMoveBetuSky(
                        komaBETUSusumeruMasus,
                        node_yomiCur.Value.ToKyokumenConst,
                        genjo.Pside_teban,
                        log.LogTag);
                    hubNode = Converter04.MoveBetuSkyToHubNode(ss, KifuNodeImpl.GetReverseTebanside(genjo.Pside_teban));
                }
            }

            return hubNode;
        }
        private static void Log1(
            MoveGenGenjo genjo,
            Node<IMove, KyokumenWrapper> node_yomiCur,
            GraphicalLog_Board logBrd_move1
            )
        {
            logBrd_move1.moveOrNull = ((KifuNode)node_yomiCur).Key;


            RO_Star_Koma srcKoma = Util_Koma.AsKoma(logBrd_move1.moveOrNull.MoveSource);
            RO_Star_Koma dstKoma = Util_Koma.AsKoma(logBrd_move1.moveOrNull.MoveSource);


            // ログ試し
            logBrd_move1.Arrow.Add(new Gkl_Arrow(Util_Masu.AsMasuNumber(srcKoma.Masu), Util_Masu.AsMasuNumber(dstKoma.Masu)));
            genjo.Args.LogF_moveKiki.boards.Add(logBrd_move1);
        }


        /// <summary>
        /// 次の枝を読みます。
        /// </summary>
        private static void WAAAA_Read_NextBranch(
            MoveGenGenjo yomiGenjo,
            Node<IMove, KyokumenWrapper> hubNode_genTeban,
            SsssLogGenjo log
            )
        {
            // （３）次のノードをシャッフルします。
            List<Node<IMove, KyokumenWrapper>> nextNodes_shuffled = Converter04.NextNodes_ToList(hubNode_genTeban, log.LogTag);
            LarabeShuffle<Node<IMove, KyokumenWrapper>>.Shuffle_FisherYates(ref nextNodes_shuffled);

            // （４）次の局面
            int wideCount = 0;
            foreach (KifuNode nextNode in nextNodes_shuffled)
            {
                // （５）読みの深さ２手目以降なら、横幅制限
                if (1 < yomiGenjo.YomuDeep)
                {
                    if (yomiGenjo.Args.YomuLimitter[yomiGenjo.YomuDeep] <= wideCount)
                    {
                        break;// もう次の手の横には読まない。
                    }
                }


                // 《８》カウンターを次局面へ（２手目の読み）

                // 《９》まだ深く読むなら
                if (yomiGenjo.YomuDeep + 1 < yomiGenjo.Args.YomuLimitter.Length)
                {
                    yomiGenjo.YomuDeep ++;
                    yomiGenjo.Tesumi_yomiCur++;
                    yomiGenjo.Pside_teban = yomiGenjo.Pside_teban == Playerside.P1 ? Playerside.P2 : Playerside.P1;//先後を反転します。
                    MoveGenRoutine.WAAA_Yomu_Loop(nextNode, yomiGenjo,log);
                    yomiGenjo.YomuDeep--;//元に戻します。
                    yomiGenjo.Tesumi_yomiCur--;//元に戻します。
                    yomiGenjo.Pside_teban = yomiGenjo.Pside_teban == Playerside.P1 ? Playerside.P2 : Playerside.P1;//元に戻します。
                }
                else
                {
                    // もう深くよまないなら
                }

                wideCount++;
            }


            if (yomiGenjo.Args.YomuLimitter.Length <= yomiGenjo.YomuDeep + 1)//もう深く読まないなら
            {
                //
                // ログの書き出し
                //
                Util_GraphicalLog.Log(
                    true,//enableLog,
                    "指し手生成ログA",
                    "[" + Util_GraphicalLog.BoardFileLog_ToJsonStr(yomiGenjo.Args.LogF_moveKiki) + "]"
                );

                // 書き出した分はクリアーします。
                yomiGenjo.Args.LogF_moveKiki.boards.Clear();
            }
        }

    }

}
