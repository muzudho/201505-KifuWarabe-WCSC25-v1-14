
namespace Grayscale.Kifuwarazusa.Entities.Features
{

    /// <summary>
    /// 頭の隅に置いてある、手の流れ。
    /// </summary>
    public enum TenonagareName
    {
        /// <summary>
        /// 「駒Aが、升Bを目指す」狙い。
        /// </summary>
        Ido,

        /// <summary>
        /// 「駒Aが、駒Bを取ることを目指す」狙い。
        /// </summary>
        Toru,

        /// <summary>
        /// 「駒Aの突き捨て」狙い。
        /// </summary>
        Tukisute,

        /// <summary>
        /// 「駒Aが自分の駒なら、何点」という狙い。
        /// </summary>
        KomaDoku,


        /// <summary>
        /// 「紐が付いていない駒の少なさ」という狙い。
        /// </summary>
        Himoduki,

        /// <summary>
        /// 角頭の紐付き
        /// </summary>
        KakuTouNoHimoduki,

        /// <summary>
        /// 目の前の歩を取れ
        /// </summary>
        MenomaenoFuWoTore,

        /// <summary>
        /// 「気まぐれ」という狙い。
        /// </summary>
        Kimagure,

        /// <summary>
        /// 「飛車道などが通っている」という狙い。
        /// </summary>
        Toosi,

        /// <summary>
        /// 「玉の守り」という狙い。
        /// </summary>
        GyokuNoMamori,

    }
}
