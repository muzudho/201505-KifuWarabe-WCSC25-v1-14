using System.Text;

namespace Grayscale.P006Sfen
{
    public class SfenPosition1Impl : ISfenPosition1
    {

        public string[][] Ban { get; set; }
        public int[] MotiP1 { get; set; }
        public int[] MotiP2 { get; set; }

        public SfenPosition1Impl()
        {
            this.Ban = new string[10][];// 将棋盤
            this.MotiP1 = new int[7];// 先手の持ち駒の数。[0]から、飛,角,金,銀,桂,香,歩 の順。
            this.MotiP2 = new int[7];

            // 全クリアー
            {
                // 将棋盤
                for (int suji = 0; suji < 10; suji++)
                {
                    this.Ban[suji] = new string[10];

                    for (int dan = 0; dan < 10; dan++)
                    {
                        this.Ban[suji][dan] = "";
                    }
                }
            }

        }


        public string ToSfenstring(bool white)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("sfen ");

            for (int dan = 1; dan <= 9; dan++)
            {
                int spaceCount = 0;

                for (int suji = 9; suji >= 1; suji--)
                {
                    // 将棋盤上のどこかにある駒？
                    string koma0 = this.Ban[suji][dan];

                    if ("" != koma0)
                    {
                        if (0 < spaceCount)
                        {
                            sb.Append(spaceCount);
                            spaceCount = 0;
                        }

                        sb.Append(koma0);
                    }
                    else
                    {
                        spaceCount++;
                    }

                }

                if (0 < spaceCount)
                {
                    sb.Append(spaceCount);
                    spaceCount = 0;
                }

                if (dan != 9)
                {
                    sb.Append("/");
                }
            }

            sb.Append(" ");

            //------------------------------------------------------------
            // 先後
            //------------------------------------------------------------
            if (white)
            {
                sb.Append("w");
            }
            else
            {
                sb.Append("b");
            }

            sb.Append(" ");

            //------------------------------------------------------------
            // 持ち駒
            //------------------------------------------------------------
            {
                int mK = 0;
                int mR = 0;
                int mB = 0;
                int mG = 0;
                int mS = 0;
                int mN = 0;
                int mL = 0;
                int mP = 0;

                int mk = 0;
                int mr = 0;
                int mb = 0;
                int mg = 0;
                int ms = 0;
                int mn = 0;
                int ml = 0;
                int mp = 0;
                this.GetMoti(
                    out mK,
                    out mR,
                    out mB,
                    out mG,
                    out mS,
                    out mN,
                    out mL,
                    out mP,

                    out mk,
                    out mr,
                    out mb,
                    out mg,
                    out ms,
                    out mn,
                    out ml,
                    out mp
                    );



                if (0 == mK + mR + mB + mG + mS + mN + mL + mP + mk + mr + mb + mg + ms + mn + ml + mp)
                {
                    sb.Append("-");
                }
                else
                {
                    if (0 < mK)
                    {
                        if (1 < mK)
                        {
                            sb.Append(mK);
                        }
                        sb.Append("K");
                    }

                    if (0 < mR)
                    {
                        if (1 < mR)
                        {
                            sb.Append(mR);
                        }
                        sb.Append("R");
                    }

                    if (0 < mB)
                    {
                        if (1 < mB)
                        {
                            sb.Append(mB);
                        }
                        sb.Append("B");
                    }

                    if (0 < mG)
                    {
                        if (1 < mG)
                        {
                            sb.Append(mG);
                        }
                        sb.Append("G");
                    }

                    if (0 < mS)
                    {
                        if (1 < mS)
                        {
                            sb.Append(mS);
                        }
                        sb.Append("S");
                    }

                    if (0 < mN)
                    {
                        if (1 < mN)
                        {
                            sb.Append(mN);
                        }
                        sb.Append("N");
                    }

                    if (0 < mL)
                    {
                        if (1 < mL)
                        {
                            sb.Append(mL);
                        }
                        sb.Append("L");
                    }

                    if (0 < mP)
                    {
                        if (1 < mP)
                        {
                            sb.Append(mP);
                        }
                        sb.Append("P");
                    }

                    if (0 < mk)
                    {
                        if (1 < mk)
                        {
                            sb.Append(mk);
                        }
                        sb.Append("k");
                    }

                    if (0 < mr)
                    {
                        if (1 < mr)
                        {
                            sb.Append(mr);
                        }
                        sb.Append("r");
                    }

                    if (0 < mb)
                    {
                        if (1 < mb)
                        {
                            sb.Append(mb);
                        }
                        sb.Append("b");
                    }

                    if (0 < mg)
                    {
                        if (1 < mg)
                        {
                            sb.Append(mg);
                        }
                        sb.Append("g");
                    }

                    if (0 < ms)
                    {
                        if (1 < ms)
                        {
                            sb.Append(ms);
                        }
                        sb.Append("s");
                    }

                    if (0 < mn)
                    {
                        if (1 < mn)
                        {
                            sb.Append(mn);
                        }
                        sb.Append("n");
                    }

                    if (0 < ml)
                    {
                        if (1 < ml)
                        {
                            sb.Append(ml);
                        }
                        sb.Append("l");
                    }

                    if (0 < mp)
                    {
                        if (1 < mp)
                        {
                            sb.Append(mp);
                        }
                        sb.Append("p");
                    }
                }

            }

            // 手目
            sb.Append(" 1");

            return sb.ToString();
        }

        private void GetMoti(
            out int mK,
            out int mR,
            out int mB,
            out int mG,
            out int mS,
            out int mN,
            out int mL,
            out int mP,

            out int mk,
            out int mr,
            out int mb,
            out int mg,
            out int ms,
            out int mn,
            out int ml,
            out int mp
        )
        {
            mK = 0;
            mR = this.MotiP1[0];
            mB = this.MotiP1[1];
            mG = this.MotiP1[2];
            mS = this.MotiP1[3];
            mN = this.MotiP1[4];
            mL = this.MotiP1[5];
            mP = this.MotiP1[6];

            mk = 0;
            mr = this.MotiP2[0];
            mb = this.MotiP2[1];
            mg = this.MotiP2[2];
            ms = this.MotiP2[3];
            mn = this.MotiP2[4];
            ml = this.MotiP2[5];
            mp = this.MotiP2[6];
        }

    }
}
