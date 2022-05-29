using OzetAPP.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OzetAPP.Controllers
{
    public class ProfileController : Controller
    {
        OzetAppDBEntities2 db = new OzetAppDBEntities2();
        // GET: Profile
        [Authorize]
        public ActionResult Index()
        {
            var id = int.Parse(Session["userId"].ToString());
            var kullanici = db.tbl_kullanicilar.FirstOrDefault(x => x.kullanici_id == id);
            ViewData["isim"] = kullanici.kullanici_isim;
            ViewData["kullaniciad"] = kullanici.kullanici_ad;
            ViewData["sifre"] = kullanici.kullanici_sifre;
            return View();
        }

        [HttpPost]
        public ActionResult Index(tbl_kullanicilar p)
        {

            var id = int.Parse(Session["userId"].ToString());
            var kullanici = db.tbl_kullanicilar.FirstOrDefault(x => x.kullanici_id == id);
            p.kullanici_ad = kullanici.kullanici_ad;
            try
            {
                if (p.kullanici_sifre != p.kullanici_sifretekrar) throw new Exception("şifre geçersiz");


                if (Request.Files.Count > 0)
                {
                    string uzanti = Path.GetExtension(Request.Files[0].FileName);
                    string dosyaadi = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + "" + DateTime.Now.Millisecond + "" + Path.GetFileName(Request.Files[0].FileName);
                    if ((DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + "" + DateTime.Now.Millisecond) != dosyaadi)
                    {
                        string fullpath = Request.MapPath("~" + kullanici.kullanici_fotograf);
                        if (System.IO.File.Exists(fullpath) && kullanici.kullanici_fotograf != "/userimages/default.jpg") System.IO.File.Delete(fullpath);


                        string yol = "~/userimages/" + dosyaadi + uzanti;

                        Request.Files[0].SaveAs(Server.MapPath(yol));
                        p.kullanici_fotograf = "/userimages/" + dosyaadi + uzanti;
                        kullanici.kullanici_fotograf = p.kullanici_fotograf;
                    }

                }

                kullanici.kullanici_isim = p.kullanici_isim;
                kullanici.kullanici_sifre = p.kullanici_sifre;


                db.SaveChanges();
                ViewData["isim"] = kullanici.kullanici_isim;
                ViewData["kullaniciad"] = kullanici.kullanici_ad;
                ViewData["sifre"] = kullanici.kullanici_sifre;
                Session["userPhoto"] = kullanici.kullanici_fotograf;
                ViewData["basarili"] = "basarili";
                return View();
            }


            catch (Exception e)
            {
                ViewData["hata"] = "hata";
                return View("Index");
            }
        }
    }
}