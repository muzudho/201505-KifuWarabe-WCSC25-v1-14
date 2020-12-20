using System;
using System.Runtime.CompilerServices;
using Grayscale.Kifuwarazusa.Entities.Features.Gui;
using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.P025_KifuLarabe.L00060_KifuParser;
using Grayscale.P025_KifuLarabe.L012_Common;

namespace Grayscale.P025_KifuLarabe.L100_KifuIO
{

    public delegate void DELEGATE_ChangeSky_Im_Srv(
        IRoomViewModel roomViewModel,
        StartposImporter startposImporter,
        KifuParserA_Genjo genjo
    );

    /// <summary>
    /// 変化なし
    /// </summary>
    public class KifuParserA_Impl : KifuParserA
    {

        public KifuParserA_State State { get; set; }


        public DELEGATE_ChangeSky_Im_Srv Delegate_OnChangeSky_Im_Srv { get; set; }


        public KifuParserA_Impl()
        {
            // 初期状態＝ドキュメント
            this.State = KifuParserA_StateA0_Document.GetInstance();

            this.Delegate_OnChangeSky_Im_Srv = this.Empty_OnChangeSky_Im_Srv;
        }


        private void Empty_OnChangeSky_Im_Srv(
            object obj_shogiGui,
            StartposImporter startposImporter,
            KifuParserA_Genjo genjo
            )
        {
        }

        /// <summary>
        /// １ステップずつ実行します。
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="kifu"></param>
        /// <param name="larabeLogger"></param>
        /// <returns></returns>
        public string Execute_Step(
            ref KifuParserA_Result result,
            IRoomViewModel roomViewModel,
            KifuParserA_Genjo genjo
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {

            Logger.Trace( "┏━━━━━┓");
            Logger.Trace( "わたしは　" + this.State.GetType().Name + "　の　Execute_Step　だぜ☆　：　呼出箇所＝" + memberName + "." + sourceFilePath + "." + sourceLineNumber);

            KifuParserA_State nextState;
            genjo.InputLine = this.State.Execute(
                ref result,
                roomViewModel,
                out nextState, this,
                genjo);
            this.State = nextState;

            return genjo.InputLine;
        }
    }
}
