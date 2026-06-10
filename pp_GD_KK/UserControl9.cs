using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace pp_GD_KK
{
    public partial class UserControl9 : UserControl
    {
        private readonly string connString = "Server=localhost;Database=wydarzeniastudenckie;Uid=root;Pwd=;";

        private TabControl tabControlMain;
        private TabPage tabOgloszenia, tabWydarzenia, tabUzytkownicy;

        private DataGridView dgvOgloszenia;
        private TextBox txtOgloszenieTytul, txtOgloszenieTresc;
        private PictureBox pbOgloszenieZdjecie;
        private Button btnDodajOgloszenie, btnEdytujOgloszenie, btnUsunOgloszenie, btnWybierzZdjecieOgloszenia;
        private int ID_WybranegoOgloszenia = -1;
        private byte[] ogloszenieZdjecieBytes = null;

        private DataGridView dgvWydarzenia;
        private TextBox txtWydarzenieNazwa, txtWydarzenieOpis;
        private DateTimePicker dtpWydarzenieData, dtpFromT, dtpToT;
        private CheckBox chkWholeDay;
        private PictureBox pbWydarzenieZdjecie;
        private Button btnDodajWydarzenie, btnEdytujWydarzenie, btnUsunWydarzenie, btnWybierzZdjecieWydarzenia;
        private int ID_WybranegoWydarzenia = -1;
        private byte[] wydarzenieZdjecieBytes = null;

        private DataGridView dgvUzytkownicy;
        private Button btnZmienUprawnienia;

        public UserControl9()
        {
            InitializeComponent();
            InicjalizujInterfejs();
        }

        private void UserControl9_Load(object sender, EventArgs e)
        {
            OdswiezOgloszenia();
            OdswiezWydarzenia();
            OdswiezUzytkownikow();
        }

        #region
        private void InicjalizujInterfejs()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            tabControlMain = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10f) };

            tabOgloszenia = new TabPage { Text = "Ogłoszenia", BackColor = Color.White };
            tabWydarzenia = new TabPage { Text = "Wydarzenia", BackColor = Color.White };
            tabUzytkownicy = new TabPage { Text = "Użytkownicy i Uprawnienia", BackColor = Color.White };

            tabControlMain.TabPages.Add(tabOgloszenia);
            tabControlMain.TabPages.Add(tabWydarzenia);
            tabControlMain.TabPages.Add(tabUzytkownicy);
            this.Controls.Add(tabControlMain);

            tabControlMain.SelectedIndexChanged += TabControlMain_SelectedIndexChanged;

            StworzPanelOgloszen();
            StworzPanelWydarzen();
            StworzPanelUzytkownikow();
        }

        private void TabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControlMain.SelectedIndex)
            {
                case 0:
                    OdswiezOgloszenia();
                    CzyscPolaOgloszen();
                    break;
                case 1:
                    OdswiezWydarzenia();
                    CzyscPolaWydarzen();
                    break;
                case 2:
                    OdswiezUzytkownikow();
                    break;
            }
        }

        private void StworzPanelOgloszen()
        {
            dgvOgloszenia = new DataGridView { Location = new Point(10, 10), Size = new Size(500, 480), SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            dgvOgloszenia.SelectionChanged += dgvOgloszenia_SelectionChanged;

            Label lblTytul = new Label { Text = "Tytuł ogłoszenia:", Location = new Point(530, 10), AutoSize = true };
            txtOgloszenieTytul = new TextBox { Location = new Point(530, 30), Size = new Size(300, 25) };

            Label lblTresc = new Label { Text = "Treść:", Location = new Point(530, 65), AutoSize = true };
            txtOgloszenieTresc = new TextBox { Location = new Point(530, 85), Size = new Size(300, 120), Multiline = true, ScrollBars = ScrollBars.Vertical };

            Label lblZdjecie = new Label { Text = "Zdjęcie ogłoszenia:", Location = new Point(530, 215), AutoSize = true };
            pbOgloszenieZdjecie = new PictureBox { Location = new Point(530, 235), Size = new Size(130, 90), BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };

            btnWybierzZdjecieOgloszenia = new Button { Text = "Wybierz...", Location = new Point(670, 235), Size = new Size(160, 30), BackColor = Color.WhiteSmoke, FlatStyle = FlatStyle.Flat };
            btnWybierzZdjecieOgloszenia.Click += (s, e) => WybierzZdjecieZDisku(ref ogloszenieZdjecieBytes, pbOgloszenieZdjecie);

            btnDodajOgloszenie = new Button { Text = "Dodaj nowe", Location = new Point(530, 340), Size = new Size(140, 35), BackColor = Color.LightGreen, FlatStyle = FlatStyle.Flat };
            btnDodajOgloszenie.Click += BtnDodajOgloszenie_Click;

            btnEdytujOgloszenie = new Button { Text = "Zapisz edycję", Location = new Point(690, 340), Size = new Size(140, 35), BackColor = Color.LightSkyBlue, FlatStyle = FlatStyle.Flat };
            btnEdytujOgloszenie.Click += BtnEdytujOgloszenie_Click;

            btnUsunOgloszenie = new Button { Text = "Usuń ogłoszenie", Location = new Point(530, 385), Size = new Size(300, 35), BackColor = Color.MistyRose, ForeColor = Color.DarkRed, FlatStyle = FlatStyle.Flat };
            btnUsunOgloszenie.Click += BtnUsunOgloszenie_Click;

            tabOgloszenia.Controls.AddRange(new Control[] { dgvOgloszenia, lblTytul, txtOgloszenieTytul, lblTresc, txtOgloszenieTresc, lblZdjecie, pbOgloszenieZdjecie, btnWybierzZdjecieOgloszenia, btnDodajOgloszenie, btnEdytujOgloszenie, btnUsunOgloszenie });
        }

        private void StworzPanelWydarzen()
        {
            dgvWydarzenia = new DataGridView { Location = new Point(10, 10), Size = new Size(500, 480), SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            dgvWydarzenia.SelectionChanged += dgvWydarzenia_SelectionChanged;

            Label lblNazwa = new Label { Text = "Nazwa wydarzenia:", Location = new Point(530, 10), AutoSize = true };
            txtWydarzenieNazwa = new TextBox { Location = new Point(530, 30), Size = new Size(300, 25) };

            Label lblData = new Label { Text = "Data wydarzenia:", Location = new Point(530, 65), AutoSize = true };
            dtpWydarzenieData = new DateTimePicker { Location = new Point(530, 85), Size = new Size(300, 25), Format = DateTimePickerFormat.Short };

            chkWholeDay = new CheckBox { Text = "Cały dzień?", Location = new Point(530, 120), AutoSize = true };
            chkWholeDay.CheckedChanged += ChkWholeDay_CheckedChanged;

            Label lblOd = new Label { Text = "Od:", Location = new Point(530, 150), AutoSize = true };
            dtpFromT = new DateTimePicker { Location = new Point(560, 148), Size = new Size(80, 25), Format = DateTimePickerFormat.Time, ShowUpDown = true };

            Label lblDo = new Label { Text = "Do:", Location = new Point(660, 150), AutoSize = true };
            dtpToT = new DateTimePicker { Location = new Point(690, 148), Size = new Size(80, 25), Format = DateTimePickerFormat.Time, ShowUpDown = true };

            Label lblOpis = new Label { Text = "Opis wydarzenia:", Location = new Point(530, 185), AutoSize = true };
            txtWydarzenieOpis = new TextBox { Location = new Point(530, 205), Size = new Size(300, 80), Multiline = true, ScrollBars = ScrollBars.Vertical };

            Label lblZdjecieW = new Label { Text = "Zdjęcie wydarzenia:", Location = new Point(530, 295), AutoSize = true };
            pbWydarzenieZdjecie = new PictureBox { Location = new Point(530, 315), Size = new Size(130, 90), BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };

            btnWybierzZdjecieWydarzenia = new Button { Text = "Wybierz...", Location = new Point(670, 315), Size = new Size(160, 30), BackColor = Color.WhiteSmoke, FlatStyle = FlatStyle.Flat };
            btnWybierzZdjecieWydarzenia.Click += (s, e) => WybierzZdjecieZDisku(ref wydarzenieZdjecieBytes, pbWydarzenieZdjecie);

            btnDodajWydarzenie = new Button { Text = "Dodaj nowe", Location = new Point(530, 420), Size = new Size(140, 35), BackColor = Color.LightGreen, FlatStyle = FlatStyle.Flat };
            btnDodajWydarzenie.Click += BtnDodajWydarzenie_Click;

            btnEdytujWydarzenie = new Button { Text = "Zapisz edycję", Location = new Point(690, 420), Size = new Size(140, 35), BackColor = Color.LightSkyBlue, FlatStyle = FlatStyle.Flat };
            btnEdytujWydarzenie.Click += BtnEdytujWydarzenie_Click;

            btnUsunWydarzenie = new Button { Text = "Usuń wydarzenie", Location = new Point(530, 465), Size = new Size(300, 35), BackColor = Color.MistyRose, ForeColor = Color.DarkRed, FlatStyle = FlatStyle.Flat };
            btnUsunWydarzenie.Click += BtnUsunWydarzenie_Click;

            tabWydarzenia.Controls.AddRange(new Control[] { dgvWydarzenia, lblNazwa, txtWydarzenieNazwa, lblData, dtpWydarzenieData, chkWholeDay, lblOd, dtpFromT, lblDo, dtpToT, lblOpis, txtWydarzenieOpis, lblZdjecieW, pbWydarzenieZdjecie, btnWybierzZdjecieWydarzenia, btnDodajWydarzenie, btnEdytujWydarzenie, btnUsunWydarzenie });
        }

        private void StworzPanelUzytkownikow()
        {
            dgvUzytkownicy = new DataGridView { Location = new Point(10, 10), Size = new Size(500, 480), SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            btnZmienUprawnienia = new Button { Text = "Nadaj / Odbierz uprawnienia Admina", Location = new Point(530, 10), Size = new Size(280, 45), BackColor = Color.Orange, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10f, FontStyle.Bold) };
            btnZmienUprawnienia.Click += BtnZmienUprawnienia_Click;

            tabUzytkownicy.Controls.AddRange(new Control[] { dgvUzytkownicy, btnZmienUprawnienia });
        }

        private void ChkWholeDay_CheckedChanged(object sender, EventArgs e)
        {
            dtpFromT.Enabled = !chkWholeDay.Checked;
            dtpToT.Enabled = !chkWholeDay.Checked;
        }
        #endregion

        #region

        private void WybierzZdjecieZDisku(ref byte[] targetBytes, PictureBox targetPb)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Pliki graficzne (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (Image img = Image.FromFile(openFileDialog.FileName))
                        {
                            targetPb.Image = new Bitmap(img);

                            using (MemoryStream ms = new MemoryStream())
                            {
                                ImageCodecInfo jpgCodec = GetEncoder(ImageFormat.Jpeg);
                                EncoderParameters encoderParameters = new EncoderParameters(1);
                                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 75L);

                                img.Save(ms, jpgCodec, encoderParameters);
                                targetBytes = ms.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Błąd ładowania i kompresji obrazu: " + ex.Message);
                    }
                }
            }
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private void OdswiezOgloszenia()
        {
            PobierzDaneDoGridu("SELECT ID, Title AS Tytuł, Description AS Treść, Image FROM ogloszenia", dgvOgloszenia);
            if (dgvOgloszenia.Columns.Contains("Image")) dgvOgloszenia.Columns["Image"].Visible = false;
        }

        private void dgvOgloszenia_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvOgloszenia.SelectedRows.Count > 0)
            {
                var row = dgvOgloszenia.SelectedRows[0];

                if (row.Cells["ID"].Value == null || row.Cells["ID"].Value == DBNull.Value)
                    return;

                ID_WybranegoOgloszenia = Convert.ToInt32(row.Cells["ID"].Value);
                txtOgloszenieTytul.Text = row.Cells["Tytuł"].Value?.ToString() ?? "";
                txtOgloszenieTresc.Text = row.Cells["Treść"].Value?.ToString() ?? "";

                if (row.Cells["Image"].Value != DBNull.Value && row.Cells["Image"].Value != null)
                {
                    ogloszenieZdjecieBytes = (byte[])row.Cells["Image"].Value;
                    using (MemoryStream ms = new MemoryStream(ogloszenieZdjecieBytes))
                    {
                        pbOgloszenieZdjecie.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    pbOgloszenieZdjecie.Image = null;
                    ogloszenieZdjecieBytes = null;
                }
            }
        }

        private void BtnDodajOgloszenie_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOgloszenieTytul.Text)) return;
            WykonajZapytanie("INSERT INTO ogloszenia (Title, Description, Image) VALUES (@Title, @Description, @Image)",
                new MySqlParameter("@Title", txtOgloszenieTytul.Text),
                new MySqlParameter("@Description", txtOgloszenieTresc.Text),
                new MySqlParameter("@Image", (object)ogloszenieZdjecieBytes ?? DBNull.Value));
            OdswiezOgloszenia();
            CzyscPolaOgloszen();
        }

        private void BtnEdytujOgloszenie_Click(object sender, EventArgs e)
        {
            if (ID_WybranegoOgloszenia == -1) return;
            WykonajZapytanie("UPDATE ogloszenia SET Title = @Title, Description = @Description, Image = @Image WHERE ID = @ID",
                new MySqlParameter("@Title", txtOgloszenieTytul.Text),
                new MySqlParameter("@Description", txtOgloszenieTresc.Text),
                new MySqlParameter("@Image", (object)ogloszenieZdjecieBytes ?? DBNull.Value),
                new MySqlParameter("@ID", ID_WybranegoOgloszenia));
            OdswiezOgloszenia();
        }

        private void BtnUsunOgloszenie_Click(object sender, EventArgs e)
        {
            if (ID_WybranegoOgloszenia == -1)
            {
                MessageBox.Show("Wybierz ogłoszenie z tabeli, które chcesz usunąć.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Czy na pewno chcesz permanentnie usunąć to ogłoszenie?", "Potwierdzenie usunięcia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                WykonajZapytanie("DELETE FROM ogloszenia WHERE ID = @ID", new MySqlParameter("@ID", ID_WybranegoOgloszenia));
                OdswiezOgloszenia();
                CzyscPolaOgloszen();
            }
        }

        private void CzyscPolaOgloszen() { txtOgloszenieTytul.Clear(); txtOgloszenieTresc.Clear(); pbOgloszenieZdjecie.Image = null; ogloszenieZdjecieBytes = null; ID_WybranegoOgloszenia = -1; }


        private void OdswiezWydarzenia()
        {
            PobierzDaneDoGridu("SELECT ID, Title AS Nazwa, date AS Data, Description AS Opis, wholeDay, fromT, toT, Image FROM wydarzenia", dgvWydarzenia);
            string[] kolumnyDoUkrycia = { "wholeDay", "fromT", "toT", "Image" };
            foreach (string col in kolumnyDoUkrycia)
                if (dgvWydarzenia.Columns.Contains(col)) dgvWydarzenia.Columns[col].Visible = false;
        }

        private void dgvWydarzenia_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvWydarzenia.SelectedRows.Count > 0)
            {
                var row = dgvWydarzenia.SelectedRows[0];

                if (row.Cells["ID"].Value == null || row.Cells["ID"].Value == DBNull.Value)
                    return;

                ID_WybranegoWydarzenia = Convert.ToInt32(row.Cells["ID"].Value);
                txtWydarzenieNazwa.Text = row.Cells["Nazwa"].Value?.ToString() ?? "";
                txtWydarzenieOpis.Text = row.Cells["Opis"].Value?.ToString() ?? "";

                if (row.Cells["Data"].Value != DBNull.Value && row.Cells["Data"].Value != null)
                    dtpWydarzenieData.Value = Convert.ToDateTime(row.Cells["Data"].Value);

                if (row.Cells["wholeDay"].Value != DBNull.Value && row.Cells["wholeDay"].Value != null)
                    chkWholeDay.Checked = Convert.ToBoolean(row.Cells["wholeDay"].Value);

                if (row.Cells["fromT"].Value != DBNull.Value && row.Cells["fromT"].Value != null)
                    dtpFromT.Value = DateTime.Parse(row.Cells["fromT"].Value.ToString());

                if (row.Cells["toT"].Value != DBNull.Value && row.Cells["toT"].Value != null)
                    dtpToT.Value = DateTime.Parse(row.Cells["toT"].Value.ToString());

                if (row.Cells["Image"].Value != DBNull.Value && row.Cells["Image"].Value != null)
                {
                    try
                    {
                        byte[] rawBytes = row.Cells["Image"].Value as byte[];

                        if (rawBytes != null && rawBytes.Length > 0)
                        {
                            wydarzenieZdjecieBytes = rawBytes;
                            using (MemoryStream ms = new MemoryStream(wydarzenieZdjecieBytes))
                            {
                                pbWydarzenieZdjecie.Image = Image.FromStream(ms);
                            }
                        }
                        else
                        {
                            ZresetujZdjecieWydarzenia();
                        }
                    }
                    catch (Exception)
                    {
                        ZresetujZdjecieWydarzenia();
                    }
                }
                else
                {
                    ZresetujZdjecieWydarzenia();
                }
            }
        }

        private void ZresetujZdjecieWydarzenia()
        {
            pbWydarzenieZdjecie.Image = null;
            wydarzenieZdjecieBytes = null;
        }

        private void BtnDodajWydarzenie_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtWydarzenieNazwa.Text)) return;

            string query = "INSERT INTO wydarzenia (Title, date, Description, UserAmount, wholeDay, fromT, toT, Image) VALUES (@Title, @Date, @Description, 0, @WholeDay, @FromT, @ToT, @Image)";

            WykonajZapytanie(query,
                new MySqlParameter("@Title", txtWydarzenieNazwa.Text),
                new MySqlParameter("@Date", dtpWydarzenieData.Value.ToString("yyyy-MM-dd")),
                new MySqlParameter("@Description", txtWydarzenieOpis.Text),
                new MySqlParameter("@WholeDay", chkWholeDay.Checked ? 1 : 0),
                new MySqlParameter("@FromT", chkWholeDay.Checked ? "00:00:00" : dtpFromT.Value.ToString("HH:mm:ss")),
                new MySqlParameter("@ToT", chkWholeDay.Checked ? "23:59:59" : dtpToT.Value.ToString("HH:mm:ss")),
                new MySqlParameter("@Image", (object)wydarzenieZdjecieBytes ?? DBNull.Value));

            OdswiezWydarzenia();
            CzyscPolaWydarzen();
        }

        private void BtnEdytujWydarzenie_Click(object sender, EventArgs e)
        {
            if (ID_WybranegoWydarzenia == -1) return;

            string query = "UPDATE wydarzenia SET Title = @Title, date = @Date, Description = @Description, wholeDay = @WholeDay, fromT = @FromT, toT = @ToT, Image = @Image WHERE ID = @ID";

            WykonajZapytanie(query,
                new MySqlParameter("@Title", txtWydarzenieNazwa.Text),
                new MySqlParameter("@Date", dtpWydarzenieData.Value.ToString("yyyy-MM-dd")),
                new MySqlParameter("@Description", txtWydarzenieOpis.Text),
                new MySqlParameter("@WholeDay", chkWholeDay.Checked ? 1 : 0),
                new MySqlParameter("@FromT", chkWholeDay.Checked ? "00:00:00" : dtpFromT.Value.ToString("HH:mm:ss")),
                new MySqlParameter("@ToT", chkWholeDay.Checked ? "23:59:59" : dtpToT.Value.ToString("HH:mm:ss")),
                new MySqlParameter("@Image", (object)wydarzenieZdjecieBytes ?? DBNull.Value),
                new MySqlParameter("@ID", ID_WybranegoWydarzenia));

            OdswiezWydarzenia();
        }

        private void BtnUsunWydarzenie_Click(object sender, EventArgs e)
        {
            if (ID_WybranegoWydarzenia == -1)
            {
                MessageBox.Show("Wybierz wydarzenie z tabeli, które chcesz usunąć.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Czy na pewno chcesz permanentnie usunąć to wydarzenie?", "Potwierdzenie usunięcia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                WykonajZapytanie("DELETE FROM wydarzenia WHERE ID = @ID", new MySqlParameter("@ID", ID_WybranegoWydarzenia));
                OdswiezWydarzenia();
                CzyscPolaWydarzen();
            }
        }

        private void CzyscPolaWydarzen()
        {
            txtWydarzenieNazwa.Clear();
            txtWydarzenieOpis.Clear();
            chkWholeDay.Checked = false;
            pbWydarzenieZdjecie.Image = null;
            wydarzenieZdjecieBytes = null;
            ID_WybranegoWydarzenia = -1;
        }

        private void OdswiezUzytkownikow()
        {
            PobierzDaneDoGridu("SELECT ID, Name AS Imię, Surname AS Nazwisko, Admin AS Administrator FROM uzytkownicy", dgvUzytkownicy);
        }

        private void BtnZmienUprawnienia_Click(object sender, EventArgs e)
        {
            if (dgvUzytkownicy.SelectedRows.Count > 0)
            {
                var row = dgvUzytkownicy.SelectedRows[0];
                int userId = Convert.ToInt32(row.Cells["ID"].Value);
                bool currentAdminStatus = Convert.ToBoolean(row.Cells["Administrator"].Value);

                if (userId == Globals.ID)
                {
                    MessageBox.Show("Nie możesz odebrać uprawnień administratora samemu sobie!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool newAdminStatus = !currentAdminStatus;

                WykonajZapytanie("UPDATE uzytkownicy SET Admin = @Admin WHERE ID = @ID",
                    new MySqlParameter("@Admin", newAdminStatus),
                    new MySqlParameter("@ID", userId));

                OdswiezUzytkownikow();
                MessageBox.Show("Uprawnienia zostały pomyślnie zaktualizowane!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void PobierzDaneDoGridu(string query, DataGridView dgv)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgv.DataSource = dt;

                        if (dgv.Columns.Contains("ID")) dgv.Columns["ID"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd pobierania danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WykonajZapytanie(string query, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parameters);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu w bazie danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}