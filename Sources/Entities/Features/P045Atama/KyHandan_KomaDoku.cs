//スプライト番号
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwarazusa.Entities.Features
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
                    case PieceType.P: komaScore = 1.0d; break;
                    case PieceType.L: komaScore = 3.0d; break;
                    case PieceType.N: komaScore = 3.5d; break;
                    case PieceType.S: komaScore = 9.0d; break;
                    case PieceType.G: komaScore = 9.5d; break;
                    case PieceType.K: komaScore = 100.0d; break;
                    case PieceType.R: komaScore = 25.0d; break;
                    case PieceType.B: komaScore = 20.0d; break;
                    case PieceType.PR: komaScore = 25.0d; break;
                    case PieceType.PB: komaScore = 20.0d; break;
                    case PieceType.PP: komaScore = 9.5d; break;
                    case PieceType.PL: komaScore = 10.0d; break;
                    case PieceType.PN: komaScore = 10.5d; break;
                    case PieceType.PS: komaScore = 11.0d; break;
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
