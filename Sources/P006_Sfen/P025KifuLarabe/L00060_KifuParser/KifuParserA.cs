using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Diagnostics;

using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P025_KifuLarabe.L100_KifuIO;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;

namespace Grayscale.P025_KifuLarabe.L00060_KifuParser
{
    public interface KifuParserA
    {

        DELEGATE_ChangeSky_Im_Srv Delegate_OnChangeSky_Im_Srv { get; set; }


        /// <summary>
        /// １ステップずつ実行します。
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="kifu"></param>
        /// <param name="larabeLogger"></param>
        /// <returns></returns>
        string Execute_Step(
            ref KifuParserA_Result result,
            ShogiGui_Base obj_shogiGui_Base,
            KifuParserA_Genjo genjo,
            KifuParserA_Log log
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            );

        /// <summary>
        /// 最初から最後まで実行します。（きふわらべCOMP用）
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="kifu"></param>
        /// <param name="larabeLogger"></param>
        void Execute_All(
            ref KifuParserA_Result result,
            ShogiGui_Base obj_shogiGui_Base,
            KifuParserA_Genjo genjo,
            KifuParserA_Log log
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            );



    }
}
