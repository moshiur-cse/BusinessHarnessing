using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using CEGIS_Project_ProgressApp.Models;

namespace CEGIS_Project_ProgressApp.Controllers
{
    public class SendMailerController : Controller
    {
        //
        // GET: /SendMailer/ 
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(MailModel obj)
        {

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
                WebMail.From = "moshiur_cse@hotmail.com";

                //Send email  
                WebMail.Send(to: obj.ToEmail, subject: obj.EmailSubject, body: obj.EMailBody, cc: obj.EmailCC, bcc: obj.EmailBCC, isBodyHtml: true);
                ViewBag.Status = "Email Sent Successfully.";
            }
            catch (Exception ex)
            {
                ViewBag.Status = "Problem while sending email, Please check details.";

            }
            return View();
        }


    }
}