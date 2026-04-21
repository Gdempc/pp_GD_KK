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
    public partial class UserControl1 : UserControl
    {
        String login = "123";
        String passwd = "123";
        public UserControl1()
        {
            InitializeComponent();
        }


        private void LoginBtn_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == login && maskedTextBox1.Text == passwd) 
            {
                Form mainForm = this.FindForm();

                if (mainForm != null)
                {
                    Control old = mainForm.Controls.Find("userControl3", true).FirstOrDefault();


                    if (old != null)
                    {
                        Point position = old.Location;
                        var parent = old.Parent;

                        parent.Controls.Remove(old);

                        UserControl5 menuPL = new UserControl5();
                        parent.Controls.Add(menuPL);
                        menuPL.BringToFront();
                        this.Parent.Controls.Remove(this);
                        this.Dispose();
                    }
                        
                            
                            
                }
            }
        }
    }
}
