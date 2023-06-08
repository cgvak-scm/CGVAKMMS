using System;
using System.Linq;
using System.Web.Mvc;
using MMS.Models;
using System.Web.Security;
using MMS.Security;
using System.Data.Entity;
using System.Web;
using System.Net.Mail;

namespace MMS.Controllers
{
    [HandleError]
 
    public class AccountController : Controller
    {
        public static SmtpClient smtp;
        [Authorize]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
         {
            if (Session["LoggedUserId"] != null)
            {
                return RedirectToAction("Index", "MyTasks");
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
        }

        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {

                if (model.RememberMe)
                {
                    HttpCookie rememberCookie = new HttpCookie("UserCredientials");

                    rememberCookie.Values.Add("username", model.LoginUserName);
                    rememberCookie.Values.Add("password", model.LoginPassword);
                    rememberCookie.Expires.AddDays(2);

                    Response.Cookies.Add(rememberCookie);
                }
                else
                {
                    var removeCookie = new HttpCookie("UserCredientials");
                    removeCookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(removeCookie);
                }

                var userId = LoginViewModel.Login(model);
               
                if (userId != null)
                {                   
                    FormsAuthentication.SetAuthCookie(model.EmployeeICode.ToString(), false);
                    Session["LoggedUserId"] = userId.EmployeeIcode.ToString();                    
                    Session["LoggedEmployeeName"] = LoginViewModel.GetEmployeeName(model.LoginUserName).ToString();
                    if(!string.IsNullOrEmpty(LoginViewModel.GetEmployeeMailId(userId.EmployeeIcode)))
                        Session["LoggedEmail"] = LoginViewModel.GetEmployeeMailId(userId.EmployeeIcode).ToString();
                    return RedirectToLocal(returnUrl);
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The Username or Password you entered is incorrect.");
            return View(model);
        }

        // POST: /Account/LogOff

        public ActionResult LogOff()
        {
            //Request.Cookies.Remove("EmployeeICode");
            FormsAuthentication.SignOut();
            Session.Clear();

            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        [SessionExpire]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [SessionExpire]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model, string returnUrl)
        {
            bool message;
            if (Session["LoggedUserId"] != null)
            {
                if (ModelState.IsValid)
                {
                    int userId = Convert.ToInt32(Session["LoggedUserId"]);
                    var password = LoginViewModel.ChangePassword(model, userId);
                    if (password != null)
                    {
                        TempData["Success"] = null;
                        message = LoginViewModel.ChangePasswordReset(model, userId);
                        TempData["Success"] = "Your password has been changed successfully!";
                        return RedirectToLocal(returnUrl);
                    }
                }
            }

            ModelState.AddModelError("", "Please check your old password.");
            return View(model);
        }


        //#region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "MyTasks");
            }
        }

        //Forgot Password

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string EmailId)
        {
            /*Create instance of entity model*/
            MMSCGVAKDBEntities objentity = new MMSCGVAKDBEntities();

            /*Getting data from database for email validation*/
            var _objuserdetail = objentity.Employee_Master.Where(x => x.EmployeeCorporateEmailId == EmailId).FirstOrDefault();

            if (_objuserdetail != null)
            {
                string status = SendPassword(_objuserdetail.LoginPassword, _objuserdetail.EmployeeCorporateEmailId, _objuserdetail.LoginUserName);
                ViewBag.Message = 1;
            }
            else
            {
                ViewBag.Message = 0;
            }
            return View();
        }

        //Functionality for mail

        public string SendPassword(string Password, string EmailId, string Name)
        {
            try
            {

                MailMessage mail = new MailMessage();
                mail.To.Add(EmailId);
                mail.From = new MailAddress("tasks@cgvakindia.com");
                mail.Subject = "Your password for account " + EmailId;

                string userMessage = "";

                userMessage = userMessage + "<br/><b>Login Id:</b> " + EmailId;
                userMessage = userMessage + "<br/><b>Passsword: </b>" + Password;

                string Body = "Dear " + Name + ", <br/><br/>Login detail for your account is a follows:<br/></br> " + userMessage + "<br/><br/>Thanks";
                mail.Body = Body;
                mail.IsBodyHtml = true;


                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.sendgrid.net"; //SMTP Server Address of gmail
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("MMSMailSystem", "CG-vak123");
                // Smtp Email ID and Password For authentication
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return "<b>Please check your email for account login detail.</b>";
            }
            catch
            {
                return "Error............";
            }
        }        
    }
}