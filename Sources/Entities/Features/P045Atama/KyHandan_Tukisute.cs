namespace Grayscale.Kifuwarazusa.Entities.Features
{


    /// <summary>
    /// 「突き捨て」。
    /// </summary>
    public class KyHandan_Tukisute : KyHandanAbstract
    {

        public KyHandan_Tukisute() : base(TenonagareName.Tukisute)
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
            double score = 0.0d;
            SkyConst src_Sky = args.Node.Value.ToKyokumenConst;

            //
            // 指定された駒。
            //
            RO_Star_Koma koma1 = args.TenonagareGenjo.Koma1;

            if (Okiba.ShogiBan != Util_Masu.Masu_ToOkiba(koma1.Masu))
            {
                // 指定された駒が将棋盤の上になければ、無視。
                goto gt_EndMethod;
            }

            //
            // 初期位置にいると、点数が低いものとします。
            //
            if (koma1.Masu == args.TenonagareGenjo.Masu)
            {
                score = 0.0d;
            }
            else
            {
                score = 100.0d;
            }

        ////
        //// 先手から見た盤に回転。
        ////
        //SyElement komaMasu = Util_Masu.BothSenteView(koma1.Masu, koma1.Pside);
        //int dan;
        //Util_MasuNum.MasuToDan(komaMasu, out dan);

        gt_EndMethod:
            string meisai = "";
#if DEBUG
            // 明細
            {
                StringBuilder sb = new StringBuilder();
                // 初期位置にいると、点数が低いものとします。
                if (koma1.Masu == args.TenonagareGenjo.Masu)
                {
                    sb.Append("(初期位置 0.0d)");
                }
                else
                {
                    sb.Append("(初期位置から離れている 100.0d)");
                }
                meisai = sb.ToString();
            }
#endif
            kyokumenScore = new KyHyoka100limitItemImpl(args.TenonagareGenjo.ScoreKeisu, score, meisai);
        }


    }
}
