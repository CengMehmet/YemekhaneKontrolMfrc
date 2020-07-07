using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;

namespace Keyboard
{
    public class Turnike
    {
        private Dictionary<int, string> mesaj = new Dictionary<int, string> { { 0, "A" }, { 1, "a" } };
        private string _baglanti;
        public int No { get; set; }
        public string KartNo { get; set; }
        public int Reader  { get; set; }
        public string Haberlesme { get; set; }
        public string PortNo { get; set; }

        private string XmlOku()
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
                            Haberlesme = oku.ReadString();
                            break;
                    }
                }
            }
            oku.Close();
            return _baglanti;
        }
        public bool Ac()
        {
            XmlOku();
            bool turnikeizni = false;
            if (Haberlesme == "SER")
            {
                try
                {
                    using (var conn = new SqlConnection(_baglanti))
                    {
                        conn.Open();
                        SqlCommand command = new SqlCommand("INSERT INTO komutlar (komut) values('" + "turnike" + "')", conn);
                        command.ExecuteNonQuery();
                        turnikeizni = true;
                    }
                }
                catch (Exception ex) { }
            }
            else if (Haberlesme == "ETH")
            {
                try
                {
                    KomutGonder("t" + No + mesaj[Reader]);
                }
                catch (Exception ex)
                {
                }
            }

            return turnikeizni;
        }
        private void KomutGonder(string komut)
        {
            int Port = 8888;
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            byte[] sendbuf = Encoding.UTF8.GetBytes(komut);
            IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, Port);
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            s.SendTo(sendbuf, ep);
        }
    }
}
