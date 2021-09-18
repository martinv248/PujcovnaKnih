using PujcovnaKnih.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PujcovnaKnih.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        
        private DbEntities db = new DbEntities();
        public ActionResult Index()
        {
            return View();
        }

        // prihlaseny uzivatel je odkazan na svuj profil
        public ActionResult Registration()
        {
            if (Session["ID"] != null)
            {
                return RedirectToAction("UserDashBoard");
            }
            return View();
        }      

        [HttpPost]
        public ActionResult Registration(Users obj)
        {
            if (ModelState.IsValid)
            {
                    obj.Role = "user";
                    db.Users.Add(obj);
                    db.SaveChanges();
                    return RedirectToAction("Index");       
            }
            return View(obj);
        }

        // prihlaseny uzivatel je odkazan na svuj profil
        public ActionResult Login()
        {
            if(Session["ID"] != null)
            {
                return RedirectToAction("UserDashBoard");
            }
            return View();
        }
        
        // k prihlaseni se porovanaji udaje uvedene uzivatelem s udaji ulozene v databazi, nektere udaje se potom ulozi v ramci Session
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users objUser) 
        {
            if(ModelState.IsValid)
            {
                var obj = db.Users.Where(a => a.Email.Equals(objUser.Email) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["ID"] = obj.ID.ToString();
                    Session["FName"] = obj.FName.ToString();
                    Session["Email"] = obj.Email.ToString();
                    Session["Role"] = obj.Role.ToString();
                    return RedirectToAction("UserDashBoard");
                }
            }  
            return View(objUser);
        }

        // Profil uzivatele
        public ActionResult UserDashBoard()
        {
            if (Session["ID"] != null)
            {
                Users user = db.Users.Find(Convert.ToInt32(Session["ID"]));
                return View(user);
            } else
            {
                return RedirectToAction("Login");
            }
        }
    }
}