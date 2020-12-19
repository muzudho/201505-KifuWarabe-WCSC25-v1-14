// 進行が停止するテストを含むデバッグモードです。
#define DEBUG_STOPPABLE

using System;
using System.Windows.Forms;
using Grayscale.P200_KifuNarabe.L100_GUI;

namespace Grayscale.P200_KifuNarabe
{

    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
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
