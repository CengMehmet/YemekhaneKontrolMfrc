using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Keyboard
{
    public partial class VeriTabaniOlustur : Form
    {
        public VeriTabaniOlustur()
        {
            InitializeComponent();
        }
        private void VeriTabaniOlustur_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
        }

        string BaglantiOlustur()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            if (!String.IsNullOrEmpty(txtServer.Text))
                builder.DataSource = txtServer.Text;
            else
            builder.DataSource = Environment.MachineName;
            builder.InitialCatalog = "master";
            //builder.DataSource = "192.168.2.234";
            //builder.UserID = "sa";
            //builder.Password = "Recep123";
            builder.IntegratedSecurity = true;
            return builder.ConnectionString;
        }

        void VeritabanıKurulumu()
        {
            SqlConnection baglanti = new SqlConnection(BaglantiOlustur());
            string dosya = File.ReadAllText(txtDosya.Text);
            string[] komutlar = Regex.Split(dosya, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            baglanti.Open();
            progressBar1.Maximum = komutlar.Length;
            bool sonuc = true;
            foreach (string komut in komutlar)
            {
                if (komut.Trim() != "")
                {
                    try
                    {
                        new SqlCommand(komut, baglanti).ExecuteNonQuery();
                        progressBar1.Value++;
                        label4.Text = progressBar1.Value.ToString();
                    }
                    catch(Exception ex)
                    {
                        sonuc = false;
                        MessageBox.Show("Bu Veritabanı Zaten Var!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                }
            }
            if (sonuc)
            MessageBox.Show("Script Başarıyla Çalıştırıldı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            baglanti.Close();
            this.DialogResult = DialogResult.Yes;
        }
        private void buttonSecriptSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog ac = new OpenFileDialog();
            ac.Filter = "SQL Script (*.sql) |*.sql";
            ac.Multiselect = false;
            ac.InitialDirectory = Application.StartupPath;
            if (ac.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtDosya.Text = ac.FileName;
            else
                txtDosya.Text = "";
        }

        private void buttonKurulumBaslat_Click(object sender, EventArgs e)
        {
            progressBar1.Value++;
            Thread t = new Thread(new ThreadStart(VeritabanıKurulumu));
            t.Start();
        }
    }
}
