using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace pp_GD_KK
{
    public partial class UserControl7 : UserControl
    {
        private readonly string connString = "Server=localhost;Database=wydarzeniastudenckie;Uid=root;Pwd=;";
        private readonly WydarzenieData _wydarzenie;

        private FlowLayoutPanel flowUczestnicy;
        private FlowLayoutPanel flowKomentarze;

        public UserControl7(WydarzenieData wydarzenie)
        {
            InitializeComponent();

            _wydarzenie = wydarzenie;

            textBox1.Text = _wydarzenie.Title;
            richTextBox2.Text = _wydarzenie.Description;
            pictureBox1.Image = _wydarzenie.Image;
            label3.Text = $"👤 Osób: {_wydarzenie.UserAmount}";

            label1.Text = $"📅 {_wydarzenie.Date.ToString("dd.MM.yyyy")}";

            if (_wydarzenie.WholeDay)
            {
                label2.Text = "⏰ Cały dzień";
            }
            else
            {
                label2.Text = $"⏰ {_wydarzenie.FromT.ToString(@"hh\:mm")} - {_wydarzenie.ToT.ToString(@"hh\:mm")}";
            }

            UstawWygladGuzikaDołącz();
        }

        private void UserControl7_Load(object sender, EventArgs e)
        {
            panel3.BackColor = Color.FromArgb(245, 246, 248);
            InicjalizujPanelUczestnikow();
            InicjalizujPanelKomentarzy();

            ZaladujZdjeciaUczestnikow();
            ZaladujKomentarze();
        }

        private bool CzyUzytkownikZapisany(int wydarzenieId, int uzytkownikId)
        {
            string query = "SELECT COUNT(*) FROM uzytkownicy_wydarzenia WHERE uzytkownik_id = @UserId AND wydarzenie_id = @EventId";
            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", uzytkownikId);
                    command.Parameters.AddWithValue("@EventId", wydarzenieId);
                    try
                    {
                        connection.Open();
                        long count = (long)command.ExecuteScalar();
                        return count > 0;
                    }
                    catch { return false; }
                }
            }
        }

        private void UstawWygladGuzikaDołącz()
        {
            bool zapisany = CzyUzytkownikZapisany(_wydarzenie.Id, Globals.ID);
            if (zapisany)
            {
                button2.Text = "Opuść";
                button2.BackColor = Color.FromArgb(220, 53, 69);
            }
            else
            {
                button2.Text = "Dołącz";
                button2.BackColor = Color.FromArgb(13, 110, 253);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int wydarzenieId = _wydarzenie.Id;
            int uzytkownikId = Globals.ID;

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                try
                {
                    connection.Open();

                    if (button2.Text == "Dołącz")
                    {
                        string insertQuery = "INSERT INTO uzytkownicy_wydarzenia (uzytkownik_id, wydarzenie_id) VALUES (@UserId, @EventId)";
                        using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                        {
                            insertCmd.Parameters.AddWithValue("@UserId", uzytkownikId);
                            insertCmd.Parameters.AddWithValue("@EventId", wydarzenieId);
                            insertCmd.ExecuteNonQuery();
                        }

                        _wydarzenie.UserAmount += 1;
                    }
                    else
                    {
                        string deleteQuery = "DELETE FROM uzytkownicy_wydarzenia WHERE uzytkownik_id = @UserId AND wydarzenie_id = @EventId";
                        using (MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, connection))
                        {
                            deleteCmd.Parameters.AddWithValue("@UserId", uzytkownikId);
                            deleteCmd.Parameters.AddWithValue("@EventId", wydarzenieId);
                            deleteCmd.ExecuteNonQuery();
                        }

                        _wydarzenie.UserAmount -= 1;
                    }

                    string updateCountQuery = "UPDATE wydarzenia SET UserAmount = (SELECT COUNT(*) FROM uzytkownicy_wydarzenia WHERE wydarzenie_id = @EventId) WHERE id = @EventId";
                    using (MySqlCommand updateCmd = new MySqlCommand(updateCountQuery, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@EventId", wydarzenieId);
                        updateCmd.ExecuteNonQuery();
                    }

                    label3.Text = $"👤 Osób: {_wydarzenie.UserAmount}";
                    UstawWygladGuzikaDołącz();

                    InicjalizujPanelUczestnikow();
                    ZaladujZdjeciaUczestnikow();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Wystąpił błąd: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string tekstKomentarza = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(tekstKomentarza))
            {
                return;
            }

            string insertQuery = "INSERT INTO komentarze (wydarzenie_id, uzytkownik_id, Tresc) VALUES (@WydarzenieId, @UzytkownikId, @Tresc)";

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@WydarzenieId", _wydarzenie.Id);
                    command.Parameters.AddWithValue("@UzytkownikId", Globals.ID);
                    command.Parameters.AddWithValue("@Tresc", tekstKomentarza);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                        textBox2.Clear();

                        ZaladujKomentarze();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Błąd podczas dodawania komentarza: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void InicjalizujPanelUczestnikow()
        {
            if (flowUczestnicy != null)
            {
                panel7.Controls.Remove(flowUczestnicy);
                flowUczestnicy.Dispose();
            }

            flowUczestnicy = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = false,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(3),
                BackColor = Color.FromArgb(245, 246, 248)
            };
            panel7.Controls.Add(flowUczestnicy);
            flowUczestnicy.SendToBack();
        }

        private void InicjalizujPanelKomentarzy()
        {
            flowKomentarze = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10),
                BackColor = Color.White
            };
            panel5.Controls.Add(flowKomentarze);
        }

        private void ZaladujZdjeciaUczestnikow()
        {
            string query = @"SELECT u.ID, u.Name, u.Surname, u.Image 
                             FROM uzytkownicy u 
                             INNER JOIN uzytkownicy_wydarzenia uw ON u.ID = uw.uzytkownik_id 
                             WHERE uw.wydarzenie_id = @WydarzenieId";

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@WydarzenieId", _wydarzenie.Id);

                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Image fotoUzytkownika = null;
                                if (!reader.IsDBNull(reader.GetOrdinal("Image")))
                                {
                                    byte[] imageBytes = (byte[])reader["Image"];
                                    fotoUzytkownika = BytesToImage(imageBytes);
                                }

                                Panel userAvatar = StworzMiniaturkeUzytkownika(fotoUzytkownika);
                                flowUczestnicy.Controls.Add(userAvatar);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Błąd ładowania uczestników: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ZaladujKomentarze()
        {
            flowKomentarze.Controls.Clear();

            string query = @"SELECT k.ID, k.Tresc, k.DataDodania, u.Name, u.Surname, u.Image 
                             FROM komentarze k 
                             INNER JOIN uzytkownicy u ON k.uzytkownik_id = u.ID 
                             WHERE k.wydarzenie_id = @WydarzenieId 
                             ORDER BY k.DataDodania DESC";

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@WydarzenieId", _wydarzenie.Id);

                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string imie = reader.IsDBNull(reader.GetOrdinal("Name")) ? "Anonim" : reader.GetString("Name");
                                string nazwisko = reader.IsDBNull(reader.GetOrdinal("Surname")) ? "" : reader.GetString("Surname");

                                KomentarzData kom = new KomentarzData
                                {
                                    Id = reader.GetInt32("ID"),
                                    Tresc = reader.IsDBNull(reader.GetOrdinal("Tresc")) ? "" : reader.GetString("Tresc"),
                                    DataDodania = reader.GetDateTime("DataDodania"),
                                    AutorImieNazwisko = $"{imie} {nazwisko}"
                                };

                                if (!reader.IsDBNull(reader.GetOrdinal("Image")))
                                {
                                    byte[] imageBytes = (byte[])reader["Image"];
                                    kom.AutorAvatar = BytesToImage(imageBytes);
                                }

                                Panel paskiKomentarza = StworzKafelekKomentarza(kom);
                                flowKomentarze.Controls.Add(paskiKomentarza);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Błąd ładowania komentarzy: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private Panel StworzKafelekKomentarza(KomentarzData kom)
        {
            int panelWidth = panel2.Width - 30;
            if (panelWidth < 200) panelWidth = 300;

            Panel komCard = new Panel
            {
                Size = new Size(panelWidth, 75),
                BackColor = Color.FromArgb(248, 249, 250),
                Margin = new Padding(0, 0, 0, 10),
                Padding = new Padding(5)
            };

            CircularPictureBox avatar = new CircularPictureBox
            {
                BorderSize=0,
                Size = new Size(45, 45),
                Location = new Point(8, 8),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = kom.AutorAvatar ?? Properties.Resources.latest
            };

            Label lblHeader = new Label
            {
                Text = $"{kom.AutorImieNazwisko}  •  {kom.DataDodania.ToString("dd.MM HH:mm")}",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64),
                Location = new Point(60, 6),
                Size = new Size(panelWidth - 70, 18)
            };

            Label lblTresc = new Label
            {
                Text = kom.Tresc,
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                ForeColor = Color.FromArgb(40, 40, 40),
                Location = new Point(60, 26),
                Size = new Size(panelWidth - 70, 42),
                AutoSize = false
            };

            komCard.Controls.Add(avatar);
            komCard.Controls.Add(lblHeader);
            komCard.Controls.Add(lblTresc);

            return komCard;
        }

        public class KomentarzData
        {
            public int Id { get; set; }
            public int WydarzenieId { get; set; }
            public int UzytkownikId { get; set; }
            public string Tresc { get; set; }
            public DateTime DataDodania { get; set; }
            public string AutorImieNazwisko { get; set; }
            public Image AutorAvatar { get; set; }
        }

        private Panel StworzMiniaturkeUzytkownika(Image zdjecie)
        {
            Panel container = new Panel
            {
                Size = new Size(60, 60),
                Margin = new Padding(8)
            };

            CircularPictureBox avatar = new CircularPictureBox
            {
                BorderSize=0,
                Size = new Size(50, 50),
                Location = new Point(5, 0),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = zdjecie ?? Properties.Resources.latest,
                BorderStyle = BorderStyle.None
            };

            container.Controls.Add(avatar);
            return container;
        }

        private Image BytesToImage(byte[] imageBytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    return Image.FromStream(ms);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}