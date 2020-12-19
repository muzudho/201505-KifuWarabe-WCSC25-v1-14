
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P200_KifuNarabe.L00048_ShogiGui;
using Grayscale.P200_KifuNarabe.L100_GUI;
using Grayscale.P400_KifuNaraVs.L025_ShogiEngine;
using Grayscale.P400_KifuNaraVs.L00048_Engine;
using Grayscale.Kifuwarazusa.Entities.Logging;

//スプライト番号
namespace Grayscale.P400_KifuNaraVs.L100_GUI
{
    public class KifuNarabeVsImpl : KifuNarabeImpl, ShogiGui
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
        public override void ChangeTurn( ILogTag logTag)
        {
            this.ShogiEnginePrWrapperLauncher.ChangeTurn99(this.Model_PnlTaikyoku.Kifu, logTag);
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
