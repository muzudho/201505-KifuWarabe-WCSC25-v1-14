using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;

namespace Grayscale.P025_KifuLarabe.L100_KifuIO
{

    /// <summary>
    /// ************************************************************************************************************************
    /// 記録係
    /// ************************************************************************************************************************
    /// </summary>
    public abstract class KirokuGakari
    {

        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜データを元に、符号リスト１(*1)を出力します。
        /// ************************************************************************************************************************
        /// 
        ///     *1…「▲２六歩△８四歩▲７六歩」といった書き方。
        /// 
        /// </summary>
        /// <param name="fugoList"></param>
        public static string ToJapaneseKifuText(
            KifuTree kifu,
            LarabeLoggerable logTag
            )
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("position ");

            sb.Append(kifu.GetProperty(KifuTreeImpl.PropName_Startpos));
            sb.Append(" moves ");

            // 採譜用に、新しい対局を用意します。
            KifuTree saifuKifu;
            {
                saifuKifu = new KifuTreeImpl(
                        new KifuNodeImpl(
                            Util_Sky.NullObjectMove,
                            new KyokumenWrapper(new SkyConst(Util_Sky.New_Hirate())),//日本の符号読取時
                            Playerside.P2
                        )
                );
                saifuKifu.Clear();// 棋譜を空っぽにします。

                saifuKifu.SetProperty(KifuTreeImpl.PropName_FirstPside, Playerside.P1);
                saifuKifu.SetProperty(KifuTreeImpl.PropName_Startpos, "startpos");//平手の初期局面 // FIXME:平手とは限らないのでは？
            }


            kifu.ForeachHonpu(kifu.CurNode, (int tesumi, KyokumenWrapper kWrap, Node<IMove, KyokumenWrapper> node6, ref bool toBreak) =>
            {
                if (0 == tesumi)
                {
                    goto gt_EndLoop;
                }


                FugoJ fugo;

                //------------------------------
                // 符号の追加（記録係）
                //------------------------------
                KyokumenWrapper saifu_kWrap = saifuKifu.CurNode.Value;

                KifuNode newNode = new KifuNodeImpl(
                    node6.Key,
                    new KyokumenWrapper(saifu_kWrap.ToKyokumenConst),
                    KifuNodeImpl.GetReverseTebanside(((KifuNode)saifuKifu.CurNode).Tebanside)
                );

                // TODO: ↓？
                ((KifuNode)saifuKifu.CurNode).AppendChildA_New(newNode);
                saifuKifu.CurNode = newNode;

                RO_Star_Koma koma = Util_Koma.AsKoma(((IMove)node6.Key).MoveSource);

                fugo = JFugoCreator15Array.ItemMethods[(int)Haiyaku184Array.Syurui(koma.Haiyaku)](node6.Key, saifu_kWrap, logTag);//「▲２二角成」なら、馬（dst）ではなくて角（src）。

                sb.Append(fugo.ToText_UseDou( node6));

            gt_EndLoop:
                ;
            });

            return sb.ToString();
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜データを元に、符号リスト２(*1)を出力します。
        /// ************************************************************************************************************************
        /// 
        ///     *1…「position startpos moves 7g7f 3c3d 2g2f」といった書き方。
        /// 
        /// </summary>
        /// <param name="fugoList"></param>
        public static string ToSfen_PositionString(KifuTree kifu)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("position ");
            sb.Append(kifu.GetProperty(KifuTreeImpl.PropName_Startpos));
            sb.Append(" moves ");

            int count = 0;
            kifu.ForeachHonpu(kifu.CurNode, (int tesumi, KyokumenWrapper kWrap, Node<IMove, KyokumenWrapper> node6, ref bool toBreak) =>
            {
                if (0 == tesumi)
                {
                    goto gt_EndLoop;
                }

                sb.Append(Util_Sky.ToSfenMoveText(node6.Key));

                //// TODO:デバッグ用
                //switch (sasite.TottaKoma)
                //{
                //    case KomaSyurui.UNKNOWN:
                //    case KomaSyurui.TOTTA_KOMA_NASI:
                //        break;
                //    default:
                //        sb.Append("(");
                //        sb.Append(Converter.SyuruiToSfen(sasite.Pside,sasite.TottaKoma));
                //        sb.Append(")");
                //        break;
                //}

                sb.Append(" ");


            gt_EndLoop:
                count++;
            });

            return sb.ToString();
        }

    }
}
