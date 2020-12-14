using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grayscale.P006_Syugoron
{
    public partial class Ui_SyugoronTestForm : Form
    {

        public Ui_SyugoronTestPanel Ui_SyugoronTestPanel
        {
            get
            {
                return this.ui_SyugoronTestPanel;
            }
        }

        public Ui_SyugoronTestForm()
        {
            InitializeComponent();
        }

    }
}
