namespace Grayscale.Kifuwarazusa.Entities.Features
{
    public interface KifuTree : Tree<ShootingStarlightable, KyokumenWrapper>
    {
        //Tree<ShootingStarlightable, KyokumenWrapper> Tree { get; }

        Playerside CountPside(Node<ShootingStarlightable, KyokumenWrapper> node);

        /// <summary>
        /// 現在の先後
        /// </summary>
        Playerside CountPside(int tesumi);

        /// <summary>
        /// 取った駒を差替えます。
        /// 
        /// 棋譜読取時用です。マウス操作時は、流れが異なるので使えません。
        /// </summary>
        void AppendChildB_Swap(
            Ks14 tottaSyurui,
            SkyConst src_Sky,
            string hint
            );

        /// <summary>
        /// [ここから採譜]機能
        /// </summary>
        void SetStartpos_KokokaraSaifu(Playerside pside);

        /// <summary>
        /// 現局面のプレイヤーサイド。
        /// </summary>
        /// <returns></returns>
        Playerside CurrentPside();

        /// <summary>
        /// 現局面の、手済みカウントです。
        /// </summary>
        /// <returns></returns>
        int CurrentTesumi();
    }
}
