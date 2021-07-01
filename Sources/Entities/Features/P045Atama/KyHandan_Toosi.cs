//スプライト番号
using System.Collections.Generic;
using System.Linq;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwarazusa.Entities.Features
{

    /// <summary>
    /// 飛車道が通っているとか、香車道が通っているとか
    /// </summary>
    public class KyHandan_Toosi : KyHandanAbstract
    {

        public double Keisu_Fu;
        public double Keisu_Kyo;
        public double Keisu_Kei;
        public double Keisu_Gin;
        public double Keisu_Kin;
        public double Keisu_Oh;
        public double Keisu_Hisya;
        public double Keisu_Kaku;
        public double Keisu_Ryu;
        public double Keisu_Uma;
        public double Keisu_Tokin;
        public double Keisu_NariKyo;
        public double Keisu_NariKei;
        public double Keisu_NariGin;


        public KyHandan_Toosi() : base(TenonagareName.Toosi)
        {
            // 1マスの得点。
            this.Keisu_Fu = 0.0d;
            this.Keisu_Kyo = 0.0d;//香車は別に、たくさんの升に利かそうとがんばらなくていい。
            this.Keisu_Kei = 0.0d;
            this.Keisu_Gin = 0.0d;
            this.Keisu_Kin = 0.0d;
            this.Keisu_Oh = 0.0d;
            this.Keisu_Hisya = 5.0d;//飛車・龍は２枚ある。
            this.Keisu_Kaku = 4.0d;//角・馬は２枚ある。
            this.Keisu_Ryu = 5.0d;
            this.Keisu_Uma = 4.0d;
            this.Keisu_Tokin = 0.0d;
            this.Keisu_NariKyo = 0.0d;
            this.Keisu_NariKei = 0.0d;
            this.Keisu_NariGin = 0.0d;
        }

        /// <summary>
        /// 0.0d ～100.0d の範囲で、評価値を返します。数字が大きい方がグッドです。
        /// </summary>
        /// <param name="input_node"></param>
        /// <param name="playerInfo"></param>
        /// <returns></returns>
        public override void Keisan(out KyHyokaItem kyokumenScore, KyHandanArgs args)
        {
            double score = 50.0d;
            SkyConst src_Sky = args.Node.Value.ToKyokumenConst;



            //
            // 「紐が付いていない駒の少なさ」による加点。
            //
            // 自分の駒について、味方の駒の利きが被っていれば＋１（貫通）
            //                     敵の駒の利きが被っていれば－１（貫通）
            //
            //   敵の駒について、味方の駒の利きが被っていれば＋１（貫通）
            //                     敵の駒の利きが被っていれば－１（貫通）
            //




            //
            // 駒のない升は無視し、
            // 駒のあるマスに、その駒の味方のコマが効いていれば　＋１
            // 駒のあるマスに、その駒の相手のコマが効いていれば　－１
            //
            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)// 全駒
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(figKoma).Now);

                if (Okiba.ShogiBan != Util_Masu.Masu_ToOkiba(koma.Masu))
                {
                    goto gt_Next1;
                }
                // 将棋盤上のみ。

                // 駒の利き
                SySet<SyElement> kikiZukei = Util_Sky.KomaKidou_Potential(figKoma, src_Sky);

                IEnumerable<SyElement> kikiMasuList = kikiZukei.Elements;
                // 通し
                double toosi = kikiMasuList.Count();

                // 係数
                switch (koma.Syurui)
                {
                    case PieceType.P: toosi *= this.Keisu_Fu; break;
                    case PieceType.L: toosi *= this.Keisu_Kyo; break;
                    case PieceType.N: toosi *= this.Keisu_Kei; break;
                    case PieceType.S: toosi *= this.Keisu_Gin; break;
                    case PieceType.G: toosi *= this.Keisu_Kin; break;
                    case PieceType.K: toosi *= this.Keisu_Oh; break;
                    case PieceType.R: toosi *= this.Keisu_Hisya; break;
                    case PieceType.B: toosi *= this.Keisu_Kaku; break;
                    case PieceType.PR: toosi *= this.Keisu_Ryu; break;
                    case PieceType.PB: toosi *= this.Keisu_Uma; break;
                    case PieceType.PP: toosi *= this.Keisu_Tokin; break;
                    case PieceType.PL: toosi *= this.Keisu_NariKyo; break;
                    case PieceType.PN: toosi *= this.Keisu_NariKei; break;
                    case PieceType.PS: toosi *= this.Keisu_NariGin; break;
                    default: break;
                }

                if (args.PlayerInfo.Playerside == koma.Pside)
                {
                    // 味方の駒
                    score += toosi;
                }
                else
                {
                    // 敵の駒
                    score -= toosi;
                }

            gt_Next1:
                ;
            }


            string meisai = "";
#if DEBUG
            // 明細
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("略");
                meisai = sb.ToString();
            }
#endif
            kyokumenScore = new KyHyoka100limitItemImpl(args.TenonagareGenjo.ScoreKeisu, score, meisai);
        }


    }

}
