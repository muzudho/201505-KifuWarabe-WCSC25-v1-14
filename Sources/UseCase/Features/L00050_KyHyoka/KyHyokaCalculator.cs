using System.Collections.Generic;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.P050_KifuWarabe.L00049_Kokoro;

namespace Grayscale.P050_KifuWarabe.L00050_KyHyoka
{

    /// <summary>
    /// 局面評価のエンジン。
    /// </summary>
    public interface KyHyokaCalculator
    {

        /// <summary>
        /// 局面スコアラーの一覧。
        /// </summary>
        Dictionary<TenonagareName, KyHandan> KyHyokas { get; }


        /// <summary>
        /// （１）ノードは、局面であるとともに、点数を覚えておくこともできます。
        /// （２）考え方の条件を指定します。
        /// （３）このメソッドは、「指定の駒が、指定のマスより、どれぐらい離れているか」で点数付けします。
        /// （４）ノードに、点数を設定します。
        /// </summary>
        KyHyokaItem LetHandan(
            Tenonagare atamanosumi,
            KifuNode node,
            PlayerInfo playerInfo
            );

    }
}
