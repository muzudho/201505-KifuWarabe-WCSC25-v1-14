using Grayscale.P025_KifuLarabe;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.Kifuwarazusa.Entities.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Grayscale.P025_KifuLarabe.L012_Common
{

    /// <summary>
    /// 駒の種類１５個ごとの、「周りに障害物がないときに、ルール上移動可能なマス」。
    /// </summary>
    public abstract class Rule01_PotentialMove_15Array
    {

        public delegate SySet<SyElement> DELEGATE_CreateLegalMoveLv1(Playerside pside, SyElement masu_ji);

        public static DELEGATE_CreateLegalMoveLv1[] ItemMethods
        {
            get
            {
                return Rule01_PotentialMove_15Array.itemMethods;
            }
        }
        private static DELEGATE_CreateLegalMoveLv1[] itemMethods;

        static Rule01_PotentialMove_15Array()
        {
            Rule01_PotentialMove_15Array.itemMethods = new DELEGATE_CreateLegalMoveLv1[]{
                null,//[0]
                Rule01_PotentialMove_15Array.Create_01Fu,//[1]
                Rule01_PotentialMove_15Array.Create_02Kyo,
                Rule01_PotentialMove_15Array.Create_03Kei,
                Rule01_PotentialMove_15Array.Create_04Gin,
                Rule01_PotentialMove_15Array.Create_05Kin,
                Rule01_PotentialMove_15Array.Create_06Oh,
                Rule01_PotentialMove_15Array.Create_07Hisya,
                Rule01_PotentialMove_15Array.Create_08Kaku,
                Rule01_PotentialMove_15Array.Create_09Ryu,
                Rule01_PotentialMove_15Array.Create_10Uma,//[10]
                Rule01_PotentialMove_15Array.Create_05Kin,
                Rule01_PotentialMove_15Array.Create_05Kin,
                Rule01_PotentialMove_15Array.Create_05Kin,
                Rule01_PotentialMove_15Array.Create_05Kin,
                Rule01_PotentialMove_15Array.Create_15ErrorKoma,//[15]
            };
        }


        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static SySet<SyElement> Create_01Fu(Playerside pside, SyElement masu_ji)
        {
            SySet_Default<SyElement> dst = new SySet_Default<SyElement>("歩の移動先");

            if (Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(masu_ji))
            {
                dst.AddSupersets(KomanoKidou.DstIppo_上(pside, masu_ji));
            }
            else if( (Okiba.Sente_Komadai|Okiba.Gote_Komadai).HasFlag(
                Util_Masu.Masu_ToOkiba(masu_ji)))
            {
                dst.AddSupersets(KomanoKidou.Dst_歩打面(pside));
            }

            return dst;
        }

        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static SySet<SyElement> Create_02Kyo(Playerside pside, SyElement masu_ji)
        {
            SySet_Default<SyElement> dst = new SySet_Default<SyElement>("香の移動先");

            if (Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(masu_ji))
            {
                dst.AddSupersets(KomanoKidou.DstKantu_上(pside, masu_ji));
            }
            else if ((Okiba.Sente_Komadai | Okiba.Gote_Komadai).HasFlag(
                Util_Masu.Masu_ToOkiba(masu_ji)))
            {
                dst.AddSupersets(KomanoKidou.Dst_歩打面(pside));//香も同じ
            }

            return dst;
        }


        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static SySet<SyElement> Create_03Kei(Playerside pside, SyElement masu_ji)
        {
            SySet_Default<SyElement> dst = new SySet_Default<SyElement>("桂の移動先");

            if (Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(masu_ji))
            {
                dst.AddSupersets(KomanoKidou.DstKeimatobi_駆(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstKeimatobi_跳(pside, masu_ji));
            }
            else if ((Okiba.Sente_Komadai | Okiba.Gote_Komadai).HasFlag(
                Util_Masu.Masu_ToOkiba(masu_ji)))
            {
                dst.AddSupersets(KomanoKidou.Dst_桂打面(pside));
            }

            return dst;
        }


        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static SySet<SyElement> Create_04Gin(Playerside pside, SyElement masu_ji)
        {
            SySet_Default<SyElement> dst = new SySet_Default<SyElement>("銀の移動先");

            if (Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(masu_ji))
            {
                dst.AddSupersets(KomanoKidou.DstIppo_上(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_昇(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_沈(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_降(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_浮(pside, masu_ji));
            }
            else if ((Okiba.Sente_Komadai | Okiba.Gote_Komadai).HasFlag(
                Util_Masu.Masu_ToOkiba(masu_ji)))
            {
                dst.AddSupersets(KomanoKidou.Dst_全打面(pside));
            }

            return dst;
        }

        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static SySet<SyElement> Create_05Kin(Playerside pside, SyElement masu_ji)
        {
            SySet<SyElement> dst = new SySet_Default<SyElement>("金の移動先");

            if (Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(masu_ji))
            {
                dst = Rule01_PotentialMove_15Array.CreateKin_static(pside, masu_ji);
            }
            else if ((Okiba.Sente_Komadai | Okiba.Gote_Komadai).HasFlag(
                Util_Masu.Masu_ToOkiba(masu_ji)))
            {
                dst.AddSupersets(KomanoKidou.Dst_全打面(pside));
            }

            return dst;
        }

        public static SySet<SyElement> CreateKin_static(Playerside pside, SyElement masu_ji)
        {
            SySet_Default<SyElement> dst = new SySet_Default<SyElement>("カナゴマの移動先");

            if (Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(masu_ji))
            {
                dst.AddSupersets(KomanoKidou.DstIppo_上(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_昇(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_射(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_引(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_滑(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_浮(pside, masu_ji));
            }

            return dst;
        }


        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static SySet<SyElement> Create_06Oh(Playerside pside, SyElement masu_ji)
        {
            SySet_Default<SyElement> dst = new SySet_Default<SyElement>("王の移動先");

            if (Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(masu_ji))
            {
                dst.AddSupersets(KomanoKidou.DstIppo_上(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_昇(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_射(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_沈(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_引(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_降(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_滑(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_浮(pside, masu_ji));
            }
            else if ((Okiba.Sente_Komadai | Okiba.Gote_Komadai).HasFlag(
                Util_Masu.Masu_ToOkiba(masu_ji)))
            {
                dst.AddSupersets(KomanoKidou.Dst_全打面(pside));
            }

            return dst;
        }


        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static SySet<SyElement> Create_07Hisya(Playerside pside, SyElement masu_ji)
        {
            SySet_Default<SyElement> dst = new SySet_Default<SyElement>("飛車の移動先");

            if (Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(masu_ji))
            {
                dst.AddSupersets(KomanoKidou.DstKantu_上(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstKantu_射(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstKantu_引(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstKantu_滑(pside, masu_ji));
            }
            else if ((Okiba.Sente_Komadai | Okiba.Gote_Komadai).HasFlag(
                Util_Masu.Masu_ToOkiba(masu_ji)))
            {
                dst.AddSupersets(KomanoKidou.Dst_全打面(pside));
            }

            return dst;
        }


        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static SySet<SyElement> Create_08Kaku(Playerside pside, SyElement masu_ji)
        {
            SySet_Default<SyElement> dst = new SySet_Default<SyElement>("角の移動先");

            if (Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(masu_ji))
            {
                dst.AddSupersets(KomanoKidou.DstKantu_昇(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstKantu_沈(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstKantu_降(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstKantu_浮(pside, masu_ji));
            }
            else if ((Okiba.Sente_Komadai | Okiba.Gote_Komadai).HasFlag(
                Util_Masu.Masu_ToOkiba(masu_ji)))
            {
                dst.AddSupersets(KomanoKidou.Dst_全打面(pside));
            }

            return dst;
        }



        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static SySet<SyElement> Create_09Ryu(Playerside pside, SyElement masu_ji)
        {
            SySet_Default<SyElement> dst = new SySet_Default<SyElement>("竜の移動先");

            if (Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(masu_ji))
            {
                dst.AddSupersets(KomanoKidou.DstKantu_上(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_昇(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstKantu_射(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_沈(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstKantu_引(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_降(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstKantu_滑(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_浮(pside, masu_ji));
            }
            else if ((Okiba.Sente_Komadai | Okiba.Gote_Komadai).HasFlag(
                Util_Masu.Masu_ToOkiba(masu_ji)))
            {
                dst.AddSupersets(KomanoKidou.Dst_全打面(pside));
            }

            return dst;
        }



        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static SySet<SyElement> Create_10Uma(Playerside pside, SyElement masu_ji)
        {
            SySet_Default<SyElement> dst = new SySet_Default<SyElement>("馬の移動先");

            if (Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(masu_ji))
            {
                dst.AddSupersets(KomanoKidou.DstIppo_上(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstKantu_昇(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_射(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstKantu_沈(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_引(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstKantu_降(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstIppo_滑(pside, masu_ji));
                dst.AddSupersets(KomanoKidou.DstKantu_浮(pside, masu_ji));
            }
            else if ((Okiba.Sente_Komadai | Okiba.Gote_Komadai).HasFlag(
                Util_Masu.Masu_ToOkiba(masu_ji)))
            {
                dst.AddSupersets(KomanoKidou.Dst_全打面(pside));
            }

            return dst;
        }

        /// <summary>
        /// 合法手レベル１
        /// </summary>
        /// <returns></returns>
        public static SySet<SyElement> Create_15ErrorKoma(Playerside pside, SyElement masu_ji)
        {
            return new SySet_Default<SyElement>("エラー駒の移動先");
        }


    }


}
