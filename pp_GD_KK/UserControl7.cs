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
    public partial class UserControl7 : UserControl
    {
        public UserControl7()
        {
            InitializeComponent();
        }

        public void SetData(string title, string desc, Image picture)
        {
            textBox1.Text = title;
            richTextBox2.Text = desc;
            pictureBox1.Image = picture;
        }
    }
}
