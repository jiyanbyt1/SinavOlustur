﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SinavOlustur.Models;
using Newtonsoft.Json;
using Dapper;
using SQLite;
using System.Data;
using System.Drawing;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Security.Policy;

namespace SinavOlustur.Controllers
{
    public class HomeController : Controller
    {
        public static SQLiteConnection dbCon = new SQLiteConnection(Startup.DatabaseLocation);
 
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        List<string> liste = new List<string>();
        public IActionResult Index()
        {
            Fonksiyonlar.BaslikListe.Clear();
            Fonksiyonlar.LinkListe.Clear();
            string url = "https://www.wired.com";
            WebClient client = new WebClient();
            string html = client.DownloadString(url);
            HtmlAgilityPack.HtmlDocument dokuman = new HtmlAgilityPack.HtmlDocument();
            dokuman.LoadHtml(html);
            HtmlNodeCollection baslıklar1 = dokuman.DocumentNode.SelectNodes("/html/body/div[3]/div/div[3]/div/div/div[2]/div[1]/div/div[1]/div[1]/div[1]/div/ul/li[2]/a[1]");
            HtmlNodeCollection baslıklar2 = dokuman.DocumentNode.SelectNodes("/html/body/div[3]/div/div[3]/div/div/div[2]/div[1]/div/div[1]/div[1]/div[2]/div/ul/li[2]/a[1]");
            HtmlNodeCollection baslıklar3 = dokuman.DocumentNode.SelectNodes("/html/body/div[3]/div/div[3]/div/div/div[2]/div[1]/div/div[1]/div[2]/div[1]/div[1]/div/ul/li[2]/a[1]");
            HtmlNodeCollection baslıklar4 = dokuman.DocumentNode.SelectNodes("/html/body/div[3]/div/div[3]/div/div/div[2]/div[1]/div/div[1]/div[2]/div[1]/div[2]/div/ul/li[2]/a[2]");
            HtmlNodeCollection baslıklar5 = dokuman.DocumentNode.SelectNodes("/html/body/div[3]/div/div[3]/div/div/div[2]/div[1]/div/div[1]/div[2]/div[2]/div/ul/li[2]/a[1]");
            //başlıklar
            HtmlNodeCollection baslıklar6 = dokuman.DocumentNode.SelectNodes("/html/body/div[3]/div/div[3]/div/div/div[2]/div[1]/div/div[1]/div[1]/div[1]/div/ul/li[2]/a[2]/h2");
            HtmlNodeCollection baslıklar7 = dokuman.DocumentNode.SelectNodes("/html/body/div[3]/div/div[3]/div/div/div[2]/div[1]/div/div[1]/div[1]/div[2]/div/ul/li[2]/a[2]/h2");
            HtmlNodeCollection baslıklar8 = dokuman.DocumentNode.SelectNodes("/html/body/div[3]/div/div[3]/div/div/div[2]/div[1]/div/div[1]/div[2]/div[1]/div[1]/div/ul/li[2]/a[2]/h2");
            HtmlNodeCollection baslıklar9 = dokuman.DocumentNode.SelectNodes("/html/body/div[3]/div/div[3]/div/div/div[2]/div[1]/div/div[1]/div[2]/div[1]/div[2]/div/ul/li[2]/a[2]/h2");
            HtmlNodeCollection baslıklar10 = dokuman.DocumentNode.SelectNodes("/html/body/div[3]/div/div[3]/div/div/div[2]/div[1]/div/div[1]/div[2]/div[2]/div/ul/li[2]/a[2]/h2");

            foreach (var baslik in baslıklar1)
            {
                string link = baslik.Attributes["href"].Value;
                Fonksiyonlar.LinkListe.Add(link);
            }
            foreach (var baslik in baslıklar2)
            {
                string link = baslik.Attributes["href"].Value;
                Fonksiyonlar.LinkListe.Add(link);
            }
            foreach (var baslik in baslıklar3)
            {
                string link = baslik.Attributes["href"].Value;
                Fonksiyonlar.LinkListe.Add(link);
            }
            foreach (var baslik in baslıklar4)
            {
                string link = baslik.Attributes["href"].Value;
                Fonksiyonlar.LinkListe.Add(link);
            }
            foreach (var baslik in baslıklar5)
            {
                string link = baslik.Attributes["href"].Value;
                Fonksiyonlar.LinkListe.Add(link);
            }
            foreach (var baslik in baslıklar6)
            {
                Fonksiyonlar.BaslikListe.Add(baslik.InnerText);
            }
            foreach (var baslik in baslıklar7)
            {
                Fonksiyonlar.BaslikListe.Add(baslik.InnerText);
            }
            foreach (var baslik in baslıklar8)
            {
                Fonksiyonlar.BaslikListe.Add(baslik.InnerText);
            }
            foreach (var baslik in baslıklar9)
            {
                Fonksiyonlar.BaslikListe.Add(baslik.InnerText);
            }
            foreach (var baslik in baslıklar10)
            {
                Fonksiyonlar.BaslikListe.Add(baslik.InnerText);
            }

            return View();
        }
        public class Login
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Sifre { get; set; }
            public string Yetki { get; set; }
        }
        public class Sinav
        {
            public int SinavID { get; set; }
            public int Sayi { get; set; }
        }
        public class Sorular
        {
            public int SorularID { get; set; }
            public int SinavID { get; set; }
            public int Kullanici { get; set; }
            public string SoruAdi { get; set; }
            
