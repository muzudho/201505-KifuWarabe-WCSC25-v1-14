using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using Grayscale.P025_KifuLarabe;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L006_SfenEx;
using Grayscale.P025_KifuLarabe.L007_Random;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P025_KifuLarabe.L100_KifuIO;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P025_KifuLarabe.L00060_KifuParser;
using Grayscale.Kifuwarazusa.Entities;

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
            ShogiGui_Base obj_shogiGui_Base,
            out KifuParserA_State nextState,
            KifuParserA owner,
            KifuParserA_Genjo genjo,
            KifuParserA_Log log
            )
        {
            nextState = this;

            try
            {
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
                        obj_shogiGui_Base,
                        startposImporter,
                        genjo,
                        log
                        );

                    nextState = KifuParserA_StateA2_SfenMoves.GetInstance();
                }
                else
                {
                    // 解析に失敗しました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    genjo.ToBreak = true;
                }

            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                string message = this.GetType().Name + "#Execute：" + ex.GetType().Name + "：" + ex.Message;
                Logger.WriteLineError(LogTags.Error, message);
            }

            return genjo.InputLine;
        }

    }
}
