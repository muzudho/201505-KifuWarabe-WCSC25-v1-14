using Grayscale.P006_Syugoron;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P045_Atama.L00025_KyHandan;
//スプライト番号
using System.Collections.Generic;
using System.Linq;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P040_Kokoro.L00050_Kokoro;
using System.Text;

namespace Grayscale.P045_Atama.L050_KyHandan
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


        public KyHandan_Toosi():base(TenonagareName.Toosi)
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
                    case Ks14.H01_Fu: toosi *= this.Keisu_Fu; break;
                    case Ks14.H02_Kyo: toosi *= this.Keisu_Kyo; break;
                    case Ks14.H03_Kei: toosi *= this.Keisu_Kei; break;
                    case Ks14.H04_Gin: toosi *= this.Keisu_Gin; break;
                    case Ks14.H05_Kin: toosi *= this.Keisu_Kin; break;
                    case Ks14.H06_Oh: toosi *= this.Keisu_Oh; break;
                    case Ks14.H07_Hisya: toosi *= this.Keisu_Hisya; break;
                    case Ks14.H08_Kaku: toosi *= this.Keisu_Kaku; break;
                    case Ks14.H09_Ryu: toosi *= this.Keisu_Ryu; break;
                    case Ks14.H10_Uma: toosi *= this.Keisu_Uma; break;
                    case Ks14.H11_Tokin: toosi *= this.Keisu_Tokin; break;
                    case Ks14.H12_NariKyo: toosi *= this.Keisu_NariKyo; break;
                    case Ks14.H13_NariKei: toosi *= this.Keisu_NariKei; break;
                    case Ks14.H14_NariGin: toosi *= this.Keisu_NariGin; break;
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
