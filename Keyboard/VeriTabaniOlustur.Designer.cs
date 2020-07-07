namespace Keyboard
{
    partial class VeriTabaniOlustur
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
            this.buttonSecriptSec = new System.Windows.Forms.Button();
            this.txtDosya = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.buttonKurulumBaslat = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonSecriptSec
            // 
            this.buttonSecriptSec.Location = new System.Drawing.Point(260, 12);
            this.buttonSecriptSec.Name = "buttonSecriptSec";
            this.buttonSecriptSec.Size = new System.Drawing.Size(72, 23);
            this.buttonSecriptSec.TabIndex = 0;
            this.buttonSecriptSec.Text = "Script Sec";
            this.buttonSecriptSec.UseVisualStyleBackColor = true;
            this.buttonSecriptSec.Click += new System.EventHandler(this.buttonSecriptSec_Click);
            // 
            // txtDosya
            // 
            this.txtDosya.Enabled = false;
            this.txtDosya.Location = new System.Drawing.Point(60, 12);
            this.txtDosya.Name = "txtDosya";
            this.txtDosya.Size = new System.Drawing.Size(189, 20);
            this.txtDosya.TabIndex = 1;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 130);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(316, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // txtServer
            // 
            this.txtServer.Enabled = false;
            this.txtServer.Location = new System.Drawing.Point(60, 52);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(189, 20);
            this.txtServer.TabIndex = 3;
            // 
            // buttonKurulumBaslat
            // 
            this.buttonKurulumBaslat.Location = new System.Drawing.Point(136, 89);
            this.buttonKurulumBaslat.Name = "buttonKurulumBaslat";
            this.buttonKurulumBaslat.Size = new System.Drawing.Size(113, 23);
            this.buttonKurulumBaslat.TabIndex = 4;
            this.buttonKurulumBaslat.Text = "Kurulumu Baslat";
            this.buttonKurulumBaslat.UseVisualStyleBackColor = true;
            this.buttonKurulumBaslat.Click += new System.EventHandler(this.buttonKurulumBaslat_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Dosya";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Server";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Lime;
            this.label3.Location = new System.Drawing.Point(295, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "%";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.ForeColor = System.Drawing.Color.Lime;
            this.label4.Location = new System.Drawing.Point(316, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 8;
            // 
            // VeriTabaniOlustur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 176);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonKurulumBaslat);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.txtDosya);
            this.Controls.Add(this.buttonSecriptSec);
            this.Name = "VeriTabaniOlustur";
            this.Text = "VeriTabaniOlustur";
            this.Load += new System.EventHandler(this.VeriTabaniOlustur_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSecriptSec;
        private System.Windows.Forms.TextBox txtDosya;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Button buttonKurulumBaslat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}