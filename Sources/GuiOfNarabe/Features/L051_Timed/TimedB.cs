using System.Collections.Generic;
using System.Drawing;
using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Gui;
using Grayscale.P006_Syugoron;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P100_ShogiServer.L100_InServer;
using Grayscale.P200_KifuNarabe.L00006_Shape;
using Grayscale.P200_KifuNarabe.L00047_Scene;
using Grayscale.P200_KifuNarabe.L00048_ShogiGui;
using Grayscale.P200_KifuNarabe.L015_Sprite;
using Grayscale.P200_KifuNarabe.L025_Macro;
using Grayscale.P200_KifuNarabe.L050_Scene;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P200_KifuNarabe.L051_Timed
{
    /// <summary>
    /// マウス操作の一連の流れです。（主に１手指す動き）
    /// </summary>
    public class TimedB : TimedAbstract
    {
        private NarabeRoomViewModel shogiGui;

        /// <summary>
        /// マウス操作の状態です。
        /// </summary>
        public Queue<MouseEventState> MouseEventQueue { get; set; }

        public static void Check_MouseoverKomaKiki(object obj_shogiGui, Finger finger)
        {
            NarabeRoomViewModel shogiGui = (NarabeRoomViewModel)obj_shogiGui;

            Starlight light = shogiGui.GameViewModel.GuiSkyConst.StarlightIndexOf(finger);
            shogiGui.Shape_PnlTaikyoku.Shogiban.KikiBan = new SySet_Default<SyElement>("利き盤");// .Clear();

            // 駒の利き
            SySet<SyElement> kikiZukei = Util_Sky.KomaKidou_Potential(finger, shogiGui.GameViewModel.GuiSkyConst);
            //kikiZukei.DebugWrite("駒の利きLv1");

            // 味方の駒
            Node<ShootingStarlightable, KyokumenWrapper> siteiNode = KifuNarabe_KifuWrapper.CurNode(shogiGui.GameViewModel.Kifu);
            SySet<SyElement> mikataZukei = Util_Sky.Masus_Now(siteiNode.Value.ToKyokumenConst, Util_InServer.CurPside(shogiGui));
            //mikataZukei.DebugWrite("味方の駒");

            // 駒の利き上に駒がないか。
            SySet<SyElement> ban2 = kikiZukei.Minus_Closed(mikataZukei);
            //kikiZukei.DebugWrite("駒の利きLv2");

            shogiGui.Shape_PnlTaikyoku.Shogiban.KikiBan = ban2;

        }



        public TimedB(NarabeRoomViewModel shogiGui)
        {
            this.shogiGui = shogiGui;
            this.MouseEventQueue = new Queue<MouseEventState>();
        }


        public override void Step()
        {
            // 入っているマウス操作イベントのうち、マウスムーブは　１つに　集約　します。
            bool bMouseMove_SceneB_1TumamitaiKoma = false;

            // 入っているマウス操作イベントは、全部捨てていきます。
            MouseEventState[] queue = this.MouseEventQueue.ToArray();
            this.MouseEventQueue.Clear();
            foreach (MouseEventState eventState in queue)
            {
                switch (this.shogiGui.Scene)
                {
                    case SceneName.SceneB_1TumamitaiKoma:
                        {
                            #region つまみたい駒


                            switch (eventState.Name2)
                            {
                                case MouseEventStateName.Arive:
                                    {
                                        #region アライブ
                                        //------------------------------
                                        // メナス
                                        //------------------------------
                                        Util_Menace.Menace(this.shogiGui);
                                        #endregion
                                    }
                                    break;

                                case MouseEventStateName.MouseMove:
                                    {
                                        #region マウスムーブ
                                        if (bMouseMove_SceneB_1TumamitaiKoma)
                                        {
                                            continue;
                                        }
                                        bMouseMove_SceneB_1TumamitaiKoma = true;

                                        SkyConst src_Sky = shogiGui.GameViewModel.GuiSkyConst;

                                        Point mouse = eventState.MouseLocation;

                                        //----------
                                        // 将棋盤：升目
                                        //----------
                                        foreach (UserWidget widget in shogiGui.Shape_PnlTaikyoku.Widgets.Values)
                                        {
                                            if ("Masu" == widget.Type && Okiba.ShogiBan == widget.Okiba)
                                            {
                                                Shape_BtnMasuImpl cell = (Shape_BtnMasuImpl)widget.Object;
                                                cell.LightByMouse(mouse.X, mouse.Y);
                                                if (cell.Light)
                                                {
                                                    shogiGui.ResponseData.ToRedraw();
                                                }
                                                break;
                                            }
                                        }

                                        //----------
                                        // 駒置き、駒袋：升目
                                        //----------
                                        foreach (UserWidget widget in shogiGui.Shape_PnlTaikyoku.Widgets.Values)
                                        {
                                            if ("Masu" == widget.Type && widget.Okiba.HasFlag(Okiba.Sente_Komadai | Okiba.Gote_Komadai | Okiba.KomaBukuro))
                                            {
                                                Shape_BtnMasuImpl cell = (Shape_BtnMasuImpl)widget.Object;
                                                cell.LightByMouse(mouse.X, mouse.Y);
                                                if (cell.Light)
                                                {
                                                    shogiGui.ResponseData.ToRedraw();
                                                }
                                            }
                                        }

                                        //----------
                                        // 駒
                                        //----------
                                        foreach (Shape_BtnKomaImpl btnKoma in shogiGui.Shape_PnlTaikyoku.BtnKomaDoors)
                                        {
                                            btnKoma.LightByMouse(mouse.X, mouse.Y);
                                            if (btnKoma.Light)
                                            {
                                                shogiGui.ResponseData.ToRedraw();

                                                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(btnKoma.Koma).Now);

                                                if (Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(koma.Masu))
                                                {
                                                    // マウスオーバーした駒の利き
                                                    TimedB.Check_MouseoverKomaKiki(shogiGui, btnKoma.Koma);


                                                    break;
                                                }
                                            }
                                        }


                                        foreach (UserWidget widget in shogiGui.Shape_PnlTaikyoku.Widgets.Values)
                                        {
                                            if (widget.IsLight_OnFlowB_1TumamitaiKoma)
                                            {
                                                widget.LightByMouse(mouse.X, mouse.Y);
                                                if (widget.Light) { shogiGui.ResponseData.ToRedraw(); }
                                            }
                                        }
                                        #endregion
                                    }
                                    break;

                                case MouseEventStateName.MouseLeftButtonDown:
                                    {
                                        #region マウス左ボタンダウン
                                        SceneName nextPhaseB = SceneName.Ignore;
                                        SkyConst src_Sky = shogiGui.GameViewModel.GuiSkyConst;

                                        //----------
                                        // 駒
                                        //----------
                                        foreach (Shape_BtnKomaImpl btnKoma in shogiGui.Shape_PnlTaikyoku.BtnKomaDoors)
                                        {
                                            if (btnKoma.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))
                                            {
                                                //>>>>>>>>>> 駒にヒットしました。

                                                if (null != shogiGui.Shape_PnlTaikyoku.Btn_TumandeiruKoma(shogiGui))
                                                {
                                                    //>>>>>>>>>> 既に選択されている駒があります。→★成ろうとしたときの、取られる相手の駒かも。
                                                    goto gt_Next1;
                                                }

                                                // 既に選択されている駒には無効
                                                if (shogiGui.Shape_PnlTaikyoku.FigTumandeiruKoma == (int)btnKoma.Koma)
                                                {
                                                    goto gt_Next1;
                                                }



                                                if (btnKoma.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y)) //>>>>> 駒をつまみました。
                                                {
                                                    // 駒をつまみます。
                                                    shogiGui.Shape_PnlTaikyoku.SetFigTumandeiruKoma((int)btnKoma.Koma);
                                                    shogiGui.Shape_PnlTaikyoku.SelectFirstTouch = true;

                                                    nextPhaseB = SceneName.SceneB_2OkuKoma;

                                                    shogiGui.Shape_PnlTaikyoku.SetMouseStarlightOrNull2(
                                                        src_Sky.StarlightIndexOf(btnKoma.Koma)//TODO:改造
                                                        );

                                                    shogiGui.Shape_PnlTaikyoku.SetHMovedKoma(Fingers.Error_1);
                                                    shogiGui.ResponseData.ToRedraw();
                                                }
                                            }

                                        gt_Next1:
                                            ;
                                        }


                                        //----------
                                        // 既に選択されている駒
                                        //----------
                                        Shape_BtnKoma btnKoma_Selected = shogiGui.Shape_PnlTaikyoku.Btn_TumandeiruKoma(shogiGui);



                                        //----------
                                        // 各種ボタン
                                        //----------
                                        {
                                            foreach (UserWidget widget in shogiGui.Shape_PnlTaikyoku.Widgets.Values)
                                            {
                                                if (widget.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))
                                                {
                                                    if (null != widget.Delegate_MouseHitEvent)
                                                    {
                                                        widget.Delegate_MouseHitEvent(
                                                            shogiGui
                                                           , btnKoma_Selected
                                                           );
                                                    }
                                                }
                                            }
                                        }


                                        shogiGui.SetScene(nextPhaseB);

                                        //------------------------------
                                        // このメインパネルの反応
                                        //------------------------------
                                        shogiGui.Response("MouseOperation");
                                        #endregion
                                    }
                                    break;

                                case MouseEventStateName.MouseLeftButtonUp:
                                    {
                                        #region マウス左ボタンアップ
                                        SkyConst src_Sky = shogiGui.GameViewModel.GuiSkyConst;

                                        //----------
                                        // 将棋盤：升目
                                        //----------
                                        foreach (UserWidget widget in shogiGui.Shape_PnlTaikyoku.Widgets.Values)
                                        {
                                            if ("Masu" == widget.Type && Okiba.ShogiBan == widget.Okiba)
                                            {
                                                Shape_BtnMasuImpl cell = (Shape_BtnMasuImpl)widget.Object;
                                                if (cell.DeselectByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))
                                                {
                                                    shogiGui.ResponseData.ToRedraw();
                                                }
                                            }
                                        }

                                        //----------
                                        // 駒置き、駒袋：升目
                                        //----------
                                        foreach (UserWidget widget in shogiGui.Shape_PnlTaikyoku.Widgets.Values)
                                        {
                                            if ("Masu" == widget.Type && widget.Okiba.HasFlag(Okiba.Sente_Komadai | Okiba.Gote_Komadai | Okiba.KomaBukuro))
                                            {
                                                Shape_BtnMasuImpl cell = (Shape_BtnMasuImpl)widget.Object;
                                                if (cell.DeselectByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))
                                                {
                                                    shogiGui.ResponseData.ToRedraw();
                                                }
                                            }
                                        }

                                        //----------
                                        // 駒
                                        //----------
                                        foreach (Shape_BtnKomaImpl btnKoma in shogiGui.Shape_PnlTaikyoku.BtnKomaDoors)
                                        {
                                            if (btnKoma.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))
                                            {
                                                //>>>>> つまんでいる駒から、指を放しました
                                                shogiGui.Shape_PnlTaikyoku.SetFigTumandeiruKoma(-1);


                                                RO_Star_Koma koma1 = Util_Koma.AsKoma(src_Sky.StarlightIndexOf((int)btnKoma.Koma).Now);


                                                if (Okiba.ShogiBan == Util_Masu.GetOkiba(koma1.Masu))
                                                {
                                                    //----------
                                                    // 移動済表示
                                                    //----------
                                                    shogiGui.Shape_PnlTaikyoku.SetHMovedKoma(btnKoma.Koma);

                                                    //------------------------------
                                                    // 棋譜に符号を追加（マウスボタンが放されたとき）TODO:まだ早い。駒が成るかもしれない。
                                                    //------------------------------
                                                    // 棋譜

                                                    Starlight dstStarlight = shogiGui.Shape_PnlTaikyoku.MouseStarlightOrNull2;
                                                    System.Diagnostics.Debug.Assert(null != dstStarlight, "mouseStarlightがヌル");

                                                    Starlight srcStarlight = src_Sky.StarlightIndexOf(btnKoma.Koma);
                                                    System.Diagnostics.Debug.Assert(null != srcStarlight, "komaStarlightがヌル");


                                                    ShootingStarlightable move = new RO_ShootingStarlight(
                                                        //btnKoma.Koma,
                                                        dstStarlight.Now,
                                                        srcStarlight.Now,
                                                        shogiGui.Shape_PnlTaikyoku.MousePos_FoodKoma != null ? shogiGui.Shape_PnlTaikyoku.MousePos_FoodKoma.Syurui : Ks14.H00_Null
                                                        );// 選択している駒の元の場所と、移動先


                                                    // TODO: 一手[巻戻し]のときは追加したくない
                                                    Node<ShootingStarlightable, KyokumenWrapper> newNode = new KifuNodeImpl(move,
                                                                                        new KyokumenWrapper(new SkyConst(src_Sky)),
                                                                                        KifuNodeImpl.GetReverseTebanside( ((KifuNode)shogiGui.GameViewModel.Kifu.CurNode).Tebanside)
                                                                                        );


                                                    //マウスの左ボタンを放したときです。
                                                    ((KifuNode)KifuNarabe_KifuWrapper.CurNode(shogiGui.GameViewModel.Kifu)).AppendChildA_New(newNode);
                                                    Util_InServer.SetCurNode_Srv(shogiGui, newNode);
                                                    shogiGui.ResponseData.ToRedraw();


                                                    //------------------------------
                                                    // 符号表示
                                                    //------------------------------
                                                    FugoJ fugoJ;

                                                    RO_Star_Koma koma2 = Util_Koma.AsKoma(move.LongTimeAgo);

                                                    fugoJ = JFugoCreator15Array.ItemMethods[(int)Haiyaku184Array.Syurui(koma2.Haiyaku)](move, new KyokumenWrapper(src_Sky));//「▲２二角成」なら、馬（dst）ではなくて角（src）。


                                                    shogiGui.Shape_PnlTaikyoku.SetFugo(fugoJ.ToText_UseDou(
                                                        KifuNarabe_KifuWrapper.CurNode(shogiGui.GameViewModel.Kifu)
                                                        ));



                                                    //------------------------------
                                                    // チェンジターン
                                                    //------------------------------
                                                    if (!shogiGui.Shape_PnlTaikyoku.Requested_NaruDialogToShow)
                                                    {
                                                        shogiGui.ChangeTurn();//マウス左ボタンを放したのでチェンジターンします。
                                                    }

                                                    shogiGui.ResponseData.OutputTxt = ResponseGedanTxt.Kifu;
                                                    shogiGui.ResponseData.ToRedraw();
                                                }
                                            }
                                        }




                                        //------------------------------------------------------------
                                        // 選択解除か否か
                                        //------------------------------------------------------------
                                        {
                                            foreach (UserWidget widget in shogiGui.Shape_PnlTaikyoku.Widgets.Values)
                                            {
                                                if (widget.DeselectByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y, shogiGui))
                                                {
                                                    shogiGui.ResponseData.ToRedraw();
                                                }
                                            }
                                        }

                                        //------------------------------
                                        // このメインパネルの反応
                                        //------------------------------
                                        shogiGui.Response("MouseOperation");

                                        #endregion
                                    }
                                    break;

                            }
                            #endregion
                        }
                        break;

                    case SceneName.SceneB_2OkuKoma:
                        {
                            #region 置く駒

                            switch (eventState.Name2)
                            {
                                case MouseEventStateName.MouseLeftButtonUp:
                                    {
                                        #region マウス左ボタンアップ
                                        Node<ShootingStarlightable, KyokumenWrapper> siteiNode = KifuNarabe_KifuWrapper.CurNode(shogiGui.GameViewModel.Kifu);
                                        SkyConst src_Sky = shogiGui.GameViewModel.GuiSkyConst;


                                        //----------
                                        // 駒
                                        //----------
                                        foreach (Shape_BtnKomaImpl btnKoma in shogiGui.Shape_PnlTaikyoku.BtnKomaDoors)
                                        {
                                            if (btnKoma.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))
                                            {
                                                //>>>>> マウスが重なっていました

                                                if (shogiGui.Shape_PnlTaikyoku.SelectFirstTouch)
                                                {
                                                    // クリックのマウスアップ
                                                    shogiGui.Shape_PnlTaikyoku.SelectFirstTouch = false;
                                                }
                                                else
                                                {
                                                    RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(btnKoma.Koma).Now);


                                                    if (Okiba.ShogiBan == Util_Masu.GetOkiba(koma.Masu))
                                                    {
                                                        //>>>>> 将棋盤の上に置いてあった駒から、指を放しました
                                                        //System.C onsole.WriteLine("つまんでいる駒を放します。(4)");
                                                        shogiGui.Shape_PnlTaikyoku.SetFigTumandeiruKoma(-1);


                                                        //----------
                                                        // 移動済表示
                                                        //----------
                                                        shogiGui.Shape_PnlTaikyoku.SetHMovedKoma(btnKoma.Koma);

                                                        //------------------------------
                                                        // 棋譜に符号を追加（マウスボタンが放されたとき）TODO:まだ早い。駒が成るかもしれない。
                                                        //------------------------------

                                                        ShootingStarlightable move = new RO_ShootingStarlight(
                                                            //btnKoma.Koma,
                                                            shogiGui.Shape_PnlTaikyoku.MouseStarlightOrNull2.Now,

                                                            src_Sky.StarlightIndexOf(btnKoma.Koma).Now,

                                                            shogiGui.Shape_PnlTaikyoku.MousePos_FoodKoma != null ? shogiGui.Shape_PnlTaikyoku.MousePos_FoodKoma.Syurui : Ks14.H00_Null
                                                            );// 選択している駒の元の場所と、移動先

                                                        ShootingStarlightable last;
                                                        {
                                                            last = siteiNode.Key;
                                                        }
                                                        //ShootingStarlightable previousMove = last; //符号の追加が行われる前に退避

                                                        Node<ShootingStarlightable, KyokumenWrapper> newNode =
                                                            new KifuNodeImpl(
                                                                move,
                                                                new KyokumenWrapper(new SkyConst(src_Sky)),
                                                                KifuNodeImpl.GetReverseTebanside( ((KifuNode)shogiGui.GameViewModel.Kifu.CurNode).Tebanside)
                                                            );


                                                        //マウスの左ボタンを放したときです。
                                                        ((KifuNode)KifuNarabe_KifuWrapper.CurNode(shogiGui.GameViewModel.Kifu)).AppendChildA_New(newNode);
                                                        Util_InServer.SetCurNode_Srv(shogiGui, newNode);
                                                        shogiGui.ResponseData.ToRedraw();


                                                        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                                                        // ここでツリーの内容は変わっている。
                                                        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

                                                        //------------------------------
                                                        // 符号表示
                                                        //------------------------------
                                                        RO_Star_Koma koma2 = Util_Koma.AsKoma(move.LongTimeAgo);

                                                        FugoJ fugoJ;

                                                        fugoJ = JFugoCreator15Array.ItemMethods[(int)Haiyaku184Array.Syurui(koma2.Haiyaku)](move, new KyokumenWrapper(src_Sky));//「▲２二角成」なら、馬（dst）ではなくて角（src）。

                                                        shogiGui.Shape_PnlTaikyoku.SetFugo(fugoJ.ToText_UseDou(
                                                            KifuNarabe_KifuWrapper.CurNode(shogiGui.GameViewModel.Kifu)
                                                            ));



                                                        //------------------------------
                                                        // チェンジターン
                                                        //------------------------------
                                                        if (!shogiGui.Shape_PnlTaikyoku.Requested_NaruDialogToShow)
                                                        {
                                                            //System.C onsole.WriteLine("マウス左ボタンを放したのでチェンジターンします。");
                                                            shogiGui.ChangeTurn();//マウス左ボタンを放したのでチェンジターンします。
                                                        }

                                                        shogiGui.ResponseData.OutputTxt = ResponseGedanTxt.Kifu;
                                                        shogiGui.ResponseData.ToRedraw();
                                                    }



                                                }
                                            }
                                        }

                                        //------------------------------
                                        // このメインパネルの反応
                                        //------------------------------
                                        shogiGui.Response("MouseOperation");
                                        #endregion
                                    }
                                    break;

                                case MouseEventStateName.MouseLeftButtonDown:
                                    {
                                        #region マウス左ボタンダウン
                                        SceneName nextPhaseB = SceneName.Ignore;

                                        //System.C onsole.WriteLine("B2マウスダウン");

                                        //----------
                                        // つまんでいる駒
                                        //----------
                                        Shape_BtnKoma btnTumandeiruKoma = shogiGui.Shape_PnlTaikyoku.Btn_TumandeiruKoma(shogiGui);
                                        if (null == btnTumandeiruKoma)
                                        {
                                            //System.C onsole.WriteLine("つまんでいる駒なし");
                                            goto gt_nextBlock;
                                        }

                                        //>>>>> 選択されている駒があるとき

                                        Starlight tumandeiruLight = shogiGui.GameViewModel.GuiSkyConst.StarlightIndexOf((int)btnTumandeiruKoma.Finger);


                                        //----------
                                        // 指したい先
                                        //----------
                                        Shape_BtnMasuImpl btnSasitaiMasu = null;

                                        //----------
                                        // 将棋盤：升目   ＜移動先など＞
                                        //----------
                                        foreach (UserWidget widget in shogiGui.Shape_PnlTaikyoku.Widgets.Values)
                                        {
                                            if ("Masu" == widget.Type && Okiba.ShogiBan == widget.Okiba)
                                            {
                                                if (widget.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))//>>>>> 指したいマスはここです。
                                                {
                                                    btnSasitaiMasu = (Shape_BtnMasuImpl)widget.Object;
                                                    break;
                                                }
                                            }
                                        }


                                        //----------
                                        // 駒置き、駒袋：升目
                                        //----------
                                        foreach (UserWidget widget in shogiGui.Shape_PnlTaikyoku.Widgets.Values)
                                        {
                                            if ("Masu" == widget.Type && widget.Okiba.HasFlag(Okiba.Sente_Komadai | Okiba.Gote_Komadai | Okiba.KomaBukuro))
                                            {
                                                Shape_BtnMasuImpl btnSasitaiMasu2 = (Shape_BtnMasuImpl)widget.Object;
                                                if (btnSasitaiMasu2.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))//>>>>> 升目をクリックしたとき
                                                {
                                                    bool match = false;

                                                    shogiGui.GameViewModel.GuiSkyConst.Foreach_Starlights((Finger finger, Starlight ml, ref bool toBreak) =>
                                                    {
                                                        RO_Star_Koma koma = Util_Koma.AsKoma(ml.Now);

                                                        if (koma.Masu == btnSasitaiMasu2.Zahyo)
                                                        {
                                                            //>>>>> そこに駒が置いてあった。
#if DEBUG
                                    MessageBox.Show("駒が置いてあった","デバッグ中");
#endif
                                                            match = true;
                                                            toBreak = true;
                                                        }
                                                    });

                                                    if (!match)
                                                    {
                                                        btnSasitaiMasu = btnSasitaiMasu2;
                                                        goto gt_EndKomaoki;
                                                    }
                                                }
                                            }
                                        }
                                    gt_EndKomaoki:
                                        ;

                                        if (null == btnSasitaiMasu)
                                        {
                                            // 指したいマスなし
                                            goto gt_nextBlock;
                                        }



                                        //指したいマスが選択されたとき

                                        // TODO:合法手かどうか判定したい。

                                        if (Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(btnSasitaiMasu.Zahyo))//>>>>> 将棋盤：升目   ＜移動先など＞
                                        {

                                            //------------------------------
                                            // 成る／成らない
                                            //------------------------------
                                            //
                                            //      盤上の、不成の駒で、　／　相手陣に入るものか、相手陣から出てくる駒　※先手・後手区別なし
                                            //
                                            RO_Star_Koma koma = Util_Koma.AsKoma(tumandeiruLight.Now);

                                            if (
                                                    Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(koma.Masu) && Util_Sky.IsNareruKoma(tumandeiruLight)
                                                    &&
                                                    (
                                                        Util_Masu.InAitejin(btnSasitaiMasu.Zahyo, Util_InServer.CurPside(shogiGui))
                                                        ||
                                                        Util_Sky.InAitejin(tumandeiruLight)
                                                    )
                                                )
                                            {
                                                // 成るか／成らないか ダイアログボックスを表示します。
                                                shogiGui.Shape_PnlTaikyoku.Request_NaruDialogToShow(true);
                                            }


                                            if (shogiGui.Shape_PnlTaikyoku.Requested_NaruDialogToShow)
                                            {
                                                // 成る／成らないボタン表示
                                                shogiGui.Shape_PnlTaikyoku.GetWidget("BtnNaru").Visible = true;
                                                shogiGui.Shape_PnlTaikyoku.GetWidget("BtnNaranai").Visible = true;
                                                shogiGui.Shape_PnlTaikyoku.SetNaruMasu(btnSasitaiMasu);
                                                nextPhaseB = SceneName.SceneB_3ErabuNaruNaranai;
                                            }
                                            else
                                            {
                                                shogiGui.Shape_PnlTaikyoku.GetWidget("BtnNaru").Visible = false;
                                                shogiGui.Shape_PnlTaikyoku.GetWidget("BtnNaranai").Visible = false;

                                                // 駒を動かします。
                                                {
                                                    // GuiからServerへ渡す情報
                                                    Ks14 syurui;
                                                    Starlight dst;
                                                    Util_InGui.Komamove1a_49Gui(out syurui, out dst, btnTumandeiruKoma, btnSasitaiMasu, shogiGui);

                                                    // ServerからGuiへ渡す情報
                                                    bool torareruKomaAri;
                                                    RO_Star_Koma koma_Food_after;
                                                    Util_InServer.Komamove1a_50Srv(out torareruKomaAri, out koma_Food_after, dst, btnTumandeiruKoma.Koma, Util_Koma.AsKoma(dst.Now), shogiGui);

                                                    Util_InGui.Komamove1a_51Gui(torareruKomaAri, koma_Food_after, shogiGui);
                                                }

                                                nextPhaseB = SceneName.SceneB_1TumamitaiKoma;
                                            }

                                            shogiGui.ResponseData.ToRedraw();
                                        }
                                        else if ((Okiba.Sente_Komadai | Okiba.Gote_Komadai).HasFlag(
                                            Util_Masu.GetOkiba(btnSasitaiMasu.Zahyo)))//>>>>> 駒置き：升目
                                        {
                                            //System.C onsole.WriteLine("駒台上");

                                            RO_Star_Koma koma = Util_Koma.AsKoma(shogiGui.GameViewModel.GuiSkyConst.StarlightIndexOf(btnTumandeiruKoma.Koma).Now);

                                            SkyBuffer buffer_Sky = new SkyBuffer(shogiGui.GameViewModel.GuiSkyConst);
                                            buffer_Sky.AddOverwriteStarlight(btnTumandeiruKoma.Koma, new RO_MotionlessStarlight(
                                                //btnTumandeiruKoma.Koma,
                                                new RO_Star_Koma(Converter04.Okiba_ToPside(Util_Masu.GetOkiba(btnSasitaiMasu.Zahyo)),// 先手の駒置きに駒を置けば、先手の向きに揃えます。
                                                btnSasitaiMasu.Zahyo,
                                                KomaSyurui14Array.NarazuCaseHandle(Haiyaku184Array.Syurui(koma.Haiyaku)))
                                            ));
                                            shogiGui.GameViewModel.SetGuiSky(new SkyConst(buffer_Sky));

                                            nextPhaseB = SceneName.SceneB_1TumamitaiKoma;

                                            shogiGui.ResponseData.RedrawStarlights();// 駒の再描画要求
                                            shogiGui.ResponseData.ToRedraw();
                                        }


                                    gt_nextBlock:

                                        //----------
                                        // 既に選択されている駒
                                        //----------
                                        Shape_BtnKoma btnKoma_Selected = shogiGui.Shape_PnlTaikyoku.Btn_TumandeiruKoma(shogiGui);



                                        //----------
                                        // 初期配置ボタン
                                        //----------

                                        {
                                            foreach (UserWidget widget in shogiGui.Shape_PnlTaikyoku.Widgets.Values)
                                            {
                                                if (widget.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))
                                                {
                                                    if (null != widget.Delegate_MouseHitEvent)
                                                    {
                                                        widget.Delegate_MouseHitEvent(
                                                            shogiGui
                                                           , btnKoma_Selected
                                                           );
                                                    }
                                                }
                                            }
                                        }


                                        shogiGui.SetScene(nextPhaseB);

                                        //------------------------------
                                        // このメインパネルの反応
                                        //------------------------------
                                        shogiGui.Response("MouseOperation");
                                        #endregion
                                    }
                                    break;

                                case MouseEventStateName.MouseRightButtonDown:
                                    {
                                        #region マウス右ボタンダウン
                                        // 各駒の、移動済フラグを解除
                                        //System.C onsole.WriteLine("つまんでいる駒を放します。(5)");
                                        shogiGui.Shape_PnlTaikyoku.SetFigTumandeiruKoma(-1);
                                        shogiGui.Shape_PnlTaikyoku.SelectFirstTouch = false;

                                        //------------------------------
                                        // 状態を戻します。
                                        //------------------------------
                                        shogiGui.SetScene(SceneName.SceneB_1TumamitaiKoma);

                                        //------------------------------
                                        // このメインパネルの反応
                                        //------------------------------
                                        shogiGui.Response("MouseOperation");
                                        #endregion
                                    }
                                    break;
                            }
                            #endregion
                        }
                        break;

                    case SceneName.SceneB_3ErabuNaruNaranai:
                        {
                            #region 成る成らない

                            switch (eventState.Name2)
                            {
                                case MouseEventStateName.MouseLeftButtonDown:
                                    {
                                        #region マウス左ボタンダウン
                                        SceneName nextPhaseB = SceneName.Ignore;
                                        //GuiSky この関数の途中で変更される。ローカル変数に入れているものは古くなる。

                                        //----------
                                        // 既に選択されている駒
                                        //----------
                                        Shape_BtnKoma btnKoma_Selected = shogiGui.Shape_PnlTaikyoku.Btn_TumandeiruKoma(shogiGui);

                                        string[] buttonNames = new string[]{
                                        "BtnNaru",// [成る]ボタン
                                        "BtnNaranai"// [成らない]ボタン
                                    };
                                        foreach (string buttonName in buttonNames)
                                        {
                                            UserWidget widget = shogiGui.Shape_PnlTaikyoku.GetWidget(buttonName);

                                            if (widget.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))
                                            {
                                                if (null != widget.Delegate_MouseHitEvent)
                                                {
                                                    widget.Delegate_MouseHitEvent(
                                                        shogiGui
                                                       , btnKoma_Selected
                                                       );
                                                }
                                            }
                                        }


                                        //



                                        //----------
                                        // 既に選択されている駒
                                        //----------
                                        //Shape_BtnKomaImpl btnKoma_Selected = shogiGui.Shape_PnlTaikyoku.Btn_TumandeiruKoma(shogiGui);



                                        //----------
                                        // 初期配置ボタン
                                        //----------

                                        {
                                            foreach (UserWidget widget in shogiGui.Shape_PnlTaikyoku.Widgets.Values)
                                            {
                                                if (widget.HitByMouse(eventState.MouseLocation.X, eventState.MouseLocation.Y))
                                                {
                                                    if (null != widget.Delegate_MouseHitEvent)
                                                    {
                                                        widget.Delegate_MouseHitEvent(
                                                            shogiGui
                                                           , btnKoma_Selected
                                                           );
                                                    }
                                                }
                                            }
                                        }


                                        shogiGui.SetScene(nextPhaseB);

                                        //------------------------------
                                        // このメインパネルの反応
                                        //------------------------------
                                        shogiGui.Response("MouseOperation");
                                        #endregion
                                    }
                                    break;
                            }
                            #endregion

                        }
                        break;
                }
            }
            





        //gt_EndMethod1:
        //    ;
        }
    }


}
