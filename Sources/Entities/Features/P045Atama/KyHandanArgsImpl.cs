//スプライト番号

namespace Grayscale.Kifuwarazusa.Entities.Features
{

    /// <summary>
    /// 局面評価の計算用の引数。
    /// </summary>
    public class KyHandanArgsImpl : KyHandanArgs
    {
        public TenonagareGenjo TenonagareGenjo { get { return this.tenonagareGenjo; } }
        private TenonagareGenjo tenonagareGenjo;

        public KifuNode Node { get { return this.node; } }
        private KifuNode node;

        public PlayerInfo PlayerInfo { get { return this.playerInfo; } }
        private PlayerInfo playerInfo;

        public KyHandanArgsImpl(
            TenonagareGenjo tenonagareGenjo,
            KifuNode node,
            PlayerInfo playerInfo
            )
        {
            this.tenonagareGenjo = tenonagareGenjo;
            this.node = node;
            this.playerInfo = playerInfo;
        }

    }
}
