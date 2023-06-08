using MMS.Models;
using MMS.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MMS.Controllers
{
    [SessionExpire]
    [HandleError]

    public class UploadController : Controller
    {       
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }        

        [HttpPost]
        public JsonResult UploadFile(HttpPostedFileBase file)
        {
            string fileLocation = "";
            string RespMessage = string.Empty;
            List<string> fileNameList = new List<string>();

            for (int i = 0; i < Request.Files.Count; i++)
            {
                file = Request.Files[i];

                if (file.ContentLength > 0)
                {
                    string path = Server.MapPath("~/Documents/");
                    UploadResults uploadResult = new UploadResults();
                    uploadResult.uploadedFile.Add(file);

                    string extension = Path.GetExtension(file.FileName.ToString());
                    string extn = "";
                    if (extension == ".pdf" || extension == ".docx")
                    {
                        if (extension == ".pdf")
                            extn = ".docx";
                        else
                            extn = ".pdf";
                        path = Server.MapPath("~/Documents/");
                    }
                    else
                    {
                        if (extension == ".jpg")
                            extn = ".png";
                        else
                            extn = ".pdf";
                        path = Server.MapPath("~/App_Data/photos/");
                    }

                    DirectoryInfo Dr = new DirectoryInfo(path);
                    FileInfo[] files = Dr.GetFiles("*" + extension).Where(p => p.Extension == extension || p.Extension == extn).ToArray();
                    foreach (FileInfo f in files)
                    {
                        f.Attributes = FileAttributes.Normal;
                        System.IO.File.Delete(f.FullName);
                    }
                        

                    if (extension == ".pdf" || extension == ".docx")
                    {                        
                        fileLocation = Server.MapPath("~/Documents/") + Request.Files[i].FileName;
                    }
                    else
                    {
                        fileLocation = Server.MapPath("~/App_Data/photos/") + Request.Files[i].FileName;
                    }

                    Request.Files[i].SaveAs(fileLocation);                
                    fileNameList.Add(Request.Files[i].FileName);
                    RespMessage = "File uploaded";
                }

            }

            //after uploading message and filenames were stored in upload result 
            UploadResults uploadResults = new UploadResults(RespMessage, fileNameList);
            return Json(uploadResults, JsonRequestBehavior.AllowGet);
        }

        private void UploadFileToAzure(HttpPostedFileBase file)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTPCredentials.FTPRequestUrl + file.FileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.  
            request.Credentials = new NetworkCredential(FTPCredentials.FTPUserName, FTPCredentials.FTPPassword);

            // Copy the contents of the file to the request stream.  

            byte[] fileContents;
            using (var reader = new BinaryReader(file.InputStream))
            {
                fileContents = reader.ReadBytes(file.ContentLength);

            }
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

        }

        private void UploadFileToAzure1(HttpPostedFileBase file)
        {
            //FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(@"ftp://waws-prod-sg1-007.ftp.azurewebsites.windows.net/site/wwwroot/Documents/" + file.FileName);//MMsTEst
            FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(@"ftp://waws-prod-sg1-007.ftp.azurewebsites.windows.net/site/wwwroot/");//MMsTEst
            ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftpWebRequest.Timeout = -1;
            ftpWebRequest.ReadWriteTimeout = -1;
            ftpWebRequest.UsePassive = true;

            ftpWebRequest.Credentials = new NetworkCredential(@"cgvakmmstest\cgvakmmstest", "xzvgvXDp1xAp5biv9hEymxQjjNfGGkzBk3AjWzLlTnEMcvefJenGEnjt6SqT");//MMSTest

            ftpWebRequest.KeepAlive = true;
            ftpWebRequest.UseBinary = true;

            byte[] fileContents;
            using (var reader = new BinaryReader(file.InputStream))
            {
                fileContents = reader.ReadBytes(file.ContentLength);


            }

            using (Stream requestStream = ftpWebRequest.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();
            }

        }





    }
}