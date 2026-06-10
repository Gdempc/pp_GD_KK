using pp_GD_KK.Properties;
using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace pp_GD_KK
{
    public partial class UserControl4 : UserControl
    {
        private readonly string connectionString = "Server=localhost;Database=wydarzeniastudenckie;Uid=root;Pwd=;";

        public UserControl4()
        {
            InitializeComponent();
        }

        private void UserControl4_Load(object sender, EventArgs e)
        {
            ZaladujOgloszenia();
        }

        private void ZaladujOgloszenia()
        {
            flowLayoutPanel1.Controls.Clear();

            string query = "SELECT ID, Title, Description, Image FROM ogloszenia ORDER BY ID DESC";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string title = reader.IsDBNull(reader.GetOrdinal("Title")) ? "Bez tytułu" : reader.GetString("Title");
                                string description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "Brak treści..." : reader.GetString("Description");

                                Image image = null;
                                if (!reader.IsDBNull(reader.GetOrdinal("Image")))
                                {
                                    try
                                    {
                                        byte[] imageBytes = (byte[])reader["Image"];
                                        using (MemoryStream ms = new MemoryStream(imageBytes))
                                        {
                                            image = Image.FromStream(ms);
                                        }
                                    }
                                    catch
                                    {
                                        image = null;
                                    }
                                }

                                Panel card = new Panel
                                {
                                    Width = flowLayoutPanel1.Width - 25,
                                    Height = 160,
                                    BorderStyle = BorderStyle.FixedSingle,
                                    BackColor = Color.White,
                                    Margin = new Padding(0, 0, 0, 15)
                                };

                                PictureBox pictureBox = new PictureBox
                                {
                                    Dock = DockStyle.Left,
                                    Width = 160,
                                    Image = image ?? Properties.Resources.latest,
                                    SizeMode = PictureBoxSizeMode.Zoom,
                                    BackColor = Color.FromArgb(250, 250, 250),
                                    Padding = new Padding(5)
                                };

                                Panel textPanel = new Panel
                                {
                                    Dock = DockStyle.Fill,
                                    Padding = new Padding(15, 10, 15, 10)
                                };

                                Label lblTitle = new Label
                                {
                                    Text = title,
                                    Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                                    ForeColor = Color.FromArgb(33, 37, 41),
                                    Dock = DockStyle.Top,
                                    Height = 28,
                                    AutoSize = false
                                };

                                Label lblDesc = new Label
                                {
                                    Text = description,
                                    Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                                    ForeColor = Color.FromArgb(108, 117, 125),
                                    Dock = DockStyle.Fill,
                                    AutoSize = false
                                };

                                textPanel.Controls.Add(lblDesc);
                                textPanel.Controls.Add(lblTitle);

                                card.Controls.Add(textPanel);
                                card.Controls.Add(pictureBox);

                                flowLayoutPanel1.Controls.Add(card);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Błąd pobierania ogłoszeń: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}