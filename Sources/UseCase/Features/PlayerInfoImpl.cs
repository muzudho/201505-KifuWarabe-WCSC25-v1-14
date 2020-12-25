using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.Kifuwarazusa.UseCases.Features
{

    /// <summary>
    /// プレイヤーの情報
    /// </summary>
    public class PlayerInfoImpl : PlayerInfo
    {

        public Playerside Playerside { get; set; }


        public PlayerInfoImpl()
        {
            this.Playerside = Playerside.Empty;
        }

        public PlayerInfoImpl(PlayerInfo playerInfo)
        {
            this.Playerside = playerInfo.Playerside;
        }


    }

}
