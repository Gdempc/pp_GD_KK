using pp_GD_KK.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace pp_GD_KK
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GlobalnePanele.PanelMenu = splitContainer1.Panel1;
            GlobalnePanele.PanelTresc = splitContainer1.Panel2;

            UserControl3 menuL = new UserControl3 { Dock = DockStyle.Fill };
            splitContainer1.Panel1.Controls.Add(menuL);

            UserControl4 newsBox = new UserControl4 { Dock = DockStyle.Fill };
            splitContainer1.Panel2.Controls.Add(newsBox);
        }


    }
    public static class GlobalnePanele
    {
        public static System.Windows.Forms.SplitterPanel PanelMenu { get; set; }
        public static System.Windows.Forms.SplitterPanel PanelTresc { get; set; }
    }

    public static class Globals
    {
        public static int ID { get; set; }
    }

}
