using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.P050_KifuWarabe.L003_Kokoro
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
