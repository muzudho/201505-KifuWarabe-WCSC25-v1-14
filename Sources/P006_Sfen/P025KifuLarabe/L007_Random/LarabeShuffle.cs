using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.P025_KifuLarabe.L007_Random
{
    public abstract class LarabeShuffle<T>
    {

        public static void Shuffle_FisherYates(ref List<T> items)
        {

            int n = items.Count;
            while (n > 1)
            {
                n--;
                int k = LarabeRandom.Random.Next(n + 1);
                T tmp = items[k];
                items[k] = items[n];
                items[n] = tmp;
            }

        }

    }
}
