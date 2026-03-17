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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            string password = PasswordTxt.Text;

            bool minLength = password.Length >= 8;
            bool minSChars = password.Count(c => !char.IsLetterOrDigit(c)) >= 2;

            if (minLength && minSChars)
            {
                label6.Visible = false;

                //Kod do dodania konta na baze danych
            }
            else label6.Visible = true;
        }
    }
}
