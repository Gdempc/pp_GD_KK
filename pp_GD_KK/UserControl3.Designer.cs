namespace pp_GD_KK
{
    partial class UserControl3
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
            this.LoginBtn = new NoFocusButton();
            this.RegisterBtn = new NoFocusButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.RegisterBtn);
            this.panel1.Controls.Add(this.LoginBtn);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(8);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(1186, 58);
            this.panel1.TabIndex = 3;
            // 
            // LoginBtn
            // 
            this.LoginBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.LoginBtn.Location = new System.Drawing.Point(1092, 5);
            this.LoginBtn.Name = "LoginBtn";
            this.LoginBtn.Size = new System.Drawing.Size(87, 46);
            this.LoginBtn.TabIndex = 1;
            this.LoginBtn.Text = "Logowanie";
            this.LoginBtn.UseVisualStyleBackColor = true;
            this.LoginBtn.MouseEnter += new System.EventHandler(this.LoginBtn_MouseEnter);
            // 
            // RegisterBtn
            // 
            this.RegisterBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.RegisterBtn.Location = new System.Drawing.Point(1005, 5);
            this.RegisterBtn.Name = "RegisterBtn";
            this.RegisterBtn.Size = new System.Drawing.Size(87, 46);
            this.RegisterBtn.TabIndex = 0;
            this.RegisterBtn.Text = "Rejestracja";
            this.RegisterBtn.UseVisualStyleBackColor = true;
            this.RegisterBtn.MouseEnter += new System.EventHandler(this.RegisterBtn_MouseEnter);
            // 
            // UserControl3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "UserControl3";
            this.Size = new System.Drawing.Size(1187, 58);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private NoFocusButton RegisterBtn;
        private NoFocusButton LoginBtn;
    }
}
