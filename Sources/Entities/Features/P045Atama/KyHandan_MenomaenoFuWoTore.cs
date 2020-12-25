using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwarazusa.Entities.Features
{


    /// <summary>
    /// 「目の前の歩を取れ」。
    /// 
    /// 
    /// （１）味方の駒の前に歩がある。  （１箇所につき  －２０点。最大－１００点）
    /// </summary>
    public class KyHandan_MenomaenoFuWoTore : KyHandanAbstract
    {

        public KyHandan_MenomaenoFuWoTore() : base(TenonagareName.MenomaenoFuWoTore)
        {
        }


        /// <summary>
        /// 0.0d ～100.0d の範囲で、評価値を返します。数字が大きい方がグッドです。
        /// </summary>
        /// <param name="input_node"></param>
        /// <param name="playerInfo"></param>
        /// <returns></returns>
        public override void Keisan(out KyHyokaItem kyokumenScore, KyHandanArgs args)
        {
            double score = 100.0d;
            StringBuilder sbMeisai = new StringBuilder();
            SkyConst src_Sky = args.Node.Value.ToKyokumenConst;

            //
            // 各、味方の駒
            //
            Fingers jiFigs = Util_Sky.Fingers_ByOkibaPsideNow(src_Sky, Okiba.ShogiBan, args.PlayerInfo.Playerside);
            foreach (Finger figKoma in jiFigs.Items)
            {
                // 自駒
                RO_Star_Koma jiKoma = Util_Koma.FromFinger(src_Sky, figKoma);

                SyElement aiteMasu;
                if (args.PlayerInfo.Playerside == Playerside.P1)
                {
                    aiteMasu = Util_Masu.Offset(Okiba.ShogiBan, jiKoma.Masu, 0, -1);
                }
                else
                {
                    aiteMasu = Util_Masu.Offset(Okiba.ShogiBan, jiKoma.Masu, 0, +1);
                }

                Finger aiteKomaFig = Util_Sky.Fingers_AtMasuNow(src_Sky, aiteMasu).ToFirst();
                if (Fingers.Error_1 == aiteKomaFig)
                {
                    goto gt_Next1;
                }

                // 相手駒
                RO_Star_Koma aiteKoma = Util_Koma.FromFinger(src_Sky, aiteKomaFig);
                if (aiteKoma.Syurui != Ks14.H01_Fu)
                {
                    // 歩じゃなければ無視。「と金」も無視。
                    goto gt_Next1;
                }

                score -= 20.0d;

#if DEBUG
                // 明細
                {
                    sbMeisai.Append("-(20.0d)");
                }
#endif

                if (score <= 0.0d)
                {
                    break;
                }

            gt_Next1:
                ;
            }



            kyokumenScore = new KyHyoka100limitItemImpl(args.TenonagareGenjo.ScoreKeisu, score, sbMeisai.ToString());
        }


    }
}
