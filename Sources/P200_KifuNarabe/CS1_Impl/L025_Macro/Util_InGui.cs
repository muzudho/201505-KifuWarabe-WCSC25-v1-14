
//スプライト番号


using Grayscale.P025_KifuLarabe;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P200_KifuNarabe.L00048_ShogiGui;
using Grayscale.P200_KifuNarabe.L008_TextBoxListener;
//スプライト番号
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

using System.Drawing;
using Grayscale.P200_KifuNarabe.L015_Sprite;
using Grayscale.P200_KifuNarabe.L00006_Shape;

using System.Collections.Generic;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;

namespace Grayscale.P200_KifuNarabe.L025_Macro
{


    public class Util_InGui
    {

        /// <summary>
        /// ************************************************************************************************************************
        /// [巻戻し]ボタン
        /// ************************************************************************************************************************
        /// </summary>
        public static bool Makimodosi_Gui(
            ShogiGui shogiGui,
            Finger movedKoma,
            Finger foodKoma,
            string fugoJStr,
            string backedInputText,
            LarabeLoggerable logTag)
        {
            //------------------------------
            // チェンジターン
            //------------------------------
            shogiGui.ChangeTurn(logTag);//[巻戻し]ボタンを押したあと


            //------------------------------
            // 符号表示
            //------------------------------
            shogiGui.Shape_PnlTaikyoku.SetFugo(fugoJStr);


            Shape_BtnKoma btn_movedKoma = Util_InGui.FingerToKomaBtn(movedKoma, shogiGui);
            Shape_BtnKoma btn_foodKoma = Util_InGui.FingerToKomaBtn(foodKoma, shogiGui);//取られた駒
            //------------------------------------------------------------
            // 駒・再描画
            //------------------------------------------------------------
            if (
                null != btn_movedKoma//動かした駒
                ||
                null != btn_foodKoma//取ったときに下にあった駒（巻戻しのときは、これは無し）
                )
            {
                shogiGui.ResponseData.RedrawStarlights();// 駒の再描画要求
            }

            // 巻き戻したので、符号が入ります。
            {
                shogiGui.ResponseData.InputTextString = fugoJStr + " " + backedInputText;// 入力欄
                shogiGui.ResponseData.OutputTxt = ResponseGedanTxt.Kifu;
            }
            shogiGui.ResponseData.ToRedraw();

            return true;
        }




        public static bool Komaokuri_Gui( string restText, ShogiGui shogiGui, LarabeLoggerable logTag)
        {
            //------------------------------
            // チェンジ・ターン
            //------------------------------
            if (shogiGui.ResponseData.ChangedTurn)
            {
                shogiGui.ChangeTurn( logTag);
            }


            //------------------------------
            // 符号表示
            //------------------------------
            {
                Node<IMove, KyokumenWrapper> node6 = shogiGui.Model_PnlTaikyoku.Kifu.CurNode;

                RO_Star_Koma koma = Util_Koma.AsKoma(((IMove)node6.Key).MoveSource);

                FugoJ fugoJ = JFugoCreator15Array.ItemMethods[(int)Haiyaku184Array.Syurui(koma.Haiyaku)](node6.Key, new KyokumenWrapper(shogiGui.Model_PnlTaikyoku.GuiSkyConst), logTag);//「▲２二角成」なら、馬（dst）ではなくて角（src）。

                string fugoJStr = fugoJ.ToText_UseDou(node6);
                shogiGui.Shape_PnlTaikyoku.SetFugo(fugoJStr);
            }



            shogiGui.ResponseData.RedrawStarlights();// 再描画1

            shogiGui.ResponseData.InputTextString = restText;//追加
            shogiGui.ResponseData.ToRedraw(); // GUIに通知するだけ。


            return true;
        }



        /// <summary>
        /// ************************************************************************************************************************
        /// テキストボックスに入力された、符号の読込み
        /// ************************************************************************************************************************
        /// </summary>
        public static string ReadLine_FromTextbox()
        {
            return TextboxListener.DefaultInstance.ReadLine1().Trim();
        }






        public static void Komamove1a_49Gui(
            out Ks14 toSyurui,
            out IMoveHalf dst,
            Shape_BtnKoma btnKoma_Selected,
            Shape_BtnMasu btnMasu,
            ShogiGui shogiGui
        )
        {
            // 駒の種類
            if (shogiGui.Shape_PnlTaikyoku.Naru)
            {
                // 成ります

                toSyurui = KomaSyurui14Array.NariCaseHandle[(int)Haiyaku184Array.Syurui(Util_Koma.AsKoma(shogiGui.Shape_PnlTaikyoku.MouseStarlightOrNull2.MoveSource).Haiyaku)];
                shogiGui.Shape_PnlTaikyoku.SetNaruFlag(false);
            }
            else
            {
                // そのまま
                toSyurui = Haiyaku184Array.Syurui(Util_Koma.AsKoma(shogiGui.Shape_PnlTaikyoku.MouseStarlightOrNull2.MoveSource).Haiyaku);
            }


            // 置く駒
            {
                dst = new RO_MotionlessStarlight(
                    //btnKoma_Selected.Finger,
                    new RO_Star_Koma(
                        Util_Koma.AsKoma(shogiGui.Model_PnlTaikyoku.GuiSkyConst.StarlightIndexOf(btnKoma_Selected.Finger).MoveSource).Pside,
                        btnMasu.Zahyo,
                        toSyurui
                        )
                );
            }


            //------------------------------------------------------------
            // 「取った駒種類_巻戻し用」をクリアーします。
            //------------------------------------------------------------
            shogiGui.Shape_PnlTaikyoku.MousePos_FoodKoma = null;

        }





