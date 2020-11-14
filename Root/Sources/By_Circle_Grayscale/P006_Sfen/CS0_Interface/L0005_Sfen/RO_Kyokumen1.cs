using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.P006_Sfen.L0005_Sfen
{
    public interface RO_Kyokumen1
    {

        /// <summary>
        /// 将棋盤上の駒。[suji][dan]。
        /// sujiは1～9。danは1～9。0は空欄。つまり 100要素ある。
        /// 「K」「+p」といった形式で書く。(SFEN形式)
        /// </summary>
        string[][] Ban { get; set; }

        /// <summary>
        /// 先手の持ち駒の数。[0]から、飛,角,金,銀,桂,香,歩 の順。
        /// </summary>
        int[] MotiP1 { get; set; }

        /// <summary>
        /// 後手の持ち駒の数。[0]から、飛,角,金,銀,桂,香,歩 の順。
        /// </summary>
        int[] MotiP2 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="white">「w」なら真、「b」なら偽。</param>
        /// <returns></returns>
        string ToSfenstring(bool white);

    }
}
