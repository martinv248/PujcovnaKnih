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

namespace PujcovnaKnih.Controllers
{
    public class OrdersController : Controller
    {
        private DbEntities db = new DbEntities();

        // GET: Orders
        public ActionResult Index()
        {
            if(Session["Email"].Equals("admin")) 
            {
                return View(db.Orders.ToList());
            }
            if (Session["ID"] != null)
            {
                var orders = from m in db.Orders select m;
                int customerID = Convert.ToInt32(Session["ID"]);
                orders = orders.Where(o => o.CustomerID == customerID);
                return View(orders.ToList());
            }
            return RedirectToAction("Login", "Home");
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
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

        // GET: Orders/Create
        public ActionResult Create(int? id)
        {
            if (Session["ID"] != null)
            {
                Orders order = new Orders();
                order.BookID = (int)id;
                order.CustomerID = Convert.ToInt32(Session["ID"]);
                order.OrderDate = DateTime.Now;

                return View(order);
            }
            return RedirectToAction("Login", "Home");
        }

        // POST: Orders/Create
        // Chcete-li zajistit ochranu před útoky typu OVERPOST, povolte konkrétní vlastnosti, k nimž 
        // chcete vytvořit vazbu. Další informace viz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CustomerID,BookID,OrderDate")] Orders order)
        {
            if (ModelState.IsValid)
            {

                Book book = db.Books.Find(order.BookID);
                book.IsAvailable = "Žádost o zapůjčení";
                db.Entry(book).State = EntityState.Modified;
                db.Orders.Add(order);
                Debug.WriteLine("Customer ID: " + order.CustomerID + " Book ID: " + order.BookID + " Order date: " + order.OrderDate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult ReturnBook(int? id)
        {
            if (id != null)
            {
                Book book = db.Books.Find(id);
                book.IsAvailable = "Žádost o vrácení";
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserDashBoard", "Home");
            }
            return RedirectToAction("Index");
        }

        // GET: Orders/Edit/
        public ActionResult Edit(int? id)
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

        // POST: Orders/Edit/5
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

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Orders/Delete/5
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
