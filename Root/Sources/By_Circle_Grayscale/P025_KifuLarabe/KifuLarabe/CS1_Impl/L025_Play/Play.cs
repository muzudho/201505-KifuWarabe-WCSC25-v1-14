using Grayscale.P025_KifuLarabe;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L007_Random;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P006_Syugoron;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P025_KifuLarabe.L025_Play
{


    public abstract class Play
    {

        /// <summary>
        /// 指定した駒全てについて、基本的な駒の動きを返します。（金は「前、ななめ前、横、下に進める」のような）
        /// </summary>
        /// <param name="src_Sky"></param>
        /// <param name="fingers"></param>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public static Maps_OneAndOne<Finger, SySet<SyElement>> Get_PotentialMoves_Ikusa(
            SkyConst src_Sky,
            Fingers fingers,
            LarabeLoggerable logTag_orNull
            )
        {
            Maps_OneAndOne<Finger, SySet<SyElement>> kiki_fMs = new Maps_OneAndOne<Finger, SySet<SyElement>>();// 「どの駒を、どこに進める」の一覧

            foreach (Finger finger in fingers.Items)
            {
                // ポテンシャル・ムーブを調べます。
                SySet<SyElement> move = Util_Sky.KomaKidou_Potential(finger, src_Sky);//←ポテンシャル・ムーブ取得関数を選択。歩とか。

                if (!move.IsEmptySet())
                {
                    // 移動可能升があるなら
                    Util_KomabetuMasus.AddOverwrite(kiki_fMs, finger, move);
                }
            }

            return kiki_fMs;
        }

        /// <summary>
        /// 指定した持ち駒全てについて、基本的な駒の動きを返します。（金は「前、ななめ前、横、下に進める」のような）
        /// 
        /// TODO: 打ち歩詰めﾁｪｯｸ
        /// 
        /// </summary>
        /// <param name="src_Sky"></param>
        /// <param name="fingers"></param>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public static List_OneAndMulti<Finger, SySet<SyElement>> Get_PotentialMove_Motikoma(
            SkyConst src_Sky,
            Fingers fingers,
            SySet<SyElement> okenaiMasus,
            LarabeLoggerable logTag_orNull
            )
        {

            // 「どの持ち駒を」「どこに置いたとき」「どこに利きがある」のコレクション。
            List_OneAndMulti<Finger, SySet<SyElement>> resultOmm = new List_OneAndMulti<Finger, SySet<SyElement>>();

            foreach (Finger donoKoma in fingers.Items)
            {
                // ポテンシャル・ムーブを調べます。

                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(donoKoma).Now);


                Ks14 syurui = Haiyaku184Array.Syurui(koma.Haiyaku);

                // 置ける升
                SySet<SyElement> okeruMasus = Util_Sky.KomaKidou_Potential(donoKoma, src_Sky);

                // 置けない升を引きます。
                okeruMasus = okeruMasus.Minus_Closed( okenaiMasus);

                if (syurui == Ks14.H01_Fu)
                {
                    // 二歩チェック

                    int suji;
                    Util_MasuNum.MasuToSuji(koma.Masu, out suji);

                    Fingers sujiKomas = Util_Sky.Fingers_InSuji(src_Sky,suji);

                    Starlight fu = src_Sky.StarlightIndexOf(donoKoma);
                    Fingers existFu = Util_Sky.Matches(fu, src_Sky, sujiKomas);

                    if (0 < existFu.Count)
                    {
                        // 二歩だ☆！
                        goto gt_Next;
                    }

                    // TODO:打ち歩詰めチェック
                }


                if (!okeruMasus.IsEmptySet())
                {
                    // 空でないなら
                    resultOmm.AddNew(donoKoma, okeruMasus);
                }

            gt_Next:
                ;
            }

            return resultOmm;
        }

    }


}
