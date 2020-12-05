using Grayscale.P200_KifuNarabe.L100_GUI;
using System;
using System.Windows.Forms;

namespace Grayscale.P400_KifuNaraVs.L100_GUI
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
             */

            KifuNarabeVsImpl kifuNarabeVs = new KifuNarabeVsImpl();

            //↓ [STAThread]指定のあるメソッドで フォームを作成してください。
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            kifuNarabeVs.OwnerForm = new Ui_ShogiForm1(kifuNarabeVs);
            //↑ [STAThread]指定のあるメソッドで フォームを作成してください。

            kifuNarabeVs.Load_AsStart();
            kifuNarabeVs.WidgetLoaders.Add(new WidgetsLoader_KifuNarabe("../../Data/data_widgets_KifuNarabe.csv"));
            kifuNarabeVs.WidgetLoaders.Add(new WidgetsLoader_KifuNarabeVs("../../Data/data_widgets_KifuNarabeVs.csv"));

            kifuNarabeVs.LaunchForm_AsBody();

        }
    }
}
