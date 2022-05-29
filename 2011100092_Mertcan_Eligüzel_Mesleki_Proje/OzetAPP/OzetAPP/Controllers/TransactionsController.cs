using OzetAPP.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OzetAPP.Controllers
{

    public class TransactionsController : Controller
    {
        OzetAppDBEntities2 db = new OzetAppDBEntities2();


        // GET: Transactions
        [Authorize]
        public ActionResult Index()
        {

            var id = int.Parse(Session["userId"].ToString());
            var values = db.tbl_islemler.Where(x => x.islem_kullanici == id).ToList();
            var values2 = db.tbl_kategoriler.Where(x => x.kategori_kullanici == id).ToList();

            return View(Tuple.Create(values, values2));
        }

        [HttpGet]
        public ActionResult IslemEkle()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult IslemEkle(tbl_islemler p)
        {

            var id = int.Parse(Session["userId"].ToString());

            var values = db.tbl_islemler.Where(x => x.islem_kullanici == id).ToList();
            var values2 = db.tbl_kategoriler.Where(x => x.kategori_kullanici == id).ToList();

            var bireysel = db.tbl_kullanicilar.FirstOrDefault(x => x.kullanici_id == id);
            var hane = db.tbl_haneler.FirstOrDefault(x => x.hane_id == bireysel.kullanici_hane);
            p.islem_tip = p.islem_tip == null || p.islem_tip == false ? false : true;
            if (ModelState.IsValid)
            {
                if (p.islem_islem)
                {

                    p.islem_hane = null;
                    p.islem_tarih = DateTime.Now;
                    p.islem_kategori = int.Parse(p.islem_kategori.ToString()) == 0 ? null : p.islem_kategori;
                    p.islem_kullanici = id;

                    if (p.islem_tuketim != null && int.Parse(Math.Floor(decimal.Parse(p.islem_tuketim.ToString())).ToString()) != 0 && p.islem_tip != false) p.islem_deger2 = decimal.Parse(p.islem_tuketim); else p.islem_deger2 = null;
                    p.islem_deger1 = decimal.Parse(p.islem_tutar.ToString());
                    p.islem_bakiye = p.islem_tip == true ? (decimal.Parse(bireysel.kullanici_bakiye.ToString()) - (decimal.Parse(p.islem_tutar.ToString()))) : (bireysel.kullanici_bakiye + (decimal.Parse(p.islem_tutar.ToString())));
                    bireysel.kullanici_bakiye = p.islem_tip == true ? (decimal.Parse(bireysel.kullanici_bakiye.ToString()) - (decimal.Parse(p.islem_tutar.ToString()))) : (bireysel.kullanici_bakiye + (decimal.Parse(p.islem_tutar.ToString())));
                    db.tbl_islemler.Add(p);
                    db.SaveChanges();

                }
                else
                {


                    p.islem_hane = hane.hane_id;
                    p.islem_tarih = DateTime.Now;
                    p.islem_kullanici = id;
                    p.islem_kategori = int.Parse(p.islem_kategori.ToString()) == 0 ? null : p.islem_kategori;

                    if (p.islem_tuketim != null && int.Parse(Math.Floor(decimal.Parse(p.islem_tuketim.ToString())).ToString()) != 0 && p.islem_tip != false) p.islem_deger2 = decimal.Parse(p.islem_tuketim); else p.islem_deger2 = null;
                    p.islem_deger1 = decimal.Parse(p.islem_tutar.ToString());
                    p.islem_bakiye = p.islem_tip == true ? (hane.hane_bakiye - (decimal.Parse(p.islem_tutar.ToString()))) : (hane.hane_bakiye + (decimal.Parse(p.islem_tutar.ToString())));
                    hane.hane_bakiye = p.islem_tip == true ? (hane.hane_bakiye - (decimal.Parse(p.islem_tutar.ToString()))) : (hane.hane_bakiye + (decimal.Parse(p.islem_tutar.ToString())));
                    db.tbl_islemler.Add(p);
                    db.SaveChanges();
                }
                ViewData["basarili"] = "basarili";
                return View("Index", Tuple.Create(values, values2));
            }

            ViewData["hata"] = "hata";


            return View("Index", Tuple.Create(values, values2));



        }

        [HttpPost]
        public ActionResult IslemGuncelle(tbl_islemler p)
        {


            var id = int.Parse(Session["userId"].ToString());

            var islem = db.tbl_islemler.Find(p.islem_id);

            var bireysel = db.tbl_kullanicilar.FirstOrDefault(x => x.kullanici_id == id);
            var hane = db.tbl_haneler.FirstOrDefault(x => x.hane_id == bireysel.kullanici_hane);

            var values = db.tbl_islemler.Where(x => x.islem_kullanici == id).ToList();
            var values2 = db.tbl_kategoriler.Where(x => x.kategori_kullanici == id).ToList();

            if (islem.islem_kullanici != id) ModelState.AddModelError("perm", "Yetkisiz İşlem");

            if (ModelState.IsValid)
            {
                p.islem_tip = (p.islem_tip == null || p.islem_tip == false) ? false : true;


                islem.islem_kategori = int.Parse(p.islem_kategori.ToString()) == 0 ? null : p.islem_kategori;

                if (p.islem_tuketim != null && int.Parse(Math.Floor(decimal.Parse(p.islem_tuketim.ToString())).ToString()) != 0 && p.islem_tip != false) p.islem_deger2 = decimal.Parse(p.islem_tuketim); else p.islem_deger2 = null;

                islem.islem_tutar = p.islem_tutar;

                if (p.islem_islem == true && islem.islem_hane != null)
                {
                    hane.hane_bakiye = islem.islem_tip == true ? decimal.Parse(hane.hane_bakiye.ToString()) + decimal.Parse(islem.islem_deger1.ToString()) : decimal.Parse(hane.hane_bakiye.ToString()) - decimal.Parse(islem.islem_deger1.ToString());

                    bireysel.kullanici_bakiye = p.islem_tip == true ? decimal.Parse(bireysel.kullanici_bakiye.ToString()) - decimal.Parse(p.islem_tutar.ToString()) : decimal.Parse(bireysel.kullanici_bakiye.ToString()) + decimal.Parse(p.islem_tutar.ToString());

                    islem.islem_bakiye = bireysel.kullanici_bakiye;
                }
                else if (p.islem_islem == false && islem.islem_hane == null)
                {
                    bireysel.kullanici_bakiye = islem.islem_tip == true ? decimal.Parse(bireysel.kullanici_bakiye.ToString()) + decimal.Parse(islem.islem_deger1.ToString()) : decimal.Parse(bireysel.kullanici_bakiye.ToString()) - decimal.Parse(islem.islem_deger1.ToString());

                    hane.hane_bakiye = p.islem_tip == true ? decimal.Parse(hane.hane_bakiye.ToString()) - decimal.Parse(p.islem_tutar.ToString()) : decimal.Parse(hane.hane_bakiye.ToString()) + decimal.Parse(p.islem_tutar.ToString());

                    islem.islem_bakiye = hane.hane_bakiye;
                }
                else
                {
                    if (p.islem_islem)
                    {
                        bireysel.kullanici_bakiye = islem.islem_tip == true ? decimal.Parse(bireysel.kullanici_bakiye.ToString()) + decimal.Parse(islem.islem_deger1.ToString()) : decimal.Parse(bireysel.kullanici_bakiye.ToString()) - decimal.Parse(islem.islem_deger1.ToString());

                        bireysel.kullanici_bakiye = p.islem_tip == true ? decimal.Parse(bireysel.kullanici_bakiye.ToString()) - decimal.Parse(p.islem_tutar.ToString()) : decimal.Parse(bireysel.kullanici_bakiye.ToString()) + decimal.Parse(p.islem_tutar.ToString());

                        islem.islem_bakiye = bireysel.kullanici_bakiye;
                    }
                    else
                    {
                        hane.hane_bakiye = islem.islem_tip == true ? decimal.Parse(hane.hane_bakiye.ToString()) + decimal.Parse(islem.islem_deger1.ToString()) : decimal.Parse(hane.hane_bakiye.ToString()) - decimal.Parse(islem.islem_deger1.ToString());

                        hane.hane_bakiye = p.islem_tip == true ? decimal.Parse(hane.hane_bakiye.ToString()) - decimal.Parse(p.islem_tutar.ToString()) : decimal.Parse(hane.hane_bakiye.ToString()) + decimal.Parse(p.islem_tutar.ToString());

                        islem.islem_bakiye = hane.hane_bakiye;
                    }
                }
                islem.islem_deger1 = decimal.Parse(p.islem_tutar.ToString());
                islem.islem_deger2 = p.islem_deger2;
                islem.islem_hane = p.islem_islem == true ? null : bireysel.kullanici_hane;
                islem.islem_tip = p.islem_tip;
                ViewData["basarili"] = "basarili";
                db.SaveChanges();
                return View("Index", Tuple.Create(values, values2));
            }
            ViewData["hata"] = "hata";

            return View("Index", Tuple.Create(values, values2));

        }

        [HttpPost]
        public ActionResult IslemSil(int p)
        {
            var id = int.Parse(Session["userId"].ToString());

            var islem = db.tbl_islemler.Find(p);

            var bireysel = db.tbl_kullanicilar.FirstOrDefault(x => x.kullanici_id == id);
            var hane = db.tbl_haneler.FirstOrDefault(x => x.hane_id == bireysel.kullanici_hane);

            var values = db.tbl_islemler.Where(x => x.islem_kullanici == id).ToList();
            var values2 = db.tbl_kategoriler.Where(x => x.kategori_kullanici == id).ToList();

            if (islem.islem_kullanici != id) ModelState.AddModelError("perm", "Yetkisiz İşlem");
            DateTime past = DateTime.Parse(islem.islem_tarih.ToString());
            DateTime now = DateTime.Now;
            var datespan = (now.Month - past.Month) + (now.Year - past.Year);
            if (datespan != 0) ModelState.AddModelError("time", "1 Ay Geçmiş");
            islem.islem_islem = true;
            
            if (ModelState.IsValid)
            {
                if(islem.islem_hane == null)
                {
                    bireysel.kullanici_bakiye = islem.islem_tip == true ? decimal.Parse(bireysel.kullanici_bakiye.ToString()) + decimal.Parse(islem.islem_deger1.ToString()) : decimal.Parse(bireysel.kullanici_bakiye.ToString()) - decimal.Parse(islem.islem_deger1.ToString());

                }
                else
                {
                    hane.hane_bakiye = islem.islem_tip == true ? decimal.Parse(hane.hane_bakiye.ToString()) + decimal.Parse(islem.islem_deger1.ToString()) : decimal.Parse(hane.hane_bakiye.ToString()) - decimal.Parse(islem.islem_deger1.ToString());
                }
                db.tbl_islemler.Remove(islem);
                db.SaveChanges();
                ViewData["basarili"] = "basarili";
                return View("Index", Tuple.Create(values, values2));
            }
            ViewData["hata"] = "hata";
            return View("Index", Tuple.Create(values, values2));
        }

    }
}