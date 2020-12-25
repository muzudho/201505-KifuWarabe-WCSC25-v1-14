
using System.IO;
using Grayscale.Kifuwarazusa.Entities.Configuration;
using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.Kifuwarazusa.UseCases.Features
{
    public class KyHyokaWriterImpl : KyHyokaWriter
    {
        private static int logFileCounter;

        /// <summary>
        /// 棋譜ツリーの、ノードに格納されている、局面評価明細を、出力していきます。
        /// </summary>
        public void Write_ForeachLeafs(
            IEngineConf engine,
            string nodePath,
            KifuNode node,
            KifuTree kifu,
            PlayerInfo playerInfo,
            string relFolder,
            ReportEnvironment reportEnvironment
            )
        {
            // 次ノードの有無
            if (0 < node.Count_NextNodes)
            {
                // 先に奥の枝から。
                node.Foreach_NextNodes((string key, Node<ShootingStarlightable, KyokumenWrapper> nextNode, ref bool toBreak) =>
                {

                    double score = ((KifuNode)nextNode).KyHyoka.Total();

                    this.Write_ForeachLeafs(
                        engine,
                        nodePath + " " + Util_Sky.ToSfenMoveTextForFilename(nextNode.Key),
                        (KifuNode)nextNode,
                        kifu,
                        playerInfo,
                        relFolder + ((int)score).ToString() + "点_" + Util_Sky.ToSfenMoveText(nextNode.Key) + "/",
                        //relFolder + ((int)((KifuNode)nextNode).KyHyoka.Total()).ToString() + "点_" + Util_Sky.ToSfenMoveText(nextNode.Key) + "/",
                        reportEnvironment
                        );

                });
            }

            // このノード
            //
            // 盤１個分のログの準備
            //
            this.Log_Board(
                engine,
                nodePath,
                node,
                kifu,
                relFolder,
                reportEnvironment
            );
        }

        /// <summary>
        /// 盤１個分のログ。
        /// </summary>
        private void Log_Board(
            IEngineConf engineConf,
            string nodePath,
            KifuNode node,
            KifuTree kifu,
            string logDirectory,
            ReportEnvironment reportEnvironment
            )
        {
            // 出力先
            string basename = $"#{((int)node.KyHyoka.Total())}点_{KyHyokaWriterImpl.logFileCounter}_{nodePath}.log.png"; // TODO

            //
            // 画像ﾛｸﾞ
            //
            if (true)
            {

                //SFEN文字列と、出力ファイル名を指定することで、局面の画像ログを出力します。
                KyokumenPngWriterImpl.Write1(
                    engineConf,
                    node.ToRO_Kyokumen1(),
                    Path.Combine(logDirectory, basename),
                    reportEnvironment
                    );
                KyHyokaWriterImpl.logFileCounter++;
            }

            //
            // スコア明細
            //
            {
                KyHyokaListWriterImpl.Write(basename, node, logDirectory, reportEnvironment);
            }
        }

    }
}
