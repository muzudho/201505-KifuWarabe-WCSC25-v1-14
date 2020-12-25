using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.Kifuwarazusa.UseCases.Features
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
