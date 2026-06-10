using MySql.Data.MySqlClient;
using pp_GD_KK.Properties;
using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace pp_GD_KK
{
    public partial class UserControl2 : UserControl
    {
        private readonly string connString = "Server=localhost;Database=wydarzeniastudenckie;Uid=root;Pwd=;";

        public UserControl2()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            label6.Visible = false;
            if (string.IsNullOrWhiteSpace(LoginTxt.Text) ||
                string.IsNullOrWhiteSpace(PasswordTxt.Text) ||
                string.IsNullOrWhiteSpace(NameTxt.Text) ||
                string.IsNullOrWhiteSpace(SurnameTxt.Text))
            {
                label6.Text = "Proszę wypełnić wszystkie pola!";
                label6.Visible = true;
                return;
            }

            string login = LoginTxt.Text.Trim();
            string password = PasswordTxt.Text;
            string name = NameTxt.Text.Trim();
            string surname = SurnameTxt.Text.Trim();
            bool isAdmin = false;

            byte[] imageBytes = ImageToBytes(Resources.Profil);

            MatchCollection matches = Regex.Matches(password, @"[^a-zA-Z0-9]");

            if (password.Length < 8 || matches.Count < 2)
            {
                label6.Text = "Błędne hasło";
                label6.Visible = true;
                return;
            }

            if (CzyLoginZajety(login))
            {
                label6.Text = "Ten login jest już zajęty. Wybierz inny.";
                label6.Visible = true;
                return;
            }

            string query = "INSERT INTO uzytkownicy (Login, Passwd, Name, Surname, Admin, Image) " +
                           "VALUES (@Login, @Passwd, @Name, @Surname, @Admin, @Image)";

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Passwd", password);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Surname", surname);
                    command.Parameters.AddWithValue("@Admin", isAdmin);
                    command.Parameters.AddWithValue("@Image", imageBytes); 

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Konto zostało pomyślnie utworzone!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            WyczyscPola();
                        }
                    }
                    catch (Exception ex)
                    {
                        label6.Text = "Błąd bazy danych podczas rejestracji: " + ex.Message;
                        label6.Visible = true;
                        Console.Write(ex.Message);
                    }
                }
            }
        }

        private byte[] ImageToBytes(Image img)
        {
            if (img == null) return null;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private bool CzyLoginZajety(string login)
        {
            string query = "SELECT COUNT(*) FROM uzytkownicy WHERE Login = @Login";

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    try
                    {
                        connection.Open();
                        long count = (long)command.ExecuteScalar();
                        return count > 0;
                    }
                    catch
                    {
                        return true;
                    }
                }
            }
        }

        private void WyczyscPola()
        {
            LoginTxt.Clear();
            PasswordTxt.Clear();
            NameTxt.Clear();
            SurnameTxt.Clear();
        }
    }
}
