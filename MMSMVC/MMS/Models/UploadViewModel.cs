using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMS.Models
{
    public class UploadResults
    {

        public string SuccessMessage { get; set; }
        public List<string> FileNames { get; set; }

        public List<HttpPostedFileBase> uploadedFile { get; set; }

        public UploadResults()
        {
            uploadedFile = new List<HttpPostedFileBase>();

        }
        public UploadResults(string successMessage, List<string> fileNames)
        {
            this.SuccessMessage = successMessage;
            this.FileNames = fileNames;
        }

        public void UploadedFiles(HttpPostedFileBase uploaded)
        {
            uploadedFile.Add(uploaded);
        }

        public void SaveUploadedFiles()
        {
            foreach (var item in uploadedFile)
            {
                string fileLocation = HttpContext.Current.Server.MapPath("~/Documents/") + HttpContext.Current.Request.Files[item.FileName];

                HttpContext.Current.Request.Files[item.FileName].SaveAs(fileLocation);


            }

        }

    }

}