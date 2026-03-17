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
    public partial class Form2 : Form
    {

        string login = "admin";
        string password = "admin";
        public Form2()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            string log = textBox1.Text;
            string pas = maskedTextBox1.Text;


            if(log==login && pas == password) 
            {

            }
            else label3.Visible = true;

            //zmienić na sprawdzanie z bazą danych
        }

        private void LoginAsGuestBtn_Click(object sender, EventArgs e)
        {
            
        }
    }
}
