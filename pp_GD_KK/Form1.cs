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
    public partial class Form1 : Form
    {
        List<string> strings = new List<string> {"ogłoszenie1", "ogłoszenie2", "ogłoszenie2", "ogłoszenie2" };

        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string s in strings)
            {
                FlowLayoutPanel p = new FlowLayoutPanel();
                p.Dock = DockStyle.Top;
                p.Height = 180;
                if (strings.Count > 3)
                {
                    p.Width = flowLayoutPanel1.Width - 30;
                }
                else p.Width = flowLayoutPanel1.Width - 20;
                p.BorderStyle = BorderStyle.FixedSingle;

                PictureBox pictureBox = new PictureBox();
                pictureBox.Height = p.Height-10;
                pictureBox.Width = pictureBox.Height;
                pictureBox.Image = Resources.latest;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.BackColor = Color.Red;

                RichTextBox txt = new RichTextBox();
                txt.Text = s;
                txt.Width = p.Width - pictureBox.Width-15;
                txt.Height = p.Height - 10;
                txt.Cursor = Cursors.Default;
                txt.Enabled = false;
                txt.BorderStyle = BorderStyle.FixedSingle;

                p.Controls.Add(pictureBox);
                p.Controls.Add(txt);

                flowLayoutPanel1.Controls.Add(p);
            }
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            Form2 okno2 = new Form2();
            okno2.Show();
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            Form3 okno3 = new Form3();
            okno3.Show();
            //komentarz?
        }
    }
}
