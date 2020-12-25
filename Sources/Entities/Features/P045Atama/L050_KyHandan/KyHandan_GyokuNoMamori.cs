using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P045_Atama.L00025_KyHandan;
using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P040_Kokoro.L00050_Kokoro;
using System.Text;

namespace Grayscale.P045_Atama.L050_KyHandan
{


    /// <summary>
    /// 「玉の守り」
    /// 
    /// 自玉の近くに、自分の金、銀があれば加点。それ以外の駒は無視。
    /// 
    /// ただし、自分の回りに３枚目の金、銀（どれかは適当）があった時点で計算を中断します。
    /// （４枚目以降の金銀は、加点しません）
    /// 
    /// </summary>
    public class KyHandan_GyokuNoMamori : KyHandanAbstract
    {
        public KyHandan_GyokuNoMamori():base(TenonagareName.GyokuNoMamori)
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
            // 自玉
            //
            RO_Star_Koma jiGyoku = null;
            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)// 全駒
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(figKoma).Now);

                if (koma.Pside == args.PlayerInfo.Playerside && koma.Syurui == Ks14.H06_Oh)
                {
                    jiGyoku = koma;
                    break;
                }
            }

            if (null == jiGyoku)
            {
                goto gt_EndMethod;
            }

            int jiGyoku_Suji;
            Util_MasuNum.MasuToSuji(jiGyoku.Masu, out jiGyoku_Suji);
            int jiGyoku_Dan;
            Util_MasuNum.MasuToDan(jiGyoku.Masu, out jiGyoku_Dan);

            int[] sujis = new int[25];
            int[] dans = new int[25];
            {
                //
                // 25近傍。12の位置に、視点の中心となる駒があるとします。
                //
                //    -2  -1   0  +1  +2
                //  ┌─┬─┬─┬─┬─┐
                //-2│ 0│ 1│ 2│ 3│ 4│
                //  ├─┼─┼─┼─┼─┤
                //-1│ 5│ 6│ 7│ 8│ 9│
                //  ├─┼─┼─┼─┼─┤
                // 0│10│11│12│13│14│
                //  ├─┼─┼─┼─┼─┤
                //+1│15│16│17│18│19│
                //  ├─┼─┼─┼─┼─┤
                //+2│20│21│22│23│24│
                //  └─┴─┴─┴─┴─┘
                //
                int index = 0;
                for (int dDan = -2; dDan < 3; dDan++)
                {
                    for (int dSuji = -2; dSuji < 3; dSuji++)
                    {
                        sujis[index] = jiGyoku_Suji + dSuji;
                        dans[index] = jiGyoku_Dan + dDan;
                        index++;
                    }
                }
            }

            // 25近傍に、得点の重みを付けます。
            double aa = 0.0d;
            double bb = 100.0d;
            double cc = 100.0d/3.0d;
            double dd = 100.0d / 3.0d / 3.0d;
            double ee = 100.0d / 3.0d / 3.0d /3.0d;
            double ff = 100.0d / 3.0d / 3.0d / 3.0d/3.0d;
            double[] omomi = new double[]{
                ff, ee, dd, ee, ff,
                ee, cc, bb, cc, ee,
                dd, bb, aa, bb, dd,
                ee, cc, bb, cc, ee,
                ff, ee, dd, ee, ff,
            };

            // 25近傍のマス番号を取得しておきます。該当がなければ Masu_Honshogi.Error。
            Dictionary<SyElement, double> masuOmomi = new Dictionary<SyElement, double>();
            {
                for (int index = 0; index < 25; index++)
                {
                    SyElement masu = Util_Masu.OkibaSujiDanToMasu(Okiba.ShogiBan, sujis[index], dans[index]);
                    if (masu != Masu_Honshogi.Error)
                    {
                        masuOmomi.Add(masu, omomi[index]);
                    }
                }
            }

            //
            // 味方の金、銀について。
            //
            int countKanagoma = 0; // カナゴマ
            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)// 全駒
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(figKoma).Now);

                if (koma.Pside == args.PlayerInfo.Playerside &&
                    koma.Syurui == Ks14.H04_Gin &&
                    koma.Syurui == Ks14.H05_Kin
                    )
                {
                    if (masuOmomi.ContainsKey(koma.Masu))
                    {
                        score += masuOmomi[koma.Masu];
                        countKanagoma++;
                        if (3 <= countKanagoma)
                        {
                            break;
                        }
                    }
                }
            }


        gt_EndMethod:
            string meisai = "";
#if DEBUG
            // 明細の作成
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
