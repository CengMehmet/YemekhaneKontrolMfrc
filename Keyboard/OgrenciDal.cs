using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;

namespace Keyboard
{
    class OgrenciDal
    {
        private static string _baglanti = "";
        private static string _haberlesme = "";
        public OgrenciDal()
        {
            _baglanti=Helper.XmlOku()[0];
        }

        public List<Ogrenci> Listele(string where="")
        {
            List<Ogrenci> ogrencilist = new List<Ogrenci>();
            using (SqlConnection conn = new SqlConnection(_baglanti))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM ogrenci "+where, conn);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Ogrenci ogrenci = new Ogrenci()
                    {
                        no = reader["ogrencino"].ToString(),
                        TC = reader["TC"].ToString(),
                        adi = reader["ogrenciadi"].ToString(),
                        soyadi = reader["ogrencisoyadi"].ToString(),
                        cinsiyeti = reader["cinsiyeti"].ToString(),
                        sinifi = reader["ogrencisinifi"].ToString(),
                        alani = reader["ogrencialani"].ToString(),
                        subesi = reader["ogrencisubesi"].ToString(),
                        ogretmeni = reader["sinifogretmeni"].ToString(),
                        anneadi = reader["anneadi"].ToString(),
                        babaadi = reader["babaadi"].ToString(),
                        velitel = reader["velitel"].ToString(),
                        telefonu = reader["ogrencitel"].ToString(),
                        adresi = reader["adres"].ToString(),
                        kartno = reader["parmakizi"].ToString(),
                        durumu = reader["ogrencidurum"].ToString(),
                        ogrencibakiye = Convert.ToDouble(reader["ogrencibakiye"].ToString()),
                        kalangiris=Convert.ToInt32(reader["kalangiris"]),
                        ogrenciTip = reader["ogrenciTip"].ToString()
                    };
                    ogrencilist.Add(ogrenci);
                }
            }
            return ogrencilist;
        }

        public static Ogrenci Getir(string kartNo)
        {
            _baglanti = Helper.XmlOku()[0];
            Ogrenci ogrenci = new Ogrenci();
            using (SqlConnection conn = new SqlConnection(_baglanti))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM ogrenci where parmakizi='" + kartNo + "'", conn);
                SqlDataReader reader= command.ExecuteReader();
                while (reader.Read())
                {
                    ogrenci = new Ogrenci()
                    {
                        no = reader["ogrencino"].ToString(),
                        TC = reader["TC"].ToString(),
                        adi = reader["ogrenciadi"].ToString(),
                        soyadi = reader["ogrencisoyadi"].ToString(),
                        cinsiyeti = reader["cinsiyeti"].ToString(),
                        sinifi = reader["ogrencisinifi"].ToString(),
                        alani = reader["ogrencisubesi"].ToString(),
                        subesi = reader["ogrencialani"].ToString(),
                        ogretmeni = reader["sinifogretmeni"].ToString(),
                        anneadi = reader["anneadi"].ToString(),
                        babaadi = reader["babaadi"].ToString(),
                        velitel = reader["velitel"].ToString(),
                        telefonu = reader["ogrencitel"].ToString(),
                        adresi = reader["adres"].ToString(),
                        kartno = reader["parmakizi"].ToString(),
                        durumu = reader["ogrencidurum"].ToString(),
                        ogrencibakiye = Convert.ToDouble(reader["ogrencibakiye"].ToString()),
                        kalangiris = Convert.ToInt32(reader["kalangiris"]),
                        ogrenciTip = reader["ogrenciTip"].ToString()
                    };
                }
            }
            return ogrenci;
        }

        public static Ogrenci Getir2(string ogrenciNo)
        {
            _baglanti = Helper.XmlOku()[0];
            Ogrenci ogrenci = new Ogrenci();
            using (SqlConnection conn = new SqlConnection(_baglanti))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM ogrenci where ogrencino='" + ogrenciNo + "'", conn);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    double kalan = 0;
                    kalan = Convert.ToDouble((reader["ogrencibakiye"]));
                    ogrenci = new Ogrenci()
                    {
                        no = reader["ogrencino"].ToString(),
                        TC = reader["TC"].ToString(),
                        adi = reader["ogrenciadi"].ToString(),
                        soyadi = reader["ogrencisoyadi"].ToString(),
                        cinsiyeti = reader["cinsiyeti"].ToString(),
                        sinifi = reader["ogrencisinifi"].ToString(),
                        subesi = reader["ogrencisubesi"].ToString(),
                        alani = reader["ogrencialani"].ToString(),                        
                        ogretmeni = reader["sinifogretmeni"].ToString(),
                        anneadi = reader["anneadi"].ToString(),
                        babaadi = reader["babaadi"].ToString(),
                        velitel = reader["velitel"].ToString(),
                        telefonu = reader["ogrencitel"].ToString(),
                        adresi = reader["adres"].ToString(),
                        kartno = reader["parmakizi"].ToString(),
                        durumu = reader["ogrencidurum"].ToString(),
                        ogrencibakiye = Convert.ToDouble(reader["ogrencibakiye"]),
                        kalangiris = Convert.ToInt32(reader["kalangiris"]),
                        ogrenciTip = reader["ogrenciTip"].ToString()
                    };
                }
            }
            return ogrenci;
        }

        public void Guncelle(Ogrenci ogrenci)
        {
            using (SqlConnection conn = new SqlConnection(_baglanti))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("Update ogrenci set ogrencino=@ogrencino,TC=@TC, ogrenciadi=@ogrenciadi, ogrencisoyadi=@ogrencisoyadi, cinsiyeti=@cinsiyeti, ogrencisinifi=@ogrencisinifi, ogrencialani=@ogrencialani, ogrencisubesi=@ogrencisubesi, sinifogretmeni=@sinifogretmeni, ogrencidurum=@ogrencidurum, parmakizi=@parmakizi, anneadi=@anneadi, babaadi=@babaadi, ogrencitel=@ogrencitel, velitel=@velitel, adres=@adres, ogrencibakiye=@ogrencibakiye, kalangiris=@kalangiris, ogrencitip=@ogrencitip WHERE ogrencino='"+ogrenci.no+"'", conn);
                command.Parameters.AddWithValue("ogrencino", ogrenci.no);
                command.Parameters.AddWithValue("TC", ogrenci.TC);
                command.Parameters.AddWithValue("ogrenciadi", ogrenci.adi);
                command.Parameters.AddWithValue("ogrencisoyadi", ogrenci.soyadi);
                command.Parameters.AddWithValue("cinsiyeti", ogrenci.cinsiyeti);
                command.Parameters.AddWithValue("ogrencisinifi", ogrenci.sinifi);
                command.Parameters.AddWithValue("ogrencisubesi", ogrenci.subesi);
                command.Parameters.AddWithValue("ogrencialani", ogrenci.alani);
                command.Parameters.AddWithValue("sinifogretmeni", ogrenci.ogretmeni);
                command.Parameters.AddWithValue("anneadi", ogrenci.anneadi);
                command.Parameters.AddWithValue("babaadi", ogrenci.babaadi);
                command.Parameters.AddWithValue("velitel", ogrenci.velitel);
                command.Parameters.AddWithValue("ogrencitel", ogrenci.telefonu);
                command.Parameters.AddWithValue("adres", ogrenci.adresi);
                command.Parameters.AddWithValue("parmakizi", ogrenci.kartno);
                command.Parameters.AddWithValue("ogrencidurum", ogrenci.durumu);
                command.Parameters.AddWithValue("ogrencibakiye", ogrenci.ogrencibakiye);
                command.Parameters.AddWithValue("kalangiris", ogrenci.kalangiris);
                command.Parameters.AddWithValue("ogrencitip", ogrenci.ogrenciTip);
                command.ExecuteNonQuery();
            }
        }


    }
}