        /// <summary>
        /// 取った駒がある場合のみ。
        /// </summary>
        /// <param name="koma_Food_after"></param>
        /// <param name="shogiGui"></param>
        public static void Komamove1a_51Gui(
            bool torareruKomaAri,
            RO_Star_Koma koma_Food_after,
            ShogiGui shogiGui
        )
        {
            if (torareruKomaAri)
            {
                //------------------------------
                // 「取った駒種類_巻戻し用」を棋譜に覚えさせます。（差替え）
                //------------------------------
                shogiGui.Shape_PnlTaikyoku.MousePos_FoodKoma = koma_Food_after;//2014-10-19 21:04 追加
            }

            shogiGui.ResponseData.RedrawStarlights();
        }




        /// <summary>
        /// ************************************************************************************************************************
        /// 局面に合わせて、駒ボタンのx,y位置を変更します
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="btnKoma">駒</param>
        public static void Redraw_KomaLocation(
            Finger figKoma,
            ShogiGui shogiGui,
            LarabeLoggerable logTag
            )
        {
            RO_Star_Koma koma = Util_Koma.AsKoma(shogiGui.Model_PnlTaikyoku.GuiSkyConst.StarlightIndexOf(figKoma).MoveSource);

            Shape_BtnKoma btnKoma = Util_InGui.FingerToKomaBtn(figKoma, shogiGui);

            int suji;
            int dan;
            Util_MasuNum.MasuToSuji(koma.Masu, out suji);
            Util_MasuNum.MasuToDan(koma.Masu, out dan);

            switch (Util_Masu.GetOkiba(koma.Masu))
            {
                case Okiba.ShogiBan:
                    btnKoma.SetBounds(new Rectangle(
                        shogiGui.Shape_PnlTaikyoku.Shogiban.SujiToX(suji),
                        shogiGui.Shape_PnlTaikyoku.Shogiban.DanToY(dan),
                        btnKoma.Bounds.Width,
                        btnKoma.Bounds.Height
                        ));
                    break;

                case Okiba.Sente_Komadai:
                    btnKoma.SetBounds(new Rectangle(
                        shogiGui.Shape_PnlTaikyoku.KomadaiArr[0].SujiToX(suji),
                        shogiGui.Shape_PnlTaikyoku.KomadaiArr[0].DanToY(dan),
                        btnKoma.Bounds.Width,
                        btnKoma.Bounds.Height
                        ));
                    break;

                case Okiba.Gote_Komadai:
                    btnKoma.SetBounds(new Rectangle(
                        shogiGui.Shape_PnlTaikyoku.KomadaiArr[1].SujiToX(suji),
                        shogiGui.Shape_PnlTaikyoku.KomadaiArr[1].DanToY(dan),
                        btnKoma.Bounds.Width,
                        btnKoma.Bounds.Height
                        ));
                    break;

                case Okiba.KomaBukuro:
                    btnKoma.SetBounds(new Rectangle(
                        shogiGui.Shape_PnlTaikyoku.KomadaiArr[2].SujiToX(suji),
                        shogiGui.Shape_PnlTaikyoku.KomadaiArr[2].DanToY(dan),
                        btnKoma.Bounds.Width,
                        btnKoma.Bounds.Height
                        ));
                    break;
            }
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 駒のハンドル(*1)を元に、ボタンを返します。
        /// ************************************************************************************************************************
        /// 
        ///     *1…将棋の駒１つ１つに付けられた番号です。
        /// 
        /// </summary>
        /// <param name="hKomas"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <returns></returns>
        public static List<Shape_BtnKoma> HKomasToBtns(List<int> hKomas, ShogiGui shogiGui)
        {
            List<Shape_BtnKoma> btns = new List<Shape_BtnKoma>();

            foreach (int handle in hKomas)
            {
                Shape_BtnKoma btn = shogiGui.Shape_PnlTaikyoku.BtnKomaDoors[handle];
                if (null != btn)
                {
                    btns.Add(btn);
                }
            }

            return btns;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 駒のハンドル(*1)を元に、ボタンを返します。
        /// ************************************************************************************************************************
        /// 
        ///     *1…将棋の駒１つ１つに付けられた番号です。
        /// 
        /// </summary>
        /// <param name="hKoma"></param>
        /// <param name="shape_PnlTaikyoku"></param>
        /// <returns>なければヌル</returns>
        public static Shape_BtnKoma FingerToKomaBtn(Finger koma, ShogiGui shogiGui)
        {
            Shape_BtnKoma found = null;

            int hKoma = (int)koma;

            if (0 <= hKoma && hKoma < shogiGui.Shape_PnlTaikyoku.BtnKomaDoors.Length)
            {
                found = shogiGui.Shape_PnlTaikyoku.BtnKomaDoors[hKoma];
            }

            return found;
        }

    }


}
