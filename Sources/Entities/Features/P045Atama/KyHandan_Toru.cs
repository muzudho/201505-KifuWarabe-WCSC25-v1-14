namespace Grayscale.Kifuwarazusa.Entities.Features
{


    /// <summary>
    /// 「取る」。
    /// </summary>
    public class KyHandan_Toru : KyHandanAbstract
    {

        public KyHandan_Toru() : base(TenonagareName.Toru)
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
            // どの駒＜その１＞（のある場所）が
            // どの駒＜その２＞（のある場所）より
            // どれぐらい離れているか（距離）
            //
            int kyori = Util_KomanoKyori.GetKyori(args.TenonagareGenjo.Koma2.Masu, args.TenonagareGenjo.Koma1.Masu);
            score = 100.0d - 3.0d * kyori;//てきとーな数字です。


            //// ログ
            //{
            //    string move = Converter04.Move_ToString_ForLog(args.Input_node.Key);// どの指し手の局面で
            //    ((ShogiEngine)args.Owner).Log_Engine.WriteLine_AddMemo("駒[" + Haiyaku184Array.Name[(int)koma.Haiyaku].Trim() + "(No." + (int)((Atamanosumi)args.Input_atamanosumi).Finger + ")]が、マス[" + Converter04.Masu_ToKanji(((Atamanosumi)args.Input_atamanosumi).Masu) + "]を目指したいんだぜ～☆。指し手が[" + move + "]なら、距離は[" + kyori + "]、スコアは[" + score + "]だぜ☆");
            //}

            string meisai = "";
#if DEBUG
            // 明細
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("100.0d - 3.0d * (距離 " + kyori + ")");
                meisai = sb.ToString();
            }
#endif
            kyokumenScore = new KyHyoka100limitItemImpl(args.TenonagareGenjo.ScoreKeisu, score, meisai);
        }


    }
}
