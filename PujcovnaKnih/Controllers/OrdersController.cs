using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PujcovnaKnih.Models;
using PagedList;

namespace PujcovnaKnih.Controllers
{
    public class OrdersController : Controller
    {
        private DbEntities db = new DbEntities();

        // beznemu uzivateli se zobrazi jejich vlastni objednavky, admin ma pristup ke vsem objednavkam
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (Session["Role"].Equals("admin")) 
            {
                var orders = from m in db.Orders select m;
                orders = orders.OrderBy(o => o.ID);
                return View(orders.ToPagedList(pageNumber, pageSize));
            }
            if (Session["ID"] != null)
            {
                var orders = from m in db.Orders select m;
                int customerID = Convert.ToInt32(Session["ID"]);
                orders = orders.Where(o => o.CustomerID == customerID);
                orders = orders.OrderBy(o => o.ID);
                return View(orders.ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Home");
        }

        // GET: Orders/Details/
        public ActionResult Details(int? id)
        {
            if(Session["Role"].Equals("admin")) 
            { 
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Orders orders = db.Orders.Find(id);
                if (orders == null)
                {
                    return HttpNotFound();
                }
                return View(orders);
            }
            return RedirectToAction("Index");
        }

        // pouze prihlaseny uzivatel muze vytvorit novou objednavku
        public ActionResult Create(int? id)
        {
            if (Session["ID"] != null)
            {
                Orders order = new Orders();
                order.BookID = (int)id;
                order.CustomerID = Convert.ToInt32(Session["ID"]);
                order.OrderDate = DateTime.Now;
                Book book = db.Books.Find(order.BookID);
                ViewBag.BookName = book.Title;

                return View(order);
            }
            return RedirectToAction("Login", "Home");
        }

        // po vytvoreni objednavky se zmeni stav knihy na rezervnovana a stav objednavky na zadost o zapujceni
        public ActionResult CompleteOrder(int bookID, int customerID, DateTime orderDate )
        {
            if (ModelState.IsValid)
            {
                Orders order = new Orders();
                order.BookID = bookID;
                order.CustomerID = customerID;
                order.OrderDate = orderDate;
                Book book = db.Books.Find(order.BookID);
                if (book.IsAvailable.Equals("Volná")) 
                {
                    book.IsAvailable = "Rezervovaná";
                    order.State = "Žádost o zapůjčení";
                    order.Invoiced = "Ne";
                    db.Entry(book).State = EntityState.Modified;
                    db.Orders.Add(order);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }        
            }
            return RedirectToAction("Index");
        }

        // pokud chce uzivatel vratit knihu, tak se status objednavky zmeni na zadost o vraceni
        public ActionResult ReturnBook(int? id)
        {
            if (id != null)
            {
                Orders order = db.Orders.Find(id);
                order.State = "Žádost o vrácení";
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserDashBoard", "Home");
            }
            return RedirectToAction("Index");
        }

        // GET: Orders/Edit/
        public ActionResult Edit(int? id)
        {
            if(Session["Role"].Equals("admin")) 
            { 
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Orders orders = db.Orders.Find(id);
                if (orders == null)
                {
                    return HttpNotFound();
                }
                return View(orders);
            }
            return RedirectToAction("Index");
        }

        // POST: Orders/Edit/
        // Chcete-li zajistit ochranu před útoky typu OVERPOST, povolte konkrétní vlastnosti, k nimž 
        // chcete vytvořit vazbu. Další informace viz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CustomerID,BookID,OrderDate,BorrowDate,ReturnDate")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orders);
        }

        // GET: Orders/Delete/
        public ActionResult Delete(int? id)
        {
            if (Session["Role"].Equals("admin"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Orders orders = db.Orders.Find(id);
                if (orders == null)
                {
                    return HttpNotFound();
                }
                return View(orders);
            }
            return RedirectToAction("Index");
        }

        // POST: Orders/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orders orders = db.Orders.Find(id);
            db.Orders.Remove(orders);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
