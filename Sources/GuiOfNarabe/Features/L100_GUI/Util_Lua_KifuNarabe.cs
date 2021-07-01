namespace Grayscale.Kifuwarazusa.GuiOfNarabe.Features
{
    using Grayscale.Kifuwarazusa.Entities.Features;
#if USING_LUA
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.UseCases.Features;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Features;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Gui;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Features;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Features;
using System;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Logging;
using NLua;
#else
    using Grayscale.Kifuwarazusa.GuiOfNarabe.Gui;
    using Grayscale.Kifuwarazusa.UseCases.Features;
#endif

    public abstract class Util_Lua_KifuNarabe
    {
#if USING_LUA
        private static Lua lua;
#endif

        public static NarabeRoomViewModel ShogiGui { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="luaFuncName">実行したいLua関数の名前。</param>
        public static void Perform(string luaFuncName)
        {

#if USING_LUA
            using (Util_Lua_KifuNarabe.lua = new Lua())
            {
                // 初期化
                lua.LoadCLRPackage();


                //
                // 関数の登録
                //

                // Lua「debugOut("あー☆")」
                // ↓
                // C#「C onsole.WriteLine("あー☆")」
                lua.RegisterFunction("debugOut", typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));

                // Lua「screen_refresh()」
                // ↓
                // C#「Util_Lua.Screen_Redraw()」
                lua.RegisterFunction("screen_refresh", typeof(Util_Lua_KifuNarabe).GetMethod("Screen_Redraw", new Type[] { }));

                // Lua「screen_clearOutput()」
                // ↓
                // C#「Util_Lua.screen_redrawStarlights()」
                lua.RegisterFunction("screen_refreshStarlights", typeof(Util_Lua_KifuNarabe).GetMethod("Screen_RedrawStarlights", new Type[] { }));



                // Lua「inputBox_play()」
                // ↓
                // C#「Util_Lua.InputBox_Play()」
                lua.RegisterFunction("inputBox_play", typeof(Util_Lua_KifuNarabe).GetMethod("InputBox_Play", new Type[] { }));



                // Lua「outputBox_clear()」
                // ↓
                // C#「Util_Lua.Screen_ClearOutput()」
                lua.RegisterFunction("outputBox_clear", typeof(Util_Lua_KifuNarabe).GetMethod("OutputBox_Clear", new Type[] { }));



                // Lua「kifu_clear()」
                // ↓
                // C#「Util_Lua.Kifu_Clear()」
                lua.RegisterFunction("kifu_clear", typeof(Util_Lua_KifuNarabe).GetMethod("Kifu_Clear", new Type[] { }));


                //----------------------------------------------------------------------------------------------------

                Util_Lua_KifuNarabe.lua.DoFile("../../Profile/Data/lua/KifuNarabe/data_gui.lua");//固定
                Util_Lua_KifuNarabe.lua.GetFunction(luaFuncName).Call();

                // FIXME:Close()でエラーが起こってしまう。
                //Util_Lua_KifuNarabe.lua.Close();

                //----------------------------------------------------------------------------------------------------

            }
#endif
        }


        public static void Screen_Redraw()
        {
            Util_Lua_KifuNarabe.ShogiGui.ResponseData.ToRedraw();
        }

        public static void Screen_RedrawStarlights()
        {
            Util_Lua_KifuNarabe.ShogiGui.ResponseData.RedrawStarlights();// 駒の再描画要求
        }

        public static void InputBox_Play()
        {
            // [再生]タイマー開始☆
            ((TimedC)Util_Lua_KifuNarabe.ShogiGui.TimedC).SaiseiEventQueue.Enqueue(new SaiseiEventState(SaiseiEventStateName.Start));
        }

        public static void OutputBox_Clear()
        {
            Util_Lua_KifuNarabe.ShogiGui.ResponseData.OutputTxt = ResponseGedanTxt.Clear;
        }

        public static void Kifu_Clear()
        {
            Util_Lua_KifuNarabe.ClearKifu(Util_Lua_KifuNarabe.ShogiGui, Util_Lua_KifuNarabe.ShogiGui.ResponseData);
        }





        /// <summary>
        /// ************************************************************************************************************************
        /// 将棋盤の上の駒を、全て駒袋に移動します。 [クリアー]
        /// ************************************************************************************************************************
        /// </summary>
        public static void ClearKifu(NarabeRoomViewModel shogiGui, Response response)
        {
            shogiGui.GameViewModel.Kifu.Clear();// 棋譜を空っぽにします。

            SkyBuffer buffer_Sky = new SkyBuffer(shogiGui.GameViewModel.GuiSkyConst);

            int figKoma;

            // 先手
            figKoma = (int)Finger_Honshogi.SenteOh;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(/*figKoma,*/ new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro01, PieceType.K))); //先手王
            figKoma = (int)Finger_Honshogi.GoteOh;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro02, PieceType.K))); //後手王

            figKoma = (int)Finger_Honshogi.Hi1;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro03, PieceType.R))); //飛
            figKoma = (int)Finger_Honshogi.Hi2;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro04, PieceType.R)));

            figKoma = (int)Finger_Honshogi.Kaku1;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro05, PieceType.B))); //角
            figKoma = (int)Finger_Honshogi.Kaku2;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro06, PieceType.B)));

            figKoma = (int)Finger_Honshogi.Kin1;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro07, PieceType.G))); //金
            figKoma = (int)Finger_Honshogi.Kin2;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro08, PieceType.G)));
            figKoma = (int)Finger_Honshogi.Kin3;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro09, PieceType.G)));
            figKoma = (int)Finger_Honshogi.Kin4;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro10, PieceType.G)));

            figKoma = (int)Finger_Honshogi.Gin1;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro11, PieceType.S))); //銀
            figKoma = (int)Finger_Honshogi.Gin2;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro12, PieceType.S)));
            figKoma = (int)Finger_Honshogi.Gin3;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro13, PieceType.S)));
            figKoma = (int)Finger_Honshogi.Gin4;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro14, PieceType.S)));

            figKoma = (int)Finger_Honshogi.Kei1;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro15, PieceType.N))); //桂
            figKoma = (int)Finger_Honshogi.Kei2;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro16, PieceType.N)));
            figKoma = (int)Finger_Honshogi.Kei3;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro17, PieceType.N)));
            figKoma = (int)Finger_Honshogi.Kei4;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro18, PieceType.N)));

            figKoma = (int)Finger_Honshogi.Kyo1;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro19, PieceType.L))); //香
            figKoma = (int)Finger_Honshogi.Kyo2;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro20, PieceType.L)));
            figKoma = (int)Finger_Honshogi.Kyo3;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro21, PieceType.L)));
            figKoma = (int)Finger_Honshogi.Kyo4;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro22, PieceType.L)));

            figKoma = (int)Finger_Honshogi.Fu1;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro23, PieceType.P))); //歩
            figKoma = (int)Finger_Honshogi.Fu2;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro24, PieceType.P)));
            figKoma = (int)Finger_Honshogi.Fu3;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro25, PieceType.P)));
            figKoma = (int)Finger_Honshogi.Fu4;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro26, PieceType.P)));
            figKoma = (int)Finger_Honshogi.Fu5;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro27, PieceType.P)));
            figKoma = (int)Finger_Honshogi.Fu6;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro28, PieceType.P)));
            figKoma = (int)Finger_Honshogi.Fu7;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro29, PieceType.P)));
            figKoma = (int)Finger_Honshogi.Fu8;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro30, PieceType.P)));
            figKoma = (int)Finger_Honshogi.Fu9;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro31, PieceType.P)));

            figKoma = (int)Finger_Honshogi.Fu10;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro32, PieceType.P))); //歩
            figKoma = (int)Finger_Honshogi.Fu11;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro33, PieceType.P)));
            figKoma = (int)Finger_Honshogi.Fu12;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro34, PieceType.P)));
            figKoma = (int)Finger_Honshogi.Fu13;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro35, PieceType.P)));
            figKoma = (int)Finger_Honshogi.Fu14;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro36, PieceType.P)));
            figKoma = (int)Finger_Honshogi.Fu15;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro37, PieceType.P)));
            figKoma = (int)Finger_Honshogi.Fu16;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro38, PieceType.P)));
            figKoma = (int)Finger_Honshogi.Fu17;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro39, PieceType.P)));
            figKoma = (int)Finger_Honshogi.Fu18;
            buffer_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro40, PieceType.P)));


            {
                KifuNode newNode = new KifuNodeImpl(
                            Util_Sky.NullObjectMove,//ルートなので
                            new KyokumenWrapper(new SkyConst(buffer_Sky)),
                            Playerside.P2
                        );

                Util_InServer.SetCurNode_Srv(shogiGui, newNode);
                response.ToRedraw();

                shogiGui.GameViewModel.Kifu.SetProperty(KifuTreeImpl.PropName_Startpos, "9/9/9/9/9/9/9/9/9 b K1R1B1G2S2N2L2P9 k1r1b1g2s2n2l2p9 1");
            }
        }


    }
}
