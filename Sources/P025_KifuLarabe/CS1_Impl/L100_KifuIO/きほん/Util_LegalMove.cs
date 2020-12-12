using Grayscale.P025_KifuLarabe;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L002_GraphicLog;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L007_Random;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P025_KifuLarabe.L025_Play;
using Grayscale.P025_KifuLarabe.L050_Things;
using Grayscale.P006_Syugoron;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P025_KifuLarabe.L00050_StructShogi;

namespace Grayscale.P025_KifuLarabe.L100_KifuIO
{


    /// <summary>
    /// 基本的に、本将棋でなければ　正しく使えません。
    /// </summary>
    public abstract class Util_LegalMove
    {

        /// <summary>
        /// 指定された手の中から、王手局面を除外します。
        /// 
        /// 王手回避漏れを防ぎたいんだぜ☆
        /// </summary>
        /// <param name="km_available">自軍の各駒の移動できる升セット</param>
        /// <param name="sbGohosyu"></param>
        /// <param name="logTag"></param>
        public static KifuNode LA_RemoveMate(
            bool isHonshogi,
            Maps_OneAndMulti<Finger, IMove> genTebanKomabetuAllMove1,// 現手番の、どの駒が、どんな手を指すことができるか
            int yomuDeep,//脳内読み手数
            int tesumi_yomiGenTeban,
            Playerside pside_yomiGenTeban,
            KifuNode siteiNode_yomiGenTeban,
            bool enableLog,
            GraphicalLog_File logF_kiki,
            string hint,
            LarabeLoggerable logTag)
        {
            Node<IMove, KyokumenWrapper> hubNode = UtilKomabetuMove.ToNextNodes_AsHubNode(
                genTebanKomabetuAllMove1, siteiNode_yomiGenTeban, pside_yomiGenTeban, logTag
                );// ハブ・ノード自身はダミーノードなんだが、子ノードに、次のノードが入っている。
            Converter04.AssertNariMove(hubNode, "#LA_RemoveMate(1)");//ここはok
            Util_LegalMove.Log1(hubNode,enableLog, tesumi_yomiGenTeban, hint, logTag);


            if (isHonshogi)
            {
                // 王手が掛かっている局面を除きます。

                Util_LegalMove.LAA_RemoveNextNode_IfMate(
                    hubNode,
                    enableLog,
                    yomuDeep,
                    tesumi_yomiGenTeban,
                    pside_yomiGenTeban,
                    logF_kiki,
                    logTag);
            }
            Converter04.AssertNariMove(hubNode, "#LA_RemoveMate(2)王手局面削除直後");//ここはok


            // 「指し手一覧」を、「駒別の全指し手」に分けます。
            Maps_OneAndMulti<Finger, IMove> komabetuAllMoves2 = siteiNode_yomiGenTeban.SplitMoveByKoma(hubNode, logTag);
            Converter04.AssertNariMove(komabetuAllMoves2, "#LA_RemoveMate(3)更に変換後");//ここはok

            //
            // 「駒別の指し手一覧」を、「駒別の進むマス一覧」になるよう、データ構造を変換します。
            //
            Maps_OneAndOne<Finger, SySet<SyElement>> komabetuSusumuMasus = new Maps_OneAndOne<Finger, SySet<SyElement>>();// 「どの駒を、どこに進める」の一覧
            foreach (KeyValuePair<Finger, List<IMove>> entry in komabetuAllMoves2.Items)
            {
                Finger finger = entry.Key;
                List<IMove> teList = entry.Value;

                // ポテンシャル・ムーブを調べます。
                SySet<SyElement> masus_PotentialMove = new SySet_Default<SyElement>("ポテンシャルムーブ");
                foreach (IMove te in teList)
                {
                    RO_Star_Koma koma = Util_Koma.AsKoma(te.MoveSource);

                    masus_PotentialMove.AddElement(koma.Masu);
                }

                if (!masus_PotentialMove.IsEmptySet())
                {
                    // 空でないなら
                    Util_KomabetuMasus.AddOverwrite(komabetuSusumuMasus, finger, masus_PotentialMove);
                }
            }

            // まず、ディクショナリー構造へ変換。
            Dictionary<IMove, KyokumenWrapper> moveBetuSky = Converter04.KomabetuMasusToMoveBetuSky(
                komabetuSusumuMasus, siteiNode_yomiGenTeban.Value.ToKyokumenConst, pside_yomiGenTeban, logTag);

            // 棋譜ノード構造へ変換。
            return Converter04.MoveBetuSkyToHubNode(moveBetuSky, KifuNodeImpl.GetReverseTebanside(pside_yomiGenTeban));
        }
        private static void Log1(
            Node<IMove, KyokumenWrapper> hubNode,
            bool enableLog,
            int tesumi_yomiGenTeban,
            string hint,
            LarabeLoggerable logTag
            )
        {
            Util_GraphicalLog.Log(enableLog, "Util_LegalMove(王手回避漏れ02)王手を回避するかどうかに関わらず、ひとまず全ての次の手", "[" +
                ((KifuNode)hubNode).Json_NextNodes_MultiSky(
                    "(王手回避漏れ02." + tesumi_yomiGenTeban + "手目)",
                    hint + "_Lv3_RMHO",
                    tesumi_yomiGenTeban,
                    logTag) + "]");// ログ出力
        }

