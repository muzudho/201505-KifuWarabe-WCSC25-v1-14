using System;
using System.Windows.Forms;
using Grayscale.Kifuwarazusa.Entities.Features.Gui;
using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L00060_KifuParser;
using Grayscale.P025_KifuLarabe.L012_Common;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P025_KifuLarabe.L100_KifuIO
{
    /// <summary>
    /// 「moves」を読込みました。
    /// </summary>
    public class KifuParserA_StateA2_SfenMoves : KifuParserA_State
    {
        public static KifuParserA_StateA2_SfenMoves GetInstance()
        {
            if (null == instance)
            {
                instance = new KifuParserA_StateA2_SfenMoves();
            }

            return instance;
        }
        private static KifuParserA_StateA2_SfenMoves instance;

        private KifuParserA_StateA2_SfenMoves()
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
            // 現局面。
            SkyConst src_Sky = roomViewModel.GameViewModel.Kifu.CurNode.Value.ToKyokumenConst;
            //            Debug.Assert(!Util_MasuNum.OnKomabukuro((int)((RO_Star_Koma)src_Sky.StarlightIndexOf((Finger)0).Now).Masu), "カレント、駒が駒袋にあった。");

            bool isHonshogi = true;//FIXME:暫定
            int tesumi_yomiGenTeban_forLog = roomViewModel.GameViewModel.Kifu.CountTesumi(roomViewModel.GameViewModel.Kifu.CurNode);// 0;//FIXME:暫定。読み進めている現在の手目
            //            Debug.Assert(!Util_MasuNum.OnKomabukuro((int)((RO_Star_Koma)kifu.NodeAt(tesumi_yomiGenTeban_forLog).Value.ToKyokumenConst.StarlightIndexOf((Finger)0).Now).Masu), "[" + tesumi_yomiGenTeban_forLog + "]手目、駒が駒袋にあった。");

            nextState = this;

            if (0 < genjo.InputLine.Trim().Length)
            {
                ShootingStarlightable nextTe = Util_Sky.NullObjectMove;
                string rest;

                //「6g6f」形式と想定して、１手だけ読込み
                string str1;
                string str2;
                string str3;
                string str4;
                string str5;
                string str6;
                string str7;
                string str8;
                string str9;
                if (KifuIO_MovesParsers.ParseSfen_FromText(
                    genjo.InputLine, out str1, out str2, out str3, out str4, out str5, out rest)
                    &&
                    !(str1 == "" && str2 == "" && str3 == "" && str4 == "" && str5 == "")
                    )
                {

                    KifuIO_MovesExecuter.ExecuteSfenMoves_FromTextSub(
                        isHonshogi,
                        str1,  //123456789 か、 PLNSGKRB
                        str2,  //abcdefghi か、 *
                        str3,  //123456789
                        str4,  //abcdefghi
                        str5,  //+
                        out nextTe,
                        roomViewModel.GameViewModel.Kifu,
                        "_SFENパース1",
                        tesumi_yomiGenTeban_forLog
                        );
                }
                else
                {
                    //>>>>> 「6g6f」形式ではなかった☆

                    //「▲６六歩」形式と想定して、１手だけ読込み
                    if (KifuIO_MovesParsers.ParseJfugo_FromText(
                        genjo.InputLine, out str1, out str2, out str3, out str4, out str5, out str6, out str7, out str8, out str9, out rest))
                    {
                        if (!(str1 == "" && str2 == "" && str3 == "" && str4 == "" && str5 == "" && str6 == "" && str7 == "" && str8 == "" && str9 == ""))
                        {
                            KifuIO_MovesExecuter.ExecuteJfugo_FromTextSub(
                                str1,  //▲△
                                str2,  //123…9、１２３…９、一二三…九
                                str3,  //123…9、１２３…９、一二三…九
                                str4,  // “同”
                                str5,  //(歩|香|桂|…
                                str6,           // 右|左…
                                str7,  // 上|引
                                str8, //成|不成
                                str9,  //打
                                out nextTe,
                                roomViewModel.GameViewModel.Kifu
                                );
                        }

                    }
                    else
                    {
                        //「6g6f」形式でもなかった☆

                        Logger.Trace("（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　！？　次の一手が読めない☆　inputLine=[" + genjo.InputLine + "]");
                        genjo.ToBreak = true;
                        goto gt_EndMethod;
                    }

                }

                genjo.InputLine = rest;




                if (null != nextTe)
                {
                    Finger figMovedKoma = Fingers.Error_1;
                    Finger figFoodKoma = Fingers.Error_1;

                    Application.DoEvents(); // 時間のかかる処理の間にはこれを挟みます。

                    //------------------------------
                    // ★棋譜読込専用  駒移動
                    //------------------------------

                    Logger.Trace("一手指し開始　：　残りの符号つ「" + genjo.InputLine + "」");
                    bool isBack = false;
                    Node<ShootingStarlightable, KyokumenWrapper> out_newNode_OrNull;
                    KifuIO.Ittesasi(
                        nextTe,
                        roomViewModel.GameViewModel.Kifu,
                        isBack,
                        out figMovedKoma,
                        out figFoodKoma,
                        out out_newNode_OrNull//変更した現局面が、ここに入る。
                        );
                    result.Out_newNode_OrNull = out_newNode_OrNull;
                    Logger.Trace(Util_Sky.Json_1Sky(
                        src_Sky, "一手指し終了",
                        "_SFENパース2",
                        tesumi_yomiGenTeban_forLog//読み進めている現在の手目
                        ));


                }
                else
                {
                    genjo.ToBreak = true;
                    throw new Exception($"＼（＾ｏ＾）／teMoveオブジェクトがない☆！　inputLine=[{genjo.InputLine}]");
                }
            }
            else
            {
                Logger.Trace("（＾△＾）現局面まで進んだのかだぜ☆？\n" + Util_Sky.Json_1Sky(
                    src_Sky, "棋譜パース",
                    "_SFENパース3",
                    tesumi_yomiGenTeban_forLog//読み進めている現在の手目
                    ));
                genjo.ToBreak = true;
            }

        gt_EndMethod:
            return genjo.InputLine;
        }

    }
}
