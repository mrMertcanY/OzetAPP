using OzetAPP.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OzetAPP.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        OzetAppDBEntities2 db = new OzetAppDBEntities2();

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index");
        }
        
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Index(tbl_kullanicilar p)
        {
            var user = db.tbl_kullanicilar.FirstOrDefault(x => x.kullanici_ad == p.kullanici_ad && x.kullanici_sifre == p.kullanici_sifre);
            if(user != null)
            {
                FormsAuthentication.SetAuthCookie(user.kullanici_ad, false);
                
                
                Session["userId"] = user.kullanici_id;
                Session["userName"] = user.kullanici_ad.ToString();
                Session["userPhoto"] = user.kullanici_fotograf.ToString();
                return RedirectToAction("Index","Home");
            }
            else
            {
                ViewData["hata"] = "hata";
                return View();
            }
           
        }
    }
}