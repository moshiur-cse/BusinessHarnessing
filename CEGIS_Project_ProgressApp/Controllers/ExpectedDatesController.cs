using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CEGIS_Project_ProgressApp.Models;

namespace CEGIS_Project_ProgressApp.Controllers
{
    public class ExpectedDatesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ExpectedDates
        public ActionResult Index()
        {
            return View(db.ExpectedDates.ToList());
        }

        // GET: ExpectedDates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpectedDate expectedDate = db.ExpectedDates.Find(id);
            if (expectedDate == null)
            {
                return HttpNotFound();
            }
            return View(expectedDate);
        }

        // GET: ExpectedDates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExpectedDates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date")] ExpectedDate expectedDate)
        {
            if (ModelState.IsValid)
            {
                db.ExpectedDates.Add(expectedDate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(expectedDate);
        }

        // GET: ExpectedDates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpectedDate expectedDate = db.ExpectedDates.Find(id);
            if (expectedDate == null)
            {
                return HttpNotFound();
            }
            return View(expectedDate);
        }

        // POST: ExpectedDates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date")] ExpectedDate expectedDate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expectedDate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(expectedDate);
        }

        // GET: ExpectedDates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpectedDate expectedDate = db.ExpectedDates.Find(id);
            if (expectedDate == null)
            {
                return HttpNotFound();
            }
            return View(expectedDate);
        }

        // POST: ExpectedDates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExpectedDate expectedDate = db.ExpectedDates.Find(id);
            db.ExpectedDates.Remove(expectedDate);
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
