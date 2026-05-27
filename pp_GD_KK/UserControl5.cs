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
        private UserControl4 uc4Instance;
        private UserControl6 uc6Instance;

        public UserControl5()
        {
            InitializeComponent();
            uc4Instance = new UserControl4() { Dock = DockStyle.Fill, Name = "UserControl4" };
            uc6Instance = new UserControl6() { Dock = DockStyle.Fill, Name = "UserControl6" };
        }

        private void btn_Click(object sender, EventArgs e)
        {

        }
    }
}