using OzetAPP.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OzetAPP.Controllers
{
    public class HouseholdTimelineController : Controller
    {
        // GET: HouseholdTimeline
        OzetAppDBEntities2 db = new OzetAppDBEntities2();

        [Authorize]
        public ActionResult Index()
        {
            var id = int.Parse(Session["userId"].ToString());
            var bireysel = db.tbl_kullanicilar.FirstOrDefault(x => x.kullanici_id == id);
            var values = db.tbl_islemler.Where(x=> ((DateTime)x.islem_tarih).Month == DateTime.Now.Month && x.islem_hane == bireysel.kullanici_hane && ((DateTime)x.islem_tarih).Year == DateTime.Now.Year).ToList();
            var month = db.tbl_islemler.FirstOrDefault(x => ((DateTime)x.islem_tarih).Month == DateTime.Now.Month);
            if (month != null) ViewBag.tarih = ((DateTime)month.islem_tarih).Month;
            else ViewBag.tarih = DateTime.Now.Month;
            return View(values);
        }
    }
}