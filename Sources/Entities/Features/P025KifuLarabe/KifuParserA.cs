﻿using System.Runtime.CompilerServices;
using Grayscale.Kifuwarazusa.Entities.Features.Gui;

namespace Grayscale.Kifuwarazusa.Entities.Features
{
    public interface KifuParserA
    {
        KifuParserA_State State { get; set; }

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
            IRoomViewModel roomViewModel,
            KifuParserA_Genjo genjo
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            );
    }
}
