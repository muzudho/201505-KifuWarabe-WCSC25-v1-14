using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.P050_KifuWarabe.L00049_Kokoro;
using System.Collections.Generic;

namespace Grayscale.P050_KifuWarabe.L012_ScoreSibori
{

    /// <summary>
    /// 棋譜ツリーのスコアの低いノードを捨てていきます。
    /// </summary>
    public class ScoreSiboriEngine
    {

        /// <summary>
        /// 棋譜ツリーの　点数の低いノードを　ばっさばっさ捨てて行きます。
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="atamanosumiCollection"></param>
        public void ScoreSibori(KifuTree kifu, Kokoro atama)
        {
            //
            // ノードが２つもないようなら、スキップします。
            //
            if (kifu.CurNode.Count_NextNodes < 2)
            {
                goto gt_EndMethod;
            }


            List<Node<ShootingStarlightable, KyokumenWrapper>> rankedNodes = this.RankingNode_WithJudge_ForeachNextNodes(kifu.CurNode);

            // 1番高いスコアを調べます。
            double maxScore = int.MinValue;
            foreach (Node<ShootingStarlightable, KyokumenWrapper> node in rankedNodes)
            {
                if(node is KifuNode)
                {
                    double score = ((KifuNode)node).KyHyoka.Total();

                    if (maxScore < score)
                    {
                        maxScore = score;
                    }
                }
            }

            // 1番高いスコアのノードだけ残します。


            Dictionary<string, Node<ShootingStarlightable, KyokumenWrapper>> dic = new Dictionary<string, Node<ShootingStarlightable, KyokumenWrapper>>();

            kifu.CurNode.Foreach_NextNodes((string key, Node<ShootingStarlightable, KyokumenWrapper> node, ref bool toBreak) =>
            {
                double score;
                if (node is KifuNode)
                {
                    score = ((KifuNode)node).KyHyoka.Total();
                }
                else
                {
                    score = 0.0d;
                }

                if (maxScore <= score)
                {
                    dic.Add(key, node);
                }
            });


            if (kifu.CurNode.Count_NextNodes == dic.Count)
            {
                // 全ての手が同じスコアなら、妄想は失敗かもしれません。

                // FIXME: 他の手が原因で、全ての手が同じスコアになってしまった可能性がある☆
                foreach (Tenonagare atamanosumi in atama.TenonagareItems)
                {
                    atamanosumi.ResultKioku.Sippai++;
                }
            }


            // 枝を更新します。
            kifu.CurNode.Set_NextNodes( dic);

        gt_EndMethod:
            ;
        }



        /// <summary>
        /// 局面が、妄想に近いかどうかで点数付けします。
        /// </summary>
        /// <param name="nextNodes"></param>
        /// <returns></returns>
        public List<Node<ShootingStarlightable, KyokumenWrapper>> RankingNode_WithJudge_ForeachNextNodes( Node<ShootingStarlightable, KyokumenWrapper> hubNode)
        {
            // ランク付けしたあと、リスト構造に移し変えます。
            List<Node<ShootingStarlightable, KyokumenWrapper>> list = new List<Node<ShootingStarlightable, KyokumenWrapper>>();

            hubNode.Foreach_NextNodes((string key, Node<ShootingStarlightable, KyokumenWrapper> node, ref bool toBreak) =>
            {
                list.Add(node);
            });

            // ランク付けするために、リスト構造に変換します。

            ScoreSiboriEngine.Sort(list);

            return list;
        }


        public static void Sort(List<Node<ShootingStarlightable, KyokumenWrapper>> items)
        {
            items.Sort((a, b) =>
            {
                double bScore;
                double aScore;

                if (b is KifuNode)
                {
                    bScore = ((KifuNode)b).KyHyoka.Total();
                }
                else
                {
                    bScore = 0.0d;
                }


                if (a is KifuNode)
                {
                    aScore = ((KifuNode)a).KyHyoka.Total();
                }
                else
                {
                    aScore = 0.0d;
                }

                return (int)(bScore - aScore);//点数が大きいほうが前に行きます。
            });

        }

    }

}
