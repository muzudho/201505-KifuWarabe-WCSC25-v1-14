using Grayscale.Kifuwarazusa.Entities;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.Kifuwarazusa.UseCases
{
    public class Game : IGame
    {
        public Game()
        {
            this.Kifu = new KifuTreeImpl(
        new KifuNodeImpl(
            Util_Sky.NullObjectMove,
            new KyokumenWrapper(new SkyConst(Util_Sky.New_Hirate())),// きふわらべ起動時 // FIXME:平手とは限らないが。
            Playerside.P2
        )
);

            this.Kifu.SetProperty(KifuTreeImpl.PropName_FirstPside, Playerside.P1);
            this.Kifu.SetProperty(KifuTreeImpl.PropName_Startpos, "startpos");// 平手 // FIXME:平手とは限らないが。

            this.TesumiCount = 0;// ｎ手目
            this.GoPonderNow = false;   // go ponderを将棋所に伝えたなら真

        }

        /// <summary>
        /// 棋譜です。
        /// </summary>
        public KifuTree Kifu { get; set; }

        /// <summary>
        /// USI「ponder」の使用の有無です。
        /// ポンダーに対応している将棋サーバーなら真です。
        /// </summary>
        public bool UsiPonderEnabled { get; set; }

        /// <summary>
        /// 手目済カウントです。
        /// </summary>
        public int TesumiCount { get; set; }

        /// <summary>
        /// 「go ponder」の属性一覧です。
        /// </summary>
        public bool GoPonderNow { get; set; }

    }
}
