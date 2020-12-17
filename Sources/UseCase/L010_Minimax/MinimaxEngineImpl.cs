using Grayscale.P007_SfenReport.L00025_Report;
using Grayscale.P007_SfenReport.L100_Write;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P025_KifuLarabe.L002_GraphicLog;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P027MoveGen.L050_MovableMove;
using Grayscale.P045_Atama.L00025_KyHandan;
using Grayscale.P050_KifuWarabe.L00025_UsiLoop;
using Grayscale.P050_KifuWarabe.L00049_Kokoro;
using Grayscale.P050_KifuWarabe.L00050_KyHyoka;
using Grayscale.P050_KifuWarabe.L00051_Minimax;
using Grayscale.P050_KifuWarabe.L009_Hyoka;
using Grayscale.P050_KifuWarabe.L009_KyHyoka;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P006_Syugoron;
using System;
using System.Diagnostics;
using Grayscale.Kifuwarazusa.Entities;

namespace Grayscale.P050_KifuWarabe.L010_Minimax
{
    /// <summary>
    /// ミニマックス法エンジン
    /// </summary>
    public class MinimaxEngineImpl : MinimaxEngine
    {

        //private static int logFileCounter;

        private ShogiEngine owner;
        private KyHyokaCalculator hyokaEngineImpl;

        public MinimaxEngineImpl(ShogiEngine owner)
        {
            this.hyokaEngineImpl = new KyHyokaCalculatorImpl();

            this.owner = owner;

        }


        /// <summary>
        /// 棋譜ツリーの、ノードのネクストノードに、点数を付けていきます。
        /// </summary>
        public void Tensuduke_ForeachLeafs(
            string nodePath,
            KifuNode node,
            KifuTree kifu,
            Kokoro kokoro,
            PlayerInfo playerInfo,
            ReportEnvironment reportEnvironment,//MinimaxEngineImpl.REPORT_ENVIRONMENT
            GraphicalLog_File logF_kiki,
            LarabeLoggerable logTag
            )
        {

            // 次ノードの有無
            if (node.Count_NextNodes < 1)
            {
                try{
                    // 次ノードが無ければ、このノードが、葉です。
                    // 点数を付けます。

                    // 局面スコア
                    node.KyHyoka.Clear();

                    // 妄想と、指定のノードを比較し、点数付けします。
                    foreach (Tenonagare nagare in kokoro.TenonagareItems)
                    {
                        node.KyHyoka.Add(
                            nagare.Name.ToString(),
                            this.hyokaEngineImpl.LetHandan(nagare, node, playerInfo)
                        );
                    }

#if DEBUG
                    //
                    // 盤１個分のログの準備
                    //
                    this.Log_Board(
                        nodePath,
                        node,
                        kifu,
                        reportEnvironment,
                        logF_kiki,
                        logTag
                    );
#endif
                }
                catch (Exception ex)
                {
                    //>>>>> エラーが起こりました。
                    string message = ex.GetType().Name + " " + ex.Message + "：葉に点数を付けたときです。：";
                    Debug.Fail(message);

                    // どうにもできないので  ログだけ取って、上に投げます。
                    Logger.Log_Engine.WriteLine_Error(message);
                    throw ;
                }

            }
            else
            {
                try{
                    node.Foreach_NextNodes((string key, Node<ShootingStarlightable, KyokumenWrapper> nextNode, ref bool toBreak) =>
                    {
                        this.Tensuduke_ForeachLeafs(
                            nodePath + " " + Util_Sky.ToSfenMoveText(nextNode.Key),
                            (KifuNode)nextNode,
                            kifu,
                            kokoro,
                            playerInfo,
                            reportEnvironment,
                            logF_kiki,
                            logTag
                            );

                    });

                    // このノードが、自分の手番かどうか。
                    bool jibun = playerInfo.Playerside == kifu.CountPside(node);
                    if (jibun)
                    {
                        // 自分のノードの場合、次ノードの中で一番点数の高いもの。
                        double maxScore = double.MinValue;
                        node.Foreach_NextNodes((string key, Node<ShootingStarlightable, KyokumenWrapper> nextNode, ref bool toBreak) =>
                        {
                            double score = ((KifuNode)nextNode).KyHyoka.Total();
                            if (maxScore < score)
                            {
                                maxScore = score;
                            }
                        });
                        node.SetBranchKyHyoka(new KyHyokaImpl(maxScore));
                    }
                    else
                    {
                        // 相手のノードの場合、次ノードの中で一番点数の低いもの。
                        double minScore = double.MaxValue;
                        node.Foreach_NextNodes((string key, Node<ShootingStarlightable, KyokumenWrapper> nextNode, ref bool toBreak) =>
                        {
                            double score = ((KifuNode)nextNode).KyHyoka.Total();
                            if (score < minScore)
                            {
                                minScore = score;
                            }
                        });
                        node.SetBranchKyHyoka(new KyHyokaImpl(minScore));
                    }
                }
                catch (Exception ex)
                {
                    //>>>>> エラーが起こりました。
                    string message = ex.GetType().Name + " " + ex.Message + "：途中のノードに点数を付けたときです。：";
                    Debug.Fail(message);

                    // どうにもできないので  ログだけ取って、上に投げます。
                    Logger.Log_Engine.WriteLine_Error(message);
                    throw ;
                }

            }
        }


        /// <summary>
        /// 盤１個分のログ。
        /// </summary>
        private void Log_Board(
            string nodePath,
            KifuNode node,
            KifuTree kifu,
            ReportEnvironment reportEnvironment,
            GraphicalLog_File logF_kiki,
            LarabeLoggerable logTag
            )
        {
            //
            // HTMLﾛｸﾞ
            //
            if (logF_kiki.boards.Count < 30)//出力件数制限
            {
                GraphicalLog_Board logBrd_move1 = new GraphicalLog_Board();

                List_OneAndMulti<Finger, SySet<SyElement>> komaBETUSusumeruMasus;
                Util_MovableMove.LA_Get_KomaBETUSusumeruMasus(
                    out komaBETUSusumeruMasus,
                    new MmGenjo_MovableMasuImpl(
                        true,//本将棋
                        node.Value.ToKyokumenConst,//現在の局面
                        kifu.CountPside(node),
                        false
                        ),
                    new MmLogGenjoImpl(
                        false,//ログなし
                        logBrd_move1,
                        0,//読みの深さ
                        0,//現在の手済み
                        node.Key,
                        logTag
                    )
                );

                logBrd_move1.moveOrNull = ((KifuNode)node).Key;

                logBrd_move1.NounaiYomiDeep = int.MinValue;
                logBrd_move1.Tesumi = int.MinValue;
                logBrd_move1.Score = (int)node.KyHyoka.Total();

                logF_kiki.boards.Add(logBrd_move1);
            }
        }

    }
}
