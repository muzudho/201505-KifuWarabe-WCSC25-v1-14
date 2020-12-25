using System;

namespace Grayscale.Kifuwarazusa.Entities
{
    public class EntitiesLayer
    {
        private static readonly Guid unique = Guid.NewGuid();
        public static Guid Unique { get { return unique; } }

    }
}
