using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace pp_GD_KK
{
    public partial class UserControl6 : UserControl
    {
        private readonly string connString = "Server=localhost;Database=wydarzeniastudenckie;Uid=root;Pwd=;";

        public UserControl6()
        {
            InitializeComponent();
        }

        // Wywołujemy ładowanie wydarzeń przy wyświetleniu kontrolki
        private void UserControl6_Load(object sender, EventArgs e)
        {
            ZaladujWydarzenia();
        }

        private void ZaladujWydarzenia()
        {
            // Czyszczenie starych elementów z panelu przed przeładowaniem
            flowLayoutPanel1.Controls.Clear();

            string query = "SELECT id, Title, Description, Image, UserAmount FROM wydarzenia";

            using (MySqlConnection connection = new MySqlConnection(connString))
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
                                int id = reader.GetInt32("id");
                                string title = reader.GetString("Title");
                                string description = reader.GetString("Description");
                                int userAmount = reader.GetInt32("UserAmount");

                                Image image = null;
                                if (!reader.IsDBNull(reader.GetOrdinal("Image")))
                                {
                                    byte[] imageBytes = (byte[])reader["Image"];
                                    image = BytesToImage(imageBytes);
                                }

                                // Tworzenie wizualnego kafelka dla wydarzenia
                                Panel eventCard = StworzKafelekWydarzenia(id, title, description, image, userAmount);
                                
                                // Dodanie kafelka do głównej listy
                                flowLayoutPanel1.Controls.Add(eventCard);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Błąd ładowania wydarzeń: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private FlowLayoutPanel StworzKafelekWydarzenia(int id, string tytul, string opis, Image zdjecie, int iloscOsob)


        {
            FlowLayoutPanel card = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Size = new Size(flowLayoutPanel1.Width - 25, 180),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.DarkBlue,
            };

            PictureBox pb = new PictureBox
            {
                Size = new Size(180, 180),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = zdjecie ?? Properties.Resources.latest
            };

            FlowLayoutPanel card2 = new FlowLayoutPanel 
            { 
                Height = 170, 
                Width = card.Width - pb.Width - 120, 
                FlowDirection = FlowDirection.TopDown,
                BackColor = Color.White,
            };
            Label lblTitle = new Label
            {
                Text = tytul,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(180, 10),
                Size = new Size(400, 20)
            };

            Label lblDesc = new Label
            {
                Text = opis,
                Font = new Font("Arial", 9),
                Location = new Point(180, 40),
                Size = new Size(100, 130),
                BackColor = Color.Teal
            };

            FlowLayoutPanel card3 = new FlowLayoutPanel
            {
                Height = 170,
                Width = 80,
                FlowDirection = FlowDirection.TopDown,
                BackColor = Color.Gray,
            };

            Label lblAmount = new Label
            {
                Text = $"{iloscOsob}",
                Font = new Font("Arial", 9, FontStyle.Italic),
                Size = new Size(50, 60)
            };

            Button btnJoin = new Button
            {
                Text = "Dołącz",
                Size = new Size(50, 80),
                Tag = id
            };
            
            btnJoin.Click += BtnJoin_Click;

            card2.Controls.Add(lblTitle);
            card2.Controls.Add(lblDesc);
            card3.Controls.Add(lblAmount);
            card3.Controls.Add(btnJoin);
            card.Controls.Add(pb);
            card.Controls.Add(card2);
            card.Controls.Add(card3);

            return card;
        }

        private void BtnJoin_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int wydarzenieId = (int)btn.Tag;

            MessageBox.Show($"Kliknięto dołącz do wydarzenia o ID: {wydarzenieId}");
        }

        private Image BytesToImage(byte[] imageBytes)
        {
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                return Image.FromStream(ms);
            }
        }
    }
}
