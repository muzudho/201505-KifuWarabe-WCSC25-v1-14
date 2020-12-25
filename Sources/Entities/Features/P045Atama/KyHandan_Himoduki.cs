using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwarazusa.Entities.Features
{


    /// <summary>
    /// 評価計算
    /// 
    /// 紐が付いていたら加点。
    /// </summary>
    public class KyHandan_Himoduki : KyHandanAbstract
    {

        public KyHandan_Himoduki() : base(TenonagareName.Himoduki)
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
            double score;
            SkyConst src_Sky = args.Node.Value.ToKyokumenConst;

            //
            // 「紐が付いていない駒の少なさ」による加点。
            //
            int mikataHimoduki = 0; // 味方の駒に、味方の駒の利きが１つ被っていれば＋１。
            int mikataNeraware = 0; // 味方の駒に、相手の駒の利きが１つ被っていれば＋１。
            int aiteHimoduki = 0; // 相手の駒に、相手の駒の利きが１つ被っていれば＋１。
            int aiteNeraware = 0; // 相手の駒に、味方の駒の利きが１つ被っていれば＋１。


            List<string> mikataYaburareMasu = new List<string>();
            int aiteWin = 0; // 利きの枚数で相手が勝っていた件数。

            List<string> aiteYaburareMasu = new List<string>();
            int mikataWin = 0; // 利きの枚数で味方が勝っていた件数。



            //
            // 駒の利きの計算。
            //
            KomanoKiki komanoKiki = KomanoKikiImpl.MasuBETUKikisu(src_Sky, args.Node.Tebanside);



            //
            // 味方の駒があるマスの得点は、マイナスなら集計。（減点法）
            // 敵の駒があるマスの得点は、プラスなら集計。（減点法）
            //
            // 駒は４０枚あり、貫通して加点するのもある。
            //
            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)// 全駒
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(figKoma).Now);

                if (Okiba.ShogiBan != Util_Masu.Masu_ToOkiba(koma.Masu))
                {
                    goto gt_Next2;
                }

                if (args.PlayerInfo.Playerside == koma.Pside)
                {
                    // 味方の駒

                    // その升に利いている味方の利きの数。
                    int mikataKiki = komanoKiki.Kikisu_AtMasu_Mikata[Util_Masu.AsMasuNumber(koma.Masu)];

                    // その升に利いている相手の利きの数。
                    int aiteKiki = komanoKiki.Kikisu_AtMasu_Teki[Util_Masu.AsMasuNumber(koma.Masu)];

                    mikataHimoduki += mikataKiki;
                    mikataNeraware += aiteKiki;

                    // 破られる場合
                    if (mikataKiki < aiteKiki)
                    {
                        aiteWin++;
                        mikataYaburareMasu.Add(Converter04.Masu_ToKanji(koma.Masu));
                    }
                }
                else
                {
                    // 相手の駒

                    // その升に利いている相手の利きの数。
                    int aiteKiki = komanoKiki.Kikisu_AtMasu_Mikata[Util_Masu.AsMasuNumber(koma.Masu)];

                    // その升に利いている味方の利きの数。
                    int mikataKiki = komanoKiki.Kikisu_AtMasu_Teki[Util_Masu.AsMasuNumber(koma.Masu)];

                    aiteHimoduki = aiteKiki;
                    aiteNeraware = mikataKiki;

                    // 破られる場合
                    if (aiteKiki < mikataKiki)
                    {
                        mikataWin++;
                        aiteYaburareMasu.Add(Converter04.Masu_ToKanji(koma.Masu));
                    }
                }

            gt_Next2:
                ;
            }


            // 集計。
            score = (double)(mikataWin - aiteWin) * 1.0 + 50.0d;

            string meisai = "";
#if DEBUG
            // 明細
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" " + score);
                sb.Append("点    ");
                sb.Append(" 衝突箇所勝ち数=" + (mikataWin - aiteWin));
                sb.Append(" 味方が破られる" + aiteWin+"箇所");
                foreach (string s in mikataYaburareMasu)
                {
                    sb.Append("[");
                    sb.Append(s);
                    sb.Append("]");
                }

                sb.Append(" 相手が破られる" + mikataWin+"箇所");
                foreach (string s in aiteYaburareMasu)
                {
                    sb.Append("[");
                    sb.Append(s);
                    sb.Append("]");
                }
                meisai = sb.ToString();
            }
#endif

            kyokumenScore = new KyHyoka100limitItemImpl(args.TenonagareGenjo.ScoreKeisu, score, meisai);
        }


    }
}
