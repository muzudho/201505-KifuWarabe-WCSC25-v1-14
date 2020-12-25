using System.Collections.Generic;
using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.Kifuwarazusa.UseCases.Features
{
    public interface ShogiEngine
    {
        /// <summary>
        /// USI「setoption」コマンドのリストです。
        /// </summary>
        Dictionary<string, string> SetoptionDictionary { get; set; }


        PlayerInfo PlayerInfo { get; }
    }
}
