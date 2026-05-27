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


        List<String> wydarzenia = new List<String> { "a", "b", "c" };
        
        private UserControl7 uc7Instance;

        public UserControl6()
        {
            InitializeComponent();
            uc7Instance = new UserControl7() { Dock = DockStyle.Fill, Name = "UserControl7" };
        }


        

        private void UserControl6_Load(object sender, EventArgs e)
        {
            foreach (string s in wydarzenia)
            {
                FlowLayoutPanel p = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 180, BorderStyle = BorderStyle.FixedSingle };
                p.Click += DynamicControl_Click;
                p.Width = (wydarzenia.Count > 3) ? flowLayoutPanel1.Width - 30 : flowLayoutPanel1.Width - 20;
                PictureBox pictureBox = new PictureBox { Height = p.Height - 10, Width = p.Height - 10, Image = Resources.latest, SizeMode = PictureBoxSizeMode.StretchImage, BackColor = Color.Red };
                FlowLayoutPanel p2 = new FlowLayoutPanel { Height = p.Height-10, Width = p.Width - pictureBox.Width - 15, FlowDirection=FlowDirection.TopDown};
                TextBox txtT = new TextBox { Text = s, Width = p2.Width - 10, Enabled = false, BorderStyle = BorderStyle.FixedSingle, TextAlign = HorizontalAlignment.Center};
                RichTextBox txtO = new RichTextBox { Text = s, Width = p2.Width - 10, Height = p2.Height - 35, Enabled = false, BorderStyle = BorderStyle.FixedSingle };
                p.Controls.Add(pictureBox);
                p2.Controls.Add(txtT);
                p2.Controls.Add(txtO);
                p.Controls.Add(p2);
                flowLayoutPanel1.Controls.Add(p);
            }
        }

        private void DynamicControl_Click(object sender, EventArgs e)
        {
            Control kliknietyElement = sender as Control;
            FlowLayoutPanel panel = (kliknietyElement is FlowLayoutPanel) ?
                                     (FlowLayoutPanel)kliknietyElement :
                                     (FlowLayoutPanel)kliknietyElement.Parent;

            var parent = this.Parent;

            parent.Controls.Remove(this);
            parent.Controls.Add(uc7Instance);

            uc7Instance.BringToFront();
        }
    }
}