            public string SoruBasligi { get; set; }
            public string SoruMetni { get; set; }
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
            public string D { get; set; }
            public string DogruCevap { get; set; }
        }
        public class Sinavlar
        {
            public int SinavID { get; set; }
            public int SinavNo { get; set; }
            public int KullaniciID { get; set; }
            public string SinavBasligi { get; set; }
            public string Tarih { get; set; }
        }

        [HttpPost]
        public JsonResult Giris1(string email, string pass)
        {
            List<Login> LoginList = dbCon.Query<Login>("select * from Login").ToList();
            int sonuc=0;
            var data = LoginList.Where<Login>(item => item.Email == email & item.Sifre==pass).FirstOrDefault();
            if (data != null)
            {
                if (data.Yetki.ToString() == "Yönetici")
                {
                    sonuc = 1;
                    Fonksiyonlar.GelenID = data.ID;
                    Fonksiyonlar.AdSoyad = data.Name.ToString();
                }
                else if(data.Yetki.ToString()== "Kullanıcı")
                {
                    sonuc = 2;
                    Fonksiyonlar.GelenID = data.ID;
                    Fonksiyonlar.AdSoyad = data.Name.ToString();
                }
                else
                {
                    sonuc = 3;
                    Fonksiyonlar.GelenID = data.ID;
                    Fonksiyonlar.AdSoyad = data.Name.ToString();
                }
 
            }
            else
            {
                sonuc = 0;
            }

            return Json(sonuc);
        }    

        public IActionResult Yonetim()
        {
            List<Login> SinavlarList = dbCon.Query<Login>("select * from Login where Yetki='Yönetici' or Yetki='Kullanıcı' ").ToList();
            ViewBag.Liste = SinavlarList;
            return View();
        }

        public JsonResult KulSil(int id)
        {
            Boolean Mesaj = false;

            string query = "delete from Login where ID='" + id + "'";
            dbCon.Execute(query);
            Mesaj = true;
            return Json(Mesaj);
        }
        public IActionResult KulGuncelle(int id,string name,string email,string yetki, string sifre)
        {
            

            string query = "UPDATE Login SET Name='"+name+"', Email='"+email+"', Sifre='"+sifre+"',Yetki='"+yetki+"' where ID='"+id+"'";
            dbCon.Execute(query);
            
            return Redirect("/Home/Yonetim");
        }

        public IActionResult KulEkle(string name, string email, string yetki, string sifre)
        {


            string query = "INSERT into Login(Name,Email,Sifre,Yetki) VALUES ('"+name+"','"+email+"','"+sifre+"','"+yetki+"')";
            dbCon.Execute(query);

            return Redirect("/Home/Yonetim");
        }

        public IActionResult Giris()
        {
            return View();
        }

        public IActionResult Kayit(string name,string email,string sifre, string yetki)
        {
            string query = "insert into Login(Name,Email,Sifre,Yetki) values('"+name+"','"+email+"','"+sifre+"','"+yetki+"')";
            dbCon.Execute(query);

            return Redirect("/Home/Giris");
        }



