using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Keyboard
{
    public class OgrenciIsleri
    {
        private OgrenciDal ogrenciDal = new OgrenciDal();
        private Ogrenci _ogrenci;
        private string _baglanti;
        private string _haberlesme;
        DateTime[] yemekSaatleri = new DateTime[8];
        public int Gecgiris { get; set; }

        public OgrenciIsleri()
        {
            
        }

        public OgrenciIsleri(Ogrenci ogrenci)
        {
            _baglanti = Helper.XmlOku()[0];
            _ogrenci = ogrenci;
            OgrenciPrograminiGetir();
        }

        public List<Ogrenci> Listele(String where)
        {
            List<Ogrenci> ogrencilistesi= ogrenciDal.Listele(where);
            return ogrencilistesi;
        }

        public static Ogrenci KartNoileGetir(string kartNo)
        {
            return OgrenciDal.Getir(kartNo); ;
        }

        public static Ogrenci OgrenciNoileGetir(string ogrenciNo)
        {
            return OgrenciDal.Getir2(ogrenciNo);
        }

        public void OgrenciGuncelle()
        {
            ogrenciDal.Guncelle(_ogrenci);
        }


        public void GirisYap(Turnike turnike)
        {
            int giris = OgrenciPrograminiGetir();
            string giristipi="";
            if (giris == 1){giristipi = "SABAH";}
            if (giris == 2) { giristipi = "OGLE"; }
            if (giris == 3) { giristipi = "AKSAM"; }
            if (giris == 4) { giristipi = "ARA"; }

            try
            {
                using (var conn = new SqlConnection(_baglanti))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("UPDATE ogrenci SET ogrencidurum='1' WHERE parmakizi='" + _ogrenci.kartno + "'", conn); command.ExecuteNonQuery();
                    if (_ogrenci.ogrenciTip == "Bakiye")
                    {
                        _ogrenci.ogrencibakiye -= YemekUcreti();
                        if (_ogrenci.no.Substring(0, 1) != "M")
                        {
                            command = new SqlCommand("UPDATE ogrenci SET ogrencibakiye='" + _ogrenci.ogrencibakiye + "' WHERE parmakizi='" + _ogrenci.kartno + "'", conn); command.ExecuteNonQuery();
                        }
                    }
                    else if (_ogrenci.ogrenciTip == "Girişsayı")
                    {
                        _ogrenci.kalangiris--;
                        if (_ogrenci.no.Substring(0, 1) != "M")
                        {
                            command = new SqlCommand("UPDATE ogrenci SET kalangiris='" + _ogrenci.kalangiris + "' WHERE parmakizi='" + _ogrenci.kartno + "'", conn); command.ExecuteNonQuery();
                        }
                    }
                }
                using (SqlConnection conn = new SqlConnection(_baglanti))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO hareketkaydi (ogrencino,adsoyad,girisdurum,tarih,cihaz,islem) VALUES('" + _ogrenci.no + "', '" + _ogrenci.adi + " " + _ogrenci.soyadi + "', 'NORMAL', GETDATE(), '" + new Turnike().No + "', '" + giristipi + "')", conn);
                    command.ExecuteNonQuery();
                }
                using (SqlConnection conn = new SqlConnection(_baglanti))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO komutlar (komut,turnikeport) VALUES('A','" + turnike.PortNo + "')", conn);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Helper.DosyayaYaz(ex.ToString());
            }
            
        }


        public void OgrencileriDurumlariniSifirla()
        {
            using (SqlConnection conn3 = new SqlConnection(_baglanti))
            {
                conn3.Open();
                SqlCommand command = new SqlCommand("UPDATE ogrenci SET ogrencidurum='0'", conn3);
                command.ExecuteNonQuery();
            }
        }

        public int OgrenciPrograminiGetir()
        {

            int yemekdurumu = 0;
            try
            {
                using (var conn = new SqlConnection(_baglanti))
                {
                    //List<DateTime> yemeksaatleri=new List<DateTime>();
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT sabahbaslamasaati,sabahbitissaati,oglebaslamasaati,oglebitissaati,aksambaslamasaati,aksambitissaati,araogunbaslamasaati,araogunbitissaati FROM program2 ", conn);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        yemekSaatleri[0] = Convert.ToDateTime(reader["sabahbaslamasaati"].ToString());
                        yemekSaatleri[1] = Convert.ToDateTime(reader["sabahbitissaati"].ToString());
                        yemekSaatleri[2] = Convert.ToDateTime(reader["oglebaslamasaati"].ToString());
                        yemekSaatleri[3] = Convert.ToDateTime(reader["oglebitissaati"].ToString());
                        yemekSaatleri[4] = Convert.ToDateTime(reader["aksambaslamasaati"].ToString());
                        yemekSaatleri[5] = Convert.ToDateTime(reader["aksambitissaati"].ToString());
                        yemekSaatleri[6] = Convert.ToDateTime(reader["araogunbaslamasaati"].ToString());
                        yemekSaatleri[7] = Convert.ToDateTime(reader["araogunbitissaati"].ToString());

                        if (DateTime.Now.TimeOfDay >= yemekSaatleri[0].TimeOfDay && DateTime.Now.TimeOfDay <= yemekSaatleri[1].TimeOfDay) { yemekdurumu = 1; }
                        else if (DateTime.Now.TimeOfDay >= yemekSaatleri[2].TimeOfDay && DateTime.Now.TimeOfDay <= yemekSaatleri[3].TimeOfDay) { yemekdurumu = 2; }
                        else if (DateTime.Now.TimeOfDay >= yemekSaatleri[4].TimeOfDay && DateTime.Now.TimeOfDay <= yemekSaatleri[5].TimeOfDay) { yemekdurumu = 3; }
                        else if (DateTime.Now.TimeOfDay >= yemekSaatleri[6].TimeOfDay && DateTime.Now.TimeOfDay <= yemekSaatleri[7].TimeOfDay) { yemekdurumu = 4; }
                        else { yemekdurumu = 0; }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.DosyayaYaz(ex.ToString());
            }
            

            return yemekdurumu;
            
        }

        public double YemekUcreti()
        {
            double yemekucreti = 0;
            using (var conn = new SqlConnection(_baglanti))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT yemekucreti FROM ayarlar", conn);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yemekucreti = Convert.ToDouble(reader["yemekucreti"]);
                }
            }

            return yemekucreti;
        }

        public int GirisYapmisMi()
        {
            int yemekvarmi = OgrenciPrograminiGetir();
            int i = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(_baglanti))
                {
                    conn.Open();
                    if (OgrenciPrograminiGetir() == 1)
                    {
                        SqlCommand command = new SqlCommand("SELECT ogrencino,adsoyad,tarih FROM hareketkaydi where " +
                                               "ogrencino='" + _ogrenci.no + "' AND tarih>'" + DateTime.Today.ToString("yyyy-MM-dd") + " " + yemekSaatleri[0].ToString("HH:mm") + "' AND tarih<'" + DateTime.Today.ToString("yyyy-MM-dd") + " " + yemekSaatleri[1].ToString("HH:mm") + "'", conn);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            i++;
                        }
                    }
                    else if (OgrenciPrograminiGetir() == 2)
                    {
                        SqlCommand command = new SqlCommand("SELECT ogrencino,adsoyad,tarih FROM hareketkaydi where " +
                                                            "ogrencino='" + _ogrenci.no + "' AND tarih>'" + DateTime.Today.ToString("yyyy-MM-dd") + " " + yemekSaatleri[2].ToString("HH:mm") + "' AND tarih<'" + DateTime.Today.ToString("yyyy-MM-dd") + " " + yemekSaatleri[3].ToString("HH:mm") + "'", conn);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            i++;
                        }
                    }
                    else if (OgrenciPrograminiGetir() == 3)
                    {
                        SqlCommand command = new SqlCommand("SELECT ogrencino,adsoyad,tarih FROM hareketkaydi where " +
                                                            "ogrencino='" + _ogrenci.no + "' AND tarih>'" + DateTime.Today.ToString("yyyy-MM-dd") + " " + yemekSaatleri[4].ToString("HH:mm") + "' AND tarih<'" + DateTime.Today.ToString("yyyy-MM-dd") + " " + yemekSaatleri[5].ToString("HH:mm") + "'", conn);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            i++;
                        }
                    }
                    else if (OgrenciPrograminiGetir() == 4)
                    {
                        SqlCommand command = new SqlCommand("SELECT ogrencino,adsoyad,tarih FROM hareketkaydi where " +
                                                            "ogrencino='" + _ogrenci.no + "' AND tarih>'" + DateTime.Today.ToString("yyyy-MM-dd") + " " + yemekSaatleri[6].ToString("HH:mm") + "' AND tarih<'" + DateTime.Today.ToString("yyyy-MM-dd") + " " + yemekSaatleri[7].ToString("HH:mm") + "'", conn);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.DosyayaYaz(ex.ToString());
            }
            
            return i;
        }

        public int BugunkugirisSayisi()
        {
            int bugunkugirisSayisi = 0;
            try
            {
                using (var conn = new SqlConnection(_baglanti)) //HAREKET KAYDINDA BUGÜN ÖĞRENCİ GİRİŞİ VAR MI 
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT ogrencino,tarih,islem FROM hareketkaydi where ogrencino='" + _ogrenci.no + "'AND tarih>'" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "' AND islem='" + "GİRİŞ" + "'", conn);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) { bugunkugirisSayisi++; }
                }
            }
            catch (Exception ex)
            {
                Helper.DosyayaYaz(ex.ToString());
            }
            
            return bugunkugirisSayisi;
        }

        public void NullDoldur()
        {
            string[] sutunAdi = { "TC", "ogrenciadi", "ogrencisoyadi", "cinsiyeti", "ogrencisinifi", "ogrencialani", "ogrencisubesi", "sinifogretmeni", "ogrencidurum", "parmakizi", "anneadi", "babaadi", "ogrencitel", "velitel", "adres", "ogrencibakiye","kalangiris","ogrencitipi"};
            for (int i = 0; i < sutunAdi.Length; i++)
            {
                if (sutunAdi[i] == "ogrencidurum")
                {
                    using (var conn = new SqlConnection(_baglanti))
                    {
                        conn.Open();
                        SqlCommand command = new SqlCommand("UPDATE ogrenci SET " + sutunAdi[i] + "='" + "DIŞARIDA" + "' WHERE (" + sutunAdi[i] + " IS NULL)", conn);
                        command.ExecuteNonQuery();
                    }
                }
                else if (sutunAdi[i] == "ogrencibakiye")
                {
                    using (var conn = new SqlConnection(_baglanti))
                    {
                        conn.Open();
                        SqlCommand command = new SqlCommand("UPDATE ogrenci SET " + sutunAdi[i] + "='" + 0 + "' WHERE (" + sutunAdi[i] + " IS NULL)", conn);
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection(_baglanti))
                    {
                        conn.Open();
                        SqlCommand command = new SqlCommand("UPDATE ogrenci SET " + sutunAdi[i] + "=''" + "" + "WHERE " + sutunAdi[i] + " IS NULL", conn);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
