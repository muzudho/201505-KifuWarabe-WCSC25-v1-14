using Grayscale.P025_KifuLarabe.L004_StructShogi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grayscale.P025_KifuLarabe.L006_SfenEx
{
    /// <summary>
    /// 将棋盤上の情報を数えます。未使用？
    /// </summary>
    [Obsolete("使ってない？")]
    public class StartposBuilder1
    {
        /// <summary>
        /// 先後。
        /// </summary>
        private bool psideIsBlack;

        /// <summary>
        /// 盤上の駒。
        /// </summary>
        private Dictionary<Basho, RO_Star_Koma> ban81;

        /// <summary>
        /// 先手の持ち駒の数。
        /// </summary>
        private Dictionary<Ks14, int> motiBlack;

        /// <summary>
        /// 後手の持ち駒の数。
        /// </summary>
        private Dictionary<Ks14, int> motiWhite;

        public StartposBuilder1()
        {
            this.psideIsBlack = true;
            this.ban81 = new Dictionary<Basho, RO_Star_Koma>();
            this.motiBlack = new Dictionary<Ks14, int>();
            this.motiWhite = new Dictionary<Ks14, int>();
        }

        public void AddKoma(Basho masu, RO_Star_Koma koma)// Ks14 komaSyurui
        {
            if (Util_MasuNum.OnShogiban((int)masu.MasuNumber))
            {
                this.ban81.Add(masu, koma);
            }
            else if (Util_MasuNum.OnSenteKomadai((int)masu.MasuNumber))
            {
                if (this.motiBlack.ContainsKey(koma.Syurui))
                {
                    this.motiBlack[koma.Syurui] = this.motiBlack[koma.Syurui];
                }
                else
                {
                    this.motiBlack.Add(koma.Syurui, 0);
                }
            }
            else if (Util_MasuNum.OnGoteKomadai((int)masu.MasuNumber))
            {
                if (this.motiWhite.ContainsKey(koma.Syurui))
                {
                    this.motiWhite[koma.Syurui] = this.motiWhite[koma.Syurui];
                }
                else
                {
                    this.motiWhite.Add(koma.Syurui, 0);
                }
            }
        }


        private string CreateDanString(int leftestMasu)
        {
            StringBuilder sb = new StringBuilder();

            List<RO_Star_Koma> list = new List<RO_Star_Koma>();
            for (int i = leftestMasu; i >= 0; i -= 9)
            {
                Basho masu = new Basho(i);

                if (this.ban81.ContainsKey(masu))
                {
                    list.Add(this.ban81[masu]);
                }
                else
                {
                    list.Add(null);
                }
            }

            foreach (RO_Star_Koma koma in list)
            {
            }

            return sb.ToString();
        }

        /// <summary>
        /// TODO:作りかけ
        /// </summary>
        /// <returns></returns>
        public string ToSfenString()
        {
            StringBuilder sb = new StringBuilder();

            // 1段目
            {
                //マス番号は、72,63,54,45,36,27,18,9,0。
                sb.Append( this.CreateDanString(72));
            }
            sb.Append("/");

            // 2段目
            {
                //マス番号は、73,64,55,46,37,28,19,10,1。
                sb.Append(this.CreateDanString(73));
            }
            sb.Append("/");

            // 3段目
            {
                sb.Append(this.CreateDanString(74));
            }
            sb.Append("/");

            // 4段目
            {
                sb.Append(this.CreateDanString(75));
            }
            sb.Append("/");

            // 5段目
            {
                sb.Append(this.CreateDanString(76));
            }
            sb.Append("/");

            // 6段目
            {
                sb.Append(this.CreateDanString(77));
            }
            sb.Append("/");

            // 7段目
            {
                sb.Append(this.CreateDanString(78));
            }
            sb.Append("/");

            // 8段目
            {
                sb.Append(this.CreateDanString(79));
            }
            sb.Append("/");

            // 9段目
            {
                sb.Append(this.CreateDanString(80));
            }

            // 先後
            if (this.psideIsBlack)
            {
                sb.Append(" b");
            }
            else
            {
                sb.Append(" w");
            }

            // 持ち駒
            if (this.motiBlack.Count < 1 && this.motiWhite.Count < 1)
            {
                sb.Append(" -");
            }
            else
            {
                sb.Append(" ");

                // 先手持ち駒
                //王
                if (this.motiBlack.ContainsKey(Ks14.H06_Oh))
                {
                    sb.Append(this.motiBlack[Ks14.H06_Oh]);
                    sb.Append("K");
                }
                //飛車
                else if (this.motiBlack.ContainsKey(Ks14.H07_Hisya))
                {
                    sb.Append(this.motiBlack[Ks14.H07_Hisya]);
                    sb.Append("R");
                }
                //角
                else if (this.motiBlack.ContainsKey(Ks14.H08_Kaku))
                {
                    sb.Append(this.motiBlack[Ks14.H08_Kaku]);
                    sb.Append("B");
                }
                //金
                else if (this.motiBlack.ContainsKey(Ks14.H05_Kin))
                {
                    sb.Append(this.motiBlack[Ks14.H05_Kin]);
                    sb.Append("G");
                }
                //銀
                else if (this.motiBlack.ContainsKey(Ks14.H04_Gin))
                {
                    sb.Append(this.motiBlack[Ks14.H04_Gin]);
                    sb.Append("S");
                }
                //桂馬
                else if (this.motiBlack.ContainsKey(Ks14.H03_Kei))
                {
                    sb.Append(this.motiBlack[Ks14.H03_Kei]);
                    sb.Append("N");
                }
                //香車
                else if (this.motiBlack.ContainsKey(Ks14.H02_Kyo))
                {
                    sb.Append(this.motiBlack[Ks14.H02_Kyo]);
                    sb.Append("L");
                }
                //歩
                else if (this.motiBlack.ContainsKey(Ks14.H01_Fu))
                {
                    sb.Append(this.motiBlack[Ks14.H01_Fu]);
                    sb.Append("P");
                }

                // 後手持ち駒
                //王
                if (this.motiWhite.ContainsKey(Ks14.H06_Oh))
                {
                    sb.Append(this.motiBlack[Ks14.H06_Oh]);
                    sb.Append("k");
                }
                //飛車
                else if (this.motiWhite.ContainsKey(Ks14.H07_Hisya))
                {
                    sb.Append(this.motiBlack[Ks14.H07_Hisya]);
                    sb.Append("r");
                }
                //角
                else if (this.motiWhite.ContainsKey(Ks14.H08_Kaku))
                {
                    sb.Append(this.motiBlack[Ks14.H08_Kaku]);
                    sb.Append("b");
                }
                //金
                else if (this.motiWhite.ContainsKey(Ks14.H05_Kin))
                {
                    sb.Append(this.motiBlack[Ks14.H05_Kin]);
                    sb.Append("g");
                }
                //銀
                else if (this.motiWhite.ContainsKey(Ks14.H04_Gin))
                {
                    sb.Append(this.motiBlack[Ks14.H04_Gin]);
                    sb.Append("s");
                }
                //桂馬
                else if (this.motiWhite.ContainsKey(Ks14.H03_Kei))
                {
                    sb.Append(this.motiBlack[Ks14.H03_Kei]);
                    sb.Append("n");
                }
                //香車
                else if (this.motiWhite.ContainsKey(Ks14.H02_Kyo))
                {
                    sb.Append(this.motiBlack[Ks14.H02_Kyo]);
                    sb.Append("l");
                }
                //歩
                else if (this.motiWhite.ContainsKey(Ks14.H01_Fu))
                {
                    sb.Append(this.motiBlack[Ks14.H01_Fu]);
                    sb.Append("p");
                }
            }

            // 1固定
            sb.Append(" 1");


            return sb.ToString();
        }


    }
}
