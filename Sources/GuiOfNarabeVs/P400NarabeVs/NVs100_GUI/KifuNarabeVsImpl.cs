using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Gui;
using Grayscale.P200_KifuNarabe.L100_GUI;
using Grayscale.P400_KifuNaraVs.L00048_Engine;
using Grayscale.P400_KifuNaraVs.L025_ShogiEngine;

namespace Grayscale.P400_KifuNaraVs.L100_GUI
{
    public class KifuNarabeVsImpl : KifuNarabeImpl, NarabeRoomViewModel
    {

        public ShogiEngineLive ShogiEnginePrWrapperLauncher { get { return this.shogiEnginePrWrapperLauncher; } }
        private ShogiEngineLive shogiEnginePrWrapperLauncher;

        public KifuNarabeVsImpl():base()
        {
            this.shogiEnginePrWrapperLauncher = new ShogiEngineLiveImpl(this);
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 手番が替わったときの挙動を、ここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        public override void ChangeTurn()
        {
            this.ShogiEnginePrWrapperLauncher.ChangeTurn99(this.GameViewModel.Kifu);
        }

        /// <summary>
        /// 将棋エンジンに、終了するように促します。
        /// </summary>
        public override void Shutdown()
        {
            this.ShogiEnginePrWrapperLauncher.ShogiEngineManInterface.Shutdown();
        }

        /// <summary>
        /// 将棋エンジンに、ログを出すように促します。
        /// </summary>
        public override void Logdase()
        {
            this.ShogiEnginePrWrapperLauncher.ShogiEngineManInterface.Logdase();
        }

        
        /// <summary>
        /// ************************************************************************************************************************
        /// 将棋エンジンを起動します。
        /// ************************************************************************************************************************
        /// </summary>
        public override void Start(string shogiEngineFilePath)
        {
            this.ShogiEnginePrWrapperLauncher.Start(shogiEngineFilePath);
        }

    }
}
