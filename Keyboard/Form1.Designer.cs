namespace Keyboard
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelogrno = new System.Windows.Forms.Label();
            this.labeldurum = new System.Windows.Forms.Label();
            this.labeladsoyad = new System.Windows.Forms.Label();
            this.labelsinif = new System.Windows.Forms.Label();
            this.pictureBoxfoto = new System.Windows.Forms.PictureBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxfoto)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Azure;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.labelogrno);
            this.panel2.Controls.Add(this.labeldurum);
            this.panel2.Controls.Add(this.labeladsoyad);
            this.panel2.Controls.Add(this.labelsinif);
            this.panel2.Location = new System.Drawing.Point(574, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(854, 779);
            this.panel2.TabIndex = 60;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(553, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(226, 102);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 54;
            this.pictureBox1.TabStop = false;
            // 
            // labelogrno
            // 
            this.labelogrno.AutoSize = true;
            this.labelogrno.Font = new System.Drawing.Font("Candara", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelogrno.Location = new System.Drawing.Point(27, 55);
            this.labelogrno.MaximumSize = new System.Drawing.Size(600, 0);
            this.labelogrno.Name = "labelogrno";
            this.labelogrno.Size = new System.Drawing.Size(49, 59);
            this.labelogrno.TabIndex = 43;
            this.labelogrno.Text = "+";
            // 
            // labeldurum
            // 
            this.labeldurum.AutoSize = true;
            this.labeldurum.Font = new System.Drawing.Font("Candara", 80.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labeldurum.Location = new System.Drawing.Point(15, 379);
            this.labeldurum.MaximumSize = new System.Drawing.Size(450, 300);
            this.labeldurum.Name = "labeldurum";
            this.labeldurum.Size = new System.Drawing.Size(109, 131);
            this.labeldurum.TabIndex = 53;
            this.labeldurum.Text = "+";
            // 
            // labeladsoyad
            // 
            this.labeladsoyad.AutoSize = true;
            this.labeladsoyad.Font = new System.Drawing.Font("Candara", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labeladsoyad.Location = new System.Drawing.Point(27, 153);
            this.labeladsoyad.MaximumSize = new System.Drawing.Size(600, 0);
            this.labeladsoyad.Name = "labeladsoyad";
            this.labeladsoyad.Size = new System.Drawing.Size(49, 59);
            this.labeladsoyad.TabIndex = 45;
            this.labeladsoyad.Text = "+";
            // 
            // labelsinif
            // 
            this.labelsinif.AutoSize = true;
            this.labelsinif.Font = new System.Drawing.Font("Candara", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelsinif.Location = new System.Drawing.Point(27, 249);
            this.labelsinif.MaximumSize = new System.Drawing.Size(450, 150);
            this.labelsinif.Name = "labelsinif";
            this.labelsinif.Size = new System.Drawing.Size(49, 59);
            this.labelsinif.TabIndex = 49;
            this.labelsinif.Text = "+";
            // 
            // pictureBoxfoto
            // 
            this.pictureBoxfoto.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBoxfoto.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxfoto.Image")));
            this.pictureBoxfoto.Location = new System.Drawing.Point(37, 42);
            this.pictureBoxfoto.Name = "pictureBoxfoto";
            this.pictureBoxfoto.Size = new System.Drawing.Size(485, 645);
            this.pictureBoxfoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxfoto.TabIndex = 56;
            this.pictureBoxfoto.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.pictureBoxfoto);
            this.Controls.Add(this.panel2);
            this.Name = "Form1";
            this.Text = "WR ÖĞRENCİ KONTROL PANOSU";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxfoto)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.PictureBox pictureBoxfoto;
        public System.Windows.Forms.Label labelogrno;
        public System.Windows.Forms.Label labeldurum;
        public System.Windows.Forms.Label labeladsoyad;
        public System.Windows.Forms.Label labelsinif;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}