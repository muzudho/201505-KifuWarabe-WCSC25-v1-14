using Grayscale.P025_KifuLarabe;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P200_KifuNarabe.L00048_ShogiGui;
using System.Diagnostics;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P025_KifuLarabe.L004_StructShogi;

namespace Grayscale.P200_KifuNarabe.L025_Macro
{

    /// <summary>
    /// ライブラリーは 「Kifu_Tree」をインターフェースとして使っているので、
    /// 
    /// Kifu_Treeを隠します。
    /// </summary>
    public abstract class KifuNarabe_KifuWrapper
    {



        public static Node<ShootingStarlightable, KyokumenWrapper> CurNode(ShogiGui shogiGui)
        {
            return shogiGui.Model_PnlTaikyoku.Kifu.CurNode;
        }

        public static Node<ShootingStarlightable, KyokumenWrapper> CurNode(KifuTree kifu)
        {
            return kifu.CurNode;
        }




        /// <summary>
        /// コンピューターの棋譜用
        /// </summary>
        /// <param name="kifu"></param>
        /// <returns></returns>
        public static int CountCurTesumi1(KifuTree kifu)
        {
            return kifu.CountTesumi(kifu.CurNode);
        }

    }
}
