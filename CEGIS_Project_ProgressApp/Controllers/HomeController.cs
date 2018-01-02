using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CEGIS_Project_ProgressApp.Models;
using CEGIS_Project_ProgressApp.Report;
using CrystalDecisions.CrystalReports.Engine;

namespace CEGIS_Project_ProgressApp.Controllers
{
    public class HomeController : Controller
    {
        //private MyDemoEntities mde = new MyDemoEntities();D:\CEGIS_Project_ProgressApp\CEGIS_Project_ProgressApp\Controllers\HomeController.cs
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            
            string initial = User.Identity.Name;
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Initial == initial);
            int userType = currentUser.UserType;
            ViewBag.userType = currentUser.UserType;

            if (!Request.IsAuthenticated) //|| User.Identity.Name!="SIH"
            {
                return RedirectToAction("Login", "Account");
            }

            //ViewBag.Count = db.ProjectInfos.ToList().Count;
            return View();
        }
        public ActionResult Report()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            //ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult DivisionWiseReport()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            DataSetProjectProgress a = new DataSetProjectProgress();
            ReportDocument crp = new ReportDocument();
            crp.Load(Path.Combine(Server.MapPath("~/Report/CrystalReportProjectProgress.rpt")));

            crp.SetDataSource(db.ProjectInfos.Select(p => new
            {
                Project = p.ProjectName.ToString(),
                Division = p.Division.DivShortName.ToString(),
                Client = p.ProjectType.TypeName.ToString(),
                FocalPerson = p.FocalPerson.ToString(),
                Progress = p.ProgressType.Progress.ToString(),
                ContactValue = p.ContactValue.ToString(),
                Type = p.ProjectType.TypeName.ToString(),
                Probility = p.Probility.ToString(),
                Duration = p.Duration.ToString(),
                ExpectedDate = p.ExpectedDate.Dates.ToString()

            }).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = crp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(stream, "application/pdf");
            

            //return File(stream, "application/pdf", "Division_Wise_Projects");

        }

        public ActionResult SummaryReport()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            DataSetProjectProgress a = new DataSetProjectProgress();
            ReportDocument crp = new ReportDocument();
            crp.Load(Path.Combine(Server.MapPath("~/Report/CrystalReportProgressSummary.rpt")));
            crp.SetDataSource(db.ProjectInfos.Select(p => new
            {
                Project = p.ProjectName.ToString(),
                Division = p.Division.DivShortName.ToString(),
                Client = p.ProjectType.TypeName.ToString(),
                FocalPerson = p.FocalPerson.ToString(),
                Progress = p.ProgressType.Progress.ToString(),
                ContactValue = p.ContactValue.ToString(),
                Type = p.ProjectType.TypeName.ToString(),
                Probility = p.Probility.ToString(),
                Duration = p.Duration.ToString(),
                ExpectedDate = p.ExpectedDate.Dates.ToString()

            }).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = crp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(stream, "application/pdf");
            //return File(stream, "application/pdf", "Summary_Of_Progress");
        }

        public ActionResult DateWiseProjects()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            //        //SELECT DivisionId, ExpectedDateId, COUNT(ProjectName), SUM(ContactValue) FROM `projectinfoes`WHERE ExpectedDateId = 1 GROUP BY DivisionId;
            var allInfo = db.ProjectInfos.ToList();
            var allDiv = db.LookUpDivisions.ToList();

            var totalHarness = (from p in allInfo
                                join dv in allDiv on p.DivisionId equals dv.DivisionId
                                where p.ExpectedDateId==4
                                group p by new {Divid = p.DivisionId } into g
                                select new {DivisionName = g.First().Division.DivShortName,
                                             Total = g.Sum(p => p.ContactValue),
                                             Count=g.Count()
                                           }).ToList();


            DataSetProjectProgress a = new DataSetProjectProgress();
            ReportDocument crp = new ReportDocument();
            crp.Load(Path.Combine(Server.MapPath("~/Report/CrystalReportSubReprt.rpt")));
            crp.SetDataSource(db.ProjectInfos.Where(p=>p.Probility>= 0).Select(p => new
            {
                Project = p.ProjectName.ToString(),
                Division = p.Division.DivShortName.ToString(),
                Client = p.ProjectType.TypeName.ToString(),
                FocalPerson = p.FocalPerson.ToString(),
                Progress = p.ProgressType.Progress.ToString(),
                ContactValue = p.ContactValue.ToString(),
                Type = p.ProjectType.TypeName.ToString(),
                Probility = p.Probility.ToString(),
                Duration = p.Duration.ToString(),
                ExpectedDate = p.ExpectedDate.Dates.ToString()
                
            }).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = crp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(stream, "application/pdf");
            //return File(stream, "application/pdf", "Date_Wise_Projects");
        }

        public ActionResult BusinessHarness()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            List<BusinessHarnessing> aList = new List<BusinessHarnessing>();
            BusinessHarnessing aBusinessHarness;
            var allInfo = db.ProjectInfos.ToList();
            var allDiv = db.LookUpDivisions.ToList();

            var totalHarness = (from p in allInfo
                                join dv in allDiv on p.DivisionId equals dv.DivisionId
                                where p.ProgressTypeId == 11
                                group p by new { DivName = dv.DivShortName, Divid = p.DivisionId } into g
                                select new { KeyValue = g.Key, DivisionName = g.First().Division.DivShortName, Total = g.Sum(p => p.ContactValue) }).ToList();

            var total = (from p in allInfo
                         join dv in allDiv on p.DivisionId equals dv.DivisionId
                         group p by new { DivName = dv.DivShortName, Divid = p.DivisionId } into g
                         select new { KeyValue = g.Key, DivisionName = g.First().Division.DivShortName, Total = g.Sum(p => p.ContactValue) }).ToList();

            var infos = (from t in total
                         join h in totalHarness on t.KeyValue equals h.KeyValue into sub
                         from subtotalHarness in sub.DefaultIfEmpty()
                         select new
                         {
                             Division = t.DivisionName,
                             TotalAmount = t.Total,
                             HarnessAmount = (subtotalHarness == null ? 0 : subtotalHarness.Total)
                         }).ToList();

            foreach (var aDta in infos)
            {
                aBusinessHarness = new BusinessHarnessing();
                aBusinessHarness.Division = aDta.Division;
                aBusinessHarness.Budget = aDta.TotalAmount;
                aBusinessHarness.Harness = aDta.HarnessAmount;
                aList.Add(aBusinessHarness);

            }

            DataSetProjectProgress a = new DataSetProjectProgress();
            ReportDocument crp = new ReportDocument();
            crp.Load(Path.Combine(Server.MapPath("~/Report/CrystalReportActualHarness.rpt")));
            crp.SetDataSource(aList.Select(p => new
            {               
                Division = p.Division.ToString(),
                Budget=p.Budget.ToString(),
                Harness=p.Harness.ToString()

            }).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = crp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(stream, "application/pdf");





            //ViewBag.a = aList;
            //var aInfo= allInfo.Join(allDiv,p=>p.DivisionId,d=>d.id, (post, meta) => new { Post = post, Meta = meta }).Where(post=>p.ProgressTypeId==11).GroupBy(d => new
            //{
            //    Divid = d.DivisionId,

            //} ).Select(
            //             g => new
            //             {
            //                 Key = g.Key,
            //                 Value = g.Sum(s => s.ContactValue),
            //                 Name = g.First().Division
            //             });

            //return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}