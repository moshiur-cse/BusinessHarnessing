using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.WebPages;
using CEGIS_Project_ProgressApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CEGIS_Project_ProgressApp.Controllers
{
    public class ProjectInfoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProjectInfoes
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            string initial = User.Identity.Name;
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Initial == initial);
            int userType = currentUser.UserType;
            int divisionId = currentUser.DivisionId;
            string email = currentUser.Email;



            var projectInfos = db.ProjectInfos.Include(p => p.Division).Include(p => p.ExpectedDate).Include(p => p.ProgressType).Include(p => p.ProjectType);
            if (userType == 2)
            {
                try
                {
                    var filteredResult = projectInfos.Where(s => s.DivisionId == divisionId);
                    ViewBag.UserTypes = currentUser.UserType;
                    return View(filteredResult.ToList());

                }
                catch (Exception)
                {
                    
                }             
 
            }
            if (userType == 3)
            {
                ViewBag.UserTypes = currentUser.UserType;
                return View(projectInfos.ToList());
            }

            return View(projectInfos.ToList());
        }

        // GET: ProjectInfoes/Details/5

        //public JsonResult Details(int id)
        //{
        //    //var districtList = db.Districts.Where(a => a.Division_id == divisionId).ToList();
        //    //if (id == null)
        //    //{
        //    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    //}
        //    var projectInfo = db.ProjectInfos.Where(p=>p.Id==id).ToList();

        //    //if (projectInfo == null)
        //    //{
        //    //    return HttpNotFound();
        //    //}
        //    //return View(projectInfo);
        //    return Json(projectInfo, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectInfo projectInfo = db.ProjectInfos.Find(id);
            if (projectInfo == null)
            {
                return HttpNotFound();
            }


           return PartialView(projectInfo); 


            //return View(projectInfo);
        }

        // GET: ProjectInfoes/Create
        public ActionResult Create()
        {

            string initial = User.Identity.Name;
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Initial == initial);
            ViewBag.UserTypes = currentUser.UserType;
            int dId = currentUser.DivisionId;
            ViewBag.DivId = currentUser.DivisionId;
            ViewBag.AllDivision = db.Divisions.Where(a => a.id == dId).ToList();

            //string email = currentUser.Email;
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.DivisionId = new SelectList(db.Divisions, "id", "FullName");
            ViewBag.ExpectedDateId = new SelectList(db.ExpectedDates, "Id", "Dates");
            ViewBag.ProgressTypeId = new SelectList(db.ProgressTypes, "Id", "Progress");
            ViewBag.ProjectTypeId = new SelectList(db.ProjectTypes, "Id", "TypeName");


            return View();
        }

        // POST: ProjectInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DivisionId,ProjectName,Client,FocalPerson,ProgressTypeId,ContactValue,ProjectTypeId,Probility,Duration,ExpectedDateId,DateTimes,UserInitial")] ProjectInfo projectInfo)
        {
            string initial = User.Identity.Name;
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Initial == initial);
            string email = currentUser.Email;


            int dId = currentUser.DivisionId;
            var divName = db.Divisions.Find(dId);
            string DivisionName = divName.FullName;

            if (ModelState.IsValid)
            {
                db.ProjectInfos.Add(projectInfo);
                db.SaveChanges();

                //Data maping object to our database
                UpdateHistory aUpdateHistory = new UpdateHistory();
                aUpdateHistory.ProjectId = projectInfo.Id;
                aUpdateHistory.UpdatedBy = projectInfo.UserInitial+ " (Project Creator)";                
                aUpdateHistory.ProgressTypeId = projectInfo.ProgressTypeId;
                aUpdateHistory.UpdateTime = DateTime.Now;
                db.UpdateHistorys.Add(aUpdateHistory);
                db.SaveChanges();

                try
                {
                    //Configuring webMail class to send emails  
                    //gmail smtp server  
                    WebMail.SmtpServer = "smtp.gmail.com";
                    //gmail port to send emails  
                    WebMail.SmtpPort = 587;
                    WebMail.SmtpUseDefaultCredentials = true;
                    //sending emails with secure protocol  
                    WebMail.EnableSsl = true;
                    //EmailId used to send emails from application  
                    WebMail.UserName = "rimu.cse45@gmail.com";
                    WebMail.Password = "rimu1234";

                    //Sender email address.  
                    WebMail.From = email;
                    //$message_body = 'Dear User,' . "\r\n" . "\r\n" . 'Your organization '. $orgName . ' is not fullfilling the MRA criteria. This is a showcause letter to this organization'. " \r\n" . "\r\n" . 'Thanks' . "\r\n" . 'Microcredit Regulatory Authority';
                    //Send email  
                    //WebMail.Send(to: "moshiur.mgmh@gmail.com moshiur_cse@hotmail.com", subject: "New Project Added into Business Harnessing Monitoring Tools",
                    //             body: "Project Name: " + projectInfo.ProjectName +  "  \nAdded By: " + projectInfo.User + "\n Email: " + email,
                    //             cc: "", bcc: "", isBodyHtml: true);


                    WebMail.Send(to: "moshiur.mgmh@gmail.com sislam @cegisbd.com", subject: "New Project Added into Business Harnessing Monitoring Tools",
                                 body:  "<h1>Project Name: " + projectInfo.ProjectName + "</h1>" +
                                        "<p>Division Name: " + DivisionName + "</p>" +                                       
                                        "<p>Added By : " + projectInfo.UserInitial + "</p>" +
                                        "<p>Email : " + email + "</p>",
                                 cc: "", bcc: "", isBodyHtml: true);

                    //ViewBag.Status = "Email Sent Successfully.";
                    //sislam @cegisbd.com
                }
                catch (Exception)
                {
                    //ViewBag.Status = "Problem while sending email, Please check details.";

                }
                
                return RedirectToAction("Index");
            }

            ViewBag.DivisionId = new SelectList(db.Divisions, "id", "FullName", projectInfo.DivisionId);
            ViewBag.ExpectedDateId = new SelectList(db.ExpectedDates, "Id", "Dates", projectInfo.ExpectedDateId);
            ViewBag.ProgressTypeId = new SelectList(db.ProgressTypes, "Id", "Progress", projectInfo.ProgressTypeId);
            ViewBag.ProjectTypeId = new SelectList(db.ProjectTypes, "Id", "TypeName", projectInfo.ProjectTypeId);

            
            return View(projectInfo);
        }

        // GET: ProjectInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectInfo projectInfo = db.ProjectInfos.Find(id);
            if (projectInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.DivisionId = new SelectList(db.Divisions, "id", "FullName", projectInfo.DivisionId);
            ViewBag.ExpectedDateId = new SelectList(db.ExpectedDates, "Id", "Dates", projectInfo.ExpectedDateId);
            ViewBag.ProgressTypeId = new SelectList(db.ProgressTypes, "Id", "Progress", projectInfo.ProgressTypeId);
            ViewBag.ProjectTypeId = new SelectList(db.ProjectTypes, "Id", "TypeName", projectInfo.ProjectTypeId);
            return PartialView(projectInfo);
            //return View(projectInfo);
        }

        // POST: ProjectInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DivisionId,ProjectName,Client,FocalPerson,ProgressTypeId,ContactValue,ProjectTypeId,Probility,Duration,ExpectedDateId,DateTimes,UserInitial")] ProjectInfo projectInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectInfo).State = EntityState.Modified;
                db.SaveChanges();

                UpdateHistory aUpdateHistory = new UpdateHistory();
                aUpdateHistory.ProjectId = projectInfo.Id;
                aUpdateHistory.UpdatedBy = projectInfo.UserInitial;
                aUpdateHistory.ProgressTypeId = projectInfo.ProgressTypeId;
                aUpdateHistory.UpdateTime =DateTime.Now;
                db.UpdateHistorys.Add(aUpdateHistory);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.DivisionId = new SelectList(db.Divisions, "id", "FullName", projectInfo.DivisionId);
            ViewBag.ExpectedDateId = new SelectList(db.ExpectedDates, "Id", "Dates", projectInfo.ExpectedDateId);
            ViewBag.ProgressTypeId = new SelectList(db.ProgressTypes, "Id", "Progress", projectInfo.ProgressTypeId);
            ViewBag.ProjectTypeId = new SelectList(db.ProjectTypes, "Id", "TypeName", projectInfo.ProjectTypeId);
            return View(projectInfo);
        }

        // GET: ProjectInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectInfo projectInfo = db.ProjectInfos.Find(id);
            if (projectInfo == null)
            {
                return HttpNotFound();
            }
            return PartialView(projectInfo);
        }

        // POST: ProjectInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectInfo projectInfo = db.ProjectInfos.Find(id);
            db.ProjectInfos.Remove(projectInfo);
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
