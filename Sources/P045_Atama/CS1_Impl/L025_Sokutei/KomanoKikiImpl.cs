using Grayscale.P006_Syugoron;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P045_Atama.L000125_Sokutei;
using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P027MoveGen.L050_MovableMove;

namespace Grayscale.P045_Atama.L025_Sokutei
{

    public class KomanoKikiImpl : KomanoKiki
    {
        /// <summary>
        /// 枡毎の、味方、敵。
        /// </summary>
        private Dictionary<int, Playerside> HMasu_PlayersideList { get; set; }

        /// <summary>
        /// 枡毎の、利き数。
        /// </summary>
        public Dictionary<int, int> Kikisu_AtMasu_Mikata { get; set; }
        public Dictionary<int, int> Kikisu_AtMasu_Teki { get; set; }



        private KomanoKikiImpl()
        {
            this.HMasu_PlayersideList = new Dictionary<int, Playerside>();
            for (int masuIndex = 0; masuIndex < 202; masuIndex++)
            {
                this.HMasu_PlayersideList.Add(masuIndex, Playerside.Empty);
            }

            this.Kikisu_AtMasu_Mikata = new Dictionary<int, int>();
            for (int masuIndex = 0; masuIndex < 202; masuIndex++)
            {
                this.Kikisu_AtMasu_Mikata.Add(masuIndex, 0);
            }

            this.Kikisu_AtMasu_Teki = new Dictionary<int, int>();
            for (int masuIndex = 0; masuIndex < 202; masuIndex++)
            {
                this.Kikisu_AtMasu_Teki.Add(masuIndex, 0);
            }
        }


        /// <summary>
        /// 別方法試し中。
        /// 
        /// </summary>
        /// <param name="src_Sky"></param>
        public static KomanoKikiImpl BETUKAI(SkyConst src_Sky, Playerside tebanside)
        {
            // ①現手番の駒の移動可能場所_被王手含む
            List_OneAndMulti<Finger, SySet<SyElement>> komaBETUSusumeruMasus;

            Util_MovableMove.LA_Get_KomaBETUSusumeruMasus(
                out komaBETUSusumeruMasus,//進めるマス
                new MmGenjo_MovableMasuImpl(
                    true,//本将棋か
                    src_Sky,//現在の局面
                    tebanside,//手番
                    false//相手番か
                ),
                null
            );

            KomanoKikiImpl self = new KomanoKikiImpl();

            //
            // 「升ごとの敵味方」を調べます。
            //
            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)// 全駒
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(figKoma).Now);

                self.HMasu_PlayersideList[Util_Masu.AsMasuNumber(koma.Masu)] = koma.Pside;
            }

            //
            // 駒のない升は無視します。
            //

            //
            // 駒のあるマスに、その駒の味方のコマが効いていれば　味方＋１
            //
            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)// 全駒
            {
                //
                // 駒
                //
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(figKoma).Now);

                // 将棋盤上の戦駒のみ判定
                if (Okiba.ShogiBan != Util_Masu.Masu_ToOkiba(koma.Masu))
                {
                    goto gt_Next1;
                }


                //
                // 駒の利き FIXME:貫通してないか？
                //
                komaBETUSusumeruMasus.Foreach_Entry((Finger figKoma2, SySet<SyElement> kikiZukei, ref bool toBreak) =>
                {
                    IEnumerable<SyElement> kikiMasuList = kikiZukei.Elements;
                    foreach (SyElement masu in kikiMasuList)
                    {
                        // その枡に利いている駒のハンドルを追加
                        if (self.HMasu_PlayersideList[Util_Masu.AsMasuNumber(masu)] == Playerside.Empty)
                        {
                            // 駒のないマスは無視。
                        }
                        else if (self.HMasu_PlayersideList[Util_Masu.AsMasuNumber(masu)] == koma.Pside)
                        {
                            // 利きのあるマスにある駒と、この駒のプレイヤーサイドが同じ。
                            self.Kikisu_AtMasu_Mikata[Util_Masu.AsMasuNumber(masu)] += 1;
                        }
                        else
                        {
                            // 反対の場合。
                            self.Kikisu_AtMasu_Teki[Util_Masu.AsMasuNumber(masu)] += 1;
                        }
                    }
                });

            gt_Next1:
                ;
            }

            return self;
        }

        /// <summary>
        /// 駒の利きを調べます。
        /// </summary>
        /// <param name="src_Sky"></param>
        /// <returns></returns>
        public static KomanoKikiImpl MasuBETUKikisu(SkyConst src_Sky, Playerside tebanside)
        {


            {
                return KomanoKikiImpl.BETUKAI(src_Sky, tebanside);
            }





            /*
            KomanoKikiImpl self = new KomanoKikiImpl();

            //
            // 「升ごとの敵味方」を調べます。
            //
            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)// 全駒
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(figKoma).Now);

                self.HMasu_PlayersideList[Util_Masu.AsMasuNumber(koma.Masu)] = koma.Pside;
            }

            //
            // 駒のない升は無視します。
            //

            //
            // 駒のあるマスに、その駒の味方のコマが効いていれば　味方＋１
            //
            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)// 全駒
            {
                //
                // 駒
                //
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(figKoma).Now);

                // 将棋盤上の戦駒のみ判定
                if (Okiba.ShogiBan != Util_Masu.Masu_ToOkiba(koma.Masu))
                {
                    goto gt_Next1;
                }


                //
                // 駒の利き FIXME:貫通してないか？
                //
                SySet<SyElement> kikiZukei = Util_Sky.KomaKidou_Potential(figKoma, src_Sky);



                IEnumerable<SyElement> kikiMasuList = kikiZukei.Elements;
                foreach (SyElement masu in kikiMasuList)
                {
                    // その枡に利いている駒のハンドルを追加
                    if (self.HMasu_PlayersideList[Util_Masu.AsMasuNumber(masu)] == Playerside.Empty)
                    {
                        // 駒のないマスは無視。
                    }
                    else if (self.HMasu_PlayersideList[Util_Masu.AsMasuNumber(masu)] == koma.Pside)
                    {
                        // 利きのあるマスにある駒と、この駒のプレイヤーサイドが同じ。
                        self.Kikisu_AtMasu_Mikata[Util_Masu.AsMasuNumber(masu)] += 1;
                    }
                    else
                    {
                        // 反対の場合。
                        self.Kikisu_AtMasu_Teki[Util_Masu.AsMasuNumber(masu)] += 1;
                    }
                }

            gt_Next1:
                ;
            }

            return self;
             */
        }

    }

}
