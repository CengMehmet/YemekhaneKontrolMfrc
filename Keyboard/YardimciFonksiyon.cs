using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Keyboard
{
    class YardimciFonksiyon
    {
        public static string _baglanti;
        public static string _haberlesme;
        public static string _sqlbagstring;

        public YardimciFonksiyon() { }

        public static void komutgonder(string komut)
        {
            int Port = 8888;
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            byte[] sendbuf = Encoding.UTF8.GetBytes(komut);
            IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, Port);

            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            s.SendTo(sendbuf, ep);
        }
        public static void KomutSil(string id)
        {
            using (SqlConnection conn = new SqlConnection(_baglanti))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("delete komutlar where id='" + id + "'", conn);
                command.ExecuteNonQuery();
            }
        }
        public static string XmlOku()
        {
            XmlTextReader oku = new XmlTextReader("config.xml");
            while (oku.Read())
            {
                if (oku.NodeType == XmlNodeType.Element)
                {
                    switch (oku.Name)
                    {
                        case "SqlBaglanti":
                            _baglanti = oku.ReadString();
                            break;
                        case "Haberlesme":
                            _haberlesme = oku.ReadString();
                            break;
                    }
                }
            }
            oku.Close();
            return _baglanti;
        }

        public static void sqlconfigoku()
        {
            StreamReader oku;
            oku = File.OpenText("sqlconfig.txt");
            string yazi, yazi2 = "";
            while ((yazi = oku.ReadLine()) != null)
            {
                yazi2 = yazi;
            }
            oku.Close();
            SqlConnection sqlbag = new SqlConnection(yazi2);
            _sqlbagstring = yazi2;

            try
            {
                sqlbag.Open();
            }
            catch (Exception ex)
            {
                DosyayaYaz(ex.ToString());
                VeriTabaniOlustur olustur = new VeriTabaniOlustur();
                olustur.ShowDialog();
                if (olustur.DialogResult == DialogResult.Yes)
                {
                    MessageBox.Show("Giriş Yapabilirsiniz.");
                    return;
                }
                else
                {
                    MessageBox.Show("Veritabanı oluşturma işlemi başarısız.");
                    return;
                }
            }
        }

        public static Image ResimYukle(string ogrencino)
        {
            Image image1 = null;
            string adres = "c:/resimler/" + ogrencino + ".jpg";
            try
            {
                if (File.Exists(adres))
                {
                    using (FileStream stream = new FileStream("c:/resimler/" + ogrencino + ".jpg", FileMode.Open))
                    {
                        image1 = Image.FromStream(stream);
                        stream.Flush();
                        stream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                DosyayaYaz(ex.ToString());
            }
            return image1;
        }

        public static void DosyayaYaz(string log)
        {
            try
            {
                string dosya_yolu = @"hatalog.txt";
                using (FileStream fs = new FileStream(dosya_yolu, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(DateTime.Now);
                        sw.WriteLine(log);
                        sw.Flush();
                    }
                }
            }
            catch (Exception) { }
        }
    }
}
