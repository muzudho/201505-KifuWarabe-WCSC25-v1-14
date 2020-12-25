using Grayscale.Kifuwarazusa.Entities.Features;
using System.Collections.Generic;
//スプライト番号


namespace Grayscale.P025_KifuLarabe.L004_StructShogi
{


    /// <summary>
    /// 駒配役１８４だぜ☆
    /// 
    /// 「1,歩,1,,,,,,,,,,,,,,,,,,,,,,」といった内容を、
    /// [1]「空間1」に置き換えるぜ☆
    /// 
    /// 駒の情報はそぎ落とすぜ☆　筋の情報だけが残る☆
    /// </summary>
    public abstract class Haiyaku184Array
    {

        /// <summary>
        /// 配役名。
        /// </summary>
        public static List<string> Name { get { return Haiyaku184Array.name; } }
        private static List<string> name;

        /// <summary>
        /// 絵修飾字。
        /// </summary>
        public static List<string> Name2 { get { return Haiyaku184Array.name2; } }
        private static List<string> name2;

        /// <summary>
        /// 種類。
        /// </summary>
        public static Ks14 Syurui(Kh185 haiyaku)
        {
            return Haiyaku184Array.syurui[(int)haiyaku];
        }
        public static void AddSyurui(Ks14 syurui)
        {
            Haiyaku184Array.syurui.Add(syurui);
        }
        private static List<Ks14> syurui;


        /// <summary>
        /// 空間フィールド。（１～２４個）
        /// </summary>
        public static Dictionary<Kh185, List<SySet<SyElement>>> KukanMasus { get { return Haiyaku184Array.kukanMasus; } }
        private static Dictionary<Kh185, List<SySet<SyElement>>> kukanMasus;


        static Haiyaku184Array()
        {
            Haiyaku184Array.kukanMasus = new Dictionary<Kh185, List<SySet<SyElement>>>();
            Haiyaku184Array.syurui = new List<Ks14>();
            Haiyaku184Array.name = new List<string>();
            Haiyaku184Array.name2 = new List<string>();
        }






    }
}
