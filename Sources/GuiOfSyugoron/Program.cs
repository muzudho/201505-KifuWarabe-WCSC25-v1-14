using System;
using System.Windows.Forms;
using Grayscale.Kifuwarazusa.Engine.Configuration;
using Grayscale.Kifuwarazusa.Entities;

namespace Grayscale.Kifuwarazusa.GuiOfSyugoron.Features
{
    public class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            var engineConf = new EngineConf();
            EntitiesLayer.Implement(engineConf);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Ui_SyugoronTestForm());


            return 0;
        }

    }
}
