using System.Collections.Generic;
using System.IO;
using System.Text;
using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.Kifuwarazusa.Entities.Features
{
    public class KyHyokaListWriterImpl
    {

        public static void Write(
            string id,
            KifuNode node,
            string logDirectory,
            ReportEnvironment env
            )
        {

            StringBuilder sb = new StringBuilder();

            // 見出し
            sb.Append(id);
            sb.Append("    ");
            sb.Append(((int)node.KyHyoka.Total()).ToString());
            sb.Append("    ");
            switch (node.Tebanside)
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

            File.AppendAllText(Path.Combine(logDirectory, "_log_スコア明細.txt"), sb.ToString());
        }

    }
}
