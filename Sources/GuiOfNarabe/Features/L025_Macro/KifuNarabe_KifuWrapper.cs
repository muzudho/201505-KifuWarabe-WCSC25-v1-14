using Grayscale.Kifuwarazusa.GuiOfNarabe.Gui;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;

namespace Grayscale.P200_KifuNarabe.L025_Macro
{

    /// <summary>
    /// ライブラリーは 「Kifu_Tree」をインターフェースとして使っているので、
    /// 
    /// Kifu_Treeを隠します。
    /// </summary>
    public abstract class KifuNarabe_KifuWrapper
    {



        public static Node<ShootingStarlightable, KyokumenWrapper> CurNode(NarabeRoomViewModel shogiGui)
        {
            return shogiGui.GameViewModel.Kifu.CurNode;
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