        [HttpPost]
        public JsonResult SinavMetni(string baslik, string Index)
        {
            Boolean sonuc = false;
            int deger=0 ;
            if (Index == "1")
            {
                deger = 0;
            }
            else if(Index == "2")
            {
                deger = 1;
            }
            else if (Index == "3")
            {
                deger = 2;
            }
            else if (Index == "4")
            {
                deger = 3;
            }
            else if (Index == "5")
            {
                deger = 4;
            }

            string url = "https://www.wired.com"+Fonksiyonlar.LinkListe[deger];
            WebClient client = new WebClient();
            string html = client.DownloadString(url);
            HtmlAgilityPack.HtmlDocument dokuman = new HtmlAgilityPack.HtmlDocument();
            dokuman.LoadHtml(html);
            HtmlNodeCollection GelenYazi1 = dokuman.DocumentNode.SelectNodes("/html/body/div[1]/div/main/article/div[2]/div/div[1]/div[1]/div[1]");
            HtmlNodeCollection GelenYazi2 = dokuman.DocumentNode.SelectNodes("/html/body/div[1]/div/main/article/div[2]/div/div[1]/div/div[1]");
            HtmlNodeCollection GelenYazi3 = dokuman.DocumentNode.SelectNodes("/html/body/div[1]/div/main/article/div[2]/div/div[1]/div/div[1]");
            HtmlNodeCollection GelenYazi4 = dokuman.DocumentNode.SelectNodes("/html/body/div[1]/div/main/article/div[2]/div/div[1]/div/div[1]");
            HtmlNodeCollection GelenYazi5 = dokuman.DocumentNode.SelectNodes("/html/body/div[1]/div/main/article/div[2]/div/div[1]/div/div[1]");
            Fonksiyonlar.YaziListe.Clear();
            if (Index == "1")
            {
                foreach (var Yazi in GelenYazi1)
                {
                    Fonksiyonlar.YaziListe.Add(Yazi.InnerText);
                }
            }
            else if (Index == "2")
            {
                foreach (var Yazi in GelenYazi2)
                {
                    Fonksiyonlar.YaziListe.Add(Yazi.InnerText);
                }
            }
            else if (Index == "3")
            {
                foreach (var Yazi in GelenYazi3)
                {
                    Fonksiyonlar.YaziListe.Add(Yazi.InnerText);
                }
            }
            else if (Index == "4")
            {
                foreach (var Yazi in GelenYazi4)
                {
                    Fonksiyonlar.YaziListe.Add(Yazi.InnerText);
                }
            }
            else if (Index == "5")
            {
                foreach (var Yazi in GelenYazi5)
                {
                    Fonksiyonlar.YaziListe.Add(Yazi.InnerText);
                }
            }



            return Json(Fonksiyonlar.YaziListe);
        }




        public IActionResult SinavlarListesi()
        {
            List<Sinavlar> SinavlarList = dbCon.Query<Sinavlar>("select * from Sinavlar where KullaniciID='"+Fonksiyonlar.GelenID+"'").ToList();
            ViewBag.Liste = SinavlarList;
            return View();
        }

