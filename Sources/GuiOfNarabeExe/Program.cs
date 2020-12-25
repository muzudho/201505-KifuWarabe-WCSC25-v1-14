// 進行が停止するテストを含むデバッグモードです。
#define DEBUG_STOPPABLE

using System;
using System.Windows.Forms;
using Grayscale.Kifuwarazusa.Engine.Configuration;
using Grayscale.Kifuwarazusa.Entities;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Features;

namespace Grayscale.Kifuwarazusa.GuiOfNarabeExe
{

    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            var engineConf = new EngineConf();
            EntitiesLayer.Implement(engineConf);

            KifuNarabeImpl kifuNarabe = new KifuNarabeImpl();

            //↓ [STAThread]指定のあるメソッドで フォームを作成してください。
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            kifuNarabe.OwnerForm = new Ui_ShogiForm1(kifuNarabe);
            //↑ [STAThread]指定のあるメソッドで フォームを作成してください。

            kifuNarabe.Load_AsStart();
            kifuNarabe.WidgetLoaders.Add(new WidgetsLoader_KifuNarabe("../../Data/data_widgets_KifuNarabe.csv"));
            kifuNarabe.LaunchForm_AsBody();
        }
    }
}
