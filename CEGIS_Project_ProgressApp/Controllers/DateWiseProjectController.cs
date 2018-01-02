using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CEGIS_Project_ProgressApp.Models;
using CEGIS_Project_ProgressApp.Report;
using CrystalDecisions.CrystalReports.Engine;

namespace CEGIS_Project_ProgressApp.Controllers
{
    public class DateWiseProjectController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: DateWiseProject
        public ActionResult Index()
        {

            return View();
        }

        // GET: DateWiseProject/Details/5
        public ActionResult Details(int? id)
        {
            var allInfo = db.ProjectInfos.ToList();
            var allDiv = db.LookUpDivisions.ToList();
            var q1Info = (from p in allInfo
                                join dv in allDiv on p.DivisionId equals dv.DivisionId
                                where (p.ExpectedDateId == 3 && p.ProgressTypeId!=11)
                                group p by new { DivName = dv.DivShortName, Divid = p.DivisionId } into g
                                select new
                                {
                                    KeyValue = g.Key,
                                    DivisionName = g.First().Division.DivShortName,
                                    Q1Total = g.Sum(p => p.ContactValue),
                                    Q1Count = g.Count()
                                }).ToList();
            var q2Info = (from p in allInfo
                      join dv in allDiv on p.DivisionId equals dv.DivisionId
                          where (p.ExpectedDateId == 4 && p.ProgressTypeId != 11)
                      group p by new { DivName = dv.DivShortName, Divid = p.DivisionId } into g
                      select new
                      {
                          KeyValue = g.Key,
                          DivisionName = g.First().Division.DivShortName,
                          Q2Total = g.Sum(p => p.ContactValue),
                          Q2Count = g.Count()
                      }).ToList();
            //var aaa = q1Info.Concat(q2Info);  https://stackoverflow.com/questions/5489987/linq-full-outer-join
            var q3Info = (from p in allInfo
                      join dv in allDiv on p.DivisionId equals dv.DivisionId
                          where (p.ExpectedDateId == 1 && p.ProgressTypeId != 11)
                      group p by new { DivName = dv.DivShortName, Divid = p.DivisionId } into g
                      select new
                      {
                          KeyValue = g.Key,
                          DivisionName = g.First().Division.DivShortName,
                          Q3Total = g.Sum(p => p.ContactValue),
                          Q3Count = g.Count()
                      }).ToList();
            var q4Info = (from p in allInfo
                      join dv in allDiv on p.DivisionId equals dv.DivisionId
                          where (p.ExpectedDateId == 2 && p.ProgressTypeId != 11)
                      group p by new { DivName = dv.DivShortName, Divid = p.DivisionId } into g
                      select new
                      {
                          KeyValue = g.Key,
                          DivisionName = g.First().Division.DivShortName,
                          Q4Total = g.Sum(p => p.ContactValue),
                          Q4Count = g.Count()
                      }).ToList();

            
            var q1q2infos = (from q1 in q1Info
                         join q2 in q2Info on q1.KeyValue equals q2.KeyValue into sub
                         from q1q2allInfo in sub.DefaultIfEmpty()
                         select new
                         {
                             q1q2keyvalue=q1.KeyValue,
                             Division = q1.DivisionName,
                             Q1Q2Total = (q1==null? 0: q1.Q1Total),
                             Q1Q2Count = (q1==null? 0: q1.Q1Count),

                             Q2Q1Total = (q1q2allInfo == null ? 0 : q1q2allInfo.Q2Total),
                             Q2Q1Count = (q1q2allInfo == null ? 0 : q1q2allInfo.Q2Count)
                         }).ToList();
            var q3q4infos = (from q3 in q3Info
                             join q4 in q4Info on q3.KeyValue equals q4.KeyValue into sub
                             from q3q4allInfos in sub.DefaultIfEmpty()
                             select new
                             {
                                 q3q4keyvalue = q3.KeyValue,
                                 Division = q3.DivisionName,
                                 Q3Q4Total = (q3 == null ? 0 : q3.Q3Total),
                                 Q3Q4Count = (q3 == null ? 0 : q3.Q3Count),
                                 
                                 Q4Q3Total = (q3q4allInfos == null ? 0 : q3q4allInfos.Q4Total),
                                 Q4Q3Count = (q3q4allInfos == null ? 0 : q3q4allInfos.Q4Count)
                             }).ToList();

            var q1q2q3q4infos = (from q1q2 in q1q2infos
                                 join q3q4 in q3q4infos on q1q2.q1q2keyvalue equals q3q4.q3q4keyvalue into sub
                             from q1q2q3q4allInfos in sub.DefaultIfEmpty()
                             select new
                             {
                                 
                                 Division = q1q2.Division,
                                 Q1Totals = (q1q2 == null ? 0 : q1q2.Q1Q2Total),
                                 Q1Counts = (q1q2 == null ? 0 : q1q2.Q1Q2Count),
                                 Q2Totals = (q1q2 == null ? 0 : q1q2.Q2Q1Total),
                                 Q2Counts = (q1q2 == null ? 0 : q1q2.Q2Q1Count),

                                 Q3Totals = (q1q2q3q4allInfos == null ? 0 : q1q2q3q4allInfos.Q3Q4Total),
                                 Q3Counts = (q1q2q3q4allInfos == null ? 0 : q1q2q3q4allInfos.Q3Q4Count),
                                 Q4Totals = (q1q2q3q4allInfos == null ? 0 : q1q2q3q4allInfos.Q4Q3Total),
                                 Q4Counts = (q1q2q3q4allInfos == null ? 0 : q1q2q3q4allInfos.Q4Q3Count),
                             }).ToList();

            DataSetProjectProgress a = new DataSetProjectProgress();
            ReportDocument crp = new ReportDocument();
            crp.Load(Path.Combine(Server.MapPath("~/Report/CrystalReportSubReprt.rpt")));
            crp.SetDataSource(db.ProjectInfos.Where(p => p.Probility >= id && p.ProgressTypeId!=11).Select(p => new
            {
                Project = p.ProjectName.ToString(),
                Division = p.Division.DivShortName.ToString(),
                Client = p.ProjectType.TypeName.ToString(),
                FocalPerson = p.FocalPerson.ToString(),
                Progress = p.ProgressType.Progress.ToString(),
                ContactValue = p.ContactValue.ToString(),
                Type = p.ProjectType.TypeName.ToString(),
                Probility = id.ToString(),
                Duration = p.Duration.ToString(),
                ExpectedDate = p.ExpectedDate.Dates.ToString()

            }).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = crp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(stream, "application/pdf");
        }        
    }
}
