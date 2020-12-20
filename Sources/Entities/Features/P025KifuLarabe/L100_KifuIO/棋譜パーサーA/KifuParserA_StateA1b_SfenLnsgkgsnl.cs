using System;
using Grayscale.Kifuwarazusa.Entities.Features.Gui;
using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.P025_KifuLarabe.L00060_KifuParser;
using Grayscale.P025_KifuLarabe.L012_Common;

namespace Grayscale.P025_KifuLarabe.L100_KifuIO
{
    /// <summary>
    /// 指定局面から始める配置です。
    /// 
    /// 「lnsgkgsnl/1r5b1/ppppppppp/9/9/6P2/PPPPPP1PP/1B5R1/LNSGKGSNL w - 1」といった文字の読込み
    /// </summary>
    public class KifuParserA_StateA1b_SfenLnsgkgsnl : KifuParserA_State
    {


        public static KifuParserA_StateA1b_SfenLnsgkgsnl GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserA_StateA1b_SfenLnsgkgsnl();
            }

            return instance;
        }
        private static KifuParserA_StateA1b_SfenLnsgkgsnl instance;



        private KifuParserA_StateA1b_SfenLnsgkgsnl()
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

            StartposImporter startposImporter;
            string restText;

            bool successful = StartposImporter.TryParse(
                genjo.InputLine,
                out startposImporter,
                out restText
                );

            if (successful)
            {
                genjo.InputLine = restText;

                // SFENの解析結果を渡すので、
                // その解析結果をどう使うかは、委譲します。
                owner.Delegate_OnChangeSky_Im_Srv(
                    roomViewModel,
                    startposImporter,
                    genjo
                    );

                nextState = KifuParserA_StateA2_SfenMoves.GetInstance();
            }
            else
            {
                // 解析に失敗しました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                genjo.ToBreak = true;
            }

            return genjo.InputLine;
        }

    }
}
