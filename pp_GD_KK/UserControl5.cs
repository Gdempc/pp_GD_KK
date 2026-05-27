using System;
using System.Windows.Forms;

namespace pp_GD_KK
{
    public partial class UserControl5 : UserControl
    {
        public UserControl5()
        {
            InitializeComponent();
        }

        private void newsBtn_Click(object sender, EventArgs e)
        {
            GlobalnePanele.PanelTresc.Controls.Clear();

            UserControl4 ogloszenia = new UserControl4 { Dock = DockStyle.Fill };
            GlobalnePanele.PanelTresc.Controls.Add(ogloszenia);
        }

        private void eventBtn_Click(object sender, EventArgs e)
        {
            GlobalnePanele.PanelTresc.Controls.Clear();

            UserControl6 wydarzenia = new UserControl6 { Dock = DockStyle.Fill };
            GlobalnePanele.PanelTresc.Controls.Add(wydarzenia);
        }
    }
}
