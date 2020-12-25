using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.Kifuwarazusa.UseCases.Features
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
