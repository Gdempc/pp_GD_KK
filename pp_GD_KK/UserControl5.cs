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
            Form mainForm = this.FindForm();
            if (mainForm == null) return;

            noFocusButton1.BackColor = SystemColors.Control;
            noFocusButton2.BackColor = SystemColors.Control;

            NoFocusButton clicked = (NoFocusButton)sender;
            clicked.BackColor = SystemColors.ActiveBorder;

            if (clicked.Text == "Ogłoszenia")
            {
                Control old = mainForm.Controls.Find("UserControl6", true).FirstOrDefault();

                if (old != null)
                {
                    var parent = old.Parent;
                    parent.Controls.Remove(old);

                    if (!parent.Controls.Contains(uc4Instance))
                    {
                        parent.Controls.Add(uc4Instance);
                        uc4Instance.BringToFront();
                    }
                }
            }

            if (clicked.Text == "Wydarzenia")
            {
                Control old = mainForm.Controls.Find("UserControl4", true).FirstOrDefault();

                if (old != null)
                {
                    var parent = old.Parent;
                    parent.Controls.Remove(old);

                    if (!parent.Controls.Contains(uc6Instance))
                    {
                        parent.Controls.Add(uc6Instance);
                        uc6Instance.BringToFront();
                    }
                }
            }
        }
    }
}