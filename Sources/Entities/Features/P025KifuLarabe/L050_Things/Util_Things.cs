using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grayscale.P025_KifuLarabe;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L002_GraphicLog;
using Grayscale.P025_KifuLarabe.L007_Random;

using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P025_KifuLarabe.L025_Play;
using Grayscale.P006_Syugoron;

using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.Kifuwarazusa.Entities.Logging;

namespace Grayscale.P025_KifuLarabe.L050_Things
{
    //public class Arg_KmEffectSameIKUSA()
    //{
    //    public Arg_KmEffectSameIKUSA()
    //    {
    //    }
    //}

    public class Util_Things
    {

        /// <summary>
        /// 戦駒の利き（利きを調べる側）
        /// </summary>
        /// <param name="enableLog"></param>
        /// <param name="fingers_seme_IKUSA"></param>
        /// <param name="masus_seme_IKUSA"></param>
        /// <param name="masus_kurau_IKUSA"></param>
        /// <param name="src_Sky"></param>
        /// <param name="hint"></param>
        /// <returns></returns>
        public static Maps_OneAndOne<Finger, SySet<SyElement>> Get_KmEffect_seme_IKUSA(
            Fingers fingers_seme_IKUSA,
            SySet<SyElement> masus_seme_IKUSA,
            SySet<SyElement> masus_kurau_IKUSA,
            SkyConst src_Sky,
            bool enableLog,
            string hint_orNull
            )
        {
            // 利きを調べる側の利き（戦駒）
            Maps_OneAndOne<Finger, SySet<SyElement>> km = Play.Get_PotentialMoves_Ikusa(src_Sky, fingers_seme_IKUSA);

            // 盤上の現手番の駒利きから、 現手番の駒がある枡を除外します。
            km = Play_KomaAndMove.MinusMasus(false, src_Sky, km, masus_seme_IKUSA);

            // そこから、相手番の駒がある枡「以降」を更に除外します。
            km = Play_KomaAndMove.Minus_OverThereMasus(enableLog, src_Sky, km, masus_kurau_IKUSA);

            return km;
        }

        /// <summary>
        /// 攻め手の持駒利き
        /// </summary>
        /// <param name="fingers_seme_MOTI"></param>
        /// <param name="masus_kurau_IKUSA"></param>
        /// <param name="src_Sky"></param>
        /// <param name="hint"></param>
        /// <returns></returns>
        public static List_OneAndMulti<Finger, SySet<SyElement>> Get_Move_Moti(
            Fingers fingers_seme_MOTI,
            SySet<SyElement> masus_seme_IKUSA,
            SySet<SyElement> masus_kurau_IKUSA,
            SkyConst src_Sky,
            string hint_orNull
            )
        {
            // 「どの持ち駒を」「どこに置けるか」のコレクション。
            List_OneAndMulti<Finger, SySet<SyElement>> om;


            // 持ち駒を置けない升
            SySet<SyElement> okenaiMasus = new SySet_Default<SyElement>("持ち駒を置けない升");
            {
                // 自分の駒がある升
                okenaiMasus.AddSupersets(masus_seme_IKUSA);

                // 相手番の駒がある升
                okenaiMasus.AddSupersets(masus_kurau_IKUSA);
            }

            om = Play.Get_PotentialMove_Motikoma(src_Sky, fingers_seme_MOTI, okenaiMasus);
            return om;
        }

        public static void AAABAAAA_SplitGroup(
            out Fingers fingers_seme_IKUSA,//戦駒（利きを調べる側）
            out Fingers fingers_kurau_IKUSA,//戦駒（喰らう側）
            out Fingers fingers_seme_MOTI,// 持駒（利きを調べる側）
            out Fingers fingers_kurau_MOTI,// 持駒（喰らう側）
            SkyConst src_Sky,
            Playerside tebanSeme,
            Playerside tebanKurau
        )
        {
            // 戦駒
            fingers_seme_IKUSA = Util_Sky.Fingers_ByOkibaPsideNow(src_Sky, Okiba.ShogiBan, tebanSeme);
            fingers_kurau_IKUSA = Util_Sky.Fingers_ByOkibaPsideNow(src_Sky, Okiba.ShogiBan, tebanKurau);

            // 攻手の持駒
            if (tebanSeme == Playerside.P1)
            {
                // 攻手が先手番のとき。
                fingers_seme_MOTI = Util_Sky.Fingers_ByOkibaPsideNow(src_Sky, Okiba.Sente_Komadai, tebanSeme);
                fingers_kurau_MOTI = Util_Sky.Fingers_ByOkibaPsideNow(src_Sky, Okiba.Gote_Komadai, tebanKurau);
            }
            else if (tebanSeme == Playerside.P2)
            {
                // 攻手が後手番のとき。
                fingers_seme_MOTI = Util_Sky.Fingers_ByOkibaPsideNow(src_Sky, Okiba.Gote_Komadai, tebanSeme);
                fingers_kurau_MOTI = Util_Sky.Fingers_ByOkibaPsideNow(src_Sky, Okiba.Sente_Komadai, tebanKurau);
            }
            else
            {
                throw new Exception("駒台の持ち駒を調べようとしましたが、先手でも、後手でもない指定でした。");
            }

            //#if DEBUG
            //            MessageBox.Show("fingers_seme_MOTI数=[" + fingers_seme_MOTI.Count.ToString() + "]", "デバッグ SplitGroup");
            //#endif
        }


    }
}
