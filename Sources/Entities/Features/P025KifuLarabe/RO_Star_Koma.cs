namespace Grayscale.Kifuwarazusa.Entities.Features
{

    /// <summary>
    /// 駒種類による指定。
    /// </summary>
    public class RO_Star_Koma : Starlightable
    {
        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 現・駒の向き
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Playerside Pside { get { return this.pside; } }
        protected Playerside pside;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 現・マス
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public SyElement Masu { get { return this.masu; } }
        protected SyElement masu;


        /// <summary>
        /// 駒種類１４
        /// </summary>
        public Ks14 Syurui { get { return this.syurui; } }
        private Ks14 syurui;

        public Kh185 Haiyaku
        {
            get
            {
                return Data_HaiyakuTransition.ToHaiyaku(this.Syurui, this.Masu, this.Pside);
            }
        }


        public RO_Star_Koma(Playerside pside, SyElement masu, Ks14 syurui)
        {
            this.pside = pside;
            this.masu = masu;
            this.syurui = syurui;
        }

        public RO_Star_Koma(Playerside pside, SyElement masu, Kh185 haiyaku)
        {
            this.pside = pside;
            this.masu = masu;
            this.syurui = Haiyaku184Array.Syurui(haiyaku);
        }

        public RO_Star_Koma(RO_Star_Koma src)
        {
            this.pside = src.Pside;
            this.masu = src.Masu;
            this.syurui = src.Syurui;
        }

        /// <summary>
        /// 不成ケース
        /// </summary>
        /// <returns></returns>
        public Ks14 ToNarazuCase()
        {
            return KomaSyurui14Array.NarazuCaseHandle(Haiyaku184Array.Syurui(this.Haiyaku));
        }

        /// <summary>
        /// 駒の表面の文字。
        /// </summary>
        public string Text_Label
        {
            get
            {
                string result;

                result = KomaSyurui14Array.Ichimoji[(int)Haiyaku184Array.Syurui(this.Haiyaku)];

                return result;
            }
        }


    }
}
