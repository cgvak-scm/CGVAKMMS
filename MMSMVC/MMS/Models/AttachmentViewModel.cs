using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMS.Models
{
    public class AttachmentViewModel
    {

        private MMSCGVAKDBEntities db;

        public dynamic GetAttachmentFile(int? meetingId)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                return db.MMS_Attachment.Where(x => x.Meeting_Id == meetingId).ToList();
            }

        }


        public List<MMS_Attachment> GetDownloadFile(string fileName)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                return db.MMS_Attachment.Where(x => x.MMS_FileName == fileName).ToList();
            }
        }
    }
}