        /// <summary>
        /// ハブノードの次手番の局面のうち、王手がかかった局面は取り除きます。
        /// </summary>
        public static void LAA_RemoveNextNode_IfMate(
            Node<IMove, KyokumenWrapper> hubNode,
            bool enableLog,
            int yomuDeep,//脳内読み手数
            int tesumi_yomiGenTeban_forLog,//読み進めている現在の手目
            Playerside pside_genTeban,
            GraphicalLog_File logF_kiki,
            LarabeLoggerable logTag
            )
        {
            // Node<,>の形で。
            Dictionary<string, Node<IMove, KyokumenWrapper>> newNextNodes = new Dictionary<string, Node<IMove, KyokumenWrapper>>();

            hubNode.Foreach_NextNodes((string key, Node<IMove, KyokumenWrapper> node, ref bool toBreak) =>
            {
                System.Diagnostics.Debug.Assert(node.Key != null);//指し手がヌルなはず無いはず。

                // 王様が利きに飛び込んだか？
                bool kingSuicide = Util_LegalMove.LAAA_KingSuicide(
                    enableLog,
                    node.Value.ToKyokumenConst,
                    yomuDeep,
                    tesumi_yomiGenTeban_forLog,
                    pside_genTeban,//現手番＝攻め手視点
                    logF_kiki,
                    node.Key,
                    logTag
                    );

                if (!kingSuicide)
                {
                    // 王様が利きに飛び込んでいない局面だけ、残します。
                    if (newNextNodes.ContainsKey(key))
                    {
                        newNextNodes[key] = node;
                    }
                    else
                    {
                        newNextNodes.Add(key, node);
                    }
                }
            });


            // 入替え
            hubNode.Set_NextNodes(newNextNodes);
        }


