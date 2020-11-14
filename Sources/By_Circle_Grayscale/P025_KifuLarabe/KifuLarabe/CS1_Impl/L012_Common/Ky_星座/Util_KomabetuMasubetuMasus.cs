using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P006_Syugoron;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P025_KifuLarabe.L012_Common
{
    public abstract class Util_KomabetuMasubetuMasus
    {

        /// <summary>
        /// 変換
        /// </summary>
        /// <returns></returns>
        public static List_OneAndMulti<Finger, SySet<SyElement>> SplitKey1And2(
            Maps_OneAndMultiAndMulti<Finger, Basho, SySet<SyElement>> komabetuMasubetuMasus
            )
        {
            List_OneAndMulti<Finger, SySet<SyElement>> result = new List_OneAndMulti<Finger, SySet<SyElement>>();

            komabetuMasubetuMasus.Foreach_Entry((Finger finger, Basho key2, SySet<SyElement> masus, ref bool toBreak) =>
            {
                result.AddNew(finger, masus);
            });

            return result;
        }

        public static string LogString_Set(
            Maps_OneAndMultiAndMulti<Finger, Basho, SySet<SyElement>> komabetuMasubetuMasus
            )
        {
            StringBuilder sb = new StringBuilder();

            // 全要素
            komabetuMasubetuMasus.Foreach_Entry((Finger key1, Basho key2, SySet<SyElement> value, ref bool toBreak) =>
            {
                sb.AppendLine("駒＝[" + key1.ToString() + "]");
                sb.AppendLine("升＝[" + key2.ToString() + "]");
                sb.AppendLine(Util_Masus<Basho>.LogString_Concrete(value));
            });

            return sb.ToString();
        }


        public static string Dump(
            Maps_OneAndMultiAndMulti<Finger, Basho, SySet<SyElement>> komabetuMasubetuMasus
            )
        {
            StringBuilder sb = new StringBuilder();

            komabetuMasubetuMasus.Foreach_Entry((Finger key1, Basho key2, SySet<SyElement> value, ref bool toBreak) =>
            {
                foreach (Basho masu3 in value.Elements)
                {
                    sb.AppendLine("finger1=[" + key1.ToString() + "] masu2=[" + key2.ToString() + "] masu3=[" + masu3.ToString() + "]");
                }
            });

            return sb.ToString();
        }


        public static string LogString_Concrete(
            Maps_OneAndMultiAndMulti<Finger, Basho, SySet<SyElement>> komabetuMasubetuMasus
            )
        {

            StringBuilder sb = new StringBuilder();


            komabetuMasubetuMasus.Foreach_Entry((Finger key1, Basho key2, SySet<SyElement> value, ref bool toBreak) =>
            {
                sb.Append("[駒");
                sb.Append(key1.ToString());
                sb.Append(" 升");
                sb.Append(key2.ToString());
                sb.Append("]");

                foreach (Basho masu in value.Elements)
                {
                    sb.Append(masu.ToString());
                }
            });


            return sb.ToString();

        }

    }
}
