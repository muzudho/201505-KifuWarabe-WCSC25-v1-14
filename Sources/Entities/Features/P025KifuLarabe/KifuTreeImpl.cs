using System;
using Grayscale.Kifuwarazusa.Entities.Logging;

namespace Grayscale.Kifuwarazusa.Entities.Features
{
    public class KifuTreeImpl : TreeImpl<ShootingStarlightable, KyokumenWrapper>, KifuTree
    {
        public const string PropName_Startpos = "prop_startpos";

        /// <summary>
        /// 初手の先後
        /// </summary>
        public const string PropName_FirstPside = "prop_firstPside";



        //public Tree<ShootingStarlightable, KyokumenWrapper> Tree { get { return this.tree; } }
        //private Tree<ShootingStarlightable, KyokumenWrapper> tree;

        public KifuTreeImpl(Node<ShootingStarlightable, KyokumenWrapper> root)
            : base(root)//Tree<ShootingStarlightable, KyokumenWrapper> tree
        {
            //this.tree = tree;
        }


        public Playerside CountPside(Node<ShootingStarlightable, KyokumenWrapper> node)
        {
            return this.CountPside(this.CountTesumi(node));
        }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 現在の先後
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Playerside CountPside(int tesumi)
        {
            Playerside result;

            switch ((Playerside)this.GetProperty(KifuTreeImpl.PropName_FirstPside))
            {
                case Playerside.P1:
                    if (tesumi % 2 == 0)
                    {
                        // 手目が偶数なら先手
                        result = Playerside.P1;
                    }
                    else
                    {
                        result = Playerside.P2;
                    }
                    break;

                case Playerside.P2:
                    if (tesumi % 2 == 0)
                    {
                        // 手目が偶数なら後手
                        result = Playerside.P2;
                    }
                    else
                    {
                        result = Playerside.P1;
                    }
                    break;

                default:
                    throw new Exception("先後エラー");
            }


            return result;
        }


        /// <summary>
        /// 取った駒を差替えます。
        /// 
        /// 棋譜読取時用です。マウス操作時は、流れが異なるので使えません。
        /// </summary>
        public void AppendChildB_Swap(
            PieceType tottaSyurui,
            SkyConst src_Sky,
            string hint
            )
        {
            if (this.CountTesumi(this.CurNode) < 1)
            {
                // ルートしか無いなら
                goto gt_EndMethod;
            }

            if (null == src_Sky)
            {
                throw new Exception("ノードを追加しようとしましたが、指定されたnewSkyがヌルです。");
            }


            Playerside genTebanside = ((KifuNode)this.CurNode).Tebanside;
            // 現在のノードを削除します。そのとき、もともとのキー を覚えておきます。
            ShootingStarlightable motoKey = (ShootingStarlightable)this.PopCurrentNode().Key;

            // もともとのキーの、取った駒の種類だけを差替えます。
            RO_ShootingStarlight sasikaeKey = Util_Sky.New(motoKey.LongTimeAgo, motoKey.Now, tottaSyurui);//motoKey.Finger,

            // キーを差替えたノード
            Node<ShootingStarlightable, KyokumenWrapper> sasikaeNode = new KifuNodeImpl(sasikaeKey, new KyokumenWrapper(src_Sky), genTebanside);

            System.Diagnostics.Debug.Assert(!this.CurNode.ContainsKey_NextNodes(Util_Sky.ToSfenMoveText(sasikaeNode.Key)));


            // さきほど　カレントノードを削除したので、
            // 今、カレントノードは、１つ前のノードになっています。
            // ここに、差替えたノードを追加します。
            this.CurNode.Add_NextNode(Util_Sky.ToSfenMoveText(sasikaeNode.Key), sasikaeNode);
            sasikaeNode.PreviousNode = this.CurNode;


            this.CurNode = sasikaeNode;

            Logger.Trace("リンクトリストの、最終ノードは差し替えられた hint=[" + hint + "] item=[" + Util_Sky.ToSfenMoveText(sasikaeKey) + "]");
        // memberName=[" + memberName + "] sourceFilePath=[" + sourceFilePath + "] sourceLineNumber=[" + sourceLineNumber + "]

        gt_EndMethod:
            ;
        }



        /// <summary>
        /// ************************************************************************************************************************
        /// [ここから採譜]機能
        /// ************************************************************************************************************************
        /// </summary>
        public void SetStartpos_KokokaraSaifu(Playerside pside)
        {

            //------------------------------------------------------------
            // 棋譜を空に
            //------------------------------------------------------------
            this.Clear();
            this.SetProperty(KifuTreeImpl.PropName_Startpos, ((KifuNode)this.CurNode).ToSfenstring(pside));
        }

        /// <summary>
        /// 現局面のプレイヤーサイド。
        /// </summary>
        /// <returns></returns>
        public Playerside CurrentPside()
        {
            return this.CountPside(this.CurNode);
        }

        /// <summary>
        /// 現局面の、手済みカウントです。
        /// </summary>
        /// <returns></returns>
        public int CurrentTesumi()
        {
            return this.CountTesumi(this.CurNode);
        }

    }


}
