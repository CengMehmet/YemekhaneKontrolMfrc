using System;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Net;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using SerialPort = System.IO.Ports.SerialPort;

namespace Keyboard
{
    public partial class Keyboard : Form
    {
        #region TANIMLAMALAR
        String gelenveri = "";
        string girissms = "", cikissms = "", gelmedisms = "", cikmadisms = "";
        int simdikiWidth = 1366;
        int simdikiHeight = 800;
        string sqlbagstring = "";
        public SqlConnection sqlbag, sqlbag2, sqlbag3;
        SqlCommand k, k2, k3;
        SqlDataReader rd, rd2, rd3;
        Thread t, t1;
        const bool CaptureOnlyInForeground = true;
        string[] cihazlar, cihazokunan;
        TextBox[] tb = new TextBox[12];
        com.ttmesaj.ws.Service1 smsClient = new com.ttmesaj.ws.Service1();
        string[] giriscikissaatleri=new string[8];
        bool turnikeizni = false;
        Panel ekran;
        TextBox[] anabilgitb;
        #endregion

        #region UDP TANIMLAMALAR
        private const int listenPort = 8888;
        UdpClient listener = new UdpClient(listenPort);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
        Thread tUDP;
        public string komut = "", cevap = "";
        public int udpdenemesayisi = 0;
        string turnikeveri = "";
        #endregion

        private string haberlesme = "SER";

        public Keyboard()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            this.Location = new Point(0, 0);
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            Rectangle calismaAlan = new Rectangle();
            calismaAlan = Screen.GetBounds(calismaAlan);
            float oranWidth = ((float)calismaAlan.Width / (float)simdikiWidth);
            float oranHeight = ((float)calismaAlan.Height / (float)simdikiHeight);
            this.Scale(new SizeF(oranWidth, oranHeight));
            this.WindowState = FormWindowState.Maximized;            
            sqlconfigoku();
            Helper.Veritabaniguncelle();
            baslangicdegerlerinial();
            groupBox4.Enabled = false;
            GetSecondaryScreen();
            Thread komutthread = new Thread(komutgonderthread); komutthread.Start();
            tUDP = new Thread(Receive); tUDP.Start();
            timerethkontrol.Start();


        }

        public void komutgonderthread()
        {
            while (true)
            {
                SqlCommand ktemp, ktemp2; SqlDataReader rdtemp;
                using (SqlConnection conn = new SqlConnection(sqlbagstring))
                {
                    string id;
                    string portadi="";
                    conn.Open();
                    ktemp = new SqlCommand("SELECT id,komut,turnikeport FROM komutlar", conn);
                    rdtemp = ktemp.ExecuteReader();
                    while (rdtemp.Read())
                    {
                        string komut = rdtemp["komut"].ToString();
                        id = rdtemp["id"].ToString();
                        portadi = rdtemp["turnikeport"].ToString();
                        if (komut.Length>1 && komut.Substring(0, 2) == "tu")
                        {

                        }
                        else
                        {
                            try
                            {
                               
                                if (!serialPortturnike.IsOpen)
                                {
                                    serialPortturnike.Open();
                                }
                                serialPortturnike.WriteLine(komut);
                            }
                            catch (Exception ex) { Helper.DosyayaYaz(ex.ToString()); }
                            
                        }

                        using (SqlConnection conn2 = new SqlConnection(sqlbagstring))
                        {
                            conn2.Open();
                            ktemp2 = new SqlCommand("delete komutlar where id='" + id + "'", conn2);
                            ktemp2.ExecuteNonQuery();
                        }
                    }
                }
                Thread.Sleep(50);
            }
        }


        private void Keyboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();

            try
            {
                Application.Exit();
                Environment.Exit(1);
            }
            catch (Exception)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        #region UDP DEĞERLENDİRME

        private void timerethkontrol_Tick(object sender, EventArgs e)
        {
            Helper.KomutGonder("ok");
        }

