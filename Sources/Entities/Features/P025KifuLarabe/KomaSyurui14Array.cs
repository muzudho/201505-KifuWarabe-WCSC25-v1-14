﻿namespace Grayscale.Kifuwarazusa.Entities.Features
{


    public abstract class KomaSyurui14Array
    {

        #region 静的プロパティー類

        /// <summary>
        /// 外字、後手
        /// </summary>
        public static char[] GaijiGote { get { return KomaSyurui14Array.gaijiGote; } }
        protected static char[] gaijiGote;

        /// <summary>
        /// 外字、先手
        /// </summary>
        public static char[] GaijiSente { get { return KomaSyurui14Array.gaijiSente; } }
        protected static char[] gaijiSente;


        public static string[] NimojiSente { get { return KomaSyurui14Array.nimojiSente; } }
        protected static string[] nimojiSente;


        public static string[] NimojiGote { get { return KomaSyurui14Array.nimojiGote; } }
        protected static string[] nimojiGote;


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 駒の表示文字。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public static string[] Ichimoji { get { return KomaSyurui14Array.ichimoji; } }
        protected static string[] ichimoji;


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 駒の符号用の単語。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public static string[] Fugo { get { return KomaSyurui14Array.fugo; } }
        protected static string[] fugo;


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 駒のSFEN符号用の単語。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public static string[] Sfen1P { get { return KomaSyurui14Array.sfen1P; } }
        protected static string[] sfen1P;

        public static string[] Sfen2P { get { return KomaSyurui14Array.sfen2P; } }
        protected static string[] sfen2P;


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 駒のSFEN(打)符号用の単語。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public static string[] SfenDa { get { return KomaSyurui14Array.sfenDa; } }
        protected static string[] sfenDa;


        /// <summary>
        /// ************************************************************************************************************************
        /// 成れる駒
        /// ************************************************************************************************************************
        /// </summary>
        public static bool[] FlagNareruKoma { get { return KomaSyurui14Array.flagNareruKoma; } }
        protected static bool[] flagNareruKoma;


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 駒が成らなかったときの駒ハンドル
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public static PieceType NarazuCaseHandle(PieceType syurui)
        {
            return KomaSyurui14Array.narazuCaseHandle[(int)syurui];
        }
        protected static PieceType[] narazuCaseHandle;


        /// <summary>
        /// ************************************************************************************************************************
        /// 成り駒なら真。
        /// ************************************************************************************************************************
        /// </summary>
        /// <returns></returns>
        public static bool[] FlagNari { get { return KomaSyurui14Array.flagNari; } }
        protected static bool[] flagNari;

        public static bool IsNari(PieceType syurui)
        {
            return KomaSyurui14Array.FlagNari[(int)syurui];
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 駒が成ったときの駒ハンドル
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public static PieceType[] NariCaseHandle { get { return KomaSyurui14Array.nariCaseHandle; } }
        protected static PieceType[] nariCaseHandle;

        public static PieceType ToNariCase(PieceType syurui)
        {
            return KomaSyurui14Array.NariCaseHandle[(int)syurui];
        }

        static KomaSyurui14Array()
        {

            KomaSyurui14Array.nariCaseHandle = new PieceType[]{
                PieceType.None,//[0]ヌル
                PieceType.PP,
                PieceType.PL,
                PieceType.PN,
                PieceType.PS,
                PieceType.G,
                PieceType.K,
                PieceType.PR,
                PieceType.PB,
                PieceType.PR,
                PieceType.PB,
                PieceType.PP,
                PieceType.PL,
                PieceType.PN,
                PieceType.PS,
            };

            KomaSyurui14Array.flagNari = new bool[]{
                false,//[0]ヌル
                false,//[1]歩
                false,//[2]香
                false,//[3]桂
                false,//[4]銀
                false,//[5]金
                false,//[6]王
                false,//[7]飛車
                false,//[8]角
                true,//[9]竜
                true,//[10]馬
                true,//[11]と
                true,//[12]杏
                true,//[13]圭
                true,//[14]全
                false,//[15]エラー
            };

            KomaSyurui14Array.narazuCaseHandle = new PieceType[]{
                PieceType.None,//[0]ヌル
                PieceType.P,//[1]歩
                PieceType.L,//[2]香
                PieceType.N,//[3]桂
                PieceType.S,//[4]銀
                PieceType.G,//[5]金
                PieceType.K,//[6]王
                PieceType.R,//[7]飛車
                PieceType.B,//[8]角
                PieceType.R,//[9]竜→飛車
                PieceType.B,//[10]馬→角
                PieceType.P,//[11]と→歩
                PieceType.L,//[12]杏→香
                PieceType.N,//[13]圭→桂
                PieceType.S,//[14]全→銀
            };

            KomaSyurui14Array.flagNareruKoma = new bool[]{
                false,//[0]ヌル
                true,//[1]歩
                true,//[2]香
                true,//[3]桂
                true,//[4]銀
                false,//[5]金
                false,//[6]王
                true,//[7]飛
                true,//[8]角
                false,//[9]竜
                false,//[10]馬
                false,//[11]と
                false,//[12]杏
                false,//[13]圭
                false,//[14]全
                false,//[15]エラー
            };

            KomaSyurui14Array.sfenDa = new string[]{
                "×",//[0]ヌル
                "P",//[1]
                "L",
                "N",
                "S",
                "G",
                "K",
                "R",
                "B",
                "R",
                "B",
                "P",
                "L",
                "N",
                "S",
                "＜打×Ｕ＞",//[15]
            };

            KomaSyurui14Array.sfen1P = new string[]{
                "×",//[0]ヌル
                "P",
                "L",
                "N",
                "S",
                "G",
                "K",
                "R",
                "B",
                "+R",
                "+B",
                "+P",
                "+L",
                "+N",
                "+S",
                "Ｕ×SFEN",
            };

            KomaSyurui14Array.sfen2P = new string[]{
                "×",//[0]ヌル
                "p",
                "l",
                "n",
                "s",
                "g",
                "k",
                "r",
                "b",
                "+r",
                "+b",
                "+p",
                "+l",
                "+n",
                "+s",
                "Ｕ×sfen",
            };

            KomaSyurui14Array.fugo = new string[]{
                "×",//[0]ヌル
                "歩",
                "香",
                "桂",
                "銀",
                "金",
                "王",
                "飛",
                "角",
                "竜",
                "馬",
                "と",
                "成香",
                "成桂",
                "成銀",
                "Ｕ×符",
            };

            KomaSyurui14Array.ichimoji = new string[]{
                "×",//[0]ヌル
                "歩",
                "香",
                "桂",
                "銀",
                "金",
                "王",
                "飛",
                "角",
                "竜",
                "馬",
                "と",
                "杏",
                "圭",
                "全",
                "Ｕ×",
            };

            KomaSyurui14Array.nimojiGote = new string[]{
                "△×",//[0]ヌル
                "△歩",
                "△香",
                "△桂",
                "△銀",
                "△金",
                "△王",
                "△飛",
                "△角",
                "△竜",
                "△馬",
                "△と",
                "△杏",
                "△圭",
                "△全",
                "△×",
            };



            KomaSyurui14Array.nimojiSente = new string[]{
                "▲×",//[0]ヌル
                "▲歩",
                "▲香",
                "▲桂",
                "▲銀",
                "▲金",
                "▲王",
                "▲飛",
                "▲角",
                "▲竜",
                "▲馬",
                "▲と",
                "▲杏",
                "▲圭",
                "▲全",
                "▲×",
            };

            KomaSyurui14Array.gaijiSente = new char[]{
                'ｘ',//[0]ヌル
                '歩',
                '香',
                '桂',
                '銀',
                '金',
                '王',
                '飛',
                '角',
                '竜',
                '馬',
                'と',
                '杏',
                '圭',
                '全',
                'ｘ',
            };

            //逆さ歩（外字）
            KomaSyurui14Array.gaijiGote = new char[]{
                'ｘ',//[0]ヌル
                '',//逆さ歩（外字）
                '',//逆さ香（外字）
                '',//逆さ桂（外字）
                '',//逆さ銀（外字）
                '',//逆さ金（外字）
                '',//逆さ王（外字）
                '',//逆さ飛（外字）
                '',//逆さ角（外字）
                '',//逆さ竜（外字）
                '',//逆さ馬（外字）
                '',//逆さと（外字）
                '',//逆さ杏（外字）
                '',//逆さ圭（外字）
                '',//逆さ全（外字）
                'Ｘ',//[15]エラー
            };
        }

        #endregion



        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 外字を利用した表示文字。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public static char ToGaiji(PieceType koma, Playerside pside)
        {
            char result;

            switch (pside)
            {
                case Playerside.P2:
                    result = KomaSyurui14Array.GaijiGote[(int)koma];
                    break;
                case Playerside.P1:
                    result = KomaSyurui14Array.GaijiSente[(int)koma];
                    break;
                default:
                    result = '×';
                    break;
            }

            return result;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 「歩」といった、外字を利用しない表示文字。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public static string ToIchimoji(PieceType koma)
        {
            return KomaSyurui14Array.Ichimoji[(int)koma];
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 「▲歩」といった、外字を利用しない表示文字。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public static string ToNimoji(PieceType koma, Playerside pside)
        {
            string result;

            switch (pside)
            {
                case Playerside.P2:
                    result = KomaSyurui14Array.NimojiGote[(int)koma];
                    break;
                case Playerside.P1:
                    result = KomaSyurui14Array.NimojiSente[(int)koma];
                    break;
                default:
                    result = "××";
                    break;
            }

            return result;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 駒のSFEN符号用の単語。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public static string SfenText(PieceType komaSyurui, Playerside pside)
        {
            string str;

            if (Playerside.P1 == pside)
            {
                str = KomaSyurui14Array.Sfen1P[(int)komaSyurui];
            }
            else
            {
                str = KomaSyurui14Array.Sfen2P[(int)komaSyurui];
            }

            return str;
        }


        public static bool Matches(PieceType koma1, PieceType koma2)
        {
            return (int)koma2 == (int)koma1;
        }

    }


}
