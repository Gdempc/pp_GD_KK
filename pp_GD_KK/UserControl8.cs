using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace pp_GD_KK
{
    public partial class UserControl8 : UserControl
    {
        private readonly string connString = "Server=localhost;Database=wydarzeniastudenckie;Uid=root;Pwd=;";

        public UserControl8()
        {
            InitializeComponent();

            circularPictureBox1.Click += CircularPictureBox1_Click;
        }

        public void UstawDaneUzytkownika(string imie, string nazwisko, Image zdjecie)
        {
            label1.Text = $"{imie} {nazwisko}";

            if (zdjecie != null)
            {
                circularPictureBox1.Image = zdjecie;
            }
        }

        private void CircularPictureBox1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Pliki graficzne (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Wybierz zdjęcie profilowe";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Image noweZdjecie = Image.FromFile(openFileDialog.FileName);

                        byte[] imageBytes = ImageToBytes(noweZdjecie);

                        if (ZapiszZdjecieDoBazy(imageBytes))
                        {
                            circularPictureBox1.Image = noweZdjecie;
                            MessageBox.Show("Zdjęcie profilowe zostało zaktualizowane!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Nie udało się załadować lub zapisać zdjęcia: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private bool ZapiszZdjecieDoBazy(byte[] imageBytes)
        {
            string query = "UPDATE uzytkownicy SET Image = @Image WHERE ID = @Id";

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Image", imageBytes);
                    command.Parameters.AddWithValue("@Id", Globals.ID); 

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Błąd bazy danych podczas zapisu zdjęcia: " + ex.Message, "Błąd bazy danych", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
        }

        private byte[] ImageToBytes(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GlobalnePanele.PanelTresc.Controls.Clear();

            UserControl4 ogloszenia = new UserControl4 { Dock = DockStyle.Fill };
            GlobalnePanele.PanelTresc.Controls.Add(ogloszenia);
            GlobalnePanele.PanelMenu.Controls.Clear();

            UserControl3 log = new UserControl3 { Dock = DockStyle.Fill };
            GlobalnePanele.PanelMenu.Controls.Add(log);
            Globals.ID = 0;
        }
    }
}