using pp_GD_KK.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace pp_GD_KK
{
    public partial class Form1 : Form
    {
        List<string> strings = new List<string> { "ogłoszenie1", "ogłoszenie2", "ogłoszenie2", "ogłoszenie2" };

        private UserControl1 popup1;
        private UserControl2 popup2;
        private Timer animationTimer;
        private Timer closeTimer;

        private int animationStep = 25;
        private int targetHeight1 = 90;
        private int targetHeight2 = 220;

        private bool isClosing1 = false;
        private bool isClosing2 = false;

        public Form1()
        {
            InitializeComponent();
            SetupTimers();
            InitializePopups();
        }

        private void InitializePopups()
        {
            popup1 = new UserControl1 { Height = 0, Visible = false };
            popup2 = new UserControl2 { Height = 0, Visible = false };

            this.Controls.Add(popup1);
            this.Controls.Add(popup2);
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
            HandlePopupAnimation(popup1, ref isClosing1, targetHeight1);
            HandlePopupAnimation(popup2, ref isClosing2, targetHeight2);

            bool p1Animating = isClosing1 ? popup1.Height > 0 : (popup1.Visible && popup1.Height < targetHeight1);
            bool p2Animating = isClosing2 ? popup2.Height > 0 : (popup2.Visible && popup2.Height < targetHeight2);

            if (!p1Animating && !p2Animating) animationTimer.Stop();
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

            if (popup1.Visible && !isClosing1)
            {
                if (!IsMouseOver(LoginBtn, mousePos) && !IsMouseOver(popup1, mousePos))
                {
                    isClosing1 = true;
                    animationTimer.Start();
                }
            }

            if (popup2.Visible && !isClosing2)
            {
                if (!IsMouseOver(RegisterBtn, mousePos) && !IsMouseOver(popup2, mousePos))
                {
                    isClosing2 = true;
                    animationTimer.Start();
                }
            }

            if (!popup1.Visible && !popup2.Visible) closeTimer.Stop();
        }

        private bool IsMouseOver(Control ctrl, Point mousePos)
        {
            return ctrl.ClientRectangle.Contains(ctrl.PointToClient(mousePos));
        }

        private void LoginBtn_MouseEnter(object sender, EventArgs e)
        {
            isClosing1 = false;
            PositionPopup(popup1, LoginBtn);
            popup1.BringToFront();
            animationTimer.Start();
            closeTimer.Start();
        }

        private void RegisterBtn_MouseEnter(object sender, EventArgs e)
        {
            isClosing2 = false;
            PositionPopup(popup2, RegisterBtn);
            popup2.BringToFront();
            animationTimer.Start();
            closeTimer.Start();
        }

        private void PositionPopup(UserControl popup, Button btn)
        {
            Point spawnPoint = this.PointToClient(btn.Parent.PointToScreen(btn.Location));
            popup.Location = new Point(spawnPoint.X - popup.Width + btn.Width, spawnPoint.Y + btn.Height);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string s in strings)
            {
                FlowLayoutPanel p = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 180, BorderStyle = BorderStyle.FixedSingle };
                p.Width = (strings.Count > 3) ? flowLayoutPanel1.Width - 30 : flowLayoutPanel1.Width - 20;
                PictureBox pictureBox = new PictureBox { Height = p.Height - 10, Width = p.Height - 10, Image = Resources.latest, SizeMode = PictureBoxSizeMode.StretchImage, BackColor = Color.Red };
                RichTextBox txt = new RichTextBox { Text = s, Width = p.Width - pictureBox.Width - 15, Height = p.Height - 10, Enabled = false, BorderStyle = BorderStyle.FixedSingle };
                p.Controls.Add(pictureBox);
                p.Controls.Add(txt);
                flowLayoutPanel1.Controls.Add(p);
            }
        }
    }
}