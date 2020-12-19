using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P200_KifuNarabe.L00048_ShogiGui;
using Grayscale.P006_Syugoron;
using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.Kifuwarazusa.Entities.Logging;

namespace Grayscale.P200_KifuNarabe.L025_Macro
{
    public abstract class Util_Menace
    {
        /// <summary>
        /// v(^▽^)v超能力『メナス』だぜ☆ 未来の脅威を予測し、可視化するぜ☆ｗｗｗ
        /// </summary>
        public static void Menace( ShogiGui shogiGui, ILogTag logTag)
        {
            if (0 < shogiGui.Model_PnlTaikyoku.GuiTesumi)
            {
                // 処理の順序が悪く、初回はうまく判定できない。
                SkyConst src_Sky = shogiGui.Model_PnlTaikyoku.GuiSkyConst;


                //----------
                // 将棋盤上の駒
                //----------
                shogiGui.ResponseData.ToRedraw();

                // [クリアー]
                shogiGui.Shape_PnlTaikyoku.Shogiban.ClearHMasu_KikiKomaList();

                // 全駒
                foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)
                {
                    RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(figKoma).Now);


                    if (
                        Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(koma.Masu)
                        &&
                        shogiGui.Model_PnlTaikyoku.GuiPside != koma.Pside
                        )
                    {
                        // 駒の利き
                        SySet<SyElement> kikiZukei = Util_Sky.KomaKidou_Potential(figKoma, src_Sky);

                        IEnumerable<SyElement> kikiMasuList = kikiZukei.Elements;
                        foreach (SyElement masu in kikiMasuList)
                        {
                            // その枡に利いている駒のハンドルを追加
                            if (Masu_Honshogi.Error != masu)
                            {
                                shogiGui.Shape_PnlTaikyoku.Shogiban.HMasu_KikiKomaList[Util_Masu.AsMasuNumber(masu)].Add((int)figKoma);
                            }
                        }
                    }
                }
            }
        }

    }
}
