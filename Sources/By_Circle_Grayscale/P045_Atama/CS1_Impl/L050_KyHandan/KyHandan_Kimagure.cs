

using Grayscale.P045_Atama.L00025_KyHandan;
using Grayscale.P045_Atama.L012_Lua;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P040_Kokoro.L00050_Kokoro;
using System.Text;
using Grayscale.P025_KifuLarabe.L007_Random;

namespace Grayscale.P045_Atama.L050_KyHandan
{


    public class KyHandan_Kimagure : KyHandanAbstract
    {

        public KyHandan_Kimagure():base(TenonagareName.Kimagure)
        {
        }

        /// <summary>
        /// 0.0d ～100.0d の範囲で、評価値を返します。数字が大きい方がグッドです。
        /// </summary>
        /// <param name="input_node"></param>
        /// <param name="playerInfo"></param>
        /// <returns></returns>
        public override void Keisan(out KyHyokaItem kyokumenScore, KyHandanArgs keisanArgs)
        {
            Util_Lua_KifuWarabe.Perform("score_kimagure");


            string meisai = "";
            double score;
#if DEBUG
            // 明細
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("きまぐれ ");
                sb.Append(Util_Lua_KifuWarabe.Score);
                meisai = sb.ToString();
            score = Util_Lua_KifuWarabe.Score;
            }
#else
            score = LarabeRandom.Random.NextDouble() * 100.0d;

#endif


            kyokumenScore = new KyHyoka100limitItemImpl(keisanArgs.TenonagareGenjo.ScoreKeisu, score, meisai);
        }

    }


}