        public void Receive()
        {
            while (true)
            {
                byte[] bytes = listener.Receive(ref groupEP);
                turnikeveri = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                try
                {
                    if (turnikeveri.Length > 3 && turnikeveri.Substring(0, 1) == "T")
                    {
                        Turnike turnike = new Turnike();
                        turnike.Haberlesme = "ETH";
                        turnike.No = Convert.ToInt32(turnikeveri.Substring(4, 1));
                        turnike.Reader = Convert.ToInt32(turnikeveri.Substring(turnikeveri.IndexOf("Reader") + 7, 1));
                        turnike.KartNo = turnikeveri.Substring(turnikeveri.IndexOf("UID:") + 4, 8);
                        textBoxparmakizikimliği.Text = turnike.KartNo;
                        Ogrenci ogrenci = OgrenciIsleri.KartNoileGetir(turnike.KartNo);
                        if (turnike.Reader == 1) { Thread tpd = new Thread(() => OgrenciGirisi(ogrenci, turnike)); tpd.Start(); }
                    }
                }
                catch (Exception ex)
                {
                    Helper.DosyayaYaz(ex.ToString());
                }
            }
        }
        #endregion


        #region TEMEL İŞLEMLER

        List<SerialPort> sparray;


        public void spislem(object sender, SerialDataReceivedEventArgs e) //SERİPORT EVENTİ 
        {
            SerialPort sp = (SerialPort)sender;
            Turnike turnike = new Turnike();
            turnike.Haberlesme = "SER";
            turnike.PortNo = sp.PortName; ;
            string gelenmesaj = sp.ReadLine();
            try
            {
                if (gelenmesaj.Length < 4) { return; }
                if (gelenmesaj.Substring(0, 4) == "<ID>")
                {
                    string modulid = gelenmesaj.Substring(gelenmesaj.IndexOf("<ID>") + 4, 1);
                    string modulport = sp.PortName;
                    turnike.No = Convert.ToInt32(modulid);
                    turnike.PortNo = modulport;
                }
                if (gelenmesaj.Substring(0, 4) == "TNO:")
                {
                    turnike.No = Convert.ToInt32(gelenmesaj.Substring(4, 1));
                    turnike.KartNo = gelenmesaj.Substring(gelenmesaj.IndexOf("UID:") + 4, 8);
                    turnike.Reader = Convert.ToInt32(gelenmesaj.Substring(gelenmesaj.IndexOf("Reader") + 7, 1));
                    textBoxparmakizikimliği.Text = turnike.KartNo;
                    Ogrenci ogrenci = OgrenciIsleri.KartNoileGetir(turnike.KartNo);
                    if (turnike.Reader == 0) { OgrenciGirisi(ogrenci, turnike); }
                }
            }
            catch (Exception ex)
            {
                Helper.DosyayaYaz(ex.ToString());
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e) //
        {
            try
            {

                labelsaat.Text = DateTime.Now.ToString("HH:mm");
                labeltarih.Text = DateTime.Now.ToString("dd-MM-yyyy");
                SqlCommand ktemp; SqlDataReader rdtemp;
                if (DateTime.Now.ToString("HH:mm") == "00:10")
                {
                    richTextBox1.Clear();
                    OgrenciIsleri ogrenciIsleri = new OgrenciIsleri();
                    ogrenciIsleri.OgrencileriDurumlariniSifirla();
                }
            }
            catch (Exception ex)
            {
                Helper.DosyayaYaz(ex.ToString());
            }
        } //Tarih-Saat Kontrolü

        private void button13_Click(object sender, EventArgs e)
        {
            Turnike turnike = new Turnike() { Haberlesme = haberlesme, No = 0, Reader = 0 };
            Ogrenci ogrenci = OgrenciIsleri.OgrenciNoileGetir(textBox45.Text);
            OgrenciGirisi(ogrenci, turnike);
        }

        public Screen GetSecondaryScreen()
        {
            if (Screen.AllScreens.Length == 1)
            {
                return null;
            }
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.Primary == true)
                {
                    return screen;
                }
            }
            return null;
            
        }

        public void sqlconfigoku()
        {
            StreamReader oku;
            oku = File.OpenText("sqlconfig.txt");
            string yazi, yazi2 = "";
            while ((yazi = oku.ReadLine()) != null)
            {
                yazi2 = yazi;
            }
            oku.Close();
            sqlbag = new SqlConnection(yazi2);
            sqlbag2 = new SqlConnection(yazi2);
            sqlbag3 = new SqlConnection(yazi2);
            sqlbagstring = yazi2;

            try
            {
                sqlbag.Open();
            }
            catch (Exception ex)
            {
                Helper.DosyayaYaz(ex.ToString());
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

        private void pictureBoxcikis_Click(object sender, EventArgs e)
        {
            Application.ExitThread();

            try
            {
                Application.Exit();
                Environment.Exit(1);
            }
            catch (Exception)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        public void baslangicdegerlerinial()
        {
            button6.Enabled = true;
            button15.Enabled = false;
            checkBox5.Checked = true;
            anabilgitb = new TextBox[] { textBoxogrencinumarasikaydet, textBoxogrno, textBoxogradi, textBoxogrsoyad, textBoxogrsinif, textBoxogrsube, textBoxogrdurum };
            try
            {
                Ayarlar ayarlar = new Ayarlar();
                ayarlar.AyarGetir();
                textBoxnetgsmusername.Text = ayarlar.smsusername;
                textBoxnetgsmpass.Text = ayarlar.smspass;
                textBoxnetgsmheader.Text = ayarlar.smsheader;
                richTextBoxgirissms.Text = ayarlar.girissms;
                checkBox1.Checked = Convert.ToBoolean(ayarlar.girdisms);
                checkBox2.Checked = Convert.ToBoolean(ayarlar.bakiyesms);
                checkBox3.Checked = Convert.ToBoolean(ayarlar.bakiyegoster);
                checkBox5.Checked = Convert.ToBoolean(ayarlar.turnikedevrede);
                checkBox9.Checked = Convert.ToBoolean(ayarlar.tekrarkontrol);
                checkBox10.Checked = Convert.ToBoolean(ayarlar.girissaatkontrol);
                comboBoxsmsfirma.Text = ayarlar.smsfirma;
                comboBox1.Text = ayarlar.seriportturnike;
                serialPortturnike.PortName = ayarlar.seriportturnike;
                
            }
            catch (Exception ex) { Helper.DosyayaYaz(ex.ToString()); }

            try
            {
                if (serialPortturnike.IsOpen) { serialPortturnike.Close();}
                serialPortturnike.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show("Seri Port Bağlantısı Açılamadı.");
            }
            
        }

        public void OgrenciGirisi(Ogrenci ogrenci, Turnike turnike)
        {
            baslangicdegerlerinial();
            textBoxogrencinumarasikaydet.Clear();
            try
            {
                pictureBox1.Image = null;
            } catch (Exception ex) { Helper.DosyayaYaz(ex.ToString()); }

            bool turnikeizni = false;
            foreach (var tb in anabilgitb) { tb.Clear(); }
            OgrenciIsleri ogrenciIsleri = new OgrenciIsleri(ogrenci);
            if (String.IsNullOrEmpty(ogrenci.no))
            {
                try
                {
                    {
                        ((Form1) Application.OpenForms["Form1"]).pictureBoxfoto.Image = null;((Form1) Application.OpenForms["Form1"]).labelogrno.Text = ""; ((Form1)Application.OpenForms["Form1"]).labeladsoyad.Text = ""; ((Form1)Application.OpenForms["Form1"]).labelsinif.Text = "";
                        ((Form1)Application.OpenForms["Form1"]).labeldurum.Font = new Font("Candara", 88, FontStyle.Bold); ((Form1)Application.OpenForms["Form1"]).labeldurum.Text = "KAYITSIZ KART"; ((Form1)Application.OpenForms["Form1"]).labeldurum.ForeColor = Color.Red;
                    }
                }
                catch (Exception ex) { Helper.DosyayaYaz(ex.ToString()); }
                return;
            }

            
            ((Form1)Application.OpenForms["Form1"]).labeldurum.Font = new Font("Candara", 48, FontStyle.Bold);
            int yemekSaati = ogrenciIsleri.OgrenciPrograminiGetir();
            int bugunkuGirisSayisi = ogrenciIsleri.BugunkugirisSayisi();
            int girisYapmisMi = ogrenciIsleri.GirisYapmisMi();
            try
            {
                
                pictureBox1.Image = Helper.ResimYukle(ogrenci.no);
                textBoxogrno.Text = ogrenci.no; textBoxogrencinumarasikaydet.Text = ogrenci.no;
                textBoxogradi.Text = ogrenci.adi; textBoxogrsoyad.Text = ogrenci.soyadi;
                textBoxogrsinif.Text = ogrenci.sinifi; textBoxogrsube.Text = ogrenci.subesi;
            }
            catch (Exception ex) { Helper.DosyayaYaz(ex.ToString()); }

            double bakiye = (ogrenci.ogrencibakiye - ogrenciIsleri.YemekUcreti());
            if (ogrenci.no.Substring(0, 1) == "P"){ ogrenci.kalangiris = 0;}
                if (yemekSaati > 0 || !checkBox10.Checked || ogrenci.no.Substring(0, 1) == "M")
                {
                    if (bakiye >= 0 || ogrenci.kalangiris>0 || ogrenci.no.Substring(0, 1) == "M")
                    {
                        if (girisYapmisMi==0 || ogrenci.no.Substring(0, 1) == "M" || !checkBox9.Checked)
                        {
                            try
                            {
                                ogrenciIsleri.GirisYap(turnike);
                                labeldurum.ForeColor = Color.Lime;
                                richTextBox1.Text = ogrenci.no + "Numaralı " + ogrenci.adi + " Giriş Yaptı." + DateTime.Now + "\n" + richTextBox1.Text;
                                labeldurum.Text = "Öğrenci Girişi Gerçekleştirildi.";
                            }
                            catch (Exception ex)
                            {
                                Helper.DosyayaYaz(ex.ToString());
                            }
                            
                            try
                            {
                                if (checkBox3.Checked) //BAKİYE GÖSTERİLSİN Mİ
                                {
                                    if (ogrenci.ogrenciTip == "Bakiye")
                                    {
                                        labeldurum.ForeColor = Color.LimeGreen; labeldurum.Text = "AFİYET OLSUN  Kalan Bakiye: \n" + ogrenci.ogrencibakiye + "  TL";
                                    }
                                    else if (ogrenci.ogrenciTip == "Girişsayı")
                                    {
                                        labeldurum.ForeColor = Color.LimeGreen; labeldurum.Text = "AFİYET OLSUN  Kalan Giriş: \n" + ogrenci.kalangiris + "  ";
                                    }
                                }
                                else
                                {
                                    if(ogrenci.no.Substring(0,1).ToString() == "P") { labeldurum.ForeColor = Color.LimeGreen; labeldurum.Text = "AFİYET OLSUN  Kalan Bakiye: \n" + ogrenci.ogrencibakiye + "  TL"; }
                                    else { labeldurum.ForeColor = Color.LimeGreen; labeldurum.Text = "AFİYET OLSUN";}
                                }
                            }catch (Exception ex) { Helper.DosyayaYaz(ex.ToString()); }

                            string mesaj = ""; mesaj = (ogrenci.no + " NUMARALI " + ogrenci.adi + " " + ogrenci.soyadi + " " + richTextBoxgirissms.Text + " Kalan Bakiye:" + ogrenci.ogrencibakiye);

                            if (checkBox5.Checked) { turnike.Ac(); }//Turnike devrede ise aç
                            if (checkBox1.Checked)  { { smsgonder(ogrenci.velitel, mesaj); } }
                            if (checkBox2.Checked)//Bakiye Azaldığında SMS Gönder
                            {
                                if (ogrenci.ogrenciTip == "Bakiye") { if (ogrenci.ogrencibakiye <= ogrenciIsleri.YemekUcreti() * 2) { mesaj = ""; mesaj = (ogrenci.no + " NUMARALI " + ogrenci.adi + " " + ogrenci.soyadi + ". Kalan Bakiye:" + ogrenci.ogrencibakiye + " TL.Giriş Yapılması İçin Lütfen Bakiye Yükleyiniz."); smsgonder(ogrenci.velitel, mesaj); } }
                                else if (ogrenci.ogrenciTip == "Girişsayı") { if (ogrenci.kalangiris <= 2) { mesaj = ""; mesaj = (ogrenci.no + " NUMARALI " + ogrenci.adi + " " + ogrenci.soyadi + ". Kalan Giriş Hakkı:" + ogrenci.kalangiris + ".Giriş Yapılması İçin Lütfen Bakiye Yükleyiniz."); smsgonder(ogrenci.velitel, mesaj); } }
                            }
                            textBoxogrdurum.Text = "İçerde";
                        }
                        else
                        {
                            try
                            {
                                if (ogrenci.no.Substring(0, 1) != "P") { try { labeldurum.ForeColor = Color.Red; labeldurum.Text = "ÖĞRENCİ ZATEN İÇERDE..."; } catch (Exception) { } }
                                else if (ogrenci.no.Substring(0, 1) == "P") { try { labeldurum.ForeColor = Color.Red; labeldurum.Text = "PERSONEL ZATEN İÇERDE..."; } catch (Exception) { } }
                            }
                            catch (Exception ex)
                            {
                                Helper.DosyayaYaz(ex.ToString());
                            }
                                                       
                        }                       
                    }
                    else {try{ labeldurum.ForeColor = Color.Red; labeldurum.Text = "YETERSİZ BAKİYE";}catch (Exception) { }}
                }
                else { try{ labeldurum.ForeColor = Color.Red; labeldurum.Text = "YEMEK SAATİ BİTTİ. GİRİŞ YOK!";} catch (Exception ex) { } }
                PaneliGuncelle(ogrenci, turnike, labeldurum.Text,labeldurum.ForeColor);
        }

        public void PaneliGuncelle(Ogrenci ogrenci, Turnike turnike, string durum,Color renk)
        {
            try
            {
                ((Form1)Application.OpenForms["Form1"]).pictureBoxfoto.Image = Helper.ResimYukle(ogrenci.no);
                ((Form1)Application.OpenForms["Form1"]).labelogrno.Text = ogrenci.no;
                ((Form1)Application.OpenForms["Form1"]).labeladsoyad.Text = ogrenci.adi + " " + ogrenci.soyadi;
                ((Form1)Application.OpenForms["Form1"]).labelsinif.Text = ogrenci.sinifi + "/" + ogrenci.alani + "/" + ogrenci.subesi;
                ((Form1)Application.OpenForms["Form1"]).labeldurum.Text = durum;
                ((Form1)Application.OpenForms["Form1"]).labeldurum.ForeColor = renk;
            }
            catch (Exception ex) { Helper.DosyayaYaz(ex.ToString()); }
        }

        #endregion

        #region SMS

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBoxsmsfirma.SelectedItem.ToString() == "MayaTek")
            {
                //netgsmgonder(textBox1.Text,richTextBox4.Text); // Numaralar string bir şekilde aralarında virgül olarak gönderilir.
                var SR = new com.megaokul.gateway.HERYONESMSWebService();
                //labelsmsdurum.Text = SR.MAYATEK_SMS_SENDER("903327131895", "sms43hmgy", "iPEK YOLU", 1, "5325005024", "işte bu", 0);
                labelsmsdurum.Text = SR.MAYATEK_SMS_SENDER(textBoxnetgsmusername.Text, textBoxnetgsmpass.Text, textBoxnetgsmheader.Text, 1, textBox6.Text, richTextBox4.Text, 0);
            }
            else if (comboBoxsmsfirma.SelectedItem.ToString() == "NetGSM")
            {
                netgsmgonder("90"+textBox6.Text, richTextBox4.Text);
            }
            else if (comboBoxsmsfirma.SelectedItem.ToString() == "TTMesaj")
            {
                ttmesajgonder("90" + textBox6.Text, richTextBox4.Text);
            }
            else if (comboBoxsmsfirma.SelectedItem.ToString() == "Labirent")
            {
                labirentsmsgonder("0" + textBox6.Text, richTextBox4.Text);
            }
        } //SINAMA SMS İ GÖNDERME

        public void smsgonder(string ogrencitel, string mesaj)
        {
            
            if (comboBoxsmsfirma.SelectedItem.ToString() == "MayaTek")
            {
                try
                {
                    var SR = new com.megaokul.gateway.HERYONESMSWebService();
                    labelsmsdurum.Text = SR.MAYATEK_SMS_SENDER(textBoxnetgsmusername.Text, textBoxnetgsmpass.Text, textBoxnetgsmheader.Text, 1, ogrencitel, mesaj, 0);
                }
                catch (Exception ex)
                {
                    Helper.DosyayaYaz(ex.ToString());
                }
            }
            else if (comboBoxsmsfirma.SelectedItem.ToString() == "NetGSM")
            {
                try
                {
                    netgsmgonder(ogrencitel.Replace("(","").Replace(")","").Replace("-","").Replace(" ",""), mesaj);
                }
                catch (Exception ex)
                {
                    Helper.DosyayaYaz(ex.ToString());
                }
            }
            else if (comboBoxsmsfirma.SelectedItem.ToString() == "TTMesaj")
            {
                try
                {
                    ttmesajgonder(ogrencitel, mesaj);
                }
                catch (Exception ex)
                {
                    Helper.DosyayaYaz(ex.ToString());
                }
            }
            else if (comboBoxsmsfirma.SelectedItem.ToString() == "Labirent")
            {
                try
                {
                    labirentsmsgonder(ogrencitel.Substring(1,ogrencitel.Length-1), mesaj);
                }
                catch (Exception ex)
                {
                    Helper.DosyayaYaz(ex.ToString());
                }
            }
        }

        private void netgsmgonder(string tel, string mesaj)
        {
            string ss = "";
            ss += "<?xml version='1.0' encoding='UTF-8'?>";
            ss += "<mainbody>";
            ss += "<header>";
            ss += "<company dil='TR'>NETGSM</company>";
            ss += "<usercode>" + textBoxnetgsmusername.Text + "</usercode>";//8503028531
            ss += "<password>" + textBoxnetgsmpass.Text + "</password>";//9A8C3Z6
            ss += "<startdate></startdate>";
            ss += "<stopdate></stopdate>";
            ss += "<type>1:n</type>";
            ss += "<msgheader>" + textBoxnetgsmheader.Text + "</msgheader>";
            ss += "</header>";
            ss += "<body>";
            ss += "<msg><![CDATA[" + mesaj + "]]></msg>";
            ss += "<no>" + tel + "</no>";
            ss += "</body>";
            ss += "</mainbody>";
            labelsmsdurum.Text = XMLPOST("http://api.netgsm.com.tr/xmlbulkhttppost.asp", ss);
        }

        public void ttmesajgonder(string tel,string mesaj)
        {
            try
            {
                string response = string.Empty;
                response = smsClient.sendSingleSMS(textBoxnetgsmusername.Text, textBoxnetgsmpass.Text, tel, mesaj, textBoxnetgsmheader.Text,"0","0");                
            }
            catch (Exception ex)
            {
                
            }
        }

        public void labirentsmsgonder(string tel, string mesaj)
        {
            labelsmsdurum.Text = labirentsmspost("<MainmsgBody><UserName>" + textBoxnetgsmusername.Text+ "</UserName>"+
                "<PassWord>" + textBoxnetgsmpass.Text + "</PassWord>"+
                "<Action>12</Action>"+
                "<Mesgbody>"+mesaj+"</Mesgbody>"+
                "<Numbers>"+tel+"</Numbers>"+
                "<Originator>"+textBoxnetgsmheader.Text+"</Originator>"+
                "<SDate></SDate></MainmsgBody>");
        }

        public string labirentsmspost(string prmSendData)
        {
            try
            {
                WebClient wUpload = new WebClient();
                wUpload.Proxy = null;
                Byte[] bPostArray = Encoding.UTF8.GetBytes(prmSendData);
                Byte[] bResponse = wUpload.UploadData("http://g.iletimx.com", "POST", bPostArray);
                Char[] sReturnChars = Encoding.UTF8.GetChars(bResponse);
                string sWebPage = new string(sReturnChars);
                return sWebPage;
            }
            catch { return "-1"; }
        }

        public string DakikSMSMesajGonder(string numaralar, string mesaj)
        {
            // DEĞİŞKENLER OLUŞTURULUYOR
            string kullaniciAdi = "8506766516", sifre = "2kzvy8", baslik = "DEMO";// mesaj = "gönderilecek mesajınız";
            // XML DESENİ YARATILIYOR.
            string xmlDesen = "<SMS><oturum><kullanici>" + kullaniciAdi + "</kullanici><sifre>" + sifre + "</sifre></oturum><mesaj><baslik>" + baslik + "</baslik><metin>" + mesaj + "</metin><alicilar>" + numaralar.ToString() + "</alicilar><tarih></tarih></mesaj></SMS>";
            string ApiAdres = "http://www.dakiksms.com/api/xml_api_ileri.php";
            // APIYE XML DESENİ VE API ADRESİ GÖNDERİLİYOR.
            WebRequest request = WebRequest.Create(ApiAdres);
            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(xmlDesen);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
            // DÖNEN CEVAP İLGİLİ YERE GÖNDERİLİYOR.
        }

        public void netgsmkalanbakiye()
        {
            string ss = "";
            ss += "<?xml version='1.0'?>";
            ss += "<mainbody>";
            ss += "<header>";
            ss += "<company>NETGSM</company>";
            ss += "<usercode>Kullaniciadi</usercode>";
            ss += "<password>Sifre</password>";
            ss += "<stip>2</stip>";
            ss += "</header>";
            ss += "</mainbody>";

            labelsmsdurum.Text = XMLPOST("https://api.netgsm.com.tr/xmlpaketkampanya.asp", ss);
        }

        private string XMLPOST(string PostAddress, string xmlData)
        {
            try
            {
                WebClient wUpload = new WebClient();
                HttpWebRequest request = WebRequest.Create(PostAddress) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                Byte[] bPostArray = Encoding.UTF8.GetBytes(xmlData);
                Byte[] bResponse = wUpload.UploadData(PostAddress, "POST", bPostArray);
                Char[] sReturnChars = Encoding.UTF8.GetChars(bResponse);
                string sWebPage = new string(sReturnChars);
                return sWebPage;
            }
            catch
            {
                return "-1";
            }
        }
        #endregion

        #region ANASAYFA
        private void pictureBoxbarkod_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void buttonogrencinumarasikaydet_Click(object sender, EventArgs e)
        {
            Ogrenci ogrenci = OgrenciIsleri.OgrenciNoileGetir(textBoxogrencinumarasikaydet.Text);
            if (ogrenci.no == null) { return; }
            ogrenci.kartno = textBoxparmakizikimliği.Text;
            (new OgrenciIsleri(ogrenci)).OgrenciGuncelle();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            (new Turnike() { Haberlesme = haberlesme, No = 0, Reader = 0 }).Ac();
        }

        private void pictureBoxayarlar_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            Turnike turnike = new Turnike();
            Ogrenci ogrenci = OgrenciIsleri.KartNoileGetir("6");
            OgrenciGirisi(ogrenci, turnike);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            (new Turnike() { Haberlesme = haberlesme, No = 0, Reader = 1 }).Ac();
        }
        #endregion

        #region ÖĞRENCİLER

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
            ogrencileriyenile();
        }

        public void ogrencileriyenile()
        {
            OgrenciDal ogrencidal = new OgrenciDal();
            dataGridView1.DataSource = ogrencidal.Listele();
            byte[] gizlenecek = new byte[] { 1, 4, 8, 9, 10, 12, 13, 16, 17, 18 };
            foreach (var index in gizlenecek) { dataGridView1.Columns[index].Visible = false; }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e) //Griddeki elemana Tıklama
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv != null && dgv.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgv.SelectedRows[0];
                if (row != null)
                {
                    textBox16.Text = row.Cells[0].Value.ToString();
                    pictureBox7.Image = Helper.ResimYukle(textBox16.Text);
                    Ogrenci ogrenci = OgrenciIsleri.OgrenciNoileGetir(textBox16.Text);
                    textBox17.Text = ogrenci.TC;
                    textBox18.Text = ogrenci.adi;
                    textBox19.Text = ogrenci.soyadi;
                    textBox26.Text = ogrenci.anneadi;
                    textBox20.Text = ogrenci.babaadi;
                    maskedTextBoxOgrenciTel.Text = ogrenci.telefonu;
                    maskedTextBoxVeliTel.Text = ogrenci.velitel;
                    textBox29.Text = ogrenci.sinifi;
                    comboBox6.Text = ogrenci.alani;
                    textBox27.Text = ogrenci.subesi;
                    textBox23.Text = ogrenci.kartno;
                }
            }
        }

        #endregion

        #region AYARLAR
        private void pictureBoxayarlar_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            label130.BackColor = Color.White;
            label130.Visible = true;
        }

        private void pictureBox6_Click(object sender, EventArgs e) //AYARLARI KAYDET
        {
            Ayarlar ayarlar = new Ayarlar();
            ayarlar.smsusername = textBoxnetgsmusername.Text;
            ayarlar.smspass = textBoxnetgsmpass.Text;
            ayarlar.smsheader = textBoxnetgsmheader.Text;
            ayarlar.girissms = richTextBoxgirissms.Text;
            ayarlar.girdisms = Convert.ToInt32(checkBox1.Checked);
            ayarlar.bakiyesms = Convert.ToInt32(checkBox2.Checked);
            ayarlar.bakiyegoster = Convert.ToInt32(checkBox3.Checked);
            ayarlar.turnikedevrede = Convert.ToInt32(checkBox5.Checked);
            ayarlar.tekrarkontrol = Convert.ToInt32(checkBox9.Checked);
            ayarlar.girissaatkontrol = Convert.ToInt32(checkBox10.Checked);
            ayarlar.smsfirma = comboBoxsmsfirma.SelectedItem.ToString();
            ayarlar.seriportturnike = comboBox1.Text;
            if (serialPortturnike.IsOpen) { serialPortturnike.Close(); }
            ayarlar.AyarGuncelle();           
            baslangicdegerlerinial();
            // random olarak r adında bir dğeişken tanımladık.
            Random r = new Random();
            //255'e kadar rastgele sayı üretecektir.
            int a, b, c;
            a = r.Next(255);
            b = r.Next(255);
            c = r.Next(255);
            label130.BackColor = Color.FromArgb(a, b, c);
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach (var ports in Helper.loadPorts()) { comboBox1.Items.Add(ports); }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            label11.Visible = false;
        }

        private void buttonTabloDoldur_Click(object sender, EventArgs e)
        {
            (new OgrenciIsleri()).NullDoldur();
            MessageBox.Show("Tablo Doldurma İşlemi Tamamlanmıştır.", "Tablo Doldurma");
        }
        #endregion

        #region MANUAL KONTROL BUTONLARI
        private void button7_Click(object sender, EventArgs e)
        {
            (new Turnike() { Haberlesme = haberlesme, No = 0, Reader = 0 }).Ac();
        }
        private void button12_Click(object sender, EventArgs e)
        {
            (new Turnike() { Haberlesme = haberlesme, No = 0, Reader = 1 }).Ac();

        }
        #endregion

    }
}
