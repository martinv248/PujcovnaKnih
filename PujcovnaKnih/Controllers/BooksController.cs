using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PujcovnaKnih.Models;

namespace PujcovnaKnih.Controllers
{
    public class BooksController : Controller
    {
        private DbEntities db = new DbEntities();

        // zobrazi seznam knih, ktere je mozne filtrovat a seradit
        public ViewResult Index(string sortOrder, string genre, string author, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.GenreSortParm = String.IsNullOrEmpty(sortOrder) ? "genre_asc" : "";
            ViewBag.AuthorSortParm = String.IsNullOrEmpty(sortOrder) ? "author_asc" : "";
            ViewBag.Role = Session["Role"];

            if(searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var GenreList = new List<string>();
            var GenreQry = from d in db.Books orderby d.Genre select d.Genre;
            GenreList.AddRange(GenreQry.Distinct());
            ViewBag.genre = new SelectList(GenreList);

            var AuthorList = new List<string>();
            var AuthorQry = from e in db.Books orderby e.Author select e.Author;
            AuthorList.AddRange(AuthorQry.Distinct());
            ViewBag.author = new SelectList(AuthorList);

            var books = from m in db.Books select m;
            if(!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString));
            }
            if(!String.IsNullOrEmpty(author))
            {
                books = books.Where(r => r.Author == author);
            }
            if(!string.IsNullOrEmpty(genre))
            {
                books = books.Where(x => x.Genre == genre);
            }

            switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;
                case "genre_asc":
                    books = books.OrderBy(b => b.Genre);
                    break;
                case "author_asc":
                    books = books.OrderBy(b => b.Author);
                    break;
                default:
                    books = books.OrderBy(b => b.Title);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(books.ToPagedList(pageNumber, pageSize));
        }


        // zobrazi podrobnosti jednotlivych knih
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // moznost vytvorit novy zaznam knihy (pouze admin)
        public ActionResult Create()
        {
            if (Session["Role"].Equals("admin")) 
            { 
                return View();
            }
            return RedirectToAction("Index");
        }

        // POST: Books/Create
        // Chcete-li zajistit ochranu před útoky typu OVERPOST, povolte konkrétní vlastnosti, k nimž 
        // chcete vytvořit vazbu. Další informace viz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Genre,Author,Price,IsAvailable")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // upravit udaje o knize (pouze admin)
        public ActionResult Edit(int? id)
        {
            if (Session["Role"].Equals("admin"))
            {
                if (id == null)
                {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Book book = db.Books.Find(id);
                if (book == null)
                {
                return HttpNotFound();
                }
                return View(book);
            }
            return RedirectToAction("Index");
        }

        // POST: Books/Edit/
        // Chcete-li zajistit ochranu před útoky typu OVERPOST, povolte konkrétní vlastnosti, k nimž 
        // chcete vytvořit vazbu. Další informace viz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Genre,Author,Price,IsAvailable")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // smazat zaznam o knize (pouze admin)
        public ActionResult Delete(int? id)
        {
            if (Session["Role"].Equals("admin")) { 
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Book book = db.Books.Find(id);
                if (book == null)
                {
                    return HttpNotFound();
                }
                return View(book);
                }
            return RedirectToAction("Index");
        }

        // POST: Books/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
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
