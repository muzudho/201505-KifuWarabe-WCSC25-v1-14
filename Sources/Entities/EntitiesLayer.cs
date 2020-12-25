using System;
using Grayscale.Kifuwarazusa.Entities.Configuration;
using Grayscale.Kifuwarazusa.Entities.Logging;

namespace Grayscale.Kifuwarazusa.Entities
{
    public class EntitiesLayer
    {
        private static readonly Guid unique = Guid.NewGuid();
        public static Guid Unique { get { return unique; } }

        public static void Implement(IEngineConf engineConf)
        {
            SpecifyFiles.Init(engineConf);
            Logger.Init(engineConf);
        }
    }
}
