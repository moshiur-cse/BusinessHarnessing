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
    public class ProgressTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProgressTypes
        public ActionResult Index()
        {
            return View(db.ProgressTypes.ToList());
        }

        // GET: ProgressTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgressType progressType = db.ProgressTypes.Find(id);
            if (progressType == null)
            {
                return HttpNotFound();
            }
            return View(progressType);
        }

        // GET: ProgressTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProgressTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Progress")] ProgressType progressType)
        {
            if (ModelState.IsValid)
            {
                db.ProgressTypes.Add(progressType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(progressType);
        }

        // GET: ProgressTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgressType progressType = db.ProgressTypes.Find(id);
            if (progressType == null)
            {
                return HttpNotFound();
            }
            return View(progressType);
        }

        // POST: ProgressTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Progress")] ProgressType progressType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(progressType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(progressType);
        }

        // GET: ProgressTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgressType progressType = db.ProgressTypes.Find(id);
            if (progressType == null)
            {
                return HttpNotFound();
            }
            return View(progressType);
        }

        // POST: ProgressTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProgressType progressType = db.ProgressTypes.Find(id);
            db.ProgressTypes.Remove(progressType);
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
