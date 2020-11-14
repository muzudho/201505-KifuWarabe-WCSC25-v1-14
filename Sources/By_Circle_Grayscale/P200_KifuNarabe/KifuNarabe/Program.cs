
// 進行が停止するテストを含むデバッグモードです。
#define DEBUG_STOPPABLE


using Grayscale.P025_KifuLarabe;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L007_Random;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P200_KifuNarabe.L00012_Ui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