        public JsonResult SOlustur(string baslik,string Yazi,string Soru1, string Soru2, string Soru3, string Soru4,string A1, string A2, string A3, string A4, string B1, string B2, string B3, string B4, string C1, string C2, string C3, string C4, string D1, string D2, string D3, string D4,string Cevap1, string Cevap2, string Cevap3, string Cevap4)
        {

            
            Boolean mesaj = false;
            List<Sinav> SinavList = dbCon.Query<Sinav>("select * from Sinav").ToList();
            var Sayi = SinavList[0].Sayi;
            string query1 = "insert into Sorular(SinavID,Kullanici,SoruAdi,SoruBasligi,SoruMetni,A,B,C,D,DogruCevap) values('" + Sayi + "','" + Fonksiyonlar.GelenID + "','" + Soru1 + "','" + baslik + "','" + Yazi + "','" + A1 + "','" + B1 + "','" + C1 + "','" + D1 + "','" + Cevap1 + "')";
            string query2 = "insert into Sorular(SinavID,Kullanici,SoruAdi,SoruBasligi,SoruMetni,A,B,C,D,DogruCevap) values('" + Sayi + "','" + Fonksiyonlar.GelenID + "','" + Soru2 + "','" + baslik + "','" + Yazi + "','" + A2 + "','" + B2 + "','" + C2 + "','" + D2 + "','" + Cevap2 + "')";
            string query3 = "insert into Sorular(SinavID,Kullanici,SoruAdi,SoruBasligi,SoruMetni,A,B,C,D,DogruCevap) values('" + Sayi + "','" + Fonksiyonlar.GelenID + "','" + Soru3 + "','" + baslik + "','" + Yazi + "','" + A3 + "','" + B3 + "','" + C3 + "','" + D3 + "','" + Cevap3 + "')";
            string query4 = "insert into Sorular(SinavID,Kullanici,SoruAdi,SoruBasligi,SoruMetni,A,B,C,D,DogruCevap) values('" + Sayi + "','" + Fonksiyonlar.GelenID + "','" + Soru4 + "','" + baslik + "','" + Yazi + "','" + A4 + "','" + B4 + "','" + C4 + "','" + D4 + "','" + Cevap4 + "')";
            dbCon.Execute(query1);
            dbCon.Execute(query2);
            dbCon.Execute(query3);
            dbCon.Execute(query4);

            List<Sorular> SinavlarLis1t = dbCon.Query<Sorular>("select * from Sorular").ToList();

            string querySinavlar = "insert into Sinavlar(SinavNo,KullaniciID,SinavBasligi,Tarih) values('" + Sayi + "','" + Fonksiyonlar.GelenID + "','" + baslik + "','" + DateTime.Now + "') ";
            dbCon.Execute(querySinavlar);

            Sayi = Sayi + 1;
            string queryupdate = "update Sinav Set Sayi='" + Sayi + "' where SinavID=1";
            dbCon.Execute(queryupdate);

            mesaj = true;

            return Json(mesaj);

        }
        [HttpPost]
        public JsonResult SinavSil(int id,int sinavno)
        {
            Boolean Mesaj = false;

            string query = "delete from Sinavlar where SinavID='" + id + "'";
            dbCon.Execute(query);

            string query2 = "delete from Sorular where SinavID='" + sinavno + "'";
            dbCon.Execute(query2);
            Mesaj = true;
            return Json(Mesaj);
        }


