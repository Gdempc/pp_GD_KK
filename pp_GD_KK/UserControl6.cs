using pp_GD_KK.Properties;
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
    public partial class UserControl6 : UserControl
    {
        List<String> wydarzenia = new List<String> {"","",""};
        public UserControl6()
        {
            InitializeComponent();
        }

        private void UserControl6_Load(object sender, EventArgs e)
        {
            foreach (string s in wydarzenia)
            {
                FlowLayoutPanel p = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 180, BorderStyle = BorderStyle.FixedSingle };
                p.Width = (wydarzenia.Count > 3) ? flowLayoutPanel1.Width - 30 : flowLayoutPanel1.Width - 20;
                PictureBox pictureBox = new PictureBox { Height = p.Height - 10, Width = p.Height - 10, Image = Resources.latest, SizeMode = PictureBoxSizeMode.StretchImage, BackColor = Color.Red };
                RichTextBox txt = new RichTextBox { Text = s, Width = p.Width - pictureBox.Width - 15, Height = p.Height - 10, Enabled = false, BorderStyle = BorderStyle.FixedSingle };
                p.Controls.Add(pictureBox);
                p.Controls.Add(txt);
                flowLayoutPanel1.Controls.Add(p);
            }
        }
    }
}
