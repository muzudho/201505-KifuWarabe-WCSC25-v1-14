using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P045_Atama.L00025_KyHandan;
using Grayscale.P050_KifuWarabe.L00049_Kokoro;

namespace Grayscale.P050_KifuWarabe.L00052_Shogisasi
{
    /// <summary>
    /// 将棋指し。
    /// </summary>
    public interface Shogisasi
    {
        /// <summary>
        /// 心エンジン。
        /// </summary>
        Kokoro Kokoro { get; set; }

        /// <summary>
        /// 対局開始のとき。
        /// </summary>
        void OnTaikyokuKaisi();
                
        /// <summary>
        /// 指し手を決めます。
        /// </summary>
        /// <param name="enableLog"></param>
        /// <param name="isHonshogi"></param>
        /// <param name="kifu"></param>
        /// <param name="playerInfo"></param>
        /// <returns></returns>
        ShootingStarlightable WA_Bestmove(
            bool enableLog,
            bool isHonshogi,
            KifuTree kifu,
            PlayerInfo playerInfo
            );
    }
}