        //Kullanıcı İşlemleri için;
        public IActionResult KulSinavListesi()
        {
            List<Sinavlar> SinavlarList = dbCon.Query<Sinavlar>("select * from Sinavlar").ToList();
            ViewBag.Liste = SinavlarList;
            return View();
        }
        public JsonResult SinavaGiris(int sinavno)
        {
            Boolean Mesaj = false;
            Fonksiyonlar.SinavGirisID = sinavno;
            Mesaj = true;
            return Json(Mesaj);
        }
        public IActionResult SinavBaslat()
        {
            var data = dbCon.Query<Sorular>("select * from Sorular where SinavID='" + Fonksiyonlar.SinavGirisID + "'").FirstOrDefault();// Tek satır
            ViewBag.Baslik = data.SoruBasligi;
            ViewBag.Yazi = data.SoruMetni;
            List<Sorular> SinavlarList = dbCon.Query<Sorular>("select * from Sorular where SinavID='" + Fonksiyonlar.SinavGirisID + "'").ToList();

            ViewBag.Soru1 = SinavlarList[0].SoruAdi;
            ViewBag.Soru2 = SinavlarList[1].SoruAdi;
            ViewBag.Soru3 = SinavlarList[2].SoruAdi;
            ViewBag.Soru4 = SinavlarList[3].SoruAdi;

            ViewBag.A1 = SinavlarList[0].A;
            ViewBag.B1 = SinavlarList[0].B;
            ViewBag.C1 = SinavlarList[0].C;
            ViewBag.D1 = SinavlarList[0].D;

            ViewBag.A2 = SinavlarList[1].A;
            ViewBag.B2 = SinavlarList[1].B;
            ViewBag.C2 = SinavlarList[1].C;
            ViewBag.D2 = SinavlarList[1].D;

            ViewBag.A3 = SinavlarList[2].A;
            ViewBag.B3 = SinavlarList[2].B;
            ViewBag.C3 = SinavlarList[2].C;
            ViewBag.D3 = SinavlarList[2].D;

            ViewBag.A4 = SinavlarList[3].A;
            ViewBag.B4 = SinavlarList[3].B;
            ViewBag.C4 = SinavlarList[3].C;
            ViewBag.D4 = SinavlarList[3].D;

            Fonksiyonlar.Cevap1 = SinavlarList[0].DogruCevap;
            Fonksiyonlar.Cevap2 = SinavlarList[1].DogruCevap;
            Fonksiyonlar.Cevap3 = SinavlarList[2].DogruCevap;
            Fonksiyonlar.Cevap4 = SinavlarList[3].DogruCevap;
            return View();
        }
        public JsonResult SinaviTamamla(string S1, string S2, string S3, string S4)
        {
            Boolean mesaj = false;
            
            if(S1!=null || S2!=null || S3!=null || S4 != null)
            {
                mesaj = true;
                if (S1 == Fonksiyonlar.Cevap1)
                {
                    Fonksiyonlar.Sonuc1 = "T";
                    if (S1 == "A") { Fonksiyonlar.RenkS1A = "green"; }
                    else if (S1 == "B") { Fonksiyonlar.RenkS1B = "green"; }
                    else if (S1 == "C") { Fonksiyonlar.RenkS1C = "green"; }
                    else if(S1=="D"){ Fonksiyonlar.RenkS1D = "green"; }
                }
                else
                {
                    Fonksiyonlar.Sonuc1 = "F";
                    if (S1 == "A") { Fonksiyonlar.RenkS1A = "red"; }
                    else if (S1 == "B") { Fonksiyonlar.RenkS1B = "red"; }
                    else if (S1 == "C") { Fonksiyonlar.RenkS1C = "red"; }
                    else if (S1=="D") { Fonksiyonlar.RenkS1D = "red"; }

                    if (Fonksiyonlar.Cevap1 == "A") { Fonksiyonlar.RenkS1A = "green"; }
                    else if (Fonksiyonlar.Cevap1 == "B") { Fonksiyonlar.RenkS1B = "green"; }
                    else if (Fonksiyonlar.Cevap1 == "C") { Fonksiyonlar.RenkS1C = "green"; }
                    else if (Fonksiyonlar.Cevap1 == "D") { Fonksiyonlar.RenkS1D = "green"; }
                }

                if (S2 == Fonksiyonlar.Cevap2)
                {
                    Fonksiyonlar.Sonuc2 = "T";
                    if (S2 == "A") { Fonksiyonlar.RenkS2A = "green"; }
                    else if (S2 == "B") { Fonksiyonlar.RenkS2B = "green"; }
                    else if (S2 == "C") { Fonksiyonlar.RenkS2C = "green"; }
                    else if(S2=="D"){ Fonksiyonlar.RenkS2D = "green"; }
                }
                else
                {
                    Fonksiyonlar.Sonuc2 = "F";
                    if (S2 == "A") { Fonksiyonlar.RenkS2A = "red"; }
                    else if (S2 == "B") { Fonksiyonlar.RenkS2B = "red"; }
                    else if (S2 == "C") { Fonksiyonlar.RenkS2C = "red"; }
                    else if(S2=="D"){ Fonksiyonlar.RenkS2D = "red"; }

                    if (Fonksiyonlar.Cevap2 == "A") { Fonksiyonlar.RenkS2A = "green"; }
                    else if (Fonksiyonlar.Cevap2 == "B") { Fonksiyonlar.RenkS2B = "green"; }
                    else if (Fonksiyonlar.Cevap2 == "C") { Fonksiyonlar.RenkS2C = "green"; }
                    else if (Fonksiyonlar.Cevap2 == "D") { Fonksiyonlar.RenkS2D = "green"; }
                }


                if (S3 == Fonksiyonlar.Cevap3)
                {
                    Fonksiyonlar.Sonuc3 = "T";
                    if (S3 == "A") { Fonksiyonlar.RenkS3A = "green"; }
                    else if (S3 == "B") { Fonksiyonlar.RenkS3B = "green"; }
                    else if (S3 == "C") { Fonksiyonlar.RenkS3C = "green"; }
                    else if (S3=="D"){ Fonksiyonlar.RenkS3D = "green"; }
                }
                else
                {
                    Fonksiyonlar.Sonuc3 = "F";
                    if (S3 == "A") { Fonksiyonlar.RenkS3A = "red"; }
                    else if (S3 == "B") { Fonksiyonlar.RenkS3B = "red"; }
                    else if (S3 == "C") { Fonksiyonlar.RenkS3C = "red"; }
                    else if(S3=="D") { Fonksiyonlar.RenkS3D = "red"; }

                    if (Fonksiyonlar.Cevap3 == "A") { Fonksiyonlar.RenkS3A = "green"; }
                    else if (Fonksiyonlar.Cevap3 == "B") { Fonksiyonlar.RenkS3B = "green"; }
                    else if (Fonksiyonlar.Cevap3 == "C") { Fonksiyonlar.RenkS3C = "green"; }
                    else if (Fonksiyonlar.Cevap3 == "D") { Fonksiyonlar.RenkS3D = "green"; }
                }


                if (S4 == Fonksiyonlar.Cevap4)
                {
                    Fonksiyonlar.Sonuc4 = "T";
                    if (S4 == "A") { Fonksiyonlar.RenkS4A = "green"; }
                    else if (S4 == "B") { Fonksiyonlar.RenkS4B = "green"; }
                    else if (S4 == "C") { Fonksiyonlar.RenkS4C = "green"; }
                    else if (S4=="D") { Fonksiyonlar.RenkS4D = "green"; }
                }
                else
                {
                    Fonksiyonlar.Sonuc4 = "F";
                    if (S4 == "A") { Fonksiyonlar.RenkS4A = "red"; }
                    else if (S4 == "B") { Fonksiyonlar.RenkS4B = "red"; }
                    else if (S4 == "C") { Fonksiyonlar.RenkS4C = "red"; }
                    else if(S4=="D") { Fonksiyonlar.RenkS4D = "red"; }

                    if (Fonksiyonlar.Cevap4 == "A") { Fonksiyonlar.RenkS4A = "green"; }
                    else if (Fonksiyonlar.Cevap4 == "B") { Fonksiyonlar.RenkS4B = "green"; }
                    else if (Fonksiyonlar.Cevap4 == "C") { Fonksiyonlar.RenkS4C = "green"; }
                    else if(Fonksiyonlar.Cevap4 == "D") { Fonksiyonlar.RenkS4D = "green"; }
                }
            }
            return Json(Fonksiyonlar.Sonuc1+S1+Fonksiyonlar.Cevap1+Fonksiyonlar.Sonuc2+S2+Fonksiyonlar.Cevap2+Fonksiyonlar.Sonuc3+S3+Fonksiyonlar.Cevap3+Fonksiyonlar.Sonuc4+S4+Fonksiyonlar.Cevap4);
        }

