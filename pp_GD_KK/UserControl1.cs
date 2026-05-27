using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace pp_GD_KK
{
    public partial class UserControl1 : UserControl
    {
        private readonly string connString = "Server=localhost;Database=wydarzeniastudenckie;Uid=root;Pwd=;";


        public UserControl1()
        {
            InitializeComponent();
        }


        private void LoginBtn_Click(object sender, EventArgs e)
        {
            label3.Visible = false;
            string login = LoginTxt.Text.Trim();
            string password = PasswdTxt.Text;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                label3.Text = "Wprowadź login i hasło!";
                label3.Visible = true;
                return;
            }

            string query = "SELECT Name, Surname, Admin FROM uzytkownicy WHERE Login = @Login AND Passwd = @Passwd";

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Passwd", password);

                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string name = reader.GetString("Name");
                                string surname = reader.GetString("Surname");
                                bool isAdmin = reader.GetBoolean("Admin");

                                GlobalnePanele.PanelMenu.Controls.Clear();

                                UserControl5 noweMenu = new UserControl5 { Dock = DockStyle.Fill };
                                GlobalnePanele.PanelMenu.Controls.Add(noweMenu);

                                WyczyscPola();

                            }
                            else
                            {
                                label3.Text = "Niepoprawny login lub hasło.";
                                label3.Visible= true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        label3.Text = "Błąd bazy danych przy logowaniu: " + ex.Message;
                        label3.Visible = true;
                    }
                }
            }
        }

        private void WyczyscPola()
        {
            LoginTxt.Clear();
            PasswdTxt.Clear();
        }
    }

 
}
