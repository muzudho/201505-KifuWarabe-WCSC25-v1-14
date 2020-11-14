using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L012_Common;
//スプライト番号
using Grayscale.P045_Atama.L000125_Sokutei;
using Grayscale.P045_Atama.L00025_KyHandan;
using Grayscale.P040_Kokoro.L00050_Kokoro;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;

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
