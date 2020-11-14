using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.P025_KifuLarabe.L004_StructShogi;

using System.IO;

namespace Grayscale.P025_KifuLarabe.L012_Common
{


    public abstract class ForcePromotionUtil
    {

        /// <summary>
        /// 配役と、升から、次の強制転成配役を求めます。
        /// 
        /// 
        /// </summary>
        /// <param name="currentHaiyaku"></param>
        /// <param name="masuHandle"></param>
        /// <returns>転生しないなら　未設定　を返します。</returns>
        public static Kh185 MasuHandleTo_ForcePromotionHaiyaku(Kh185 currentHaiyaku, int masuHandle,string hint)
        {
            Kh185 result;

            Dictionary<int, Kh185> map2 = ForcePromotionArray.HaiyakuMap[currentHaiyaku];

            if (
                null == map2
                ||
                !map2.ContainsKey(masuHandle)
                )
            {
                result = Kh185.n000_未設定;
                goto gt_EndMethod;
            }

            result = map2[masuHandle];//null非許容型


            {
                StringBuilder sbLog = new StringBuilder();

                if (File.Exists("#強制転成デバッグ.txt"))
                {
                    sbLog.Append(File.ReadAllText("#強制転成デバッグ.txt"));
                }

                sbLog.AppendLine();
                sbLog.AppendLine(hint);
                sbLog.AppendLine("　現在の配役=[" + currentHaiyaku + "]");
                sbLog.AppendLine("　masuHandle=[" + masuHandle + "]");
                sbLog.AppendLine("　強制転成後の配役=[" + result + "]");
                File.WriteAllText("#強制転成デバッグ.txt", sbLog.ToString());
            }


        gt_EndMethod:
            return result;
        }


    }


}
