using PujcovnaKnih.Models;
using System;
using System.Collections.Generic;
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
                
                db.Users.Add(obj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public ActionResult Login()
        {
            if(Session["ID"] != null)
            {
                return RedirectToAction("UserDashBoard");
            }
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users objUser) 
        {
            if(ModelState.IsValid)
            {
                var obj = db.Users.Where(a => a.Email.Equals(objUser.Email) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                if(obj != null)
                {
                    Session["ID"] = obj.ID.ToString();
                    Session["FName"] = obj.FName.ToString();
                    Session["Email"] = obj.Email.ToString();
                    return RedirectToAction("UserDashBoard");
                }
            }  
            return View(objUser);
        }

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