using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.P040_Kokoro.L00050_Kokoro;
using Grayscale.P045_Atama.L000125_Sokutei;
using Grayscale.P045_Atama.L00025_KyHandan;
using Grayscale.P045_Atama.L025_Sokutei;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P045_Atama.L050_KyHandan
{


    /// <summary>
    /// 「角頭」の紐付き。
    /// 
    /// 先手から見て。
    /// （１）８七の地点に、「味方の駒」がある。               （　　０．０１点）
    /// （２）８七の地点の駒に、味方の駒が１つ以上利いている。 （　　１．０点）
    /// （３）項１と項２を同時に満たしている。                 （１００．０点）
    /// （４）項１～３のどれでもない。                         （　　０．０点）
    /// </summary>
    public class KyHandan_KakuTouNoHimoduki : KyHandanAbstract
    {


        public KyHandan_KakuTouNoHimoduki() : base(TenonagareName.KakuTouNoHimoduki)
        {
        }


        /// <summary>
        /// 0.0d ～100.0d の範囲で、評価値を返します。数字が大きい方がグッドです。
        /// </summary>
        /// <param name="input_node"></param>
        /// <param name="playerInfo"></param>
        /// <returns></returns>
        public override void Keisan(out KyHyokaItem kyHyoka, KyHandanArgs args)
        {

            double score = 0.0d;
            SkyConst src_Sky = args.Node.Value.ToKyokumenConst;
            bool b1 = false;
            bool b2 = false;


            SyElement masuKtou;//角頭
            if (args.PlayerInfo.Playerside == Playerside.P1)
            {
                masuKtou = Masu_Honshogi.ban87_８七;
            }
            else
            {
                masuKtou = Masu_Honshogi.ban23_２三;
            }


            //
            // 角頭に味方の駒があるか？
            //
            Finger masuKtou_KomaFig;
            RO_Star_Koma komaKtou = null;
            {
                masuKtou_KomaFig = Util_Sky.Fingers_AtMasuNow(src_Sky, masuKtou).ToFirst();

                if (Fingers.Error_1 == masuKtou_KomaFig)
                {
                    // 角頭に駒がない。
                    goto gt_Next1;
                }

                komaKtou = Util_Koma.FromFinger(src_Sky, masuKtou_KomaFig);
                if (args.PlayerInfo.Playerside != komaKtou.Pside)
                {
                    // 味方の駒ではない。
                    goto gt_Next1;
                }
                score = 0.01d;
                b1 = true;
            }
        gt_Next1:


            //
            // 角頭の駒に、味方の駒が１つ以上利いているか？
            //
            int kikisu;
            {
                KomanoKiki komaNoKiki = KomanoKikiImpl.MasuBETUKikisu(src_Sky, args.Node.Tebanside);

                kikisu = komaNoKiki.Kikisu_AtMasu_Mikata[Util_Masu.AsMasuNumber(masuKtou)];

                if (kikisu < 1)
                {
                    // 1つも利いていない。
                    goto gt_Next2;
                }
                score = 1.0d;
                b2 = true;
            }
        gt_Next2:

            //
            // 項１と項２を同時に満たしている。                 （計　１００点）
            //
            if (b1 && b2)
            {
                score = 100.0d;
            }


            string meisai = "";
#if DEBUG
            // 明細
            {
                StringBuilder sb = new StringBuilder();
                if (args.PlayerInfo.Playerside == Playerside.P1)
                {
                    sb.Append("(角頭 ８七)");
                }
                else
                {
                    sb.Append("(角頭 ２三)");
                }

                if (Fingers.Error_1 == masuKtou_KomaFig)
                {
                    // 角頭に駒がない。
                    sb.Append("(角頭 味方駒無)");
                }
                else if (args.PlayerInfo.Playerside != komaKtou.Pside)
                {
                    // 味方の駒ではない。
                    sb.Append("(角頭 非味方駒)");
                }
                else if (b1)
                {
                    sb.Append("(角頭 味方駒有 0.01)");
                }

                if (kikisu < 1)
                {
                    // 1つも利いていない。
                    sb.Append("(角頭に味方駒利き無 " + kikisu + ")");
                }
                else if (b2)
                {
                    sb.Append("(角頭に味方駒利き有 1.0)");
                }

                // 項１と項２を同時に満たしている。                 （計　１００点）
                if (b1 && b2)
                {
                    sb.Append("(項1、項２を同時に満たす 100.0)");
                }

                meisai = sb.ToString();
            }
#endif

            kyHyoka = new KyHyoka100limitItemImpl(args.TenonagareGenjo.ScoreKeisu, score, meisai);
        }


    }
}
