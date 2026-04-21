using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pp_GD_KK
{
    public partial class UserControl5 : UserControl
    {
        public UserControl5()
        {
            InitializeComponent();
        }


        private void btn_Click(object sender, EventArgs e)
        {
            noFocusButton1.BackColor = SystemColors.Control;
            noFocusButton2.BackColor = SystemColors.Control;

            NoFocusButton clicked = (NoFocusButton)sender;
            clicked.BackColor = SystemColors.ActiveBorder;
        }
    }
}
