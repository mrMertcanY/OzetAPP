using OzetAPP.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OzetAPP.Controllers
{
    public class SummaryController : Controller
    {
        OzetAppDBEntities2 db = new OzetAppDBEntities2();
        // GET: Summary
        [Authorize]
        public ActionResult Index()
        {
            var id = int.Parse(Session["userId"].ToString());
            var values = db.tbl_islemler.Where(x=> ((DateTime)x.islem_tarih).Month == DateTime.Now.Month && ((DateTime)x.islem_tarih).Year == DateTime.Now.Year && x.islem_kullanici == id && x.islem_hane == null).ToList();
            return View(values);
        }
    }
}