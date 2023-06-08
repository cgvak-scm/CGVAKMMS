using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MMS.Models
{
    public class MailViewModel
    {
        #region probs
        public string username { get; set; }
        public string comments { get; set; }
        public string commentedBy { get; set; }
        public DateTime date { get; set; }
        #endregion
    }
    public class MailFunctionModel
    {
        // private static MMSCGVAKDBEntities db;
        private static MailMessage Mailing;
        private static SmtpClient smtp;
        private static string path = HttpContext.Current.Server.MapPath("~/Views/Shared/_EmailTemplate.cshtml");
        private static string path_1 = HttpContext.Current.Server.MapPath("~/Views/Shared/_EmailTemplate_1.cshtml");
        private static string EmployyeAdded_Mailpath = HttpContext.Current.Server.MapPath("~/Views/Shared/_EmployeeAdded.cshtml");
        private static string body;
        private static string ccEmail = HttpContext.Current.Session["LoggedEmail"].ToString();

        private static string Host = HttpContext.Current.Request.Url.Host;
        private static int Port = HttpContext.Current.Request.Url.Port;
        //private static string username = HttpContext.Current.Session["LoggedEmployeeName"].ToString();        

        private static string connectionString = ConfigurationManager.ConnectionStrings["MMSCGVAKDBEntities"].ConnectionString;
        private static string _host = ConfigurationManager.AppSettings["host"];
        private static int _port = Convert.ToInt16(ConfigurationManager.AppSettings["port"]);
        private static string _enableSsl = ConfigurationManager.AppSettings["enableSsl"];
        private static string _userName = ConfigurationManager.AppSettings["userName"];
        private static string _password = ConfigurationManager.AppSettings["password"];
        //private static string _from = ConfigurationManager.AppSettings["from"];
                
        #region methods

        public static int MailMeetingDetails(string resp, List<string> chairpersonmail, string tasks, string commentedBy, string comments, int meeting_id, string mailbody = "")
        {
            try
            {               
                using (Mailing = new MailMessage())
                {
                    Mailing = new MailMessage();

                    smtp = new SmtpClient()
                    {
                        Port = _port,
                        Host = _host,
                        Credentials = new System.Net.NetworkCredential(_userName, _password)
                    };

                    Mailing.From = new MailAddress("mms@cgvakindia.com");
                    //Mailing.To.Add("jayaseelan@g2tsolutions.com");
                    Mailing.IsBodyHtml = true;

                    foreach (string maillist in chairpersonmail)
                    {
                        Mailing.To.Add(maillist);
                    }

                    Mailing.CC.Add(ccEmail);

                    //Mailing.Subject = tasks;
                    Mailing.Subject = "A new task has been assigned by " + commentedBy;

                    body = System.IO.File.ReadAllText(path);

                    body = body.Replace("{0}", resp);
                    //body = body.Replace("{0}", "Jay");
                    body = body.Replace("{1}", comments);
                    body = body.Replace("{2}", commentedBy);
                    body = body.Replace("{3}", DateTime.Now.ToShortDateString());
                    body = body.Replace("{4}", tasks);
                    body = body.Replace("{5}", "https://cgvakstage.com/MMS_Live" + meeting_id);
                    body = body.Replace("{6}", "new task");
                   // body = body.Replace("{7}", _portallink);

                    Mailing.Body = body;

                    smtp.Send(Mailing);
                }
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
                //TempData["Success"] = ex.ToString();
            }
        }

        public static int MailUpdateDetails(string emailaddress, string subject, StringBuilder bodyContent, string respPerson, int taskStatus, string currentMail, string task)
        {
            try
            {
                using (Mailing = new MailMessage())
                {
                    Mailing = new MailMessage();

                    smtp = new SmtpClient()
                    {
                        Port = _port,
                        Host = _host,
                        Credentials = new System.Net.NetworkCredential(_userName, _password)
                    };
                    
                    Mailing.From = new MailAddress("mms@cgvakindia.com");
                    Mailing.To.Add(emailaddress);
                    Mailing.CC.Add(ccEmail);

                    Mailing.IsBodyHtml = true;

                    string mailbody = string.Empty;
                    Mailing.IsBodyHtml = true;
                    Mailing.Subject = "An update in " + task + "task from Meeting Management System.";
                    //read full text
                    body = System.IO.File.ReadAllText(path_1);

                    body = body.Replace("{0}", Convert.ToString(respPerson)); //mailbody = "Hi" + resp + "<br/><br/>";
                    var status = "";
                    if (taskStatus == 1)
                        status = "CLOSED";
                    else if (taskStatus == 2)
                        status = "COMPLETED";
                    else if (taskStatus == 3)
                        status = "IN PROGRESS";
                    else if (taskStatus == 4)
                        status = "RE-OPENED";
                    else if (taskStatus == 5)
                        status = "TASK ASSIGNED";
                    else
                        status = "ALL";

                    body = body.Replace("{1}", status);
                    body = body.Replace("{2}", HttpContext.Current.Session["LoggedEmployeeName"].ToString());
                    body = body.Replace("{3}", DateTime.Now.ToShortDateString());
                    body = body.Replace("{4}", task);
                    body = body.Replace("{5}", bodyContent.ToString());
                    //body + body.Replace("{6}",NrOfDays);

                    //The below code is needed because the above line of code has been modified from the below code
                    //In case of any mishappenings, the below code may be used to refer.

                    //body += bodyContent.ToString();

                    Mailing.Body = body;

                    smtp.Send(Mailing);

                }
                return 1;
            }
            catch (Exception ex)
            {
                return -1;                
            }
        }

        public static int CommentsMail(string emailaddress, string meetingTask, string meetingComments, string respPerson, string commentedBy, string mailbody = "", string cc = "")
        {
            try
            {
               cc = ccEmail;

                using (Mailing = new MailMessage())
                {
                    Mailing = new MailMessage();

                    smtp = new SmtpClient()
                    {
                        Port = _port,
                        Host = _host,
                        Credentials = new System.Net.NetworkCredential(_userName, _password)
                    };

                    Mailing.From = new MailAddress("mms@cgvakindia.com");
                    Mailing.To.Add(emailaddress);
                    Mailing.CC.Add(cc);

                    Mailing.IsBodyHtml = true;

                    //Mailing.IsBodyHtml = true;
                    //Mailing.Subject = meetingTask;
                    Mailing.Subject = "A new comment has been added by " + commentedBy;

                    //read full text
                    body = System.IO.File.ReadAllText(path);

                    body = body.Replace("{0}", respPerson);
                    body = body.Replace("{1}", meetingComments);
                    body = body.Replace("{2}", commentedBy);
                    body = body.Replace("{3}", DateTime.Now.ToShortDateString());
                    body = body.Replace("{4}", meetingTask);
                    body = body.Replace("{6}", "new comment");
                    //mailbody = "Hi Jay <br/><br/>";

                    Mailing.Body = body;

                    smtp.Send(Mailing);
                }
                return 1;
            }
            catch (Exception ex)
            {
                return -1;                
            }
        }
        #endregion

        #region Send Mail after a user has been added
        ///<summary
        ///Sending mail after a user has been created...
        ///</summary>

        public static int EmailEmployee_Added(string emailID, string FirstName, string LastName, string Password, string userName, string departmentName)
        {
            var retMsg = string.Empty;
            string ccEmailID = string.Empty;
            string meetingSubject = "A Message to inform you that You have been added In MMS...";
            try
            {
                MailFunctionModel.CommentsMail(emailID, FirstName, LastName, Password,userName, departmentName);
                {
                    Mailing = new MailMessage();
                    smtp = new SmtpClient()
                    {
                        Port = _port,
                        Host = _host,
                        Credentials = new System.Net.NetworkCredential(_userName, _password)
                    };

                    Mailing.From = new MailAddress("mms@cgvakindia.com");
                    Mailing.To.Add(emailID);                   
                    Mailing.IsBodyHtml = true;
                    Mailing.Subject = meetingSubject;

                    body = System.IO.File.ReadAllText(EmployyeAdded_Mailpath);
                    body = body.Replace("{0}", FirstName); //mailbody = "Hi" + resp + "<br/><br/>";
                    body = body.Replace("{1}", departmentName);
                    body = body.Replace("{2}", userName);
                    body = body.Replace("{3}",Password);
                    body = body.Replace("{4}", LastName);
                    
                    Mailing.Body = body;
                    smtp.Send(Mailing);
                }
                retMsg = "Updated and Mail has been sent";
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
                //TempData["Success"] = ex.ToString();
            }            
        }

        #endregion
    }
}