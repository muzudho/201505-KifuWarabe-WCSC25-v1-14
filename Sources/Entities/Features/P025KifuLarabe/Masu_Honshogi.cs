namespace Grayscale.Kifuwarazusa.Entities.Features
{
    /// <summary>
    /// 駒を置ける場所２０１箇所だぜ☆
    /// 
    /// 将棋盤０～８０。（計８１マス）
    /// 先手駒台８１～１２０。（計４０マス）
    /// 後手駒台１２１～１６０。（計４０マス）
    /// 駒袋１６１～２００。（計４０マス）
    /// エラー２０１。
    /// 
    /// int型にキャストして使うんだぜ☆
    /// </summary>
    public class Masu_Honshogi
    {
        /// <summary>
        /// 本将棋の有効駒数は40個。
        /// </summary>
        public const int HONSHOGI_KOMAS = 40;

        /// <summary>
        /// 列挙型の要素を、配列に格納しておきます。
        /// 
        /// int型→列挙型　への変換を可能にします。
        /// </summary>
        public static SyElement[] Items_All
        {
            get
            {
                return Masu_Honshogi.items_All;
            }
        }
        private static SyElement[] items_All;


        public static SyElement[] Items_81
        {
            get
            {
                return Masu_Honshogi.items_81;
            }
        }
        private static SyElement[] items_81;


        static Masu_Honshogi()
        {
            Masu_Honshogi.items_All = new SyElement[]{
                ban11_１一,
                ban12_１二,
                ban13_１三,
                ban14_１四,
                ban15_１五,
                ban16_１六,
                ban17_１七,
                ban18_１八,
                ban19_１九,

                ban21_２一,//[9]
                ban22_２二,
                ban23_２三,
                ban24_２四,
                ban25_２五,
                ban26_２六,
                ban27_２七,
                ban28_２八,
                ban29_２九,

                ban31_３一,//[18]
                ban32_３二,
                ban33_３三,
                ban34_３四,
                ban35_３五,
                ban36_３六,
                ban37_３七,
                ban38_３八,
                ban39_３九,

                ban41_４一,//[27]
                ban42_４二,
                ban43_４三,
                ban44_４四,
                ban45_４五,
                ban46_４六,
                ban47_４七,
                ban48_４八,
                ban49_４九,

                ban51_５一,//[36]
                ban52_５二,
                ban53_５三,
                ban54_５四,
                ban55_５五,
                ban56_５六,
                ban57_５七,
                ban58_５八,
                ban59_５九,

                ban61_６一,//[45]
                ban62_６二,
                ban63_６三,
                ban64_６四,
                ban65_６五,
                ban66_６六,
                ban67_６七,
                ban68_６八,
                ban69_６九,

                ban71_７一,//[54]
                ban72_７二,
                ban73_７三,
                ban74_７四,
                ban75_７五,
                ban76_７六,
                ban77_７七,
                ban78_７八,
                ban79_７九,

                ban81_８一,//[63]
                ban82_８二,
                ban83_８三,
                ban84_８四,
                ban85_８五,
                ban86_８六,
                ban87_８七,
                ban88_８八,
                ban89_８九,

                ban91_９一,//[72]
                ban92_９二,
                ban93_９三,
                ban94_９四,
                ban95_９五,
                ban96_９六,
                ban97_９七,
                ban98_９八,
                ban99_９九,//[80]

                //先手駒台
                sen01,//[81]
                sen02,
                sen03,
                sen04,
                sen05,
                sen06,
                sen07,
                sen08,
                sen09,
                sen10,
                sen11,
                sen12,
                sen13,
                sen14,
                sen15,
                sen16,
                sen17,
                sen18,
                sen19,
                sen20,
                sen21,
                sen22,
                sen23,
                sen24,
                sen25,
                sen26,
                sen27,
                sen28,
                sen29,
                sen30,
                sen31,
                sen32,
                sen33,
                sen34,
                sen35,
                sen36,
                sen37,
                sen38,
                sen39,
                sen40,

                //後手駒台
                go01,//[121]
                go02,
                go03,
                go04,
                go05,
                go06,
                go07,
                go08,
                go09,
                go10,
                go11,
                go12,
                go13,
                go14,
                go15,
                go16,
                go17,
                go18,
                go19,
                go20,
                go21,
                go22,
                go23,
                go24,
                go25,
                go26,
                go27,
                go28,
                go29,
                go30,
                go31,
                go32,
                go33,
                go34,
                go35,
                go36,
                go37,
                go38,
                go39,
                go40,

                //駒袋
                fukuro01,//[161]
                fukuro02,
                fukuro03,
                fukuro04,
                fukuro05,
                fukuro06,
                fukuro07,
                fukuro08,
                fukuro09,
                fukuro10,
                fukuro11,
                fukuro12,
                fukuro13,
                fukuro14,
                fukuro15,
                fukuro16,
                fukuro17,
                fukuro18,
                fukuro19,
                fukuro20,
                fukuro21,
                fukuro22,
                fukuro23,
                fukuro24,
                fukuro25,
                fukuro26,
                fukuro27,
                fukuro28,
                fukuro29,
                fukuro30,
                fukuro31,
                fukuro32,
                fukuro33,
                fukuro34,
                fukuro35,
                fukuro36,
                fukuro37,
                fukuro38,
                fukuro39,
                fukuro40,

                Error//[201]

            };


            Masu_Honshogi.items_81 = new SyElement[81];
            for (int i = 0; i < 81; i++)
            {
                Masu_Honshogi.items_81[i] = Masu_Honshogi.items_All[i];
            }
        }


        public static readonly Basho ban11_１一 = new Basho(0);
        public static readonly Basho ban12_１二 = new Basho(1);
        public static readonly Basho ban13_１三 = new Basho(2);
        public static readonly Basho ban14_１四 = new Basho(3);
        public static readonly Basho ban15_１五 = new Basho(4);
        public static readonly Basho ban16_１六 = new Basho(5);
        public static readonly Basho ban17_１七 = new Basho(6);
        public static readonly Basho ban18_１八 = new Basho(7);
        public static readonly Basho ban19_１九 = new Basho(8);

        public static readonly Basho ban21_２一 = new Basho(9);
        public static readonly Basho ban22_２二 = new Basho(10);
        public static readonly Basho ban23_２三 = new Basho(11);
        public static readonly Basho ban24_２四 = new Basho(12);
        public static readonly Basho ban25_２五 = new Basho(13);
        public static readonly Basho ban26_２六 = new Basho(14);
        public static readonly Basho ban27_２七 = new Basho(15);
        public static readonly Basho ban28_２八 = new Basho(16);
        public static readonly Basho ban29_２九 = new Basho(17);

        public static readonly Basho ban31_３一 = new Basho(18);
        public static readonly Basho ban32_３二 = new Basho(19);
        public static readonly Basho ban33_３三 = new Basho(20);
        public static readonly Basho ban34_３四 = new Basho(21);
        public static readonly Basho ban35_３五 = new Basho(22);
        public static readonly Basho ban36_３六 = new Basho(23);
        public static readonly Basho ban37_３七 = new Basho(24);
        public static readonly Basho ban38_３八 = new Basho(25);
        public static readonly Basho ban39_３九 = new Basho(26);

        public static readonly Basho ban41_４一 = new Basho(27);
        public static readonly Basho ban42_４二 = new Basho(28);
        public static readonly Basho ban43_４三 = new Basho(29);
        public static readonly Basho ban44_４四 = new Basho(30);
        public static readonly Basho ban45_４五 = new Basho(31);
        public static readonly Basho ban46_４六 = new Basho(32);
        public static readonly Basho ban47_４七 = new Basho(33);
        public static readonly Basho ban48_４八 = new Basho(34);
        public static readonly Basho ban49_４九 = new Basho(35);

        public static readonly Basho ban51_５一 = new Basho(36);
        public static readonly Basho ban52_５二 = new Basho(37);
        public static readonly Basho ban53_５三 = new Basho(38);
        public static readonly Basho ban54_５四 = new Basho(39);
        public static readonly Basho ban55_５五 = new Basho(40);
        public static readonly Basho ban56_５六 = new Basho(41);
        public static readonly Basho ban57_５七 = new Basho(42);
        public static readonly Basho ban58_５八 = new Basho(43);
        public static readonly Basho ban59_５九 = new Basho(44);

        public static readonly Basho ban61_６一 = new Basho(45);
        public static readonly Basho ban62_６二 = new Basho(46);
        public static readonly Basho ban63_６三 = new Basho(47);
        public static readonly Basho ban64_６四 = new Basho(48);
        public static readonly Basho ban65_６五 = new Basho(49);
        public static readonly Basho ban66_６六 = new Basho(50);
        public static readonly Basho ban67_６七 = new Basho(51);
        public static readonly Basho ban68_６八 = new Basho(52);
        public static readonly Basho ban69_６九 = new Basho(53);

        public static readonly Basho ban71_７一 = new Basho(54);
        public static readonly Basho ban72_７二 = new Basho(55);
        public static readonly Basho ban73_７三 = new Basho(56);
        public static readonly Basho ban74_７四 = new Basho(57);
        public static readonly Basho ban75_７五 = new Basho(58);
        public static readonly Basho ban76_７六 = new Basho(59);
        public static readonly Basho ban77_７七 = new Basho(60);
        public static readonly Basho ban78_７八 = new Basho(61);
        public static readonly Basho ban79_７九 = new Basho(62);

        public static readonly Basho ban81_８一 = new Basho(63);//[63]
        public static readonly Basho ban82_８二 = new Basho(64);
        public static readonly Basho ban83_８三 = new Basho(65);
        public static readonly Basho ban84_８四 = new Basho(66);
        public static readonly Basho ban85_８五 = new Basho(67);
        public static readonly Basho ban86_８六 = new Basho(68);
        public static readonly Basho ban87_８七 = new Basho(69);
        public static readonly Basho ban88_８八 = new Basho(70);
        public static readonly Basho ban89_８九 = new Basho(71);

        public static readonly Basho ban91_９一 = new Basho(72);//[72]
        public static readonly Basho ban92_９二 = new Basho(73);
        public static readonly Basho ban93_９三 = new Basho(74);
        public static readonly Basho ban94_９四 = new Basho(75);
        public static readonly Basho ban95_９五 = new Basho(76);
        public static readonly Basho ban96_９六 = new Basho(77);
        public static readonly Basho ban97_９七 = new Basho(78);
        public static readonly Basho ban98_９八 = new Basho(79);
        public static readonly Basho ban99_９九 = new Basho(80);//[80]

        //先手駒台
        public static readonly Basho sen01 = new Basho(81);//[81]
        public static readonly Basho sen02 = new Basho(82);
        public static readonly Basho sen03 = new Basho(83);
        public static readonly Basho sen04 = new Basho(84);
        public static readonly Basho sen05 = new Basho(85);
        public static readonly Basho sen06 = new Basho(86);
        public static readonly Basho sen07 = new Basho(87);
        public static readonly Basho sen08 = new Basho(88);
        public static readonly Basho sen09 = new Basho(89);
        public static readonly Basho sen10 = new Basho(90);
        public static readonly Basho sen11 = new Basho(91);
        public static readonly Basho sen12 = new Basho(92);
        public static readonly Basho sen13 = new Basho(93);
        public static readonly Basho sen14 = new Basho(94);
        public static readonly Basho sen15 = new Basho(95);
        public static readonly Basho sen16 = new Basho(96);
        public static readonly Basho sen17 = new Basho(97);
        public static readonly Basho sen18 = new Basho(98);
        public static readonly Basho sen19 = new Basho(99);
        public static readonly Basho sen20 = new Basho(100);
        public static readonly Basho sen21 = new Basho(101);
        public static readonly Basho sen22 = new Basho(102);
        public static readonly Basho sen23 = new Basho(103);
        public static readonly Basho sen24 = new Basho(104);
        public static readonly Basho sen25 = new Basho(105);
        public static readonly Basho sen26 = new Basho(106);
        public static readonly Basho sen27 = new Basho(107);
        public static readonly Basho sen28 = new Basho(108);
        public static readonly Basho sen29 = new Basho(109);
        public static readonly Basho sen30 = new Basho(110);
        public static readonly Basho sen31 = new Basho(111);
        public static readonly Basho sen32 = new Basho(112);
        public static readonly Basho sen33 = new Basho(113);
        public static readonly Basho sen34 = new Basho(114);
        public static readonly Basho sen35 = new Basho(115);
        public static readonly Basho sen36 = new Basho(116);
        public static readonly Basho sen37 = new Basho(117);
        public static readonly Basho sen38 = new Basho(118);
        public static readonly Basho sen39 = new Basho(119);
        public static readonly Basho sen40 = new Basho(120);

        //後手駒台
        public static readonly Basho go01 = new Basho(121);//[121]
        public static readonly Basho go02 = new Basho(122);
        public static readonly Basho go03 = new Basho(123);
        public static readonly Basho go04 = new Basho(124);
        public static readonly Basho go05 = new Basho(125);
        public static readonly Basho go06 = new Basho(126);
        public static readonly Basho go07 = new Basho(127);
        public static readonly Basho go08 = new Basho(128);
        public static readonly Basho go09 = new Basho(129);
        public static readonly Basho go10 = new Basho(130);
        public static readonly Basho go11 = new Basho(131);
        public static readonly Basho go12 = new Basho(132);
        public static readonly Basho go13 = new Basho(133);
        public static readonly Basho go14 = new Basho(134);
        public static readonly Basho go15 = new Basho(135);
        public static readonly Basho go16 = new Basho(136);
        public static readonly Basho go17 = new Basho(137);
        public static readonly Basho go18 = new Basho(138);
        public static readonly Basho go19 = new Basho(139);
        public static readonly Basho go20 = new Basho(140);
        public static readonly Basho go21 = new Basho(141);
        public static readonly Basho go22 = new Basho(142);
        public static readonly Basho go23 = new Basho(143);
        public static readonly Basho go24 = new Basho(144);
        public static readonly Basho go25 = new Basho(145);
        public static readonly Basho go26 = new Basho(146);
        public static readonly Basho go27 = new Basho(147);
        public static readonly Basho go28 = new Basho(148);
        public static readonly Basho go29 = new Basho(149);
        public static readonly Basho go30 = new Basho(150);
        public static readonly Basho go31 = new Basho(151);
        public static readonly Basho go32 = new Basho(152);
        public static readonly Basho go33 = new Basho(153);
        public static readonly Basho go34 = new Basho(154);
        public static readonly Basho go35 = new Basho(155);
        public static readonly Basho go36 = new Basho(156);
        public static readonly Basho go37 = new Basho(157);
        public static readonly Basho go38 = new Basho(158);
        public static readonly Basho go39 = new Basho(159);
        public static readonly Basho go40 = new Basho(160);

        //駒袋
        public static readonly Basho fukuro01 = new Basho(161);//[161]
        public static readonly Basho fukuro02 = new Basho(162);
        public static readonly Basho fukuro03 = new Basho(163);
        public static readonly Basho fukuro04 = new Basho(164);
        public static readonly Basho fukuro05 = new Basho(165);
        public static readonly Basho fukuro06 = new Basho(166);
        public static readonly Basho fukuro07 = new Basho(167);
        public static readonly Basho fukuro08 = new Basho(168);
        public static readonly Basho fukuro09 = new Basho(169);
        public static readonly Basho fukuro10 = new Basho(170);
        public static readonly Basho fukuro11 = new Basho(171);
        public static readonly Basho fukuro12 = new Basho(172);
        public static readonly Basho fukuro13 = new Basho(173);
        public static readonly Basho fukuro14 = new Basho(174);
        public static readonly Basho fukuro15 = new Basho(175);
        public static readonly Basho fukuro16 = new Basho(176);
        public static readonly Basho fukuro17 = new Basho(177);
        public static readonly Basho fukuro18 = new Basho(178);
        public static readonly Basho fukuro19 = new Basho(179);
        public static readonly Basho fukuro20 = new Basho(180);
        public static readonly Basho fukuro21 = new Basho(181);
        public static readonly Basho fukuro22 = new Basho(182);
        public static readonly Basho fukuro23 = new Basho(183);
        public static readonly Basho fukuro24 = new Basho(184);
        public static readonly Basho fukuro25 = new Basho(185);
        public static readonly Basho fukuro26 = new Basho(186);
        public static readonly Basho fukuro27 = new Basho(187);
        public static readonly Basho fukuro28 = new Basho(188);
        public static readonly Basho fukuro29 = new Basho(189);
        public static readonly Basho fukuro30 = new Basho(190);
        public static readonly Basho fukuro31 = new Basho(191);
        public static readonly Basho fukuro32 = new Basho(192);
        public static readonly Basho fukuro33 = new Basho(193);
        public static readonly Basho fukuro34 = new Basho(194);
        public static readonly Basho fukuro35 = new Basho(195);
        public static readonly Basho fukuro36 = new Basho(196);
        public static readonly Basho fukuro37 = new Basho(197);
        public static readonly Basho fukuro38 = new Basho(198);
        public static readonly Basho fukuro39 = new Basho(199);
        public static readonly Basho fukuro40 = new Basho(200);

        public static readonly Basho Error = new Basho(201);//[201]


    }
}
