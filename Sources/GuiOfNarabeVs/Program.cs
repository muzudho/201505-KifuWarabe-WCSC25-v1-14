using System;
using System.IO;
using System.Windows.Forms;
using Grayscale.P200_KifuNarabe.L100_GUI;
using Nett;

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

            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            var narabeConfigPath = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("NarabeGuiWidgets"));
            kifuNarabeVs.WidgetLoaders.Add(new WidgetsLoader_KifuNarabe(narabeConfigPath));
            var vsConfigPath = Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("VsGuiWidgets"));
            kifuNarabeVs.WidgetLoaders.Add(new WidgetsLoader_KifuNarabeVs(vsConfigPath));

            kifuNarabeVs.LaunchForm_AsBody();

        }
    }
}
