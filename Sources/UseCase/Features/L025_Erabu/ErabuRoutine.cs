using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
//スプライト番号
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P025_KifuLarabe.L007_Random;
using Grayscale.P025_KifuLarabe.L012_Common;
using System.Collections.Generic;

namespace Grayscale.P050_KifuWarabe.L025_Erabu
{
    /// <summary>
    /// 選ぶエンジン
    /// </summary>
    public class ErabuEngine
    {
        /// <summary>
        /// たった１つの指し手（ベストムーブ）を選びます。
        /// </summary>
        /// <param name="kifu">ツリー構造になっている棋譜</param>
        /// <param name="logTag">ログ</param>
        /// <returns></returns>
        public ShootingStarlightable ChoiceBestMove(
            KifuTree kifu,
            bool enableLog, bool isHonshogi, ILogTag logTag)
        {
            ShootingStarlightable bestMove = null;

            // これから調べる局面（next=現局面）
            Playerside pside_yomiNext = kifu.CountPside(kifu.CurNode);

            {
                // 次のノードをリストにします。
                List<Node<ShootingStarlightable, KyokumenWrapper>> nextNodes = Converter04.NextNodes_ToList(kifu.CurNode, logTag);

                // 次のノードをシャッフルします。
                List<Node<ShootingStarlightable, KyokumenWrapper>> nextNodes_shuffled = Converter04.NextNodes_ToList(kifu.CurNode, logTag);
                LarabeShuffle<Node<ShootingStarlightable, KyokumenWrapper>>.Shuffle_FisherYates(ref nextNodes_shuffled);

                // シャッフルした最初の指し手を選びます。
                if (0<nextNodes_shuffled.Count)
                {
                    bestMove = nextNodes_shuffled[0].Key;
                }

                // ③ランダムに１手選ぶ
                //bestMove = Program.SikouEngine.ErabuRoutine.ChoiceMove_fromNextNodes_Random(kifu, node_yomiNext, logTag);
            }

            if (null == bestMove)
            {
                // 投了
                bestMove = Util_Sky.NullObjectMove;
            }

            // TODO:    できれば、合法手のリストから　さらに相手番の合法手のリストを伸ばして、
            //          １手先、２手先……の局面を　ツリー構造（GameViewModel.Kifu）に蓄えたあと、
            //          末端の局面に評価値を付けて、ミニマックス原理を使って最善手を絞り込みたい☆
            return bestMove;
        }
    }
}
