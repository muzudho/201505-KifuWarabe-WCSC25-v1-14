using System;
using System.IO;
using System.Windows.Forms;
using Grayscale.Kifuwarazusa.Engine.Configuration;
using Grayscale.Kifuwarazusa.Entities;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Features;
using Nett;

namespace Grayscale.Kifuwarazusa.GuiOfNarabeVs.Features
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

            /*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
             */

            KifuNarabeVsImpl kifuNarabeVs = new KifuNarabeVsImpl(engineConf);

            //↓ [STAThread]指定のあるメソッドで フォームを作成してください。
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            kifuNarabeVs.OwnerForm = new Ui_ShogiForm1(kifuNarabeVs);
            //↑ [STAThread]指定のあるメソッドで フォームを作成してください。

            kifuNarabeVs.Load_AsStart();

            kifuNarabeVs.WidgetLoaders.Add(new WidgetsLoader_KifuNarabe(engineConf.GetResourceFullPath("NarabeGuiWidgets")));
            kifuNarabeVs.WidgetLoaders.Add(new WidgetsLoader_KifuNarabeVs(engineConf.GetResourceFullPath("VsGuiWidgets")));

            kifuNarabeVs.LaunchForm_AsBody();

        }
    }
}
