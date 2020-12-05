using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P006_Syugoron;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号


namespace Grayscale.P025_KifuLarabe.L004_StructShogi
{
    /// <summary>
    /// 将棋盤上の情報を数えます。
    /// </summary>
    public class StartposExporter
    {

        /// <summary>
        /// 盤上の駒。
        /// </summary>
        public Dictionary<int, RO_Star_Koma> BanObject201;//Masu



        /// <summary>
        /// 先後。
        /// </summary>
        public bool PsideIsBlack { get{return psideIsBlack;} } private bool psideIsBlack;

        /// <summary>
        /// ▲王
        /// </summary>
        public int Moti1K { get { return moti1K; } } private int moti1K;


        /// <summary>
        /// ▲飛
        /// </summary>
        public int Moti1R { get { return moti1R; } } private int moti1R;


        /// <summary>
        /// ▲角
        /// </summary>
        public int Moti1B { get { return moti1B; } } private int moti1B;


        /// <summary>
        /// ▲金
        /// </summary>
        public int Moti1G { get { return moti1G; } } private int moti1G;


        /// <summary>
        /// ▲銀
        /// </summary>
        public int Moti1S { get { return moti1S; } } private int moti1S;


        /// <summary>
        /// ▲桂
        /// </summary>
        public int Moti1N { get { return moti1N; } } private int moti1N;


        /// <summary>
        /// ▲香
        /// </summary>
        public int Moti1L { get { return moti1L; } } private int moti1L;


        /// <summary>
        /// ▲歩
        /// </summary>
        public int Moti1P { get { return moti1P; } } private int moti1P;


        /// <summary>
        /// △王
        /// </summary>
        public int Moti2k { get { return moti2k; } } private int moti2k;


        /// <summary>
        /// △飛
        /// </summary>
        public int Moti2r { get { return moti2r; } } private int moti2r;


        /// <summary>
        /// △角
        /// </summary>
        public int Moti2b { get { return moti2b; } } private int moti2b;


        /// <summary>
        /// △金
        /// </summary>
        public int Moti2g { get { return moti2g; } } private int moti2g;


        /// <summary>
        /// △銀
        /// </summary>
        public int Moti2s { get { return moti2s; } } private int moti2s;


        /// <summary>
        /// △桂
        /// </summary>
        public int Moti2n { get { return moti2n; } } private int moti2n;


        /// <summary>
        /// △香
        /// </summary>
        public int Moti2l { get { return moti2l; } } private int moti2l;


        /// <summary>
        /// △歩
        /// </summary>
        public int Moti2p { get { return moti2p; } } private int moti2p;


        /// <summary>
        /// 駒袋 王
        /// </summary>
        public int FukuroK { get { return fukuroK; } } private int fukuroK;

        /// <summary>
        /// 駒袋 飛
        /// </summary>
        public int FukuroR { get { return fukuroR; } } private int fukuroR;

        /// <summary>
        /// 駒袋 角
        /// </summary>
        public int FukuroB { get { return fukuroB; } } private int fukuroB;

        /// <summary>
        /// 駒袋 金
        /// </summary>
        public int FukuroG { get { return fukuroG; } } private int fukuroG;

        /// <summary>
        /// 駒袋 銀
        /// </summary>
        public int FukuroS { get { return fukuroS; } } private int fukuroS;

        /// <summary>
        /// 駒袋 桂
        /// </summary>
        public int FukuroN { get { return fukuroN; } } private int fukuroN;

        /// <summary>
        /// 駒袋 香
        /// </summary>
        public int FukuroL { get { return fukuroL; } } private int fukuroL;

        /// <summary>
        /// 駒袋 歩
        /// </summary>
        public int FukuroP { get { return fukuroP; } } private int fukuroP;



        public StartposExporter(SkyConst src_Sky)
        {
            Debug.Assert(src_Sky.Count == 40, "sourceSky.Starlights.Count=[" + src_Sky.Count + "]");//将棋の駒の数

            this.BanObject201 = new Dictionary<int, RO_Star_Koma>();//Masu

            this.ToBanObject201(src_Sky);
        }



        private void ToBanObject201(SkyConst src_Sky)
        {
            this.psideIsBlack = src_Sky.PsideIsBlack;// TODO:

            //Util_Sky.Assert_Honshogi(src_Sky);


            // 将棋の駒４０個の場所を確認します。
            foreach (Finger finger in src_Sky.Fingers_All().Items)
            {
                Starlightable light = src_Sky.StarlightIndexOf(finger).Now;
                RO_Star_Koma komaKs = Util_Koma.AsKoma(light);

                Debug.Assert(Util_MasuNum.OnAll(Util_Masu.AsMasuNumber(komaKs.Masu)), "(int)koma.Masu=[" + Util_Masu.AsMasuNumber(komaKs.Masu) + "]");//升番号

                this.AddKoma(komaKs.Masu,
                    new RO_Star_Koma(komaKs)
                );
            }
        }



