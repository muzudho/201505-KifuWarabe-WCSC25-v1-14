using System.Collections.Generic;
using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.Kifuwarazusa.Entities.Features
{
    /// <summary>
    /// 局面を切り取ったときの、スコアの明細
    /// </summary>
    public class KyHyokaImpl : KyHyoka
    {

        /// <summary>
        /// 全項目。
        /// </summary>
        public Dictionary<string, KyHyokaItem> Items { get { return this.items; } }
        private Dictionary<string, KyHyokaItem> items;

        public KyHyokaImpl()
        {
            this.items = new Dictionary<string, KyHyokaItem>();
        }

        /// <summary>
        /// 枝専用。
        /// </summary>
        /// <param name="kyHyokaItem"></param>
        public KyHyokaImpl(double branchScore)
        {
            this.items = new Dictionary<string, KyHyokaItem>();
            this.items.Add("枝", new KyHyokaNolimitItemImpl(1.0d, branchScore, "枝"));
        }

        public double Total()
        {
            double total = 0.0d;

            foreach (KyHyokaItem kyHyoka in this.Items.Values)
            {
                total += kyHyoka.Score;
            }

            return total;
        }

        public void Clear()
        {
            this.Items.Clear();
        }

        public void Add(string name, KyHyokaItem item)
        {
            if (this.Items.ContainsKey(name))
            {
                this.Items[name] = item;
            }
            else
            {
                this.Items.Add(name, item);
            }
        }

        public KyHyokaItem Get(string name)
        {
            KyHyokaItem kyHyoka;

            if (this.Items.ContainsKey(name))
            {
                kyHyoka = this.Items[name];
            }
            else
            {
                kyHyoka = new KyHyoka100limitItemImpl(1.0d, 0.0d, "ヌル");
            }

            return kyHyoka;
        }
    }
}
