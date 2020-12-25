

using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.P040_Kokoro.L00050_Kokoro;
using Grayscale.P045_Atama.L00025_KyHandan;
//スプライト番号
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P045_Atama.L050_KyHandan
{


    public class KyHandan_KomaDoku : KyHandanAbstract
    {

        public KyHandan_KomaDoku() : base(TenonagareName.KomaDoku)
        {
        }

        /// <summary>
        /// 0.0d ～100.0d の範囲で、評価値を返します。数字が大きい方がグッドです。
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override void Keisan(out KyHyokaItem kyokumenScore, KyHandanArgs args)
        {
            double score = 0.0d;
            SkyConst src_Sky = args.Node.Value.ToKyokumenConst;

            // まず、プレイヤー１の視点で得点計算します。
            double score_player1 = 0.0d;

            src_Sky.Foreach_Starlights((Finger finger, Starlight light, ref bool toBreak) =>
            {
                RO_MotionlessStarlight ms = (RO_MotionlessStarlight)light;

                RO_Star_Koma koma = Util_Koma.AsKoma(ms.Now);

                // 単純に駒の種類による点数。
                double komaScore = 0.0d;
                switch (koma.Syurui)
                {
                    case Ks14.H01_Fu: komaScore = 1.0d; break;
                    case Ks14.H02_Kyo: komaScore = 3.0d; break;
                    case Ks14.H03_Kei: komaScore = 3.5d; break;
                    case Ks14.H04_Gin: komaScore = 9.0d; break;
                    case Ks14.H05_Kin: komaScore = 9.5d; break;
                    case Ks14.H06_Oh: komaScore = 100.0d; break;
                    case Ks14.H07_Hisya: komaScore = 25.0d; break;
                    case Ks14.H08_Kaku: komaScore = 20.0d; break;
                    case Ks14.H09_Ryu: komaScore = 25.0d; break;
                    case Ks14.H10_Uma: komaScore = 20.0d; break;
                    case Ks14.H11_Tokin: komaScore = 9.5d; break;
                    case Ks14.H12_NariKyo: komaScore = 10.0d; break;
                    case Ks14.H13_NariKei: komaScore = 10.5d; break;
                    case Ks14.H14_NariGin: komaScore = 11.0d; break;
                    default: komaScore = 0; break;
                }

                if (koma.Pside == Playerside.P1)
                {
                    // プレイヤー１の駒はそのまま
                }
                else
                {
                    // プレイヤー１の駒以外は、ひっくり返します。
                    komaScore *= -1;
                }

                score_player1 += komaScore;
            });


            if (args.PlayerInfo.Playerside == Playerside.P1)
            {
                // プレイヤー１はそのまま
                score = score_player1;
            }
            else
            {
                // プレイヤー１以外は、ひっくり返します。
                score = 100.0d - score_player1;
            }


            string meisai = "";
#if DEBUG
            // 明細
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("駒得");
                meisai = sb.ToString();
            }
#endif

            kyokumenScore = new KyHyoka100limitItemImpl(args.TenonagareGenjo.ScoreKeisu, score, meisai);
        }

    }


}
