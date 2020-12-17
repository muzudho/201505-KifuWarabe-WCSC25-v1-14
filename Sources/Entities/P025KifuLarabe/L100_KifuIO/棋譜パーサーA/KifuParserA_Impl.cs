//スプライト番号
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L012_Common;
using System;
using System.Runtime.CompilerServices;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P025_KifuLarabe.L00060_KifuParser;
using Grayscale.Kifuwarazusa.Entities;

namespace Grayscale.P025_KifuLarabe.L100_KifuIO
{

    public delegate void DELEGATE_ChangeSky_Im_Srv(
        ShogiGui_Base shogiGui_Base,
        StartposImporter startposImporter,
        KifuParserA_Genjo genjo,
        KifuParserA_Log log
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
            KifuParserA_Genjo genjo,
            KifuParserA_Log log
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
            ShogiGui_Base shogiGui_Base,
            KifuParserA_Genjo genjo,
            KifuParserA_Log log
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {

            try
            {
                Logger.WriteLineAddMemo(log.LogTag, "┏━━━━━┓");
                Logger.WriteLineAddMemo(log.LogTag, "わたしは　" + this.State.GetType().Name + "　の　Execute_Step　だぜ☆　：　呼出箇所＝" + memberName + "." + sourceFilePath + "." + sourceLineNumber);

                KifuParserA_State nextState;
                genjo.InputLine = this.State.Execute(
                    ref result,
                    shogiGui_Base,
                    out nextState, this,
                    genjo, log);
                this.State = nextState;

            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                string message = this.GetType().Name + "#Execute_Step：" + ex.GetType().Name + "：" + ex.Message;
                Logger.Error.WriteLine_Error( message);
            }

            return genjo.InputLine;
        }

        /// <summary>
        /// 最初から最後まで実行します。（きふわらべCOMP用）
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="kifu"></param>
        /// <param name="larabeLogger"></param>
        public void Execute_All(
            ref KifuParserA_Result result,
            ShogiGui_Base obj_shogiGui_Base,
            KifuParserA_Genjo genjo,
            KifuParserA_Log log
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {

            try
            {
                Logger.WriteLineAddMemo(log.LogTag, "┏━━━━━━━━━━┓");
                Logger.WriteLineAddMemo(log.LogTag, "わたしは　" + this.State.GetType().Name + "　の　Execute_All　だぜ☆　：　呼出箇所＝" + memberName + "." + sourceFilePath + "." + sourceLineNumber);

                KifuParserA_State nextState = this.State;

                while (!genjo.ToBreak)
                {
                    genjo.InputLine = this.State.Execute(
                        ref result,
                        obj_shogiGui_Base,
                        out nextState, this,
                        genjo, log);
                    this.State = nextState;
                }
            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                string message = this.GetType().Name + "#Execute_All：" + ex.GetType().Name + "：" + ex.Message;
                Logger.Error.WriteLine_Error( message);
            }


        }

    }
}
