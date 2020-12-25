using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.Kifuwarazusa.Entities
{
    public interface IGame
    {
        /// <summary>
        /// 棋譜です。
        /// </summary>
        KifuTree Kifu { get; set; }

        /// <summary>
        /// USI「ponder」の使用の有無です。
        /// ポンダーに対応している将棋サーバーなら真です。
        /// </summary>
        bool UsiPonderEnabled { get; set; }

        /// <summary>
        /// 手目済カウントです。
        /// </summary>
        int TesumiCount { get; set; }

        /// <summary>
        /// 「go ponder」の属性一覧です。
        /// </summary>
        bool GoPonderNow { get; set; }
    }
}
