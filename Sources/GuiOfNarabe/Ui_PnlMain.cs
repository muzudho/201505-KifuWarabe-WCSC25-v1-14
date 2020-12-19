using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Gui;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L007_Random;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P025_KifuLarabe.L100_KifuIO;
using Grayscale.P100_ShogiServer.L100_InServer;
using Grayscale.P200_KifuNarabe.L00006_Shape;
using Grayscale.P200_KifuNarabe.L00012_Ui;
using Grayscale.P200_KifuNarabe.L00047_Scene;
using Grayscale.P200_KifuNarabe.L00048_ShogiGui;
using Grayscale.P200_KifuNarabe.L002_Log;
using Grayscale.P200_KifuNarabe.L008_TextBoxListener;
using Grayscale.P200_KifuNarabe.L015_Sprite;
using Grayscale.P200_KifuNarabe.L025_Macro;
using Grayscale.P200_KifuNarabe.L050_Scene;
using Grayscale.P200_KifuNarabe.L051_Timed;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P200_KifuNarabe.L100_GUI
{
    /// <summary>
    /// メイン画面です。
    /// </summary>
    [Serializable]
    public partial class Ui_PnlMain : UserControl
    {
        public NarabeRoomViewModel ShogiGui { get; set; }

        public SetteiFile SetteiFile
        {
            get
            {
                return this.setteiFile;
            }
        }
        private SetteiFile setteiFile;

        /// <summary>
        /// 入力欄のテキストを取得します。
        /// </summary>
        /// <returns></returns>
        public string ReadLine1()
        {
            return this.txtInput1.Text;
        }

        private const int NSQUARE = 9 * 9;

        /// <summary>
        /// 入力欄のテキストを取得します。
        /// </summary>
        /// <returns></returns>
        public string ReadLine2(ILogTag logTag)
        {
            int lastTesumi = Util_InServer.CountCurTesumi2(this.ShogiGui);
            SkyConst src_Sky = this.ShogiGui.GameViewModel.GuiSkyConst;

            //------------------------------------------------------------
            // 表について
            //------------------------------------------------------------

            //
            // 配列の添え字は次の通り。
            //
            //    ９　８　７　６　５　４　３　２　１
            //  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┐
            //  │ 0│ 1│ 2│ 3│ 4│ 5│ 6│ 7│ 8│一
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │ 9│10│11│12│13│14│15│16│17│二
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │18│19│20│21│22│23│24│25│26│三
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │27│28│29│30│31│32│33│34│35│四
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //　│36│37│38│39│40│41│42│43│44│五
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │45│46│47│48│49│50│51│52│53│六
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │54│55│56│57│58│59│60│61│62│七
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │63│64│65│66│67│68│69│70│71│八
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │72│73│74│75│76│77│78│79│80│九
            //  └─┴─┴─┴─┴─┴─┴─┴─┴─┘

            //------------------------------------------------------------
            // 先手駒について
            //------------------------------------------------------------

            // 先手駒の位置を表にします。
            bool[] wallSTable = new bool[Ui_PnlMain.NSQUARE];

            // 先手駒の利きを表にします。
            bool[] kikiSTable = new bool[Ui_PnlMain.NSQUARE];

            Node<ShootingStarlightable, KyokumenWrapper> siteiNode = this.ShogiGui.GameViewModel.Kifu.NodeAt(
                this.ShogiGui.GameViewModel.Kifu.CountTesumi(KifuNarabe_KifuWrapper.CurNode(this.ShogiGui))
                );

            foreach (Finger figKoma in Util_Sky.Fingers_ByOkibaPsideNow(this.ShogiGui.GameViewModel.GuiSkyConst, Okiba.ShogiBan, Playerside.P1).Items)
            {
                Starlightable light = src_Sky.StarlightIndexOf(figKoma).Now;
                RO_Star_Koma koma = Util_Koma.AsKoma(light);


                int suji;
                Util_MasuNum.MasuToSuji(koma.Masu, out suji);

                int dan;
                Util_MasuNum.MasuToDan(koma.Masu, out dan);

                // 壁
                wallSTable[(dan - 1) * 9 + (9 - suji)] = true;

                // 利き
                kikiSTable[(dan - 1) * 9 + (9 - suji)] = true;//FIXME:嘘
            }

            //------------------------------------------------------------
            // 後手駒について
            //------------------------------------------------------------

            // 先手駒の位置を表にします。
            bool[] wallGTable = new bool[Ui_PnlMain.NSQUARE];

            // 先手駒の利きを表にします。
            bool[] kikiGTable = new bool[Ui_PnlMain.NSQUARE];

            foreach (Finger figKoma in Util_Sky.Fingers_ByOkibaPsideNow(this.ShogiGui.GameViewModel.GuiSkyConst, Okiba.ShogiBan, Playerside.P2).Items)
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(figKoma).Now);


                int suji;
                Util_MasuNum.MasuToSuji(koma.Masu, out suji);

                int dan;
                Util_MasuNum.MasuToDan(koma.Masu, out dan);

                // 壁
                wallGTable[(dan - 1) * 9 + (9 - suji)] = true;

                // 利き
                kikiGTable[(dan - 1) * 9 + (9 - suji)] = true;//FIXME:嘘
            }


            string tuginoItte = "▲９九王嘘";


            Fingers fingers = Util_Sky.Fingers_ByOkibaPsideNow(this.ShogiGui.GameViewModel.GuiSkyConst, Okiba.ShogiBan, this.ShogiGui.GameViewModel.Kifu.CountPside(KifuNarabe_KifuWrapper.CurNode(this.ShogiGui)));
            if (0<fingers.Count)
            {
                ShootingStarlightable tuginoMoveData;

                Finger finger = fingers[LarabeRandom.Random.Next(fingers.Count)];//ランダムに１つ。
                Starlight sl = src_Sky.StarlightIndexOf(finger);

                RO_Star_Koma koma = Util_Koma.AsKoma(sl.Now);


                Playerside pside_getTeban = this.ShogiGui.GameViewModel.Kifu.CountPside(lastTesumi);
                    switch (pside_getTeban)
                    {
                        case Playerside.P2:
                            {
                                // 後手番です。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                int suji;
                                Util_MasuNum.MasuToSuji(koma.Masu, out suji);

                                int dan;
                                Util_MasuNum.MasuToDan(koma.Masu, out dan);

                                // 前に１つ突き出させます。
                                tuginoMoveData = new RO_ShootingStarlight(
                                    //sl.Finger,
                                    new RO_Star_Koma(
                                        pside_getTeban,
                                        Util_Masu.OkibaSujiDanToMasu(
                                            Util_Masu.GetOkiba(koma.Masu),
                                            suji,
                                            dan
                                            ),
                                        koma.Haiyaku
                                    ),

                                    new RO_Star_Koma(
                                        pside_getTeban,
                                        Util_Masu.OkibaSujiDanToMasu(
                                            Okiba.ShogiBan,
                                            suji,
                                            dan + 1
                                            ),
                                        koma.Haiyaku
                                    ),

                                    Ks14.H00_Null
                                );
                                break;
                            }
                        default:
                            {
                                // 先手番です。
                                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                int suji;
                                Util_MasuNum.MasuToSuji(koma.Masu, out suji);

                                int dan;
                                Util_MasuNum.MasuToDan(koma.Masu, out dan);

                                // 前に１つ突き出させます。
                                tuginoMoveData = new RO_ShootingStarlight(
                                    //sl.Finger,
                                    new RO_Star_Koma(
                                        pside_getTeban,
                                        Util_Masu.OkibaSujiDanToMasu(
                                            Util_Masu.GetOkiba(koma.Masu),
                                            suji,
                                            dan
                                            ),
                                        koma.Haiyaku
                                    ),

                                    new RO_Star_Koma(
                                        pside_getTeban,
                                        Util_Masu.OkibaSujiDanToMasu(
                                            Okiba.ShogiBan,
                                            suji,
                                            dan - 1
                                            ),
                                        koma.Haiyaku
                                    ),

                                    Ks14.H00_Null
                                );
                                break;
                            }
                    }


                RO_Star_Koma koma2 = Util_Koma.AsKoma(tuginoMoveData.LongTimeAgo);

                FugoJ fugoJ = JFugoCreator15Array.ItemMethods[(int)Haiyaku184Array.Syurui(koma2.Haiyaku)](tuginoMoveData, new KyokumenWrapper(src_Sky));//「▲２二角成」なら、馬（dst）ではなくて角（src）。
                tuginoItte = fugoJ.ToText_UseDou(KifuNarabe_KifuWrapper.CurNode(this.ShogiGui));
            }


            return tuginoItte;
        }

        /// <summary>
        /// 出力欄にテキストを出力します。
        /// </summary>
        /// <returns></returns>
        public void WriteLine_Syuturyoku(string text)
        {
            this.txtOutput1.Text = text;
        }

        /// <summary>
        /// コンストラクターです。
        /// </summary>
        public Ui_PnlMain()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.ShogiGui.Timer_Tick();
        }
        public static string input99 = "";

        /// <summary>
        /// 起動直後の流れです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ui_PnlMain_Load(object sender, EventArgs e)
        {
            ILogTag logTag = LogTags.Gui;

            this.setteiFile = new SetteiFile();
            if (!this.SetteiFile.Exists())
            {
                // ファイルが存在しませんでした。

                // 作ります。
                this.SetteiFile.Write();
            }

            if (!this.SetteiFile.Read())
            {
                // 読取に失敗しました。
            }

            // デバッグ
            this.SetteiFile.DebugWrite();


            //----------
            // 棋譜
            //----------
            //
            //      先後や駒など、対局に用いられる事柄、物を事前準備しておきます。
            //

            //----------
            // 駒の並べ方
            //----------
            //
            //      平手に並べます。
            //
            {
                this.ShogiGui.GameViewModel.Kifu.Clear();// 棋譜を空っぽにします。
                this.ShogiGui.GameViewModel.Kifu.SetProperty(KifuTreeImpl.PropName_Startpos, "startpos");//平手の初期局面
                this.ShogiGui.GameViewModel.SetGuiSky(
                    Util_Sky.New_Hirate()//起動直後
                    );
            }


            this.ShogiGui.GameViewModel.Kifu.SetProperty(KifuTreeImpl.PropName_FirstPside, Playerside.P1);



            // 全駒の再描画
            this.ShogiGui.ResponseData = new ResponseImpl();
            this.ShogiGui.ResponseData.RedrawStarlights();

            //----------
            // フェーズ
            //----------
            this.ShogiGui.SetScene(SceneName.SceneB_1TumamitaiKoma);

            //----------
            // 監視
            //----------
            this.gameEngineTimer1.Start();

            //----------
            // 将棋エンジンが、コンソールの振りをします。
            //----------
            //
            //      このメインパネルに、コンソールの振りをさせます。
            //      将棋エンジンがあれば、将棋エンジンの入出力を返すように内部を改造してください。
            //
            TextboxListener.SetTextboxListener(this.ReadLine1, this.WriteLine_Syuturyoku);


            //----------
            // 画面の出力欄
            //----------
            //
            //      出力欄（上下段）を空っぽにします。
            //
            this.WriteLine_Syuturyoku("");



            this.ShogiGui.Response("Launch", logTag);

            // これで、最初に見える画面の準備は終えました。
            // あとは、操作者の入力を待ちます。
        }

        /// <summary>
        /// 描画するのはここです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ui_PnlMain_Paint(object sender, PaintEventArgs e)
        {
            if (null == this.ShogiGui.Shape_PnlTaikyoku)
            {
                goto gt_EndMethod;
            }

            //------------------------------
            // 画面の描画です。
            //------------------------------
            this.ShogiGui.Shape_PnlTaikyoku.Paint(sender, e, this.ShogiGui, LogTags.NarabePaint);

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// マウスが動いたときの挙動です。
        /// 
        ///         マウスが重なったときの、表示物の反応や、将棋データの変更がこの中に書かれています。
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ui_PnlMain_MouseMove(object sender, MouseEventArgs e)
        {
            ILogTag logTag = LogTags.Gui;

            if (null != this.ShogiGui.Shape_PnlTaikyoku)
            {
                // このメインパネルに、何かして欲しいという要求は、ここに入れられます。
                this.ShogiGui.ResponseData = new ResponseImpl();

                // マウスムーブ
                {
                    TimedB timeB = ((TimedB)this.ShogiGui.TimedB);
                    timeB.MouseEventQueue.Enqueue( new MouseEventState( this.ShogiGui.Scene, MouseEventStateName.MouseMove,e.Location,logTag));
                }

                //------------------------------
                // このメインパネルの反応
                //------------------------------
                this.ShogiGui.Response("MouseOperation", logTag);
            }
        }

        /// <summary>
        /// マウスのボタンを押下したときの挙動です。
        /// 
        ///         マウスボタンが押下されたときの、表示物の反応や、将棋データの変更がこの中に書かれています。
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ui_PnlMain_MouseDown(object sender, MouseEventArgs e)
        {
            ILogTag logTag = LogTags.Gui;

            if (null == this.ShogiGui.Shape_PnlTaikyoku)
            {
                goto gt_EndMethod;
            }

            // このメインパネルに、何かして欲しいという要求は、ここに入れられます。
            this.ShogiGui.ResponseData = new ResponseImpl();


            if (e.Button == MouseButtons.Left)
            {
                //------------------------------------------------------------
                // 左ボタン
                //------------------------------------------------------------
                TimedB timeB = ((TimedB)this.ShogiGui.TimedB);
                timeB.MouseEventQueue.Enqueue( new MouseEventState( this.ShogiGui.Scene, MouseEventStateName.MouseLeftButtonDown, e.Location, logTag));
            }
            else if (e.Button == MouseButtons.Right)
            {
                //------------------------------------------------------------
                // 右ボタン
                //------------------------------------------------------------
                TimedB timeB = ((TimedB)this.ShogiGui.TimedB);
                timeB.MouseEventQueue.Enqueue(new MouseEventState(this.ShogiGui.Scene, MouseEventStateName.MouseRightButtonDown, e.Location, logTag));


                //------------------------------
                // このメインパネルの反応
                //------------------------------
                this.ShogiGui.Response("MouseOperation", logTag);

            }
            else
            {
                //------------------------------
                // このメインパネルの反応
                //------------------------------
                this.ShogiGui.Response("MouseOperation", logTag);
            }

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// マウスのボタンが放されたときの挙動です。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ui_PnlMain_MouseUp(object sender, MouseEventArgs e)
        {
            ILogTag logTag = LogTags.Gui;

            // このメインパネルに、何かして欲しいという要求は、ここに入れられます。
            this.ShogiGui.ResponseData = new ResponseImpl();

            //------------------------------
            // マウスボタンが放されたときの、表示物の反応や、将棋データの変更がこの中に書かれています。
            //------------------------------
            if (e.Button == MouseButtons.Left)
            {
                //------------------------------------------------------------
                // 左ボタン
                //------------------------------------------------------------
                TimedB timeB = ((TimedB)this.ShogiGui.TimedB);
                timeB.MouseEventQueue.Enqueue( new MouseEventState( this.ShogiGui.Scene, MouseEventStateName.MouseLeftButtonUp, e.Location, logTag));
            }
            else if (e.Button == MouseButtons.Right)
            {
                //------------------------------------------------------------
                // 右ボタン
                //------------------------------------------------------------
                TimedB timeB = ((TimedB)this.ShogiGui.TimedB);
                timeB.MouseEventQueue.Enqueue(new MouseEventState(this.ShogiGui.Scene, MouseEventStateName.MouseRightButtonUp, e.Location, logTag));
            }
        }

        private void SetInput1Text(string value)
        {
            //System.C onsole.WriteLine("☆セット：" + value);
            this.txtInput1.Text = value;
        }

        private void AppendInput1Text(string value,[CallerMemberName] string memberName = "")
        {
            //System.C onsole.WriteLine("☆アペンド(" + memberName + ")：" + value);
            this.txtInput1.Text += value;
        }

        public enum Mutex
        {
            /// <summary>
            /// ロックがかかっていない
            /// </summary>
            Empty,

            /// <summary>
            /// タイマー
            /// </summary>
            Timer,

            /// <summary>
            /// マウス操作
            /// </summary>
            MouseOperation,

            /// <summary>
            /// 棋譜再生
            /// </summary>
            Saisei,

            /// <summary>
            /// 起動時
            /// </summary>
            Launch
        }
        public Mutex MutexOwner { get; set; }

        /// <summary>
        /// 入力欄の表示・出力欄の表示・再描画
        /// 
        /// このメインパネルに何かして欲しいことがあれば、
        /// RequestForMain に要望を入れて、この関数を呼び出してください。
        ///
        /// 同時には処理できない項目もあります。
        /// </summary>
        /// <param name="response"></param>
        public void Response(
            Mutex mutex, NarabeRoomViewModel shogiGui, ILogTag logTag)
        {
            //------------------------------------------------------------
            // 駒の座標再計算
            //------------------------------------------------------------
            if (shogiGui.ResponseData.Is_RedrawStarlights())
            {
                this.ShogiGui.GameViewModel.GuiSkyConst.Foreach_Starlights((Finger finger, Starlight light, ref bool toBreak) =>
                {
                    Util_InGui.Redraw_KomaLocation(finger, this.ShogiGui, logTag);
                });
            }
            shogiGui.ResponseData.Clear_RedrawStarlights();

            //------------------------------
            // 入力欄の表示
            //------------------------------
            if (shogiGui.ResponseData.CanInputTextFlag)
            {
                // 指定のテキストで上書きします。
                this.SetInput1Text(shogiGui.ResponseData.InputTextString);
            }
            else if (shogiGui.ResponseData.CanAppendInputTextFlag)
            {
                // 指定のテキストを後ろに足します。
                this.AppendInput1Text(shogiGui.ResponseData.AppendInputTextString);
                shogiGui.ResponseData.SetAppendInputTextString( "");//要求の解除
            }

            //------------------------------
            // 出力欄（上・下段）の表示
            //------------------------------
            switch (shogiGui.ResponseData.OutputTxt)
            {
                case ResponseGedanTxt.Clear:
                    {
                        // 出力欄（上下段）を空っぽにします。
                        this.WriteLine_Syuturyoku("");

                        // ログ
                        Logger.WriteLineAddMemo(logTag, "");
                        Logger.WriteLineAddMemo(logTag, "");
                    }
                    break;

                case ResponseGedanTxt.Kifu:
                    {
                        // 出力欄（上下段）に、棋譜を出力します。
                        switch (this.ShogiGui.Shape_PnlTaikyoku.SyuturyokuKirikae)
                        {
                            case SyuturyokuKirikae.Japanese:
                                this.WriteLine_Syuturyoku(KirokuGakari.ToJapaneseKifuText(this.ShogiGui.GameViewModel.Kifu, LogTags.Gui));
                                break;
                            case SyuturyokuKirikae.Sfen:
                                this.WriteLine_Syuturyoku(KirokuGakari.ToSfen_PositionString(this.ShogiGui.GameViewModel.Kifu));
                                break;
                            case SyuturyokuKirikae.Html:
                                this.WriteLine_Syuturyoku(Ui_PnlMain.CreateHtml(this.ShogiGui));
                                break;
                        }

                        // ログ
                        Logger.WriteLineAddMemo(logTag, this.txtOutput1.Text);
                    }
                    break;

                default:
                    // スルー
                    break;
            }

            //------------------------------
            // 再描画
            //------------------------------
            if (shogiGui.ResponseData.IsRedraw())
            {
                this.Refresh();

                shogiGui.ResponseData.ClearRedraw();
            }
        }

        /// <summary>
        /// 出力欄（上段）でキーボードのキーが押されたときの挙動をここに書きます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOutput1_KeyDown(object sender, KeyEventArgs e)
        {
            AspectOriented_TextBox.KeyDown_SelectAll(sender, e);
            ////------------------------------
            //// [Ctrl]+[A] で、全選択します。
            ////------------------------------
            //if (e.KeyCode == System.Windows.Forms.Keys.A & e.Control == true)
            //{
            //    ((TextBox)sender).SelectAll();
            //} 
        }

        /// <summary>
        /// 入力欄でキーボードのキーが押されたときの挙動をここに書きます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInput1_KeyDown(object sender, KeyEventArgs e)
        {
            AspectOriented_TextBox.KeyDown_SelectAll(sender, e);
            ////------------------------------
            //// [Ctrl]+[A] で、全選択します。
            ////------------------------------
            //if (e.KeyCode == System.Windows.Forms.Keys.A & e.Control == true)
            //{
            //    ((TextBox)sender).SelectAll();
            //} 
        }

        /// <summary>
        /// HTML出力。（これは作者のホームページ用に書かれています）
        /// </summary>
        public static string CreateHtml(NarabeRoomViewModel shogiGui)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<div style=\"position:relative; left:0px; top:0px; border:solid 1px black; width:250px; height:180px;\">");

            // 後手の持ち駒
            sb.AppendLine("    <div style=\"position:absolute; left:0px; top:2px; width:30px;\">");
            sb.AppendLine("        △後手");
            sb.AppendLine("        <div style=\"margin-top:10px; width:30px;\">");
            sb.Append("            ");

            SkyConst siteiSky = shogiGui.GameViewModel.GuiSkyConst;

            siteiSky.Foreach_Starlights((Finger finger, Starlight ml, ref bool toBreak) =>
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(ml.Now);


                if (Util_Masu.GetOkiba(koma.Masu) == Okiba.Gote_Komadai)
                {
                    sb.Append(KomaSyurui14Array.Fugo[(int)Haiyaku184Array.Syurui(koma.Haiyaku)]);
                }
            });

            sb.AppendLine("        </div>");
            sb.AppendLine("    </div>");

            // 将棋盤
            sb.AppendLine("    <div style=\"position:absolute; left:30px; top:2px; width:182px;\">");
            sb.AppendLine("        <table>");
            for (int dan = 1; dan <= 9; dan++)
            {
                sb.Append("        <tr>");
                for (int suji = 9; 1 <= suji; suji--)
                {
                    bool isSpace = true;

                    siteiSky.Foreach_Starlights((Finger finger, Starlight ml, ref bool toBreak) =>
                    {
                        RO_Star_Koma koma2 = Util_Koma.AsKoma(ml.Now);


                        int suji2;
                        Util_MasuNum.MasuToSuji(koma2.Masu, out suji2);

                        int dan2;
                        Util_MasuNum.MasuToDan(koma2.Masu, out dan2);

                        if (
                            Util_Masu.GetOkiba(koma2.Masu) == Okiba.ShogiBan //盤上
                            && suji2 == suji
                            && dan2 == dan
                        )
                        {
                            if (Playerside.P2 == koma2.Pside)
                            {
                                sb.Append("<td><span class=\"koma2x\">");
                                sb.Append(KomaSyurui14Array.Fugo[(int)Haiyaku184Array.Syurui(koma2.Haiyaku)]);
                                sb.Append("</span></td>");
                                isSpace = false;
                            }
                            else
                            {
                                sb.Append("<td><span class=\"koma1x\">");
                                sb.Append(KomaSyurui14Array.Fugo[(int)Haiyaku184Array.Syurui(koma2.Haiyaku)]);
                                sb.Append("</span></td>");
                                isSpace = false;
                            }
                        }


                    });

                    if (isSpace)
                    {
                        sb.Append("<td>　</td>");
                    }
                }

                sb.AppendLine("</tr>");
            }
            sb.AppendLine("        </table>");
            sb.AppendLine("    </div>");

            // 先手の持ち駒
            sb.AppendLine("    <div style=\"position:absolute; left:215px; top:2px; width:30px;\">");
            sb.AppendLine("        ▲先手");
            sb.AppendLine("        <div style=\"margin-top:10px; width:30px;\">");
            sb.Append("            ");

            siteiSky.Foreach_Starlights((Finger finger, Starlight ml, ref bool toBreak) =>
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(ml.Now);

                if (Util_Masu.GetOkiba(koma.Masu) == Okiba.Sente_Komadai)
                {
                    sb.Append(KomaSyurui14Array.Fugo[(int)Haiyaku184Array.Syurui(koma.Haiyaku)]);
                }
            });

            sb.AppendLine("        </div>");
            sb.AppendLine("    </div>");

            //
            sb.AppendLine("</div>");

            return sb.ToString();
        }
    }
}