        private void AddKoma(SyElement masu, RO_Star_Koma koma)// Ks14 komaSyurui
        {

            Debug.Assert(!this.BanObject201.ContainsKey(Util_Masu.AsMasuNumber(masu)), "既に駒がある枡に、駒を置こうとしています。[" + Util_Masu.AsMasuNumber(masu) + "]");


            this.BanObject201.Add(Util_Masu.AsMasuNumber(masu), koma);

            if (Util_MasuNum.OnShogiban(Util_Masu.AsMasuNumber(masu)))
            {
                // 盤上

                // 特にカウントはなし
            }
            else if (Util_MasuNum.OnSenteKomadai(Util_Masu.AsMasuNumber(masu)))
            {
                // 先手駒台
                switch (koma.Syurui)
                {
                    case Ks14.H01_Fu:
                        this.moti1P++;
                        break;
                    case Ks14.H02_Kyo:
                        this.moti1L++;
                        break;
                    case Ks14.H03_Kei:
                        this.moti1N++;
                        break;
                    case Ks14.H04_Gin:
                        this.moti1S++;
                        break;
                    case Ks14.H05_Kin:
                        this.moti1G++;
                        break;
                    case Ks14.H06_Oh:
                        this.moti1K++;
                        break;
                    case Ks14.H07_Hisya:
                        this.moti1R++;
                        break;
                    case Ks14.H08_Kaku:
                        this.moti1B++;
                        break;
                }
            }
            else if (Util_MasuNum.OnGoteKomadai(Util_Masu.AsMasuNumber(masu)))
            {
                // 後手駒台
                switch (koma.Syurui)
                {
                    case Ks14.H01_Fu:
                        this.moti2p++;
                        break;
                    case Ks14.H02_Kyo:
                        this.moti2l++;
                        break;
                    case Ks14.H03_Kei:
                        this.moti2n++;
                        break;
                    case Ks14.H04_Gin:
                        this.moti2s++;
                        break;
                    case Ks14.H05_Kin:
                        this.moti2g++;
                        break;
                    case Ks14.H06_Oh:
                        this.moti2k++;
                        break;
                    case Ks14.H07_Hisya:
                        this.moti2r++;
                        break;
                    case Ks14.H08_Kaku:
                        this.moti2b++;
                        break;
                }
            }
            else
            {
                // 駒袋
                switch (koma.Syurui)
                {
                    case Ks14.H01_Fu:
                        this.fukuroP++;
                        break;
                    case Ks14.H02_Kyo:
                        this.fukuroL++;
                        break;
                    case Ks14.H03_Kei:
                        this.fukuroN++;
                        break;
                    case Ks14.H04_Gin:
                        this.fukuroS++;
                        break;
                    case Ks14.H05_Kin:
                        this.fukuroG++;
                        break;
                    case Ks14.H06_Oh:
                        this.fukuroK++;
                        break;
                    case Ks14.H07_Hisya:
                        this.fukuroR++;
                        break;
                    case Ks14.H08_Kaku:
                        this.fukuroB++;
                        break;
                }
            }
        }


        public string CreateDanString(int leftestMasu)
        {
            StringBuilder sb = new StringBuilder();

            List<RO_Star_Koma> list = new List<RO_Star_Koma>();
            for (int i = leftestMasu; i >= 0; i -= 9)
            {
                Basho masu = new Basho(i);

                if (this.BanObject201.ContainsKey((int)masu.MasuNumber))
                {
                    list.Add(this.BanObject201[(int)masu.MasuNumber]);
                }
                else
                {
                    list.Add(null);
                }
            }

            int spaceCount = 0;
            foreach (RO_Star_Koma koma in list)
            {
                if (koma == null)
                {
                    spaceCount++;
                }
                else
                {
                    if (0 < spaceCount)
                    {
                        sb.Append(spaceCount.ToString());
                        spaceCount = 0;
                    }

                    // 駒の種類だけだと先手ゴマになってしまう。先後も判定した。
                    switch(koma.Pside)
                    {
                        case Playerside.P1:
                            sb.Append(KomaSyurui14Array.Sfen1P[(int)koma.Syurui]);
                            break;
                        case Playerside.P2:
                            sb.Append(KomaSyurui14Array.Sfen2P[(int)koma.Syurui]);
                            break;
                        default:
                            throw new Exception("ない手番");
                    }
                }
            }
            if (0 < spaceCount)
            {
                sb.Append(spaceCount.ToString());
                spaceCount = 0;
            }

            return sb.ToString();
        }


    }
}
