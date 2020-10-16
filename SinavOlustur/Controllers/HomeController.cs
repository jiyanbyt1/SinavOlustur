using System;
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

        [HttpPost]
        public JsonResult Giris1(string email, string pass)
        {
            List<Login> LoginList = dbCon.Query<Login>("select * from Login").ToList();
            Boolean sonuc;
            var data = LoginList.Where<Login>(item => item.Email == email & item.Sifre==pass).FirstOrDefault();
            if (data != null)
            {
                sonuc = true;
                Fonksiyonlar.GelenYetki = data.Yetki.ToString();
            }
            else
            {
                sonuc = false;
            }

            return Json(sonuc);
        }    
        public IActionResult Giris()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
