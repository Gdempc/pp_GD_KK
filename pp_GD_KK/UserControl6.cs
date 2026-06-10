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

        private string aktualnyFiltrSQL = "";
        private string aktualneSortowanieSQL = "";

        public UserControl6()
        {
            InitializeComponent();
        }

        private void UserControl6_Load(object sender, EventArgs e)
        {
            aktualneSortowanieSQL = " ORDER BY date DESC";
            ZaladujWydarzenia();
        }

        private void ZaladujWydarzenia()
        {
            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                ctrl.Dispose();
            }
            flowLayoutPanel1.Controls.Clear();

            string query = "SELECT id, Title, Description, Image, UserAmount, date, wholeDay, fromT, toT FROM wydarzenia" 
                           + aktualnyFiltrSQL 
                           + aktualneSortowanieSQL;

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    if (aktualnyFiltrSQL.Contains("@Title"))
                        command.Parameters.AddWithValue("@Title", $"%{textBox1.Text.Trim()}%");
                    
                    if (aktualnyFiltrSQL.Contains("@DateFrom"))
                        command.Parameters.AddWithValue("@DateFrom", dateTimePicker1.Value.Date);
                    
                    if (aktualnyFiltrSQL.Contains("@DateTo"))
                        command.Parameters.AddWithValue("@DateTo", dateTimePicker2.Value.Date);
                    
                    if (aktualnyFiltrSQL.Contains("@WholeDay"))
                        command.Parameters.AddWithValue("@WholeDay", checkBox1.Checked ? 1 : 0);

                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                WydarzenieData data = new WydarzenieData
                                {
                                    Id = reader.GetInt32("id"),
                                    Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? "Brak tytułu" : reader.GetString("Title"),
                                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "Brak opisu..." : reader.GetString("Description"),
                                    UserAmount = reader.IsDBNull(reader.GetOrdinal("UserAmount")) ? 0 : reader.GetInt32("UserAmount"),

                                    Date = reader.IsDBNull(reader.GetOrdinal("date")) ? DateTime.Today : reader.GetDateTime("date"),
                                    WholeDay = reader.IsDBNull(reader.GetOrdinal("wholeDay")) ? false : reader.GetBoolean("wholeDay"),
                                    FromT = reader.IsDBNull(reader.GetOrdinal("fromT")) ? TimeSpan.Zero : reader.GetTimeSpan("fromT"),
                                    ToT = reader.IsDBNull(reader.GetOrdinal("toT")) ? TimeSpan.Zero : reader.GetTimeSpan("toT")
                                };

                                if (!reader.IsDBNull(reader.GetOrdinal("Image")))
                                {
                                    byte[] imageBytes = (byte[])reader["Image"];
                                    data.Image = BytesToImage(imageBytes);
                                }

                                Panel eventCard = StworzKafelekWydarzenia(data);
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

        private void btnFiltruj_Click(object sender, EventArgs e)
        {
            string filter = " WHERE 1=1";

            if (!string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                filter += " AND Title LIKE @Title";
            }

            filter += " AND date >= @DateFrom AND date <= @DateTo";

            if (checkBox5.Checked)
            {
                filter += " AND wholeDay = @WholeDay";
            }

            aktualnyFiltrSQL = filter;
            ZaladujWydarzenia(); 
        }

        private void btnSortuj_Click(object sender, EventArgs e)
        {
            string orderBy = " ORDER BY date DESC";

            if (checkBox1.Checked)
            {
                orderBy = " ORDER BY date DESC";
            }
            else if (checkBox2.Checked)
            {
                orderBy = " ORDER BY date ASC";
            }
            else if (checkBox4.Checked) 
            {
                orderBy = " ORDER BY id DESC";
            }
            else if (checkBox3.Checked) 
            {
                orderBy = " ORDER BY id ASC";
            }

            aktualneSortowanieSQL = orderBy;
            ZaladujWydarzenia();
        }

        private void checkBoxSort_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox activeBox = (CheckBox)sender;
            if (activeBox.Checked)
            {
                if (activeBox != checkBox1) checkBox1.Checked = false;
                if (activeBox != checkBox2) checkBox2.Checked = false;
                if (activeBox != checkBox3) checkBox3.Checked = false;
                if (activeBox != checkBox4) checkBox4.Checked = false;
            }
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

        private Panel StworzKafelekWydarzenia(WydarzenieData data)
        {
            Panel card = new Panel
            {
                Dock = DockStyle.Top,
                Size = new Size(flowLayoutPanel1.Width - 25, 180),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 15),
                Cursor = Cursors.Hand,
                Tag = data
            };
            card.Click += Karta_Click;

            PictureBox pb = new PictureBox
            {
                Dock = DockStyle.Left,
                Width = 180,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = data.Image ?? Properties.Resources.latest,
                Cursor = Cursors.Hand,
                Tag = data
            };
            pb.Click += Karta_Click;

            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 140,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(10)
            };

            Label lblAmount = new Label
            {
                Text = $"👤 Osób: {data.UserAmount}",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.DimGray,
                Dock = DockStyle.Top,
                Height = 20,
                TextAlign = ContentAlignment.MiddleRight,
                Name = "lblAmount" 
            };

            Label lblDate = new Label
            {
                Text = $"📅 {data.Date.ToString("dd.MM.yyyy")}",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(43, 43, 43),
                Dock = DockStyle.Top,
                Height = 22,
                TextAlign = ContentAlignment.MiddleRight
            };

            string czasTekst = data.WholeDay ? "⏰ Cały dzień" : $"⏰ {data.FromT.ToString(@"hh\:mm")} - {data.ToT.ToString(@"hh\:mm")}";
            Label lblTime = new Label
            {
                Text = czasTekst,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                ForeColor = Color.Gray,
                Dock = DockStyle.Top,
                Height = 20,
                TextAlign = ContentAlignment.MiddleRight
            };

            bool jestZapisany = CzyUzytkownikZapisany(data.Id, Globals.ID);

            Button btnJoin = new Button
            {
                Text = jestZapisany ? "Opuść" : "Dołącz",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                BackColor = jestZapisany ? Color.FromArgb(220, 53, 69) : Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Bottom,
                Height = 35,
                Cursor = Cursors.Hand,
                Tag = card 
            };
            btnJoin.FlatAppearance.BorderSize = 0;
            btnJoin.Click += BtnJoin_Click;

            rightPanel.Controls.Add(lblTime);
            rightPanel.Controls.Add(lblDate);
            rightPanel.Controls.Add(lblAmount);
            rightPanel.Controls.Add(btnJoin);

            Panel contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(15, 10, 15, 10),
                Cursor = Cursors.Hand,
                Tag = data
            };
            contentPanel.Click += Karta_Click;

            Label lblTitle = new Label
            {
                Text = data.Title,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Dock = DockStyle.Top,
                Height = 32,
                AutoSize = false,
                Cursor = Cursors.Hand,
                Tag = data
            };
            lblTitle.Click += Karta_Click;

            Label lblDesc = new Label
            {
                Text = data.Description,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(108, 117, 125),
                Dock = DockStyle.Fill,
                AutoSize = false,
                Cursor = Cursors.Hand,
                Tag = data
            };
            lblDesc.Click += Karta_Click;

            contentPanel.Controls.Add(lblDesc);
            contentPanel.Controls.Add(lblTitle);

            card.Controls.Add(contentPanel);
            card.Controls.Add(rightPanel);
            card.Controls.Add(pb);

            return card;
        }

        private void Karta_Click(object sender, EventArgs e)
        {
            Control clickedControl = (Control)sender;
            if (clickedControl.Tag is WydarzenieData zaladowaneDane)
            {
                u7open(zaladowaneDane);
            }
        }

        private void u7open(WydarzenieData wydarzenie)
        {
            if (GlobalnePanele.PanelTresc != null)
            {
                GlobalnePanele.PanelTresc.Controls.Clear();
                UserControl7 szczegoly = new UserControl7(wydarzenie) { Dock = DockStyle.Fill };
                GlobalnePanele.PanelTresc.Controls.Add(szczegoly);
            }
        }

        private void BtnJoin_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Panel parentCard = (Panel)btn.Tag;
            WydarzenieData data = (WydarzenieData)parentCard.Tag;

            int wydarzenieId = data.Id;
            int uzytkownikId = Globals.ID;

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                try
                {
                    connection.Open();
                    if (btn.Text == "Dołącz")
                    {
                        string insertQuery = "INSERT INTO uzytkownicy_wydarzenia (uzytkownik_id, wydarzenie_id) VALUES (@UserId, @EventId)";
                        using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                        {
                            insertCmd.Parameters.AddWithValue("@UserId", uzytkownikId);
                            insertCmd.Parameters.AddWithValue("@EventId", wydarzenieId);
                            insertCmd.ExecuteNonQuery();
                        }
                        data.UserAmount += 1;
                        btn.Text = "Opuść";
                        btn.BackColor = Color.FromArgb(220, 53, 69);
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
                        data.UserAmount -= 1;
                        btn.Text = "Dołącz";
                        btn.BackColor = Color.FromArgb(13, 110, 253);
                    }

                    string updateCountQuery = "UPDATE wydarzenia SET UserAmount = (SELECT COUNT(*) FROM uzytkownicy_wydarzenia WHERE wydarzenie_id = @EventId) WHERE id = @EventId";
                    using (MySqlCommand updateCmd = new MySqlCommand(updateCountQuery, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@EventId", wydarzenieId);
                        updateCmd.ExecuteNonQuery();
                    }

                    Control[] zgromadzoneLabelki = parentCard.Controls.Find("lblAmount", true);
                    if (zgromadzoneLabelki.Length > 0 && zgromadzoneLabelki[0] is Label lblAmount)
                    {
                        lblAmount.Text = $"👤 Osób: {data.UserAmount}";
                    }
                    ZaladujWydarzenia();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Wystąpił błąd: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
            catch { return null; }
        }
    }
    public class WydarzenieData
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Image Image { get; set; }
        public int UserAmount { get; set; }
        public DateTime Date { get; set; }
        public bool WholeDay { get; set; }
        public TimeSpan FromT { get; set; }
        public TimeSpan ToT { get; set; }
    }
}