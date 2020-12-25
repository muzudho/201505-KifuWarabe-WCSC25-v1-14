using System;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号

namespace Grayscale.Kifuwarazusa.Entities.Features
{
    public class Util_Koma
    {

        #region 定数

        /// <summary>
        /// 働かない値として、筋に埋めておくためのものです。8～-8 程度だと、角等の射程に入るので、大きく外した数字をフラグに使います。
        /// </summary>
        public const int CTRL_NOTHING_PROPERTY_SUJI = int.MinValue;

        /// <summary>
        /// 働かない値として、段に埋めておくためのものです。8～-8 程度だと、角等の射程に入るので、大きく外した数字をフラグに使います。
        /// </summary>
        public const int CTRL_NOTHING_PROPERTY_DAN = int.MinValue;

        #endregion


        public static RO_Star_Koma AsKoma(Starlightable light)
        {
            RO_Star_Koma koma;

            if (light is RO_Star_Koma)
            {
                koma = (RO_Star_Koma)light;
            }
            else
            {
                throw new Exception("未対応の星の光クラス");
            }

            return koma;
        }

        public static RO_Star_Koma FromFinger(SkyConst src_Sky, Finger finger)
        {
            RO_Star_Koma koma;

            Starlightable lightable = src_Sky.StarlightIndexOf(finger).Now;

            if (lightable is RO_Star_Koma)
            {
                koma = (RO_Star_Koma)lightable;
            }
            else
            {
                throw new Exception("未対応の星の光クラス");
            }

            return koma;
        }

    }
}
