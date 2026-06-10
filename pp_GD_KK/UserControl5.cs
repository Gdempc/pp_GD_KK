using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace pp_GD_KK
{
    public partial class UserControl5 : UserControl
    {
        private UserControl8 popupProfil;
        private Timer animationTimer;
        private Timer closeTimer;

        private NoFocusButton adminBtn;

        private int animationStep = 25;
        private int targetHeight = 270;
        private bool isClosing = false;

        private string cachedName = "";
        private string cachedSurname = "";
        private Image cachedImage = null;

        public UserControl5()
        {
            InitializeComponent();
            SetupTimers();
        }

        private void UserControl5_Load(object sender, EventArgs e)
        {
            string server = "localhost";
            string database = "wydarzeniastudenckie";
            string uid = "root";
            string password = "";

            string connectionString = $"Server={server};Database={database};Uid={uid};Pwd={password};";

            string query = "SELECT Name, Surname, Image, Admin FROM uzytkownicy WHERE ID = @Id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    Int32 Id = Globals.ID;
                    command.Parameters.AddWithValue("@Id", Id);

                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cachedName = reader.GetString("Name");
                                cachedSurname = reader.GetString("Surname");

                                if (!reader.IsDBNull(reader.GetOrdinal("Image")))
                                {
                                    byte[] imageBytes = (byte[])reader["Image"];
                                    cachedImage = BytesToImage(imageBytes);
                                }

                                circularPictureBox1.Image = cachedImage;

                                Boolean role = reader.GetBoolean("Admin");

                                if (role)
                                {
                                    StworzPrzyciskAdministratora();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            circularPictureBox1.MouseEnter += CircularPictureBox1_MouseEnter;
        }

        private void StworzPrzyciskAdministratora()
        {
            adminBtn = new NoFocusButton
            {
                Text = "Panel administratora",
                BackColor = Color.WhiteSmoke,
                FlatStyle = FlatStyle.Standard,
                Size = new Size(370, 55),
                Dock = DockStyle.Left,
            };

            newsBtn.Width = 370;
            eventBtn.Width = 370;

            adminBtn.Click += AdminBtn_Click;

            panel1.Controls.Add(adminBtn);
        }

        private void AdminBtn_Click(object sender, EventArgs e)
        {
            if (adminBtn != null) adminBtn.BackColor = Color.Silver;
            newsBtn.BackColor = Color.WhiteSmoke;
            eventBtn.BackColor = Color.WhiteSmoke;

            GlobalnePanele.PanelTresc.Controls.Clear();

            UserControl9 admin = new UserControl9 { Dock = DockStyle.Fill };
            GlobalnePanele.PanelTresc.Controls.Add(admin);
        }

        private void newsBtn_Click(object sender, EventArgs e)
        {
            newsBtn.BackColor = Color.Silver;
            eventBtn.BackColor = Color.WhiteSmoke;
            if (adminBtn != null) adminBtn.BackColor = Color.WhiteSmoke;

            GlobalnePanele.PanelTresc.Controls.Clear();

            UserControl4 ogloszenia = new UserControl4 { Dock = DockStyle.Fill };
            GlobalnePanele.PanelTresc.Controls.Add(ogloszenia);
        }

        private void eventBtn_Click(object sender, EventArgs e)
        {
            newsBtn.BackColor = Color.WhiteSmoke;
            eventBtn.BackColor = Color.Silver;
            if (adminBtn != null) adminBtn.BackColor = Color.WhiteSmoke;

            GlobalnePanele.PanelTresc.Controls.Clear();

            UserControl6 wydarzenia = new UserControl6 { Dock = DockStyle.Fill };
            GlobalnePanele.PanelTresc.Controls.Add(wydarzenia);
        }

        private void InitializePopup()
        {
            if (popupProfil == null)
            {
                var mainForm = this.FindForm();
                if (mainForm != null)
                {
                    popupProfil = new UserControl8 { Height = 0, Visible = false };
                    mainForm.Controls.Add(popupProfil);
                    popupProfil.BringToFront();

                    popupProfil.UstawDaneUzytkownika(cachedName, cachedSurname, cachedImage);
                }
            }
        }

        private void SetupTimers()
        {
            animationTimer = new Timer { Interval = 15 };
            animationTimer.Tick += AnimationTimer_Tick;

            closeTimer = new Timer { Interval = 100 };
            closeTimer.Tick += CloseTimer_Tick;
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            HandlePopupAnimation(popupProfil, ref isClosing, targetHeight);

            bool pAnimating = isClosing ? (popupProfil != null && popupProfil.Height > 0) : (popupProfil != null && popupProfil.Visible && popupProfil.Height < targetHeight);

            if (!pAnimating) animationTimer.Stop();
        }

        private void HandlePopupAnimation(UserControl popup, ref bool closingFlag, int currentTarget)
        {
            if (popup == null) return;

            if (!closingFlag)
            {
                if (popup.Height < currentTarget)
                {
                    popup.Visible = true;
                    popup.Height += animationStep;
                    if (popup.Height >= currentTarget) popup.Height = currentTarget;
                }
            }
            else
            {
                if (popup.Height > 0)
                {
                    popup.Height -= animationStep;
                    if (popup.Height <= 0)
                    {
                        popup.Height = 0;
                        popup.Visible = false;
                    }
                }
            }
        }

        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            Point mousePos = Cursor.Position;

            if (popupProfil != null && popupProfil.Visible && !isClosing)
            {
                if (!IsMouseOver(circularPictureBox1, mousePos) && !IsMouseOver(popupProfil, mousePos))
                {
                    isClosing = true;
                    animationTimer.Start();
                }
            }

            if (popupProfil == null || !popupProfil.Visible) closeTimer.Stop();
        }

        private bool IsMouseOver(Control ctrl, Point mousePos)
        {
            return ctrl.ClientRectangle.Contains(ctrl.PointToClient(mousePos));
        }

        private void CircularPictureBox1_MouseEnter(object sender, EventArgs e)
        {
            isClosing = false;
            InitializePopup();

            if (popupProfil != null)
            {
                PositionPopup(popupProfil, circularPictureBox1);
                popupProfil.BringToFront();
                animationTimer.Start();
                closeTimer.Start();
            }
        }

        private void PositionPopup(UserControl popup, Control triggerCtrl)
        {
            Point spawnPoint = this.PointToClient(triggerCtrl.Parent.PointToScreen(triggerCtrl.Location));
            popup.Location = new Point(spawnPoint.X - popup.Width + triggerCtrl.Width, spawnPoint.Y + triggerCtrl.Height);
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