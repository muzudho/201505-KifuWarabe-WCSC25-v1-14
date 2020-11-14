using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using System.Runtime.CompilerServices;

using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P025_KifuLarabe.L004_StructShogi
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
                    string message = "先後エラー";
                    //LarabeLogger.GetInstance().WriteLineError(LarabeLoggerList.ERROR, message);
                    throw new Exception(message);
            }


            return result;
        }


        /// <summary>
        /// 取った駒を差替えます。
        /// 
        /// 棋譜読取時用です。マウス操作時は、流れが異なるので使えません。
        /// </summary>
        public void AppendChildB_Swap(
            Ks14 tottaSyurui,
            SkyConst src_Sky,
            string hint,
            LarabeLoggerable logTag
            )
        {
            if (this.CountTesumi(this.CurNode) < 1)
            {
                // ルートしか無いなら
                goto gt_EndMethod;
            }

            if (null == src_Sky)
            {
                string message = "ノードを追加しようとしましたが、指定されたnewSkyがヌルです。";
                LarabeLoggerList.ERROR.WriteLine_Error(message);
                throw new Exception(message);
            }


            Playerside genTebanside = ((KifuNode)this.CurNode).Tebanside;
            // 現在のノードを削除します。そのとき、もともとのキー を覚えておきます。
            ShootingStarlightable motoKey = (ShootingStarlightable)this.PopCurrentNode().Key;

            // もともとのキーの、取った駒の種類だけを差替えます。
            RO_ShootingStarlight sasikaeKey = Util_Sky.New(motoKey.LongTimeAgo, motoKey.Now, tottaSyurui);//motoKey.Finger,

            // キーを差替えたノード
            Node<ShootingStarlightable, KyokumenWrapper> sasikaeNode = new KifuNodeImpl(sasikaeKey, new KyokumenWrapper(src_Sky), genTebanside);

            System.Diagnostics.Debug.Assert(!this.CurNode.ContainsKey_NextNodes(Util_Sky.ToSfenSasiteText(sasikaeNode.Key)));


            // さきほど　カレントノードを削除したので、
            // 今、カレントノードは、１つ前のノードになっています。
            // ここに、差替えたノードを追加します。
            this.CurNode.Add_NextNode(Util_Sky.ToSfenSasiteText(sasikaeNode.Key), sasikaeNode);
            sasikaeNode.PreviousNode = this.CurNode;


            this.CurNode = sasikaeNode;

            logTag.WriteLine_AddMemo("リンクトリストの、最終ノードは差し替えられた hint=[" + hint + "] item=[" + Util_Sky.ToSfenSasiteText(sasikaeKey) + "]");
        // memberName=[" + memberName + "] sourceFilePath=[" + sourceFilePath + "] sourceLineNumber=[" + sourceLineNumber + "]

        gt_EndMethod:
            ;
        }



        /// <summary>
        /// ************************************************************************************************************************
        /// [ここから採譜]機能
        /// ************************************************************************************************************************
        /// </summary>
        public void SetStartpos_KokokaraSaifu( Playerside pside, LarabeLoggerable logTag)
        {

            //------------------------------------------------------------
            // 棋譜を空に
            //------------------------------------------------------------
            this.Clear();
            this.SetProperty(KifuTreeImpl.PropName_Startpos, ((KifuNode)this.CurNode).ToSfenstring(pside, logTag));
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
