//スプライト番号
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.P040_Kokoro.L00050_Kokoro;
using Grayscale.P045_Atama.L00025_KyHandan;

namespace Grayscale.P045_Atama.L050_KyHandan
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
