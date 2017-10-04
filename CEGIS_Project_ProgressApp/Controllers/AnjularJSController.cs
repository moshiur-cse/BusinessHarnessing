using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CEGIS_Project_ProgressApp.Models;

namespace CEGIS_Project_ProgressApp.Controllers
{
    public class AnjularJSController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: AnjularJS
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllInfos()
        {
            var infos = db.ProjectInfos.ToList();

            return Json(infos, JsonRequestBehavior.AllowGet);
            //string message = "Success";
            //return new JsonResult { Data = infos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}