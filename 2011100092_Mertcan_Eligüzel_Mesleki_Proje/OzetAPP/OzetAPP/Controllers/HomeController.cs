using OzetAPP.Models.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OzetAPP.Controllers
{
    public class HomeController : Controller
    {
        OzetAppDBEntities2 db = new OzetAppDBEntities2();
        // GET: Home
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                ArrayList array = new ArrayList();
                var id = int.Parse(Session["userId"].ToString());
                var islem = db.tbl_islemler.Where(x => x.islem_kullanici == id && x.islem_tip == true && x.islem_kategori != null && ((DateTime)x.islem_tarih).Month == DateTime.Now.Month && ((DateTime)x.islem_tarih).Year == DateTime.Now.Year).ToList();
                var kullanicihane = db.tbl_kullanicilar.FirstOrDefault(x => x.kullanici_id == id).kullanici_hane;
                var haneid = db.tbl_haneler.FirstOrDefault(x => x.hane_id == kullanicihane).hane_id;

                //islem.GroupBy(x => x.islem_kategori).ToList();
                var encok = db.Database.SqlQuery<int>("select islem_kategori from tbl_islemler where islem_kategori is not null and islem_hane is null and islem_kullanici=" + id + " and MONTH(islem_tarih)=" + DateTime.Now.Month + " and YEAR(islem_tarih)=" + DateTime.Now.Year + " group by islem_kategori order by count(islem_kategori) DESC ").FirstOrDefault<int>();
                var enaz = db.Database.SqlQuery<int>("select islem_kategori from tbl_islemler where islem_kategori is not null and islem_hane is null and islem_kullanici=" + id + " and MONTH(islem_tarih)=" + DateTime.Now.Month + " and YEAR(islem_tarih)=" + DateTime.Now.Year + " group by islem_kategori order by count(islem_kategori) ASC ").FirstOrDefault<int>();

                var haneencok = db.Database.SqlQuery<int>("select islem_kategori from tbl_islemler where islem_kategori is not null and islem_hane is not null and islem_hane=" + haneid + " and MONTH(islem_tarih)=" + DateTime.Now.Month + " and YEAR(islem_tarih)=" + DateTime.Now.Year + " group by islem_kategori order by count(islem_kategori) DESC ").FirstOrDefault<int>();
                var haneenaz = db.Database.SqlQuery<int>("select islem_kategori from tbl_islemler where islem_kategori is not null and islem_hane is not null and islem_hane=" + haneid + " and MONTH(islem_tarih)=" + DateTime.Now.Month + " and YEAR(islem_tarih)=" + DateTime.Now.Year + " group by islem_kategori order by count(islem_kategori) ASC ").FirstOrDefault<int>();



                ViewData["encokkategori"] = encok == 0 ? "Yok" : db.tbl_kategoriler.FirstOrDefault(x => x.kategori_id == encok).kategori_ad;
                ViewData["enazkategori"] = enaz == 0 ? "Yok" : db.tbl_kategoriler.FirstOrDefault(x => x.kategori_id == enaz).kategori_ad;
                ViewData["toplamharcama"] = db.tbl_islemler.Where(x => x.islem_kullanici == id && x.islem_tip == true && x.islem_hane == null && ((DateTime)x.islem_tarih).Month == DateTime.Now.Month && ((DateTime)x.islem_tarih).Year == DateTime.Now.Year).ToList().Sum(x => x.islem_deger1) == null ? "0" : db.tbl_islemler.Where(x => x.islem_kullanici == id && x.islem_tip == true && x.islem_hane == null && ((DateTime)x.islem_tarih).Month == DateTime.Now.Month && ((DateTime)x.islem_tarih).Year == DateTime.Now.Year).ToList().Sum(x => x.islem_deger1).ToString();
                ViewData["bakiye"] = db.tbl_kullanicilar.FirstOrDefault(x => x.kullanici_id == id).kullanici_bakiye.ToString();

                ViewData["haneencokkategori"] = haneencok == 0 ? "Yok" : db.tbl_kategoriler.FirstOrDefault(x => x.kategori_id == haneencok).kategori_ad;
                ViewData["haneenazkategori"] = haneenaz == 0 ? "Yok" : db.tbl_kategoriler.FirstOrDefault(x => x.kategori_id == haneenaz).kategori_ad;
                ViewData["hanetoplamharcama"] = db.tbl_islemler.Where(x => x.islem_tip == true && x.islem_hane == haneid && x.islem_hane != null && ((DateTime)x.islem_tarih).Month == DateTime.Now.Month && ((DateTime)x.islem_tarih).Year == DateTime.Now.Year).ToList().Sum(x => x.islem_deger1) == null ? "0" : db.tbl_islemler.Where(x => x.islem_tip == true && x.islem_hane == haneid && x.islem_hane != null && ((DateTime)x.islem_tarih).Month == DateTime.Now.Month && ((DateTime)x.islem_tarih).Year == DateTime.Now.Year).ToList().Sum(x => x.islem_deger1).ToString();
                ViewData["hanebakiye"] = db.tbl_kullanicilar.FirstOrDefault(x => x.kullanici_id == id).tbl_haneler.hane_bakiye.ToString();
                return View();
            }
            catch
            {
                if (Request.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                    Session.Abandon();
                    return RedirectToAction("Index", "Login");
                }
                return RedirectToAction("Index", "Login");
            }
        }
    }
}