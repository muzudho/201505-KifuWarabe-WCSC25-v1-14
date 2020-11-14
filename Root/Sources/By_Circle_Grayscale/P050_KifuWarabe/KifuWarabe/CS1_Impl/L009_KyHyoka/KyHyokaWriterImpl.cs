
using Grayscale.P007_SfenReport.L00025_Report;
using Grayscale.P007_SfenReport.L100_Write;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P045_Atama.L00025_KyHandan;
using Grayscale.P050_KifuWarabe.L00050_KyHyoka;

namespace Grayscale.P050_KifuWarabe.L009_KyHyoka
{
    public class KyHyokaWriterImpl : KyHyokaWriter
    {
        private static int logFileCounter;


        /// <summary>
        /// 棋譜ツリーの、ノードに格納されている、局面評価明細を、出力していきます。
        /// </summary>
        public void Write_ForeachLeafs(
            string nodePath,
            KifuNode node,
            KifuTree kifu,
            PlayerInfo playerInfo,
            string relFolder,
            ReportEnvironment reportEnvironment,
            LarabeLoggerable logTag
            )
        {

            // 次ノードの有無
            if (0<node.Count_NextNodes)
            {
                // 先に奥の枝から。
                node.Foreach_NextNodes((string key, Node<ShootingStarlightable, KyokumenWrapper> nextNode, ref bool toBreak) =>
                {

                    double score = ((KifuNode)nextNode).KyHyoka.Total();

                    this.Write_ForeachLeafs(
                        nodePath + " " + Util_Sky.ToSfenSasiteText_ForFilename(nextNode.Key),
                        (KifuNode)nextNode,
                        kifu,
                        playerInfo,
                        relFolder + ((int)score).ToString()+"点_"+Util_Sky.ToSfenSasiteText(nextNode.Key) + "/",
                        //relFolder + ((int)((KifuNode)nextNode).KyHyoka.Total()).ToString() + "点_" + Util_Sky.ToSfenSasiteText(nextNode.Key) + "/",
                        reportEnvironment,
                        logTag
                        );

                });
            }

            // このノード
            //
            // 盤１個分のログの準備
            //
            this.Log_Board(
                nodePath,
                node,
                kifu,
                relFolder,
                reportEnvironment,
                logTag
            );
        }

        /// <summary>
        /// 盤１個分のログ。
        /// </summary>
        private void Log_Board(
            string nodePath,
            KifuNode node,
            KifuTree kifu,
            string relFolder,
            ReportEnvironment reportEnvironment,
            LarabeLoggerable logTag
            )
        {
            // 出力先
            string fileName = "_log_" + ((int)node.KyHyoka.Total()) + "点_" + KyHyokaWriterImpl.logFileCounter + "_" + nodePath + ".png";

            //
            // 画像ﾛｸﾞ
            //
            if (true)
            {

                //SFEN文字列と、出力ファイル名を指定することで、局面の画像ログを出力します。
                KyokumenPngWriterImpl.Write1(
                    node.ToRO_Kyokumen1(logTag),
                    relFolder,
                    fileName,
                    reportEnvironment
                    );
                KyHyokaWriterImpl.logFileCounter++;
            }

            //
            // スコア明細
            //
            {
                KyHyokaListWriterImpl.Write(fileName, node, relFolder, reportEnvironment);
            }
        }

    }
}
