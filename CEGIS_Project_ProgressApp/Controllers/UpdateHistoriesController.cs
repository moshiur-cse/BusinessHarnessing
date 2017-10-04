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
    public class UpdateHistoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UpdateHistories
        public ActionResult Index()
        {
            var updateHistorys = db.UpdateHistorys.Include(u => u.ProjectInfos);
            return View(updateHistorys.ToList());
        }

        // GET: UpdateHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UpdateHistory updateHistory = db.UpdateHistorys.Find(id);
            if (updateHistory == null)
            {
                return HttpNotFound();
            }
            return View(updateHistory);
        }

        // GET: UpdateHistories/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.ProjectInfos, "Id", "ProjectName");
            return View();
        }

        // POST: UpdateHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectId,ProgressTypeId,UpdatedBy,UpdateTime")] UpdateHistory updateHistory)
        {
            if (ModelState.IsValid)
            {
                db.UpdateHistorys.Add(updateHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.ProjectInfos, "Id", "ProjectName", updateHistory.ProjectId);
            return View(updateHistory);
        }

        // GET: UpdateHistories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UpdateHistory updateHistory = db.UpdateHistorys.Find(id);
            if (updateHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.ProjectInfos, "Id", "ProjectName", updateHistory.ProjectId);
            return View(updateHistory);
        }

        // POST: UpdateHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,ProgressTypeId,UpdatedBy,UpdateTime")] UpdateHistory updateHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(updateHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.ProjectInfos, "Id", "ProjectName", updateHistory.ProjectId);
            return View(updateHistory);
        }

        // GET: UpdateHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UpdateHistory updateHistory = db.UpdateHistorys.Find(id);
            if (updateHistory == null)
            {
                return HttpNotFound();
            }
            return View(updateHistory);
        }

        // POST: UpdateHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UpdateHistory updateHistory = db.UpdateHistorys.Find(id);
            db.UpdateHistorys.Remove(updateHistory);
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
