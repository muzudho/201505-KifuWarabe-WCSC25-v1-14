using System;
using Grayscale.Kifuwarazusa.Entities.Features.Gui;
using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.P025_KifuLarabe.L00060_KifuParser;

namespace Grayscale.P025_KifuLarabe.L100_KifuIO
{
    /// <summary>
    /// 平手の初期配置です。
    /// </summary>
    public class KifuParserA_StateA1a_SfenStartpos : KifuParserA_State
    {


        public static KifuParserA_StateA1a_SfenStartpos GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserA_StateA1a_SfenStartpos();
            }

            return instance;
        }
        private static KifuParserA_StateA1a_SfenStartpos instance;



        private KifuParserA_StateA1a_SfenStartpos()
        {
        }


        public string Execute(
            ref KifuParserA_Result result,
            IRoomViewModel roomViewModel,
            out KifuParserA_State nextState,
            KifuParserA owner,
            KifuParserA_Genjo genjo,
            KifuParserA_Log log
            )
        {
            nextState = this;

            if (genjo.InputLine.StartsWith("moves"))
            {
                //>>>>> 棋譜が始まります。

                Logger.Trace("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　ｳﾑ☆　moves 分かるぜ☆");

                genjo.InputLine = genjo.InputLine.Substring("moves".Length);
                genjo.InputLine = genjo.InputLine.Trim();


                nextState = KifuParserA_StateA2_SfenMoves.GetInstance();
            }
            else
            {
                Logger.Trace("＼（＾ｏ＾）／「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　movesがない☆！　終わるぜ☆");
                genjo.ToBreak = true;
            }

            return genjo.InputLine;
        }

    }
}
