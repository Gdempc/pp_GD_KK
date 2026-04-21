namespace pp_GD_KK
{
    partial class UserControl5
    {
        /// <summary> 
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod wygenerowany przez Projektanta składników

        /// <summary> 
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować 
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.circularPictureBox1 = new CircularPictureBox();
            this.noFocusButton1 = new NoFocusButton();
            this.noFocusButton2 = new NoFocusButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.circularPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.noFocusButton2);
            this.panel1.Controls.Add(this.noFocusButton1);
            this.panel1.Controls.Add(this.circularPictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 1, 2, 0);
            this.panel1.Size = new System.Drawing.Size(1186, 58);
            this.panel1.TabIndex = 1;
            // 
            // circularPictureBox1
            // 
            this.circularPictureBox1.BorderColor = System.Drawing.Color.Transparent;
            this.circularPictureBox1.BorderSize = 0F;
            this.circularPictureBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.circularPictureBox1.Image = global::pp_GD_KK.Properties.Resources.Profil1;
            this.circularPictureBox1.Location = new System.Drawing.Point(1127, 1);
            this.circularPictureBox1.Name = "circularPictureBox1";
            this.circularPictureBox1.Size = new System.Drawing.Size(55, 55);
            this.circularPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.circularPictureBox1.TabIndex = 1;
            this.circularPictureBox1.TabStop = false;
            // 
            // noFocusButton1
            // 
            this.noFocusButton1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.noFocusButton1.Dock = System.Windows.Forms.DockStyle.Left;
            this.noFocusButton1.Location = new System.Drawing.Point(0, 1);
            this.noFocusButton1.Name = "noFocusButton1";
            this.noFocusButton1.Size = new System.Drawing.Size(562, 55);
            this.noFocusButton1.TabIndex = 2;
            this.noFocusButton1.Text = "Ogłoszenia";
            this.noFocusButton1.UseVisualStyleBackColor = false;
            this.noFocusButton1.Click += new System.EventHandler(this.btn_Click);
            // 
            // noFocusButton2
            // 
            this.noFocusButton2.Dock = System.Windows.Forms.DockStyle.Left;
            this.noFocusButton2.Location = new System.Drawing.Point(562, 1);
            this.noFocusButton2.Name = "noFocusButton2";
            this.noFocusButton2.Size = new System.Drawing.Size(562, 55);
            this.noFocusButton2.TabIndex = 3;
            this.noFocusButton2.Text = "Wydarzenia";
            this.noFocusButton2.UseVisualStyleBackColor = true;
            this.noFocusButton2.Click += new System.EventHandler(this.btn_Click);
            // 
            // UserControl5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "UserControl5";
            this.Size = new System.Drawing.Size(1186, 58);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.circularPictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private CircularPictureBox circularPictureBox1;
        private NoFocusButton noFocusButton2;
        private NoFocusButton noFocusButton1;
    }
}
