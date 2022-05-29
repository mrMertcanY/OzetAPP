using OzetAPP.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OzetAPP.Controllers
{
    public class CategoryController : Controller
    {
        OzetAppDBEntities2 db = new OzetAppDBEntities2();
        // GET: Category
        [Authorize]
        public ActionResult Index()
        {
            var id = int.Parse(Session["userId"].ToString());
            var values = db.tbl_kategoriler.Where(x => x.kategori_kullanici == id).ToList();
            return View(values);
        }

        [HttpPost]
        public ActionResult KategoriEkle(tbl_kategoriler p)
        {
            var id = int.Parse(Session["userId"].ToString());
            var values = db.tbl_kategoriler.Where(x => x.kategori_kullanici == id).ToList();
            var cat = db.tbl_kategoriler.FirstOrDefault(x => x.kategori_ad == p.kategori_ad);

            if (cat != null) ModelState.AddModelError("exist", "Bu Kategori Daha Önce Eklenmiş");

            p.kategori_kullanici = id;
            if (ModelState.IsValid)
            {
                ViewData["basarili"] = "basarili";
                db.tbl_kategoriler.Add(p);
                db.SaveChanges();
                return View("Index", values);
            }
            ViewData["hata"] = "hata";
            return View("Index", values);
        }

        [HttpPost]
        public ActionResult KategoriGuncelle(tbl_kategoriler p)
        {
            var id = int.Parse(Session["userId"].ToString());
            var values = db.tbl_kategoriler.Where(x => x.kategori_kullanici == id).ToList();
            var category = db.tbl_kategoriler.Find(p.kategori_id);
            category.kategori_ad = p.kategori_ad;
            category.kategori_tuketimtur = p.kategori_tuketimtur;
            var cat = db.tbl_kategoriler.FirstOrDefault(x => x.kategori_ad == p.kategori_ad);

            if (cat != null) ModelState.AddModelError("exist", "Bu Kategori Daha Önce Eklenmiş");

            if (ModelState.IsValid)
            {
                ViewData["basarili"] = "basarili";
                db.SaveChanges();
                return View("Index", values);
            }
            ViewData["hata"] = "hata";
            return View("Index", values);
        }

        [HttpPost]
        public ActionResult KategoriSil(int p)
        {
            
                var id = int.Parse(Session["userId"].ToString());
                var category = db.tbl_kategoriler.Find(p);
                var values = db.tbl_kategoriler.Where(x => x.kategori_kullanici == id).ToList();
                var islem = db.tbl_islemler.Where(x => x.islem_kategori == p).ToList();

                islem.ForEach(item =>
                {
                    item.islem_kategori = p;
                    item.islem_islem = true;
                    item.islem_tutar = "11";
                });

                db.tbl_kategoriler.Remove(category);
                db.SaveChanges();

                ViewData["basarili"] = "basarili";
                return View("Index", values);
           
           

        }
    }
}