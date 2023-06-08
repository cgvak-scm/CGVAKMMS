using MMS.Models;
using MMS.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MMS.Controllers
{

    [SessionExpire]
    [HandleError]

    public class AttachmentController : Controller
    {

        private AttachmentViewModel attachmentOb = new AttachmentViewModel();

        [HttpGet]
        public ActionResult Index(int meetingId)
        {

            ViewBag.Attachments = attachmentOb.GetAttachmentFile(meetingId);

            return View();
        }

        public FileContentResult Download1(string fileName)
        {

            var item = attachmentOb.GetDownloadFile(fileName);
            fileName = item.FirstOrDefault().MMS_FileName;

            FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(@"ftp://waws-prod-sg1-007.ftp.azurewebsites.windows.net/site/wwwroot/" + fileName);//MMsTEst
            ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;

            ftpWebRequest.Credentials = new NetworkCredential(@"cgvakmmstest\$cgvakmmstest", "xzvgvXDp1xAp5biv9hEymxQjjNfGGkzBk3AjWzLlTnEMcvefJenGEnjt6SqT");//MMSTest



            FtpWebResponse response = (FtpWebResponse)ftpWebRequest.GetResponse();

            using (Stream responseStream = response.GetResponseStream())
            {

                StreamReader reader = new StreamReader(responseStream);



                string receivestream = reader.ReadToEnd();

                var fileContent = Encoding.ASCII.GetBytes(receivestream);

                return File(fileContent, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
        }

        public ActionResult Download(string fileName)
        {
            FileResult fileResult = null;
            //byte[] fileBytes = null;
            string fileLocation = Server.MapPath("~/Documents/") + fileName;
            try
            {
                byte[] fileBytes = fileBytes = System.IO.File.ReadAllBytes(fileLocation);
                string fileName1 = fileName;
                fileResult = File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName1);
            }
            catch (FileNotFoundException ex)
            {
                TempData["Success"] = "The file does't exist in the directory";
                return RedirectToAction("Index", "MyTasks", new { @meetingId = "", selectDates = "" });
            }

            return fileResult;
        }        
    }
}