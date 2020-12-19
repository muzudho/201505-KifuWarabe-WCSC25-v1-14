
using Grayscale.Kifuwarazusa.Entities.Features.Gui;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;

namespace Grayscale.P025_KifuLarabe.L00025_Struct
{
    public class Model_PnlTaikyokuImpl : IGameViewModel
    {
        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// GUI用局面データ。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        /// 局面が進むごとに更新されていきます。
        /// 
        /// </summary>
        public SkyConst GuiSkyConst { get { return this.guiSky; } }
        public void SetGuiSky(SkyConst sky)
        {
            this.guiSky = sky;
        }
        private SkyConst guiSky;
        public int GuiTesumi { get; set; }
        public Playerside GuiPside { get; set; }

        public KifuTree Kifu
        {
            get
            {
                return this.kifu;
            }
        }
        private KifuTree kifu;



        public Model_PnlTaikyokuImpl()
        {
            //
            // 駒なし
            //
            this.guiSky = Util_Sky.New_Komabukuro();// 描画モデル作成時

            this.GuiTesumi = 0;
            this.GuiPside = Playerside.P1;



            //
            //
            //

            this.kifu = new KifuTreeImpl(
                    new KifuNodeImpl(
                        Util_Sky.NullObjectMove,
                        new KyokumenWrapper( new SkyConst( this.guiSky)),
                        Playerside.P2
                    )
            );
            this.Init();
        }

        public Model_PnlTaikyokuImpl(KifuTree kifu)
        {
            this.kifu = kifu;
            this.Init();
        }

        private void Init()
        {
            this.Kifu.SetProperty(KifuTreeImpl.PropName_FirstPside, Playerside.P1);
            this.Kifu.SetProperty(KifuTreeImpl.PropName_Startpos, "9/9/9/9/9/9/9/9/9");
        }



    }
}
