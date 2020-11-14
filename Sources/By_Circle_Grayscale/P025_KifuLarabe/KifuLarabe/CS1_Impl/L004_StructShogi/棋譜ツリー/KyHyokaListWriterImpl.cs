using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P007_SfenReport.L00025_Report;
using System.IO;
using Grayscale.P025_KifuLarabe.L00012_Atom;

namespace Grayscale.P025_KifuLarabe.L004_StructShogi
{
    public class KyHyokaListWriterImpl
    {

        public static void Write(
            string id,
            KifuNode node,
            string relFolder,
            ReportEnvironment env
            )
        {

            StringBuilder sb = new StringBuilder();

            // 見出し
            sb.Append(id);
            sb.Append("    ");
            sb.Append(((int)node.KyHyoka.Total()).ToString());
            sb.Append("    ");
            switch(node.Tebanside)
            {
                case Playerside.P1: sb.Append("P2が指し終えた局面。手番P1"); break;
                case Playerside.P2: sb.Append("P1が指し終えた局面。手番P2"); break;
                case Playerside.Empty: sb.Append("手番Empty"); break;
            }
            sb.AppendLine();

            foreach (KeyValuePair<string, KyHyokaItem> entry in node.KyHyoka.Items)
            {
                sb.Append("    ");
                sb.Append(entry.Key);
                sb.Append("  ");
                sb.Append(((KyHyokaItem)entry.Value).Score);
                sb.Append("  ");
                sb.Append(((KyHyokaItem)entry.Value).Text);
                sb.AppendLine();
            }
            sb.AppendLine();

            File.AppendAllText(env.OutFolder+relFolder+"/_log_スコア明細.txt", sb.ToString());
        }

    }
}
