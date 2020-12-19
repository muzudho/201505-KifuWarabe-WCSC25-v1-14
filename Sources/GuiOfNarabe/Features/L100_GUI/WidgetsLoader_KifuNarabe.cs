using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Gui;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P100_ShogiServer.L100_InServer;
using Grayscale.P200_KifuNarabe.L00006_Shape;
using Grayscale.P200_KifuNarabe.L00012_Ui;
using Grayscale.P200_KifuNarabe.L00047_Scene;
using Grayscale.P200_KifuNarabe.L00048_ShogiGui;
using Grayscale.P200_KifuNarabe.L015_Sprite;
using Grayscale.P200_KifuNarabe.L025_Macro;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P200_KifuNarabe.L100_GUI
{
    public class WidgetsLoader_KifuNarabe : WidgetsLoader
    {
        public string FileName { get; set; }

        public WidgetsLoader_KifuNarabe(string fileName)
        {
            this.FileName = fileName;
        }

        public virtual void Step1_ReadFile(object obj_shogiGui)
        {
            NarabeRoomViewModel shogiGui = (NarabeRoomViewModel)obj_shogiGui;

            List<List<string>> rows = Util_Csv.ReadCsv(this.FileName, Encoding.UTF8);

            // 最初の1行は、列名。
            Dictionary<string, int> columnNameIndex = new Dictionary<string, int>();
            {
                int i = 0;
                foreach (string columnName in rows[0])
                {
                    columnNameIndex.Add(columnName, i);
                    i++;
                }
            }
            rows.RemoveRange(0, 1);

            foreach (List<string> row in rows)
            {
                if (0 == row.Count)
                {
                    // 列のない行は無視します。
                    goto gt_NextRow;
                }

                //
                // 名称
                //
                if (!columnNameIndex.ContainsKey("name"))
                {
                    // name列のない行は無視します。
                    goto gt_NextRow;
                }
                string name = row[columnNameIndex["name"]];

                //
                // 型
                //
                string type = "Button"; // 既定値：ボタン
                if (columnNameIndex.ContainsKey("type"))
                {
                    type = row[columnNameIndex["type"]];
                }

                //
                // ウィンドウ・ガジェット生成、パネルへ追加
                //
                UserWidget widget;
                switch (type)
                {
                    case "Masu":
                        widget = new UserMasuImpl(new Shape_BtnMasuImpl());
                        break;
                    default://Button
                        widget = new UserButtonImpl(new Shape_BtnBoxImpl());
                        break;
                }
                widget.Type = type;
                widget.Name = name;
                shogiGui.Shape_PnlTaikyoku.SetWidget(name, widget);

                //
                // 「IsLight_OnFlowB_1TumamitaiKoma」のTRUE/FALSE
                //
                if (columnNameIndex.ContainsKey("IsLight_OnFlowB_1TumamitaiKoma"))
                {
                    string value = row[columnNameIndex["IsLight_OnFlowB_1TumamitaiKoma"]];
                    bool b;
                    if (bool.TryParse(value, out b))
                    {
                        widget.IsLight_OnFlowB_1TumamitaiKoma = b;
                    }
                }

                //
                // 「x」
                //
                if (columnNameIndex.ContainsKey("x"))
                {
                    string value = row[columnNameIndex["x"]];
                    int x;
                    if (int.TryParse(value, out x))
                    {
                        widget.SetBounds(new Rectangle(x, widget.Bounds.Y, widget.Bounds.Width, widget.Bounds.Height));
                    }
                }

                //
                // 「y」
                //
                if (columnNameIndex.ContainsKey("y"))
                {
                    string value = row[columnNameIndex["y"]];
                    int y;
                    if (int.TryParse(value, out y))
                    {
                        widget.SetBounds(new Rectangle(widget.Bounds.X, y, widget.Bounds.Width, widget.Bounds.Height));
                    }
                }

                //
                // 「label」
                //
                if (columnNameIndex.ContainsKey("label"))
                {
                    string value = row[columnNameIndex["label"]];
                    widget.Text = value;
                }

                //
                // 「fontSize」
                //
                if (columnNameIndex.ContainsKey("fontSize"))
                {
                    string value = row[columnNameIndex["fontSize"]];
                    float fontSize;
                    if (float.TryParse(value, out fontSize))
                    {
                        widget.FontSize = fontSize;
                    }
                }

                //
                // 「fugo」
                //
                if (columnNameIndex.ContainsKey("fugo"))
                {
                    string value = row[columnNameIndex["fugo"]];
                    widget.Fugo = value;
                }

                //
                // 「width」
                //
                if (columnNameIndex.ContainsKey("width"))
                {
                    string value = row[columnNameIndex["width"]];
                    int width;
                    if (int.TryParse(value, out width))
                    {
                        widget.SetBounds(new Rectangle(widget.Bounds.X, widget.Bounds.Y, width, widget.Bounds.Height));
                    }
                }

                //
                // 「height」
                //
                if (columnNameIndex.ContainsKey("height"))
                {
                    string value = row[columnNameIndex["height"]];
                    int height;
                    if (int.TryParse(value, out height))
                    {
                        widget.SetBounds(new Rectangle(widget.Bounds.X, widget.Bounds.Y, widget.Bounds.Width, height));
                    }
                }

                //
                // 「visible」
                //
                if (columnNameIndex.ContainsKey("visible"))
                {
                    string value = row[columnNameIndex["visible"]];
                    bool visible;
                    if (bool.TryParse(value, out visible))
                    {
                        widget.Visible = visible;
                    }
                }

                //
                // 「backColor」
                //
                if (columnNameIndex.ContainsKey("backColor"))
                {
                    string value = row[columnNameIndex["backColor"]];
                    if ("" != value)
                    {
                        widget.BackColor = Color.FromName(value);
                    }
                }

                //
                // 「okiba」
                //
                if (columnNameIndex.ContainsKey("okiba"))
                {
                    string value = row[columnNameIndex["okiba"]];
                    switch (value)
                    {
                        case "ShogiBan":
                            widget.Okiba = Okiba.ShogiBan;
                            break;
                        case "Sente_Komadai":
                            widget.Okiba = Okiba.Sente_Komadai;
                            break;
                        case "Gote_Komadai":
                            widget.Okiba = Okiba.Gote_Komadai;
                            break;
                        case "KomaBukuro":
                            widget.Okiba = Okiba.KomaBukuro;
                            break;
                        default:
                            break;
                    }
                }

                //
                // 「suji」
                //
                if (columnNameIndex.ContainsKey("suji"))
                {
                    string value = row[columnNameIndex["suji"]];
                    int suji;
                    if (int.TryParse(value, out suji))
                    {
                        widget.Suji = suji;
                    }
                }

                //
                // 「dan」
                //
                if (columnNameIndex.ContainsKey("dan"))
                {
                    string value = row[columnNameIndex["dan"]];
                    int dan;
                    if (int.TryParse(value, out dan))
                    {
                        widget.Dan = dan;
                    }
                }

                //
                // 「masuHandle」
                //
                if (columnNameIndex.ContainsKey("masuHandle"))
                {
                    string value = row[columnNameIndex["masuHandle"]];
                    int masuHandle;
                    if (int.TryParse(value, out masuHandle))
                    {
                        widget.MasuHandle = masuHandle;
                    }
                }

            gt_NextRow:
                ;
            }
        }

        public virtual void Step2_Compile_AllWidget(object obj_shogiGui)
        {
            NarabeRoomViewModel shogiGui = (NarabeRoomViewModel)obj_shogiGui;

            foreach (UserWidget widget in shogiGui.Shape_PnlTaikyoku.Widgets.Values)
            {
                widget.Compile();
            }
        }

        public virtual void Step3_SetEvent(object obj_shogiGui)
        {
            NarabeRoomViewModel shogiGui1 = (NarabeRoomViewModel)obj_shogiGui;

            //----------
            // [成る]ボタン
            //----------
            {
                UserWidget widget = shogiGui1.Shape_PnlTaikyoku.GetWidget("BtnNaru");
                widget.Delegate_MouseHitEvent = (
                    object obj_shogiGui2
                    , Shape_BtnKoma btnKoma_Selected
                    , ILogTag logTag
                    ) =>
                {
                    NarabeRoomViewModel shogiGui = (NarabeRoomViewModel)obj_shogiGui2;

                    Ui_PnlMain ui_PnlMain = ((Ui_ShogiForm1)shogiGui.OwnerForm).Ui_PnlMain1;


                    shogiGui.Shape_PnlTaikyoku.SetNaruFlag(true);
                    this.After_NaruNaranai(
                        shogiGui
                        , btnKoma_Selected
                        , logTag
                        );
                };
            }

            //----------
            // [成らない]ボタン
            //----------
            {
                UserWidget widget = shogiGui1.Shape_PnlTaikyoku.GetWidget("BtnNaranai");
                widget.Delegate_MouseHitEvent = (
                    object obj_shogiGui2
                    , Shape_BtnKoma btnKoma_Selected
                    , ILogTag logTag
                    ) =>
                {
                    NarabeRoomViewModel shogiGui = (NarabeRoomViewModel)obj_shogiGui2;

                    Ui_PnlMain ui_PnlMain = ((Ui_ShogiForm1)shogiGui.OwnerForm).Ui_PnlMain1;

                    shogiGui.Shape_PnlTaikyoku.SetNaruFlag(false);
                    this.After_NaruNaranai(
                        shogiGui
                        , btnKoma_Selected
                        , logTag
                        );
                };
            }

            //----------
            // [クリアー]ボタン
            //----------
            {
                UserWidget widget = shogiGui1.Shape_PnlTaikyoku.GetWidget("BtnClear");
                widget.Delegate_MouseHitEvent = (
                    object obj_shogiGui2
                    , Shape_BtnKoma btnKoma_Selected
                    , ILogTag logTag
                    ) =>
                {
                    NarabeRoomViewModel shogiGui = (NarabeRoomViewModel)obj_shogiGui2;

                    Util_Lua_KifuNarabe.ShogiGui = shogiGui;
                    Util_Lua_KifuNarabe.LogTag = logTag;
                    Util_Lua_KifuNarabe.Perform("click_clearButton");
                };
            }

            //----------
            // [再生]ボタン
            //----------
            {
                UserWidget widget = shogiGui1.Shape_PnlTaikyoku.GetWidget("BtnPlay");
                widget.Delegate_MouseHitEvent = (
                    object obj_shogiGui2
                    , Shape_BtnKoma btnKoma_Selected
                    , ILogTag logTag
                    ) =>
                {
                    NarabeRoomViewModel shogiGui = (NarabeRoomViewModel)obj_shogiGui2;

                    Util_Lua_KifuNarabe.ShogiGui = shogiGui;
                    Util_Lua_KifuNarabe.LogTag = logTag;
                    Util_Lua_KifuNarabe.Perform("click_playButton");
                };
            }

            //----------
            // [コマ送り]ボタン
            //----------
            {
                UserWidget widget = shogiGui1.Shape_PnlTaikyoku.GetWidget("BtnForward");
                widget.Delegate_MouseHitEvent = (
                    object obj_shogiGui2
                    , Shape_BtnKoma btnKoma_Selected
                    , ILogTag logTag
                    ) =>
                {
                    NarabeRoomViewModel shogiGui = (NarabeRoomViewModel)obj_shogiGui2;

                    string restText = Util_InGui.ReadLine_FromTextbox();
                    Util_InServer.Komaokuri_Srv(ref restText, shogiGui, logTag);
                    Util_InGui.Komaokuri_Gui(restText, shogiGui, logTag);
                    Util_Menace.Menace(shogiGui, logTag);// メナス
                };
            }

            //----------
            // [巻戻し]ボタン
            //----------
            {
                UserWidget widget = shogiGui1.Shape_PnlTaikyoku.GetWidget("BtnBackward");
                widget.Delegate_MouseHitEvent = (
                    object obj_shogiGui2
                    , Shape_BtnKoma btnKoma_Selected
                    , ILogTag logTag
                    ) =>
                {
                    NarabeRoomViewModel shogiGui = (NarabeRoomViewModel)obj_shogiGui2;

                    Ui_PnlMain ui_PnlMain = ((Ui_ShogiForm1)shogiGui.OwnerForm).Ui_PnlMain1;

                    Finger movedKoma;
                    Finger foodKoma;//取られた駒
                    string fugoJStr;

                    if (!Util_InServer.Makimodosi_Srv(out movedKoma, out foodKoma, out fugoJStr, shogiGui, logTag))
                    {
                        goto gt_EndBlock;
                    }

                    Util_InGui.Makimodosi_Gui(shogiGui,movedKoma,foodKoma,fugoJStr, Util_InGui.ReadLine_FromTextbox(), logTag);
                    Util_Menace.Menace(shogiGui, logTag);//メナス

                gt_EndBlock:
                    ;
                };
            }


            //----------
            // ログ出せボタン
            //----------
            {
                UserWidget widget = shogiGui1.Shape_PnlTaikyoku.GetWidget("BtnLogdase");
                widget.Delegate_MouseHitEvent = (
                    object obj_shogiGui2
                    , Shape_BtnKoma btnKoma_Selected
                    , ILogTag logTag
                    ) =>
                {
                    NarabeRoomViewModel shogiGui = (NarabeRoomViewModel)obj_shogiGui2;

                    Ui_PnlMain ui_PnlMain = ((Ui_ShogiForm1)shogiGui.OwnerForm).Ui_PnlMain1;

                    ui_PnlMain.ShogiGui.Logdase();
                };
            }




            //----------
            // [壁置く]ボタン
            //----------
            {
                UserWidget widget = shogiGui1.Shape_PnlTaikyoku.GetWidget("BtnKabeOku");
                widget.Delegate_MouseHitEvent = (
                    object obj_shogiGui2
                    , Shape_BtnKoma btnKoma_Selected
                    , ILogTag logTag
                    ) =>
                {
                    NarabeRoomViewModel shogiGui = (NarabeRoomViewModel)obj_shogiGui2;

                    Ui_PnlMain ui_PnlMain = ((Ui_ShogiForm1)shogiGui.OwnerForm).Ui_PnlMain1;

                    // [壁置く]←→[駒動かす]切替
                    switch(widget.Text)
                    {
                        case "壁置く":
                            widget.Text = "駒動かす";
                            break;
                        default:
                            widget.Text = "壁置く";
                            break;
                    }

                    shogiGui.ResponseData.ToRedraw();
                };
            }


            //----------
            // [出力切替]ボタン
            //----------
            {
                UserWidget widget = shogiGui1.Shape_PnlTaikyoku.GetWidget("BtnSyuturyokuKirikae");
                widget.Delegate_MouseHitEvent = (
                    object obj_shogiGui2
                    , Shape_BtnKoma btnKoma_Selected
                    , ILogTag logTag
                    ) =>
                {
                    NarabeRoomViewModel shogiGui = (NarabeRoomViewModel)obj_shogiGui2;

                    Ui_PnlMain ui_PnlMain = ((Ui_ShogiForm1)shogiGui.OwnerForm).Ui_PnlMain1;

                    switch (shogiGui.Shape_PnlTaikyoku.SyuturyokuKirikae)
                    {
                        case SyuturyokuKirikae.Japanese:
                            shogiGui.Shape_PnlTaikyoku.SetSyuturyokuKirikae(SyuturyokuKirikae.Sfen);
                            break;
                        case SyuturyokuKirikae.Sfen:
                            shogiGui.Shape_PnlTaikyoku.SetSyuturyokuKirikae(SyuturyokuKirikae.Html);
                            break;
                        case SyuturyokuKirikae.Html:
                            shogiGui.Shape_PnlTaikyoku.SetSyuturyokuKirikae(SyuturyokuKirikae.Japanese);
                            break;
                    }

                    shogiGui.ResponseData.OutputTxt = ResponseGedanTxt.Kifu;
                };
            }

            //----------
            // [▲]～[打]符号ボタン
            //----------
            {
                string[] buttonNames = new string[]{
                        "BtnFugo_Sente"// [▲]～[打]符号ボタン
                        ,"BtnFugo_Gote"
                        ,"BtnFugo_1"
                        ,"BtnFugo_2"
                        ,"BtnFugo_3"
                        ,"BtnFugo_4"
                        ,"BtnFugo_5"
                        ,"BtnFugo_6"
                        ,"BtnFugo_7"
                        ,"BtnFugo_8"
                        ,"BtnFugo_9"
                        ,"BtnFugo_Dou"
                        ,"BtnFugo_Fu"
                        ,"BtnFugo_Hisya"
                        ,"BtnFugo_Kaku"
                        ,"BtnFugo_Kyo"
                        ,"BtnFugo_Kei"
                        ,"BtnFugo_Gin"
                        ,"BtnFugo_Kin"
                        ,"BtnFugo_Oh"
                        ,"BtnFugo_Gyoku"
                        ,"BtnFugo_Tokin"
                        ,"BtnFugo_Narikyo"
                        ,"BtnFugo_Narikei"
                        ,"BtnFugo_Narigin"
                        ,"BtnFugo_Ryu"
                        ,"BtnFugo_Uma"
                        ,"BtnFugo_Yoru"
                        ,"BtnFugo_Hiku"
                        ,"BtnFugo_Agaru"
                        ,"BtnFugo_Migi"
                        ,"BtnFugo_Hidari"
                        ,"BtnFugo_Sugu"
                        ,"BtnFugo_Nari"
                        ,"BtnFugo_Funari"
                        ,"BtnFugo_Da"
                    };

                foreach (string buttonName in buttonNames)
                {
                    UserWidget widget = shogiGui1.Shape_PnlTaikyoku.GetWidget(buttonName);
                    widget.Delegate_MouseHitEvent = (
                        object obj_shogiGui2
                        , Shape_BtnKoma btnKoma_Selected
                        , ILogTag logTag
                        ) =>
                    {
                        NarabeRoomViewModel shogiGui = (NarabeRoomViewModel)obj_shogiGui2;

                        Ui_PnlMain ui_PnlMain = ((Ui_ShogiForm1)shogiGui.OwnerForm).Ui_PnlMain1;

                        shogiGui.ResponseData.SetAppendInputTextString(shogiGui.Shape_PnlTaikyoku.GetWidget(buttonName).Fugo);
                    };
                }
            }


            //----------
            // [全消]ボタン
            //----------
            {
                UserWidget widget = shogiGui1.Shape_PnlTaikyoku.GetWidget("BtnFugo_Zenkesi");
                widget.Delegate_MouseHitEvent = (
                    object obj_shogiGui2
                    , Shape_BtnKoma btnKoma_Selected
                    , ILogTag logTag
                    ) =>
                {
                    NarabeRoomViewModel shogiGui = (NarabeRoomViewModel)obj_shogiGui2;

                    Ui_PnlMain ui_PnlMain = ((Ui_ShogiForm1)shogiGui.OwnerForm).Ui_PnlMain1;

                    shogiGui.ResponseData.InputTextString = "";
                };
            }


            //----------
            // [ここから採譜]ボタン
            //----------
            {
                UserWidget widget = shogiGui1.Shape_PnlTaikyoku.GetWidget("BtnFugo_KokokaraSaifu");
                widget.Delegate_MouseHitEvent = (
                    object obj_shogiGui2
                    , Shape_BtnKoma btnKoma_Selected
                    , ILogTag logTag
                    ) =>
                {
                    NarabeRoomViewModel shogiGui = (NarabeRoomViewModel)obj_shogiGui2;

                    Ui_PnlMain ui_PnlMain = ((Ui_ShogiForm1)shogiGui.OwnerForm).Ui_PnlMain1;

                    ui_PnlMain.ShogiGui.GameViewModel.Kifu.SetStartpos_KokokaraSaifu(Util_InServer.CurPside(ui_PnlMain.ShogiGui), logTag);
                    shogiGui.ResponseData.OutputTxt = ResponseGedanTxt.Kifu;
                };
            }

            //----------
            // 初期配置ボタン
            //----------
            {
                UserWidget widget = shogiGui1.Shape_PnlTaikyoku.GetWidget("BtnSyokihaichi");
                widget.Delegate_MouseHitEvent = (
                    object obj_shogiGui2
                    , Shape_BtnKoma btnKoma_Selected
                    , ILogTag logTag
                    ) =>
                {
                    NarabeRoomViewModel shogiGui = (NarabeRoomViewModel)obj_shogiGui2;

                    Ui_PnlMain ui_PnlMain = ((Ui_ShogiForm1)shogiGui.OwnerForm).Ui_PnlMain1;

                    WidgetsLoader_KifuNarabe.Perform_SyokiHaichi( ui_PnlMain );
                };
            }


            //----------
            // [向き]ボタン
            //----------
            {
                UserWidget widget = shogiGui1.Shape_PnlTaikyoku.GetWidget("BtnMuki");
                widget.Delegate_MouseHitEvent = (
                    object obj_shogiGui2
                    , Shape_BtnKoma btnKoma_Selected
                    , ILogTag logTag
                    ) =>
                {
                    NarabeRoomViewModel shogiGui = (NarabeRoomViewModel)obj_shogiGui2;

                    Ui_PnlMain ui_PnlMain = ((Ui_ShogiForm1)shogiGui.OwnerForm).Ui_PnlMain1;

                    Shape_BtnKoma movedKoma = shogiGui.Shape_PnlTaikyoku.Btn_MovedKoma();

                    RO_Star_Koma koma;
                    Finger figKoma = Fingers.Error_1;

                    if (null != movedKoma)
                    {
                        //>>>>> 移動直後の駒があるとき
                        koma = Util_Koma.AsKoma(ui_PnlMain.ShogiGui.GameViewModel.GuiSkyConst.StarlightIndexOf(movedKoma.Finger).Now);
                        figKoma = movedKoma.Finger;
                    }
                    else if (null != btnKoma_Selected)
                    {
                        //>>>>> 選択されている駒があるとき
                        koma = Util_Koma.AsKoma(ui_PnlMain.ShogiGui.GameViewModel.GuiSkyConst.StarlightIndexOf(btnKoma_Selected.Koma).Now);
                        figKoma = btnKoma_Selected.Koma;
                    }
                    else
                    {
                        koma = null;
                    }

                    if (null != koma)
                    {
                        switch (koma.Pside)
                        {
                            case Playerside.P1:
                                {
                                    SkyBuffer buffer_Sky = new SkyBuffer(ui_PnlMain.ShogiGui.GameViewModel.GuiSkyConst);

                                    buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(
                                        //figKoma,
                                        new RO_Star_Koma(Playerside.P2,
                                        koma.Masu,
                                        Haiyaku184Array.Syurui(koma.Haiyaku))
                                        ));

                                    KifuNode modifyNode = new KifuNodeImpl(
                                                                ui_PnlMain.ShogiGui.GameViewModel.Kifu.CurNode.Key,//現在の局面を流用
                                                                new KyokumenWrapper(new SkyConst(buffer_Sky)),
                                                                ((KifuNode)ui_PnlMain.ShogiGui.GameViewModel.Kifu.CurNode).Tebanside
                                                                );

                                    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                                    // ここで局面データを変更します。
                                    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                                    Util_InServer.SetCurNode_Srv(ui_PnlMain.ShogiGui, modifyNode);
                                    ui_PnlMain.ShogiGui.ResponseData.ToRedraw();
                                }
                                break;
                            case Playerside.P2:
                                {
                                    SkyBuffer buffer_Sky = new SkyBuffer(ui_PnlMain.ShogiGui.GameViewModel.GuiSkyConst);

                                    buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(
                                        //figKoma,
                                        new RO_Star_Koma(Playerside.P1,
                                        koma.Masu,
                                        Haiyaku184Array.Syurui(koma.Haiyaku))
                                        ));

                                    KifuNode modifyNode =
                                        new KifuNodeImpl(
                                            ui_PnlMain.ShogiGui.GameViewModel.Kifu.CurNode.Key,//現在の局面を流用
                                            new KyokumenWrapper(new SkyConst(buffer_Sky)),
                                            ((KifuNode)ui_PnlMain.ShogiGui.GameViewModel.Kifu.CurNode).Tebanside
                                            );


                                    // ここで局面データを変更します。
                                    Util_InServer.SetCurNode_Srv(ui_PnlMain.ShogiGui, modifyNode);
                                    ui_PnlMain.ShogiGui.ResponseData.ToRedraw();
                                }
                                break;
                        }
                    }
                };
            }
        }

        /// <summary>
        /// [初期配置]ボタン
        /// </summary>
        public static void Perform_SyokiHaichi(
            Ui_PnlMain ui_PnlMain
            )
        {
            ui_PnlMain.ShogiGui.GameViewModel.Kifu.Clear();// 棋譜を空っぽにします。
            ui_PnlMain.ShogiGui.GameViewModel.Kifu.SetProperty(KifuTreeImpl.PropName_Startpos, "startpos");//平手の初期局面

            KifuNode newNode = new KifuNodeImpl(
                                        Util_Sky.NullObjectMove,//ルートなので
                                        new KyokumenWrapper(Util_Sky.New_Hirate()),//[初期配置]ボタン押下時
                                        KifuNodeImpl.GetReverseTebanside(((KifuNode)ui_PnlMain.ShogiGui.GameViewModel.Kifu.CurNode).Tebanside)
                                        );

            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            // ここで棋譜の変更をします。
            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            Util_InServer.SetCurNode_Srv(ui_PnlMain.ShogiGui, newNode);
            ui_PnlMain.ShogiGui.ResponseData.ToRedraw();

            ui_PnlMain.ShogiGui.ResponseData.RedrawStarlights();// 駒の再描画要求
            ui_PnlMain.ShogiGui.ResponseData.OutputTxt = ResponseGedanTxt.Clear;
            ui_PnlMain.ShogiGui.ResponseData.ToRedraw();
        }


        private void After_NaruNaranai(
            NarabeRoomViewModel shogiGui
            , Shape_BtnKoma btnTumandeiruKoma
            , ILogTag logTag
        )
        {

            // 駒を動かします。
            {
                // GuiからServerへ渡す情報
                Ks14 syurui;
                Starlight dst;
                Util_InGui.Komamove1a_49Gui(out syurui, out dst, btnTumandeiruKoma, shogiGui.Shape_PnlTaikyoku.NaruBtnMasu, shogiGui);

                // ServerからGuiへ渡す情報
                bool torareruKomaAri;
                RO_Star_Koma koma_Food_after;
                Util_InServer.Komamove1a_50Srv(out torareruKomaAri, out koma_Food_after, dst, btnTumandeiruKoma.Koma, Util_Koma.AsKoma(dst.Now), shogiGui, logTag);

                Util_InGui.Komamove1a_51Gui(torareruKomaAri, koma_Food_after, shogiGui);
            }

            {
                //----------
                // 移動済表示
                //----------
                shogiGui.Shape_PnlTaikyoku.SetHMovedKoma(btnTumandeiruKoma.Finger);

                //------------------------------
                // 棋譜に符号を追加（マウスボタンが放されたとき）TODO:まだ早い。駒が成るかもしれない。
                //------------------------------
                // 棋譜

                ShootingStarlightable move = new RO_ShootingStarlight(
                    //btnTumandeiruKoma.Finger,
                    shogiGui.Shape_PnlTaikyoku.MouseStarlightOrNull2.Now,

                    shogiGui.GameViewModel.GuiSkyConst.StarlightIndexOf(btnTumandeiruKoma.Finger).Now,

                    shogiGui.Shape_PnlTaikyoku.MousePos_FoodKoma != null ? shogiGui.Shape_PnlTaikyoku.MousePos_FoodKoma.Syurui : Ks14.H00_Null
                    );// 選択している駒の元の場所と、移動先


                {
                    StartposImporter.Assert_HirateHonsyogi(new SkyBuffer(shogiGui.GameViewModel.GuiSkyConst), "newNode作成前");

                    KifuNode newNode =new KifuNodeImpl(
                        move,
                        new KyokumenWrapper(shogiGui.GameViewModel.GuiSkyConst),
                        KifuNodeImpl.GetReverseTebanside( ((KifuNode)shogiGui.GameViewModel.Kifu.CurNode).Tebanside)
                    );


                    StartposImporter.Assert_HirateHonsyogi(new SkyBuffer(newNode.Value.ToKyokumenConst), "newNode作成後");


                    //「成る／成らない」ボタンを押したときです。
                    ((KifuNode)KifuNarabe_KifuWrapper.CurNode(shogiGui.GameViewModel.Kifu)).AppendChildA_New(newNode);

                    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                    // ここで棋譜の変更をします。
                    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                    Util_InServer.SetCurNode_Srv(shogiGui, newNode);
                    shogiGui.ResponseData.ToRedraw();
                }


                //------------------------------
                // 符号表示
                //------------------------------
                RO_Star_Koma koma = Util_Koma.AsKoma(move.LongTimeAgo);

                FugoJ fugoJ;
                fugoJ = JFugoCreator15Array.ItemMethods[(int)Haiyaku184Array.Syurui(koma.Haiyaku)](move, new KyokumenWrapper(shogiGui.GameViewModel.GuiSkyConst));//「▲２二角成」なら、馬（dst）ではなくて角（src）。

                shogiGui.Shape_PnlTaikyoku.SetFugo(fugoJ.ToText_UseDou(
                    KifuNarabe_KifuWrapper.CurNode(shogiGui.GameViewModel.Kifu)
                    ));


                //------------------------------
                // チェンジターン
                //------------------------------
                if (!shogiGui.Shape_PnlTaikyoku.Requested_NaruDialogToShow)
                {
                    //System.C onsole.WriteLine("マウス左ボタンを押したのでチェンジターンします。");
                    shogiGui.ChangeTurn(logTag);
                }
            }


            shogiGui.ResponseData.RedrawStarlights();// 駒の再描画要求

            //System.C onsole.WriteLine("つまんでいる駒を放します。(6)");
            shogiGui.Shape_PnlTaikyoku.SetFigTumandeiruKoma(-1);//駒を放した扱いです。

            shogiGui.Shape_PnlTaikyoku.SetNaruMasu(null);

            shogiGui.ResponseData.OutputTxt = ResponseGedanTxt.Kifu;
            shogiGui.ResponseData.ToRedraw();

            ShootingStarlightable last;
            {
                Node<ShootingStarlightable, KyokumenWrapper> kifuElement = KifuNarabe_KifuWrapper.CurNode(shogiGui.GameViewModel.Kifu);

                last = (ShootingStarlightable)kifuElement.Key;
            }
            shogiGui.ChangeTurn(logTag);//マウス左ボタンを押したのでチェンジターンします。

            shogiGui.Shape_PnlTaikyoku.Request_NaruDialogToShow(false);
            shogiGui.Shape_PnlTaikyoku.GetWidget("BtnNaru").Visible = false;
            shogiGui.Shape_PnlTaikyoku.GetWidget("BtnNaranai").Visible = false;
            shogiGui.SetScene(SceneName.SceneB_1TumamitaiKoma);
        }

    }
}
