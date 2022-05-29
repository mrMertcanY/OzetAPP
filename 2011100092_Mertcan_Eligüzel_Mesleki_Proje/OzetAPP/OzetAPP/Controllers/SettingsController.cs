using OzetAPP.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OzetAPP.Controllers
{
    public class SettingsController : Controller
    {
        OzetAppDBEntities2 db = new OzetAppDBEntities2();
        // GET: Settings
        [Authorize]
        public ActionResult Index()
        {
            var id = int.Parse(Session["userId"].ToString());
            var bireysel = db.tbl_kullanicilar.Find(id);
            ViewData["hanekod"] = bireysel.tbl_haneler.hane_kod;
            ViewData["hanesahip"] = bireysel.tbl_haneler.hane_sahip != id ? null : "owner";
            return View();
        }

        [HttpPost]
        public ActionResult HaneKatil(string p)
        {
            var id = int.Parse(Session["userId"].ToString());
            var bireysel = db.tbl_kullanicilar.Find(id);
            var hane = db.tbl_haneler.FirstOrDefault(x=>x.hane_kod == p);
            var eskihane = db.tbl_haneler.FirstOrDefault(x => x.hane_sahip == id);
            bool rejoin = hane.hane_sahip == id ? true : false;
            if(hane != null && rejoin == false)
            {
                if(eskihane != null)
                {
                    var islem = db.tbl_islemler.Where(x => x.islem_kullanici == bireysel.kullanici_id && ((DateTime)x.islem_tarih).Month == DateTime.Now.Month && ((DateTime)x.islem_tarih).Year == DateTime.Now.Year).ToList();
                    islem.ForEach(item =>
                    {
                        item.islem_islem = true; item.islem_tutar = "";
                        eskihane.hane_bakiye = item.islem_tip == true ? (eskihane.hane_bakiye + (decimal.Parse(item.islem_deger1.ToString()))) : (eskihane.hane_bakiye - (decimal.Parse(item.islem_deger1.ToString())));
                        db.tbl_islemler.Remove(item);
                    });
                }
                ViewData["basarili"] = "basarili";
                bireysel.kullanici_hane = hane.hane_id;
                db.SaveChanges();
            }
            else
            {
                ViewData["hata"] = "hata";
            }
            return View("Index");
        }

        public ActionResult HaneYenile()
        {
            var id = int.Parse(Session["userId"].ToString());
            var bireysel = db.tbl_kullanicilar.Find(id);
            var hane = db.tbl_haneler.FirstOrDefault(x => x.hane_id == bireysel.kullanici_hane);
            string letter = "qwertyuıopasdfghjklzxcvbnm0123456789";
            string code = ""; int a;
            Random rnd = new Random();
            for (; ; )
            {
                code = "";
                for (int i = 0; i < 6; i++)
                {
                    a = rnd.Next(0, letter.Length);
                    code += letter[a].ToString().ToUpper();
                }
                var haned = db.tbl_haneler.FirstOrDefault(x => x.hane_kod == code);
                if (haned == null) break;
            }
            hane.hane_kod = code;
            db.SaveChanges();
            ViewData["basarili"] = "basarili";
            return View("Index");
        }

        public ActionResult HaneAyril()
        {
            var id = int.Parse(Session["userId"].ToString());
            var bireysel = db.tbl_kullanicilar.Find(id);
            var hane = db.tbl_haneler.FirstOrDefault(x => x.hane_id == bireysel.kullanici_hane);

            var islem = db.tbl_islemler.Where(x => x.islem_kullanici == bireysel.kullanici_id && ((DateTime)x.islem_tarih).Month == DateTime.Now.Month && ((DateTime)x.islem_tarih).Year == DateTime.Now.Year).ToList();
            islem.ForEach(item =>
            {
                item.islem_islem = true; item.islem_tutar = "";
                hane.hane_bakiye = item.islem_tip == true ? (hane.hane_bakiye + (decimal.Parse(item.islem_deger1.ToString()))) : (hane.hane_bakiye - (decimal.Parse(item.islem_deger1.ToString())));
                db.tbl_islemler.Remove(item);
            });
            bireysel.kullanici_hane = db.tbl_haneler.FirstOrDefault(x => x.hane_sahip == bireysel.kullanici_id).hane_id;
            db.SaveChanges();
            ViewData["basarili"] = "basarili";
            return View("Index");
        }

    }


}