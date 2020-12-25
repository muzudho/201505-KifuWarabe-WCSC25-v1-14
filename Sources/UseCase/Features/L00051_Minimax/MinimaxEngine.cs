using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.P050_KifuWarabe.L00049_Kokoro;

namespace Grayscale.P050_KifuWarabe.L00051_Minimax
{
    public interface MinimaxEngine
    {
        /// <summary>
        /// 棋譜ツリーの、ノードのネクストノードに、点数を付けていきます。
        /// </summary>
        void Tensuduke_ForeachLeafs(
            string nodePath,
            KifuNode node,
            KifuTree kifu,
            Kokoro atama,
            PlayerInfo playerInfo,
            ReportEnvironment reportEnvironment,//MinimaxEngineImpl.REPORT_ENVIRONMENT
            GraphicalLog_File logF_kiki
            );
    }
}
