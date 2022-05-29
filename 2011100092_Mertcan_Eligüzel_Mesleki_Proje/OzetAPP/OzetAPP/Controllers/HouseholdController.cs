using OzetAPP.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OzetAPP.Controllers
{
    public class HouseholdController : Controller
    {
        OzetAppDBEntities2 db = new OzetAppDBEntities2();
        // GET: Household
        [Authorize]
        public ActionResult Index()
        {
            var id = int.Parse(Session["userId"].ToString());

            var bireysel = db.tbl_kullanicilar.FirstOrDefault(x => x.kullanici_id == id);

            var hane = db.tbl_haneler.FirstOrDefault(x => x.hane_id == bireysel.kullanici_hane);

            var values = db.tbl_kullanicilar.Where(x => x.kullanici_hane == bireysel.kullanici_hane).ToList();
            ViewData["ownerid"] = hane.hane_sahip;

            return View(values);
        }

        [HttpPost]
        public ActionResult HaneAt(int p)
        {

            var id = int.Parse(Session["userId"].ToString());

            if (int.Parse(p.ToString()) == int.Parse(id.ToString())) ModelState.AddModelError("self", "Kendini Silemez");

            var bireysel = db.tbl_kullanicilar.FirstOrDefault(x => x.kullanici_id == id);

            var hane = db.tbl_haneler.FirstOrDefault(x => x.hane_id == bireysel.kullanici_hane);

            if (int.Parse(hane.hane_sahip.ToString()) != int.Parse(id.ToString())) ModelState.AddModelError("perm", "Yetkisiz İşlem");

            var values = db.tbl_kullanicilar.Where(x => x.kullanici_hane == bireysel.kullanici_hane).ToList();

            var deleteuser = db.tbl_kullanicilar.Find(p);

            var islem = db.tbl_islemler.Where(x => x.islem_kullanici == deleteuser.kullanici_id && ((DateTime)x.islem_tarih).Month == DateTime.Now.Month && ((DateTime)x.islem_tarih).Year == DateTime.Now.Year).ToList();
            islem.ForEach(item =>
            {
                item.islem_islem = true; item.islem_tutar = "";
                hane.hane_bakiye = item.islem_tip == true ? (hane.hane_bakiye + (decimal.Parse(item.islem_deger1.ToString()))) : (hane.hane_bakiye - (decimal.Parse(item.islem_deger1.ToString())));
                db.tbl_islemler.Remove(item);
            });

            deleteuser.kullanici_hane = db.tbl_haneler.FirstOrDefault(x => x.hane_sahip == deleteuser.kullanici_id).hane_id;

            ViewData["basarili"] = "basarili";
            ViewData["ownerid"] = hane.hane_sahip;
            db.SaveChanges();
            return View("Index", values);
        }
    }
}