using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwarazusa.Entities.Features
{
    public abstract class Util_Sasu
    {
        public static SkyConst Sasu(
            SkyConst src_Sky,//現局面
            Finger finger,//動かす駒
            SyElement masu,//移動先マス
            Playerside pside_genTeban//動かす駒がどちらのプレイヤーのものか
            )
        {
            SkyBuffer dst_Sky = new SkyBuffer(src_Sky.Clone()); // 現局面をコピーします。

            // 移動先に相手の駒がないか、確認します。
            Finger tottaKoma = Util_Sky.Fingers_AtMasuNow(new SkyConst(dst_Sky), masu).ToFirst();

            if (tottaKoma != Fingers.Error_1)
            {
                // なにか駒を取ったら
                SyElement akiMasu;

                if (pside_genTeban == Playerside.P1)
                {
                    akiMasu = KifuIO.GetKomadaiKomabukuroSpace(Okiba.Sente_Komadai, new SkyConst(dst_Sky));
                }
                else
                {
                    akiMasu = KifuIO.GetKomadaiKomabukuroSpace(Okiba.Gote_Komadai, new SkyConst(dst_Sky));
                }


                RO_Star_Koma koma = Util_Koma.AsKoma(dst_Sky.StarlightIndexOf(tottaKoma).Now);

                // FIXME:配役あってるか？
                dst_Sky.AddOverwriteStarlight(tottaKoma, new RO_MotionlessStarlight(new RO_Star_Koma(pside_genTeban, akiMasu, koma.Syurui)));//tottaKoma,

            }

            // 駒を１個動かします。
            // FIXME: 取った駒はどうなっている？
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(dst_Sky.StarlightIndexOf(finger).Now);

                dst_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight(new RO_Star_Koma(pside_genTeban, masu, koma.Syurui)));//finger,
            }

            return new SkyConst(dst_Sky);
        }
    }
}
