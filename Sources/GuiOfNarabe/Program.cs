
// 進行が停止するテストを含むデバッグモードです。
#define DEBUG_STOPPABLE


using System;
using System.IO;
using System.Windows.Forms;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Features;
using Nett;

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
            KifuNarabeImpl kifuNarabe = new KifuNarabeImpl();

            //↓ [STAThread]指定のあるメソッドで フォームを作成してください。
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            kifuNarabe.OwnerForm = new Ui_ShogiForm1(kifuNarabe);
            //↑ [STAThread]指定のあるメソッドで フォームを作成してください。

            kifuNarabe.Load_AsStart();

            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            var configPath = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("NarabeGuiWidgets"));
            kifuNarabe.WidgetLoaders.Add(new WidgetsLoader_KifuNarabe(configPath));

            kifuNarabe.LaunchForm_AsBody();
        }
    }
}
