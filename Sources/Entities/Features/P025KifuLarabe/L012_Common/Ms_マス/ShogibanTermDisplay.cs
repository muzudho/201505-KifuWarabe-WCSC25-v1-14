using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using System.Collections.Generic;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P025_KifuLarabe.L012_Common
{

    public abstract class ShogibanTermDisplay
    {


        public static string Kamd_ToTerm(Maps_OneAndOne<Finger, SySet<SyElement>> kamd)
        {
            StringBuilder sb = new StringBuilder();
            kamd.Foreach_Entry((Finger key, SySet<SyElement> value, ref bool toBreak) =>
            {
                sb.AppendLine("entry.Key=" + key);
                sb.AppendLine( ShogibanTermDisplay.Masus_ToTerm(value) );
                sb.AppendLine("  ");//空行
            });

            return sb.ToString();
        }


        public static string Masus_ToTerm(SySet<SyElement> masus)
        {
            Dictionary<Basho, int> kaisu = new Dictionary<Basho, int>();
            foreach (Basho masu in masus.Elements)
            {
                if (kaisu.ContainsKey(masu))
                {
                    kaisu[masu] += 1;
                }
                else
                {
                    kaisu.Add(masu, 1);
                }
            }

            Dictionary<Basho, string> kotae = new Dictionary<Basho, string>();
            foreach (Basho masu in Masu_Honshogi.Items_All)
            {
                if (kaisu.ContainsKey(masu))
                {
                    kotae[masu] = kaisu[masu].ToString().PadLeft(2);
                }
                else
                {
                    // データがなければ空マス。
                    kotae.Add(masu, "  ");
                }
            }


            // 次のような表を作ります。
            //
            // [ 0] 後手持ち駒           ９  ８  ７  ６  ５  ４  ３  ２  １     先手持ち駒         駒袋
            // [ 1] ┌─┬─┬─┬─┐ ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐   ┌─┬─┬─┬─┐ ┌─┬─┬─┬─┐
            // [ 2] │  │  │  │  │ │  │  │  │  │  │  │  │  │  │一 │  │  │  │  │ │  │  │  │  │
            // [ 3] ├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤
            // [ 4] │  │  │  │  │ │  │  │  │  │  │  │  │  │  │二 │  │  │  │  │ │  │  │  │  │
            // [ 5] ├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤
            // [ 6] │  │  │  │  │ │  │  │  │  │  │  │  │  │  │三 │  │  │  │  │ │  │  │  │  │
            // [ 7] ├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤
            // [ 8] │  │  │  │  │ │  │  │  │  │  │  │  │  │  │四 │  │  │  │  │ │  │  │  │  │
            // [ 9] ├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤
            // [10] │  │  │  │  │ │  │  │  │  │  │  │  │  │  │五 │  │  │  │  │ │  │  │  │  │
            // [11] ├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤
            // [12] │  │  │  │  │ │  │  │  │  │  │  │  │  │  │六 │  │  │  │  │ │  │  │  │  │
            // [13] ├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤
            // [14] │  │  │  │  │ │  │  │  │  │  │  │  │  │  │七 │  │  │  │  │ │  │  │  │  │
            // [15] ├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤
            // [16] │  │  │  │  │ │  │  │  │  │  │  │  │  │  │八 │  │  │  │  │ │  │  │  │  │
            // [17] ├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤
            // [18] │  │  │  │  │ │  │  │  │  │  │  │  │  │  │九 │  │  │  │  │ │  │  │  │  │
            // [19] └─┴─┴─┴─┘ └─┴─┴─┴─┴─┴─┴─┴─┴─┘   └─┴─┴─┴─┘ └─┴─┴─┴─┘
            // [20] エラー:


            StringBuilder sb = new StringBuilder();
            sb.AppendLine("kaisu.Count=" + kaisu.Count);
            sb.AppendLine("kotae.Count=" + kotae.Count);
            sb.AppendLine("数字は、その升が選ばれている回数。");
            sb.AppendLine("後手持ち駒           ９  ８  ７  ６  ５  ４  ３  ２  １     先手持ち駒         駒袋");
            sb.AppendLine("┌─┬─┬─┬─┐ ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐   ┌─┬─┬─┬─┐ ┌─┬─┬─┬─┐");
            sb.AppendLine("│" + kotae[Masu_Honshogi.go31] + "│" + kotae[Masu_Honshogi.go21] + "│" + kotae[Masu_Honshogi.go11] + "│" + kotae[Masu_Honshogi.go01] + "│ │" + kotae[Masu_Honshogi.ban91_９一] + "│" + kotae[Masu_Honshogi.ban81_８一] + "│" + kotae[Masu_Honshogi.ban71_７一] + "│" + kotae[Masu_Honshogi.ban61_６一] + "│" + kotae[Masu_Honshogi.ban51_５一] + "│" + kotae[Masu_Honshogi.ban41_４一] + "│" + kotae[Masu_Honshogi.ban31_３一] + "│" + kotae[Masu_Honshogi.ban21_２一] + "│" + kotae[Masu_Honshogi.ban11_１一] + "│一 │" + kotae[Masu_Honshogi.sen31] + "│" + kotae[Masu_Honshogi.sen21] + "│" + kotae[Masu_Honshogi.sen11] + "│" + kotae[Masu_Honshogi.sen01] + "│ │" + kotae[Masu_Honshogi.fukuro31] + "│" + kotae[Masu_Honshogi.fukuro21] + "│" + kotae[Masu_Honshogi.fukuro11] + "│" + kotae[Masu_Honshogi.fukuro01] + "│");
            sb.AppendLine("├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤");
            sb.AppendLine("│" + kotae[Masu_Honshogi.go32] + "│" + kotae[Masu_Honshogi.go22] + "│" + kotae[Masu_Honshogi.go12] + "│" + kotae[Masu_Honshogi.go02] + "│ │" + kotae[Masu_Honshogi.ban92_９二] + "│" + kotae[Masu_Honshogi.ban82_８二] + "│" + kotae[Masu_Honshogi.ban72_７二] + "│" + kotae[Masu_Honshogi.ban62_６二] + "│" + kotae[Masu_Honshogi.ban52_５二] + "│" + kotae[Masu_Honshogi.ban42_４二] + "│" + kotae[Masu_Honshogi.ban32_３二] + "│" + kotae[Masu_Honshogi.ban22_２二] + "│" + kotae[Masu_Honshogi.ban12_１二] + "│二 │" + kotae[Masu_Honshogi.sen32] + "│" + kotae[Masu_Honshogi.sen22] + "│" + kotae[Masu_Honshogi.sen12] + "│" + kotae[Masu_Honshogi.sen02] + "│ │" + kotae[Masu_Honshogi.fukuro32] + "│" + kotae[Masu_Honshogi.fukuro22] + "│" + kotae[Masu_Honshogi.fukuro12] + "│" + kotae[Masu_Honshogi.fukuro02] + "│");
            sb.AppendLine("├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤");
            sb.AppendLine("│" + kotae[Masu_Honshogi.go33] + "│" + kotae[Masu_Honshogi.go23] + "│" + kotae[Masu_Honshogi.go13] + "│" + kotae[Masu_Honshogi.go03] + "│ │" + kotae[Masu_Honshogi.ban93_９三] + "│" + kotae[Masu_Honshogi.ban83_８三] + "│" + kotae[Masu_Honshogi.ban73_７三] + "│" + kotae[Masu_Honshogi.ban63_６三] + "│" + kotae[Masu_Honshogi.ban53_５三] + "│" + kotae[Masu_Honshogi.ban43_４三] + "│" + kotae[Masu_Honshogi.ban33_３三] + "│" + kotae[Masu_Honshogi.ban23_２三] + "│" + kotae[Masu_Honshogi.ban13_１三] + "│三 │" + kotae[Masu_Honshogi.sen33] + "│" + kotae[Masu_Honshogi.sen23] + "│" + kotae[Masu_Honshogi.sen13] + "│" + kotae[Masu_Honshogi.sen03] + "│ │" + kotae[Masu_Honshogi.fukuro33] + "│" + kotae[Masu_Honshogi.fukuro23] + "│" + kotae[Masu_Honshogi.fukuro13] + "│" + kotae[Masu_Honshogi.fukuro03] + "│");
            sb.AppendLine("├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤");
            sb.AppendLine("│" + kotae[Masu_Honshogi.go34] + "│" + kotae[Masu_Honshogi.go24] + "│" + kotae[Masu_Honshogi.go14] + "│" + kotae[Masu_Honshogi.go04] + "│ │" + kotae[Masu_Honshogi.ban94_９四] + "│" + kotae[Masu_Honshogi.ban84_８四] + "│" + kotae[Masu_Honshogi.ban74_７四] + "│" + kotae[Masu_Honshogi.ban64_６四] + "│" + kotae[Masu_Honshogi.ban54_５四] + "│" + kotae[Masu_Honshogi.ban44_４四] + "│" + kotae[Masu_Honshogi.ban34_３四] + "│" + kotae[Masu_Honshogi.ban24_２四] + "│" + kotae[Masu_Honshogi.ban14_１四] + "│四 │" + kotae[Masu_Honshogi.sen34] + "│" + kotae[Masu_Honshogi.sen24] + "│" + kotae[Masu_Honshogi.sen14] + "│" + kotae[Masu_Honshogi.sen04] + "│ │" + kotae[Masu_Honshogi.fukuro34] + "│" + kotae[Masu_Honshogi.fukuro24] + "│" + kotae[Masu_Honshogi.fukuro14] + "│" + kotae[Masu_Honshogi.fukuro04] + "│");
            sb.AppendLine("├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤");
            sb.AppendLine("│" + kotae[Masu_Honshogi.go35] + "│" + kotae[Masu_Honshogi.go25] + "│" + kotae[Masu_Honshogi.go15] + "│" + kotae[Masu_Honshogi.go05] + "│ │" + kotae[Masu_Honshogi.ban95_９五] + "│" + kotae[Masu_Honshogi.ban85_８五] + "│" + kotae[Masu_Honshogi.ban75_７五] + "│" + kotae[Masu_Honshogi.ban65_６五] + "│" + kotae[Masu_Honshogi.ban55_５五] + "│" + kotae[Masu_Honshogi.ban45_４五] + "│" + kotae[Masu_Honshogi.ban35_３五] + "│" + kotae[Masu_Honshogi.ban25_２五] + "│" + kotae[Masu_Honshogi.ban15_１五] + "│五 │" + kotae[Masu_Honshogi.sen35] + "│" + kotae[Masu_Honshogi.sen25] + "│" + kotae[Masu_Honshogi.sen15] + "│" + kotae[Masu_Honshogi.sen05] + "│ │" + kotae[Masu_Honshogi.fukuro35] + "│" + kotae[Masu_Honshogi.fukuro25] + "│" + kotae[Masu_Honshogi.fukuro15] + "│" + kotae[Masu_Honshogi.fukuro05] + "│");
            sb.AppendLine("├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤");
            sb.AppendLine("│" + kotae[Masu_Honshogi.go36] + "│" + kotae[Masu_Honshogi.go26] + "│" + kotae[Masu_Honshogi.go16] + "│" + kotae[Masu_Honshogi.go06] + "│ │" + kotae[Masu_Honshogi.ban96_９六] + "│" + kotae[Masu_Honshogi.ban86_８六] + "│" + kotae[Masu_Honshogi.ban76_７六] + "│" + kotae[Masu_Honshogi.ban66_６六] + "│" + kotae[Masu_Honshogi.ban56_５六] + "│" + kotae[Masu_Honshogi.ban46_４六] + "│" + kotae[Masu_Honshogi.ban36_３六] + "│" + kotae[Masu_Honshogi.ban26_２六] + "│" + kotae[Masu_Honshogi.ban16_１六] + "│六 │" + kotae[Masu_Honshogi.sen36] + "│" + kotae[Masu_Honshogi.sen26] + "│" + kotae[Masu_Honshogi.sen16] + "│" + kotae[Masu_Honshogi.sen06] + "│ │" + kotae[Masu_Honshogi.fukuro36] + "│" + kotae[Masu_Honshogi.fukuro26] + "│" + kotae[Masu_Honshogi.fukuro16] + "│" + kotae[Masu_Honshogi.fukuro06] + "│");
            sb.AppendLine("├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤");
            sb.AppendLine("│" + kotae[Masu_Honshogi.go37] + "│" + kotae[Masu_Honshogi.go27] + "│" + kotae[Masu_Honshogi.go17] + "│" + kotae[Masu_Honshogi.go07] + "│ │" + kotae[Masu_Honshogi.ban97_９七] + "│" + kotae[Masu_Honshogi.ban87_８七] + "│" + kotae[Masu_Honshogi.ban77_７七] + "│" + kotae[Masu_Honshogi.ban67_６七] + "│" + kotae[Masu_Honshogi.ban57_５七] + "│" + kotae[Masu_Honshogi.ban47_４七] + "│" + kotae[Masu_Honshogi.ban37_３七] + "│" + kotae[Masu_Honshogi.ban27_２七] + "│" + kotae[Masu_Honshogi.ban17_１七] + "│七 │" + kotae[Masu_Honshogi.sen37] + "│" + kotae[Masu_Honshogi.sen27] + "│" + kotae[Masu_Honshogi.sen17] + "│" + kotae[Masu_Honshogi.sen07] + "│ │" + kotae[Masu_Honshogi.fukuro37] + "│" + kotae[Masu_Honshogi.fukuro27] + "│" + kotae[Masu_Honshogi.fukuro17] + "│" + kotae[Masu_Honshogi.fukuro07] + "│");
            sb.AppendLine("├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤");
            sb.AppendLine("│" + kotae[Masu_Honshogi.go38] + "│" + kotae[Masu_Honshogi.go28] + "│" + kotae[Masu_Honshogi.go18] + "│" + kotae[Masu_Honshogi.go08] + "│ │" + kotae[Masu_Honshogi.ban98_９八] + "│" + kotae[Masu_Honshogi.ban88_８八] + "│" + kotae[Masu_Honshogi.ban78_７八] + "│" + kotae[Masu_Honshogi.ban68_６八] + "│" + kotae[Masu_Honshogi.ban58_５八] + "│" + kotae[Masu_Honshogi.ban48_４八] + "│" + kotae[Masu_Honshogi.ban38_３八] + "│" + kotae[Masu_Honshogi.ban28_２八] + "│" + kotae[Masu_Honshogi.ban18_１八] + "│八 │" + kotae[Masu_Honshogi.sen38] + "│" + kotae[Masu_Honshogi.sen28] + "│" + kotae[Masu_Honshogi.sen18] + "│" + kotae[Masu_Honshogi.sen08] + "│ │" + kotae[Masu_Honshogi.fukuro38] + "│" + kotae[Masu_Honshogi.fukuro28] + "│" + kotae[Masu_Honshogi.fukuro18] + "│" + kotae[Masu_Honshogi.fukuro08] + "│");
            sb.AppendLine("├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤");
            sb.AppendLine("│" + kotae[Masu_Honshogi.go39] + "│" + kotae[Masu_Honshogi.go29] + "│" + kotae[Masu_Honshogi.go19] + "│" + kotae[Masu_Honshogi.go09] + "│ │" + kotae[Masu_Honshogi.ban99_９九] + "│" + kotae[Masu_Honshogi.ban89_８九] + "│" + kotae[Masu_Honshogi.ban79_７九] + "│" + kotae[Masu_Honshogi.ban69_６九] + "│" + kotae[Masu_Honshogi.ban59_５九] + "│" + kotae[Masu_Honshogi.ban49_４九] + "│" + kotae[Masu_Honshogi.ban39_３九] + "│" + kotae[Masu_Honshogi.ban29_２九] + "│" + kotae[Masu_Honshogi.ban19_１九] + "│九 │" + kotae[Masu_Honshogi.sen39] + "│" + kotae[Masu_Honshogi.sen29] + "│" + kotae[Masu_Honshogi.sen19] + "│" + kotae[Masu_Honshogi.sen09] + "│ │" + kotae[Masu_Honshogi.fukuro39] + "│" + kotae[Masu_Honshogi.fukuro29] + "│" + kotae[Masu_Honshogi.fukuro19] + "│" + kotae[Masu_Honshogi.fukuro09] + "│");
            sb.AppendLine("├─┼─┼─┼─┤ └─┴─┴─┴─┴─┴─┴─┴─┴─┘   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤");
            sb.AppendLine("│" + kotae[Masu_Honshogi.go40] + "│" + kotae[Masu_Honshogi.go30] + "│" + kotae[Masu_Honshogi.go20] + "│" + kotae[Masu_Honshogi.go10] + "│                                          │" + kotae[Masu_Honshogi.sen40] + "│" + kotae[Masu_Honshogi.sen30] + "│" + kotae[Masu_Honshogi.sen20] + "│" + kotae[Masu_Honshogi.sen10] + "│ │" + kotae[Masu_Honshogi.fukuro40] + "│" + kotae[Masu_Honshogi.fukuro30] + "│" + kotae[Masu_Honshogi.fukuro20] + "│" + kotae[Masu_Honshogi.fukuro10] + "│");
            sb.AppendLine("└─┴─┴─┴─┘                                          └─┴─┴─┴─┘ └─┴─┴─┴─┘");
            sb.AppendLine("エラー:");

            return sb.ToString();
        }


        //public static string[] Masus_ToTerm(Masus<SyElement> masus)
        //{
        //    List<M201> masuList = masus.Elements.ToList();


        //    // 次のような表を作ります。
        //    //
        //    // [ 0] 後手持ち駒           ９  ８  ７  ６  ５  ４  ３  ２  １     先手持ち駒         駒袋
        //    // [ 1] ┌─┬─┬─┬─┐ ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐   ┌─┬─┬─┬─┐ ┌─┬─┬─┬─┐
        //    // [ 2] │  │  │  │  │ │  │  │  │  │  │  │  │  │  │一 │  │  │  │  │ │  │  │  │  │
        //    // [ 3] ├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤
        //    // [ 4] │  │  │  │  │ │  │  │  │  │  │  │  │  │  │二 │  │  │  │  │ │  │  │  │  │
        //    // [ 5] ├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤
        //    // [ 6] │  │  │  │  │ │  │  │  │  │  │  │  │  │  │三 │  │  │  │  │ │  │  │  │  │
        //    // [ 7] ├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤
        //    // [ 8] │  │  │  │  │ │  │  │  │  │  │  │  │  │  │四 │  │  │  │  │ │  │  │  │  │
        //    // [ 9] ├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤
        //    // [10] │  │  │  │  │ │  │  │  │  │  │  │  │  │  │五 │  │  │  │  │ │  │  │  │  │
        //    // [11] ├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤
        //    // [12] │  │  │  │  │ │  │  │  │  │  │  │  │  │  │六 │  │  │  │  │ │  │  │  │  │
        //    // [13] ├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤
        //    // [14] │  │  │  │  │ │  │  │  │  │  │  │  │  │  │七 │  │  │  │  │ │  │  │  │  │
        //    // [15] ├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤
        //    // [16] │  │  │  │  │ │  │  │  │  │  │  │  │  │  │八 │  │  │  │  │ │  │  │  │  │
        //    // [17] ├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤
        //    // [18] │  │  │  │  │ │  │  │  │  │  │  │  │  │  │九 │  │  │  │  │ │  │  │  │  │
        //    // [19] └─┴─┴─┴─┘ └─┴─┴─┴─┴─┴─┴─┴─┴─┘   └─┴─┴─┴─┘ └─┴─┴─┴─┘
        //    // [20] エラー:



        //    string[] stringArray = new string[]{
        //        "後手持ち駒           ９  ８  ７  ６  ５  ４  ３  ２  １     先手持ち駒         駒袋",//[ 0]
        //        "┌─┬─┬─┬─┐ ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐   ┌─┬─┬─┬─┐ ┌─┬─┬─┬─┐",//[ 1]
        //        "│  │  │  │  │ │  │  │  │  │  │  │  │  │  │一 │  │  │  │  │ │  │  │  │  │",//[ 2]
        //        "├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤",//[ 3]
        //        "│  │  │  │  │ │  │  │  │  │  │  │  │  │  │二 │  │  │  │  │ │  │  │  │  │",//[ 4]
        //        "├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤",//[ 5]
        //        "│  │  │  │  │ │  │  │  │  │  │  │  │  │  │三 │  │  │  │  │ │  │  │  │  │",//[ 6]
        //        "├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤",//[ 7]
        //        "│  │  │  │  │ │  │  │  │  │  │  │  │  │  │四 │  │  │  │  │ │  │  │  │  │",//[ 8]
        //        "├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤",//[ 9]
        //        "│  │  │  │  │ │  │  │  │  │  │  │  │  │  │五 │  │  │  │  │ │  │  │  │  │",//[10]
        //        "├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤",//[11]
        //        "│  │  │  │  │ │  │  │  │  │  │  │  │  │  │六 │  │  │  │  │ │  │  │  │  │",//[12]
        //        "├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤",//[13]
        //        "│  │  │  │  │ │  │  │  │  │  │  │  │  │  │七 │  │  │  │  │ │  │  │  │  │",//[14]
        //        "├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤",//[15]
        //        "│  │  │  │  │ │  │  │  │  │  │  │  │  │  │八 │  │  │  │  │ │  │  │  │  │",//[16]
        //        "├─┼─┼─┼─┤ ├─┼─┼─┼─┼─┼─┼─┼─┼─┤   ├─┼─┼─┼─┤ ├─┼─┼─┼─┤",//[17]
        //        "│  │  │  │  │ │  │  │  │  │  │  │  │  │  │九 │  │  │  │  │ │  │  │  │  │",//[18]
        //        "└─┴─┴─┴─┘ └─┴─┴─┴─┴─┴─┴─┴─┴─┘   └─┴─┴─┴─┘ └─┴─┴─┴─┘",//[19]
        //        "エラー:",//[20]
        //    };

        //    return stringArray;
        //}

    }

}
