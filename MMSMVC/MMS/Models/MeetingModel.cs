using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMS.Models
{
    public class MeetingModel
    {
        //private MMSCGVAKDBEntities db;
        public int Meeting_Id { get; set; }
        public Nullable<int> Employee_Id { get; set; }
        public int? Participant { get; set; }
        public int? NoOfComments { get; set; }
        public string Participantname { get; set; }
        public string Task { get; set; }
        public Nullable<int> Task_Id { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> Completion_date { get; set; }
        public string Completion_DateStr
        {
            get
            {
                return Completion_date != null ? Completion_date.Value.ToString("dd/MMM/yyyy") : string.Empty;

            }
        }
        public string Priority { get; set; }
        public System.DateTime Meeting_Date { get; set; }
        public string Meeting_DateStr
        {
            get
            {
                return Meeting_Date.ToString("dd/MMM/yyyy");
            }
        }

        public bool review_status { get; set; }


        public string Review_Name
        {
            get
            {
                return (bool)this.review_status ? "True" : "YES";
            }
        }


        // public string Review_Name { get;set;       

        //get
        //{
        //    if(review_status==true)
        //    {
        //        return  "Yes";
        //    }
        //    else
        //    {
        //        if(review_status==false)
        //        {
        //            return "No";
        //        }
        //    }
        //    return review_status.ToString();
        //}
        //}

        // public string CategoryName { get; set; }
        //public int CategoryID { get; set; }

        public int CategoryID
        {
            get;set;
        }

        //public string CategoryName
        //{
        //    get
        //    {
        //       return db.MMS_Meeting_Category.Where(a => a.ID == CategoryID).Select(x => x.CategoryName).FirstOrDefault();

        //    }
        //}
        public string Category { get; set; }

        public string CategoryN { get; set; }

        public Nullable<bool> Seen { get; set; }
        public int Deleted { get; set; }
        public string[] reviewFreq { get; set; }
        public string[] reviewdate { get; set; }
        //public string reviewFreq1 { get; set; }

        public string reviewFreq1 { get; set; }

        public System.DateTime reviewdate1 { get; set; }
        public string Review_DateStr
        {
            get
            {
                return reviewdate1.ToString("dd/MMM/yyyy");

            }
        }

    }
}