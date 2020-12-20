using System;
using Grayscale.Kifuwarazusa.Entities.Features.Gui;
using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L00060_KifuParser;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;

namespace Grayscale.P025_KifuLarabe.L100_KifuIO
{

    /// <summary>
    /// 「position」を読込みました。
    /// </summary>
    public class KifuParserA_StateA1_SfenPosition : KifuParserA_State
    {


        public static KifuParserA_StateA1_SfenPosition GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserA_StateA1_SfenPosition();
            }

            return instance;
        }
        private static KifuParserA_StateA1_SfenPosition instance;


        private KifuParserA_StateA1_SfenPosition()
        {
        }


        public string Execute(
            ref KifuParserA_Result result,
            IRoomViewModel roomViewModel,
            out KifuParserA_State nextState,
            KifuParserA owner,
            KifuParserA_Genjo genjo
            )
        {
            nextState = this;

            if (genjo.InputLine.StartsWith("startpos"))
            {
                // 平手の初期配置です。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                Logger.Trace("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　平手のようなんだぜ☆");

                genjo.InputLine = genjo.InputLine.Substring("startpos".Length);
                genjo.InputLine = genjo.InputLine.Trim();

                {
                    roomViewModel.GameViewModel.Kifu.Clear();// 棋譜を空っぽにします。

                    roomViewModel.GameViewModel.Kifu.GetRoot().Value.SetKyokumen(new SkyConst(Util_Sky.New_Hirate()));//SFENのstartpos解析時
                    roomViewModel.GameViewModel.Kifu.SetProperty(KifuTreeImpl.PropName_Startpos, "startpos");//平手の初期局面
                }

                nextState = KifuParserA_StateA1a_SfenStartpos.GetInstance();
            }
            else
            {
                Logger.Trace("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　局面の指定のようなんだぜ☆");
                nextState = KifuParserA_StateA1b_SfenLnsgkgsnl.GetInstance();
            }

            return genjo.InputLine;
        }

    }
}
