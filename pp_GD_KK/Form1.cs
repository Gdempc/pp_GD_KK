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

        private int animationStep = 20;

        private int targetHeight1 = 90;
        private int targetHeight2 = 220;

        private bool isClosing1 = false;
        private bool isClosing2 = false;

        public Form1()
        {
            InitializeComponent();
            SetupTimers();
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
            HandlePopupAnimation(ref popup1, ref isClosing1, targetHeight1);

            HandlePopupAnimation(ref popup2, ref isClosing2, targetHeight2);

            if (popup1 == null && popup2 == null) animationTimer.Stop();
        }

        private void HandlePopupAnimation<T>(ref T popup, ref bool closingFlag, int currentTarget) where T : UserControl
        {
            if (popup == null) return;

            if (!closingFlag)
            {
                if (popup.Height < currentTarget)
                {
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
                        this.Controls.Remove(popup);
                        popup.Dispose();
                        popup = null;
                        closingFlag = false;
                    }
                }
            }
        }

        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            Point mousePos = Cursor.Position;

            if (popup1 != null && !isClosing1)
            {
                if (!IsMouseOver(LoginBtn, mousePos) && !IsMouseOver(popup1, mousePos))
                {
                    isClosing1 = true;
                    if (!animationTimer.Enabled) animationTimer.Start();
                }
            }

            if (popup2 != null && !isClosing2)
            {
                if (!IsMouseOver(RegisterBtn, mousePos) && !IsMouseOver(popup2, mousePos))
                {
                    isClosing2 = true;
                    if (!animationTimer.Enabled) animationTimer.Start();
                }
            }

            if (popup1 == null && popup2 == null) closeTimer.Stop();
        }

        private bool IsMouseOver(Control ctrl, Point mousePos)
        {
            return ctrl.ClientRectangle.Contains(ctrl.PointToClient(mousePos));
        }

        private void LoginBtn_MouseEnter(object sender, EventArgs e)
        {
            if (isClosing1) { isClosing1 = false; animationTimer.Start(); return; }
            if (popup1 != null) return;

            popup1 = new UserControl1 { Height = 0 };
            PositionPopup(popup1, LoginBtn);
            this.Controls.Add(popup1);
            popup1.BringToFront();

            isClosing1 = false;
            animationTimer.Start();
            closeTimer.Start();
        }

        private void RegisterBtn_MouseEnter(object sender, EventArgs e)
        {
            if (isClosing2) { isClosing2 = false; animationTimer.Start(); return; }
            if (popup2 != null) return;

            popup2 = new UserControl2 { Height = 0 };
            PositionPopup(popup2, RegisterBtn);
            this.Controls.Add(popup2);
            popup2.BringToFront();

            isClosing2 = false;
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
                p.Controls.Add(pictureBox); p.Controls.Add(txt);
                flowLayoutPanel1.Controls.Add(p);
            }
        }
    }
}