        /// <summary>
        /// 利きに飛び込んでいないか（王手されていないか）、調べます。
        /// 
        /// GetAvailableMove()の中では使わないでください。循環してしまいます。
        /// </summary>
        public static bool LAAA_KingSuicide(
            bool enableLog,
            SkyConst src_Sky,//調べたい局面
            int yomuDeep,//脳内読み手数
            int tesumi_yomiCur_forLog,//読み進めている現在の手目
            Playerside pside_genTeban,//現手番側
            GraphicalLog_File logF_kiki,
            IMove moveForLog,
            LarabeLoggerable logTag
            )
        {
            bool isHonshogi = true;

            System.Diagnostics.Debug.Assert(src_Sky.Count == Masu_Honshogi.HONSHOGI_KOMAS);

            // 「相手の駒を動かしたときの利き」リスト
            // 持ち駒はどう考える？「駒を置けば、思い出王手だってある」
            List_OneAndMulti<Finger, SySet<SyElement>> sMs_effect_aiTeban = Util_LegalMove.LAAAA_GetEffect(
                enableLog, 
                isHonshogi,
                src_Sky,
                pside_genTeban,
                true,// 相手盤の利きを調べます。
                logF_kiki,
                "玉自殺ﾁｪｯｸ",
                yomuDeep,
                tesumi_yomiCur_forLog,
                moveForLog,
                logTag);

            
            // 現手番側が受け手に回ったとします。現手番の、王の座標
            SyElement genTeban_kingMasu;

            if (Playerside.P2 == pside_genTeban)
            {
                // 現手番は、後手

                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(Finger_Honshogi.GoteOh).MoveSource);

                    genTeban_kingMasu = koma.Masu;
            }
            else
            {
                // 現手番は、先手
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(Finger_Honshogi.SenteOh).MoveSource);

                    genTeban_kingMasu = koma.Masu;
            }


            // 相手の利きに、自分の王がいるかどうか確認します。
            bool mate = false;
            sMs_effect_aiTeban.Foreach_Entry((Finger koma, SySet<SyElement> kikis, ref bool toBreak) =>
            {
                foreach (Basho kiki in kikis.Elements)
                {
                    if (Util_Masu.AsMasuNumber(genTeban_kingMasu) == (int)kiki.MasuNumber)
                    {
                        mate = true;
                        toBreak = true;
                    }
                }
            });