        public IActionResult Profil()
        {
            List<Login> LoginList = dbCon.Query<Login>("select * from Login").ToList();
            var data = LoginList.Where<Login>(item => item.ID == Fonksiyonlar.GelenID).FirstOrDefault();
            ViewBag.name = data.Name.ToString();
            ViewBag.email = data.Email.ToString();
            ViewBag.sifre = data.Sifre.ToString();
            return View();
        }
        public IActionResult Profil1(string name, string email, string sifre)
        {
            string query = "update Login set Name='" + name + "', Email='" + email + "', Sifre='" + sifre + "' where ID='" + Fonksiyonlar.GelenID + "'";
            dbCon.Execute(query);
            Fonksiyonlar.AdSoyad = name;
            return Redirect("/Home/Profil");
        }
        public IActionResult KulProfil()
        {
            List<Login> LoginList = dbCon.Query<Login>("select * from Login").ToList();
            var data = LoginList.Where<Login>(item => item.ID == Fonksiyonlar.GelenID).FirstOrDefault();
            ViewBag.name = data.Name.ToString();
            ViewBag.email = data.Email.ToString();
            ViewBag.sifre = data.Sifre.ToString();
            return View();
        }
        public IActionResult KulProfil1(string name, string email,string sifre)
        {
            string query = "update Login set Name='" + name + "', Email='" + email + "', Sifre='" + sifre + "' where ID='" + Fonksiyonlar.GelenID + "'";
            dbCon.Execute(query);
            Fonksiyonlar.AdSoyad = name;
            return Redirect("/Home/KulProfil");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
