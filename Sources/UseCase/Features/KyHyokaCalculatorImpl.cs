using System.Collections.Generic;
using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.Kifuwarazusa.UseCases.Features
{

    /// <summary>
    /// 局面の評価エンジン。
    /// </summary>
    public class KyHyokaCalculatorImpl : KyHyokaCalculator
    {

        /// <summary>
        /// 局面のスコアラーのリスト。
        /// </summary>
        public Dictionary<TenonagareName, KyHandan> KyHyokas { get { return this.kyHyokas; } }
        private Dictionary<TenonagareName, KyHandan> kyHyokas;


        public KyHyokaCalculatorImpl()
        {


            // 評価一覧
            {
                Dictionary<TenonagareName, KyHandan> dic = new Dictionary<TenonagareName, KyHandan>();
                KyHyokaCalculatorImpl.AddDic(dic, new KyHandan_Ido());// 移動
                KyHyokaCalculatorImpl.AddDic(dic, new KyHandan_Toru());// 取る
                KyHyokaCalculatorImpl.AddDic(dic, new KyHandan_Tukisute());// 突き捨て
                KyHyokaCalculatorImpl.AddDic(dic, new KyHandan_KomaDoku());// 駒を持っていることによる加点。平手なら、両者同じ駒を持っているので、相殺されて 0 点。
                KyHyokaCalculatorImpl.AddDic(dic, new KyHandan_Himoduki());// 駒の紐付き。
                KyHyokaCalculatorImpl.AddDic(dic, new KyHandan_KakuTouNoHimoduki());// 角頭の紐付き。
                KyHyokaCalculatorImpl.AddDic(dic, new KyHandan_MenomaenoFuWoTore());// 目の前の歩を取れ。
                KyHyokaCalculatorImpl.AddDic(dic, new KyHandan_Kimagure());//気まぐれ
                KyHyokaCalculatorImpl.AddDic(dic, new KyHandan_Toosi());// 飛車道が通っているなどによる加点。
                KyHyokaCalculatorImpl.AddDic(dic, new KyHandan_GyokuNoMamori());// 玉の守りによる加点。
                this.kyHyokas = dic;
            }

        }


        private static void AddDic(Dictionary<TenonagareName, KyHandan> dic, KyHandan score)
        {
            dic.Add(score.Name, score);
        }


        /// <summary>
        /// レッツ☆判断！
        /// 
        /// （１）ノードは、局面であるとともに、点数を覚えておくこともできます。
        /// （２）考え方の条件を指定します。
        /// （３）このメソッドは、「指定の駒が、指定のマスより、どれぐらい離れているか」で点数付けします。
        /// （４）ノードに、点数を設定します。
        /// </summary>
        public KyHyokaItem LetHandan(
            Tenonagare tenonagare,
            KifuNode node,
            PlayerInfo playerInfo
            )
        {
            KyHyokaItem scoreExp;

            if (this.KyHyokas.ContainsKey(tenonagare.Name))
            {
                this.KyHyokas[tenonagare.Name].Keisan(out scoreExp, new KyHandanArgsImpl(tenonagare, node, playerInfo));
            }
            else
            {
                scoreExp = new KyHyoka100limitItemImpl(1.0d, 0.0d, "ヌル");
            }

            return scoreExp;
        }

    }


}