            return mate;
        }
        

        /// <summary>
        /// 指定された局面で、指定された手番の駒の、利きマスを算出します。
        /// 持ち駒は盤上にないので、利きを調べる必要はありません。
        /// 
        /// 「手目」は判定できません。
        /// 
        /// </summary>
        /// <param name="kouho"></param>
        /// <param name="sbGohosyu"></param>
        /// <param name="logger"></param>
        public static List_OneAndMulti<Finger, SySet<SyElement>> LAAAA_GetEffect(
            bool enableLog,
            bool isHonshogi,
            SkyConst src_Sky,
            Playerside pside_genTeban3,
            bool isAiteban,
            GraphicalLog_File logF_kiki,
            string logBrd_caption,
            int yomuDeep_forLog,//脳内読み手数
            int tesumi_yomiCur_forLog,
            IMove moveForLog,
            LarabeLoggerable logTag
            )
        {
            GraphicalLog_Board logBrd_kiki = new GraphicalLog_Board();
            logBrd_kiki.Caption = logBrd_caption;// "利き_" 
            logBrd_kiki.Tesumi = tesumi_yomiCur_forLog;
            logBrd_kiki.NounaiYomiDeep = yomuDeep_forLog;
            //logBrd_kiki.Score = 0.0d;
            logBrd_kiki.GenTeban = pside_genTeban3;// 現手番
            logF_kiki.boards.Add(logBrd_kiki);

            // 《１》
            List_OneAndMulti<Finger, SySet<SyElement>> sMs_effect = new List_OneAndMulti<Finger,SySet<SyElement>>();//盤上の駒の利き
            {
                // 《１．１》
                Playerside tebanSeme;//手番（利きを調べる側）
                Playerside tebanKurau;//手番（喰らう側）
                {
                    if (isAiteban)
                    {
                        tebanSeme = Converter04.AlternatePside(pside_genTeban3);
                        tebanKurau = pside_genTeban3;
                    }
                    else
                    {
                        tebanSeme = pside_genTeban3;
                        tebanKurau = Converter04.AlternatePside(pside_genTeban3);
                    }

                    if (Playerside.P1 == tebanSeme)
                    {
                        logBrd_kiki.NounaiSeme = Gkl_NounaiSeme.Sente;
                    }
                    else if(Playerside.P2 == tebanSeme)
                    {
                        logBrd_kiki.NounaiSeme = Gkl_NounaiSeme.Gote;
                    }
                }


                // 《１．２》
                Fingers fingers_seme_IKUSA;//戦駒（利きを調べる側）
                Fingers fingers_kurau_IKUSA;//戦駒（喰らう側）
                Fingers dust1;
                Fingers dust2;

                Util_Things.AAABAAAA_SplitGroup(
                        out fingers_seme_IKUSA,
                        out fingers_kurau_IKUSA,
                        out dust1,
                        out dust2,
                        src_Sky,
                        tebanSeme,
                        tebanKurau,
                        logTag
                    );


                // 攻め手の駒の位置
                GraphicalLog_Board boardLog_clone = new GraphicalLog_Board(logBrd_kiki);
                foreach (Finger finger in fingers_seme_IKUSA.Items)
                {
                    RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(finger).MoveSource);


                        Gkl_KomaMasu km = new Gkl_KomaMasu(
                            Util_GraphicalLog.PsideKs14_ToString(tebanSeme, Haiyaku184Array.Syurui(koma.Haiyaku), ""),
                            Util_Masu.AsMasuNumber(koma.Masu)
                            );
                        boardLog_clone.KomaMasu1.Add(km);
                }

                foreach (Finger finger in fingers_kurau_IKUSA.Items)
                {
                    RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(finger).MoveSource);


                        logBrd_kiki.KomaMasu2.Add(new Gkl_KomaMasu(
                            Util_GraphicalLog.PsideKs14_ToString(tebanKurau, Haiyaku184Array.Syurui(koma.Haiyaku), ""),
                            Util_Masu.AsMasuNumber(koma.Masu)
                            ));
                }

                logBrd_kiki = boardLog_clone;




                // 《１．３》
                SySet<SyElement> masus_seme_IKUSA = Converter04.Fingers_ToMasus(fingers_seme_IKUSA, src_Sky);// 盤上のマス（利きを調べる側の駒）
                SySet<SyElement> masus_kurau_IKUSA = Converter04.Fingers_ToMasus(fingers_kurau_IKUSA, src_Sky);// 盤上のマス（喰らう側の駒）

                // 駒のマスの位置は、特にログに取らない。

                // 《１．４》
                Maps_OneAndOne<Finger, SySet<SyElement>> kmEffect_seme_IKUSA = Util_Things.Get_KmEffect_seme_IKUSA(
                    fingers_seme_IKUSA,//この中身がおかしい。
                    masus_seme_IKUSA,
                    masus_kurau_IKUSA,
                    src_Sky,
                    enableLog,
                    Converter04.ChangeMoveToStringForLog(moveForLog, pside_genTeban3),
                    logTag
                    );// 利きを調べる側の利き（戦駒）

                // 戦駒の利き
                logBrd_kiki = new GraphicalLog_Board(logBrd_kiki);
                kmEffect_seme_IKUSA.Foreach_Entry((Finger key, SySet<SyElement> value, ref bool toBreak) =>
                {
                    RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(key).MoveSource);


                        string komaImg = Util_GraphicalLog.PsideKs14_ToString(tebanSeme, Haiyaku184Array.Syurui(koma.Haiyaku), "");

                        foreach (Basho masu in value.Elements)
                        {
                            boardLog_clone.Masu_theEffect.Add((int)masu.MasuNumber);
                        }
                });

                logBrd_kiki = boardLog_clone;


                try
                {
                    // 《１》　＝　《１．４》の戦駒＋持駒
                    sMs_effect.AddRange_New( kmEffect_seme_IKUSA);

                }
                catch (Exception ex)
                {
                    //>>>>> エラーが起こりました。

                    // どうにもできないので  ログだけ取って無視します。
                    logTag.WriteLine_Error( ex.GetType().Name + " " + ex.Message + "：ランダムチョイス(50)：");
                    throw ;
                }

            }

            return sMs_effect;
        }




    }
}
