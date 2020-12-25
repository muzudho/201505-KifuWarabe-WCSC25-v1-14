﻿namespace Grayscale.Kifuwarazusa.Entities.Configuration
{
    public interface IEngineConf
    {
        string LogDirectory { get; }

        string GetEngine(string key);
        string GetResourceFullPath(string key);
        string GetLogBasename(string key);
    }
}
