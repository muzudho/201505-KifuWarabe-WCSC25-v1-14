using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.P007_SfenReport.L00025_Report;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P045_Atama.L00025_KyHandan;

namespace Grayscale.P050_KifuWarabe.L00050_KyHyoka
{
    public interface KyHyokaWriter
    {
        /// <summary>
        /// 棋譜ツリーの、ノードに格納されている、局面評価明細を、出力していきます。
        /// </summary>
        void Write_ForeachLeafs(
            string nodePath,
            KifuNode node,
            KifuTree kifu,
            PlayerInfo playerInfo,
            string relFolder,
            ReportEnvironment reportEnvironment
            );
    }
}
