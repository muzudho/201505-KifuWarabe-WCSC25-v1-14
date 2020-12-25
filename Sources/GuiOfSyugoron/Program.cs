using System;
using System.Windows.Forms;

namespace Grayscale.Kifuwarazusa.Entities.Features
{
    public class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Ui_SyugoronTestForm());


            return 0;
        }

    }
}
