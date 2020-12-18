using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Windows.Forms;
using Grayscale.P025_KifuLarabe;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L007_Random;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;

using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P025_KifuLarabe.L00060_KifuParser;
using Grayscale.Kifuwarazusa.Entities;

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
            ShogiGui_Base shogiGui_Base,
            out KifuParserA_State nextState,
            KifuParserA owner,
            KifuParserA_Genjo genjo,
            KifuParserA_Log log
            )
        {
            // 現局面。
            SkyConst src_Sky = shogiGui_Base.Model_PnlTaikyoku.Kifu.CurNode.Value.ToKyokumenConst;
//            Debug.Assert(!Util_MasuNum.OnKomabukuro((int)((RO_Star_Koma)src_Sky.StarlightIndexOf((Finger)0).Now).Masu), "カレント、駒が駒袋にあった。");

            bool isHonshogi = true;//FIXME:暫定
            int tesumi_yomiGenTeban_forLog = shogiGui_Base.Model_PnlTaikyoku.Kifu.CountTesumi(shogiGui_Base.Model_PnlTaikyoku.Kifu.CurNode);// 0;//FIXME:暫定。読み進めている現在の手目
            //            Debug.Assert(!Util_MasuNum.OnKomabukuro((int)((RO_Star_Koma)kifu.NodeAt(tesumi_yomiGenTeban_forLog).Value.ToKyokumenConst.StarlightIndexOf((Finger)0).Now).Masu), "[" + tesumi_yomiGenTeban_forLog + "]手目、駒が駒袋にあった。");

            nextState = this;

            try
            {
                if (0 < genjo.InputLine.Trim().Length)
                {
                    ShootingStarlightable nextTe = Util_Sky.NullObjectMove;
                    string rest;

                    try
                    {
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
                            !(str1=="" && str2=="" && str3=="" && str4=="" && str5=="")
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
                                shogiGui_Base.Model_PnlTaikyoku.Kifu,
                                log.Hint+"_SFENパース1",
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
                                        shogiGui_Base.Model_PnlTaikyoku.Kifu
                                        );
                                }

                            }
                            else
                            {
                                //「6g6f」形式でもなかった☆

                                Logger.WriteLineAddMemo(log.LogTag, "（＾△＾）「" + genjo.InputLine + "」vs【" + this.GetType().Name + "】　：　！？　次の一手が読めない☆　inputLine=[" + genjo.InputLine + "]");
                                genjo.ToBreak = true;
                                goto gt_EndMethod;
                            }

                        }

                        genjo.InputLine = rest;
                    }
                    catch (Exception ex)
                    {
                        // エラーが起こりました。
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                        // どうにもできないので  ログだけ取って無視します。
                        string message = this.GetType().Name + "#Execute（A）：" + ex.GetType().Name + "：" + ex.Message;
                        Logger.WriteLineError(LogTags.Error, message);
                    }




                    if (null != nextTe)
                    {
                        Finger figMovedKoma = Fingers.Error_1;
                        Finger figFoodKoma = Fingers.Error_1;

                        try
                        {
                            Application.DoEvents(); // 時間のかかる処理の間にはこれを挟みます。

                            //------------------------------
                            // ★棋譜読込専用  駒移動
                            //------------------------------

                            Logger.WriteLineAddMemo(log.LogTag, "一手指し開始　：　残りの符号つ「" + genjo.InputLine + "」");
                            bool isBack = false;
                            Node<ShootingStarlightable, KyokumenWrapper> out_newNode_OrNull;
                            KifuIO.Ittesasi(
                                nextTe,
                                shogiGui_Base.Model_PnlTaikyoku.Kifu,
                                isBack,
                                out figMovedKoma,
                                out figFoodKoma,
                                out out_newNode_OrNull,//変更した現局面が、ここに入る。
                                log.LogTag
                                );
                            result.Out_newNode_OrNull = out_newNode_OrNull;
                            Logger.WriteLineAddMemo(log.LogTag, Util_Sky.Json_1Sky(
                                src_Sky, "一手指し終了",
                                log.Hint + "_SFENパース2",
                                tesumi_yomiGenTeban_forLog//読み進めている現在の手目
                                ));

                        }
                        catch (Exception ex)
                        {
                            //>>>>> エラーが起こりました。

                            // どうにもできないので  ログだけ取って無視します。
                            string message = this.GetType().Name + "#Execute（B）：" + ex.GetType().Name + "：" + ex.Message;
                            Logger.WriteLineError(log.LogTag,message);
                        }

                    }
                    else
                    {
                        genjo.ToBreak = true;
                        throw new Exception($"＼（＾ｏ＾）／teMoveオブジェクトがない☆！　inputLine=[{genjo.InputLine}]");
                    }
                }
                else
                {
                    Logger.WriteLineAddMemo(log.LogTag,"（＾△＾）現局面まで進んだのかだぜ☆？\n" + Util_Sky.Json_1Sky(
                        src_Sky, "棋譜パース",
                        log.Hint + "_SFENパース3",
                        tesumi_yomiGenTeban_forLog//読み進めている現在の手目
                        ));
                    genjo.ToBreak = true;
                }
            }
            catch (Exception ex)
            {
                // エラーが起こりました。
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // どうにもできないので  ログだけ取って無視します。
                string message = this.GetType().Name + "#Execute：" + ex.GetType().Name + "：" + ex.Message;
                Logger.WriteLineError(log.LogTag,message);
            }

        gt_EndMethod:
            return genjo.InputLine;
        }

    }
}
