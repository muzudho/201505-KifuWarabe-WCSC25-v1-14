using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Gui;

namespace Grayscale.Kifuwarazusa.GuiOfNarabe.Features
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
