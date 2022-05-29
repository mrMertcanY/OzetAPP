using OzetAPP.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OzetAPP.Controllers
{
    public class RegisterController : Controller
    {
        OzetAppDBEntities2 db = new OzetAppDBEntities2();
        // GET: Register
        public ActionResult Index()
        {
            ViewBag.txtisim = "form-control";
            ViewBag.txtkullaniciad = "form-control";
            ViewBag.txtsifre = "form-control";
            ViewBag.txtsifretekrar = "form-control";
            return View();
            
        }
        [HttpPost]
        public ActionResult Index(tbl_kullanicilar p)
        {
            var user = db.tbl_kullanicilar.FirstOrDefault(x => x.kullanici_ad == p.kullanici_ad);
            if (user != null) ModelState.AddModelError("kullanici_ad", "Kullanıcı Adı Alınmış");
            if (p.kullanici_sifre != p.kullanici_sifretekrar) ModelState.AddModelError("kullanici_sifretekrar", "Şifreler Uyuşmuyor");

            ViewBag.txtisim = !ModelState.IsValidField("kullanici_isim") ? "form-control form-control-danger" : "form-control";
            ViewBag.txtkullaniciad = !ModelState.IsValidField("kullanici_ad") ? "form-control form-control-danger" : "form-control"; 
            ViewBag.txtsifre = !ModelState.IsValidField("kullanici_sifre") ? "form-control form-control-danger" : "form-control";
            ViewBag.txtsifretekrar = !ModelState.IsValidField("kullanici_sifretekrar") ? "form-control form-control-danger" : "form-control";
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                tbl_haneler h = new tbl_haneler();
                h.hane_bakiye = decimal.Parse("0.00");
                string letter = "qwertyuıopasdfghjklzxcvbnm0123456789";
                string code="";
                Random rnd = new Random();
                int a;
                for (;;)
                {
                    code = "";
                    for (int i = 0; i < 6; i++)
                    {
                        a = rnd.Next(0, letter.Length);
                        code += letter[a].ToString().ToUpper();
                    }
                    var hane = db.tbl_haneler.FirstOrDefault(x => x.hane_kod == code);
                    if (hane == null) break;
                }
                p.kullanici_bakiye = decimal.Parse("0.00");
                p.kullanici_fotograf = "/userimages/default.jpg";
                db.tbl_kullanicilar.Add(p);
                db.SaveChanges();

                h.hane_kod = code;
                h.hane_durum = true;
                h.hane_bakiye = decimal.Parse("0.00");
                db.tbl_haneler.Add(h);
                db.SaveChanges();

                var fetchuser = db.tbl_kullanicilar.FirstOrDefault(x => x.kullanici_ad == p.kullanici_ad);
                var fetchhane = db.tbl_haneler.FirstOrDefault(x => x.hane_kod == code);
                fetchuser.kullanici_hane = fetchhane.hane_id;

                fetchhane.hane_sahip = fetchuser.kullanici_id;
                db.SaveChanges();

                ViewData["mesaj"] = "başarılı";
                return View();
            }
            
        }
       
    }
}