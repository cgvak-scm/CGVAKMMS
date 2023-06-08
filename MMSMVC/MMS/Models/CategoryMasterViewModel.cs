using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MMS.Models
{
    public class CategoryMasterViewModel
    {

        private static MMSCGVAKDBEntities db;

        #region createCategory

        public static bool CreateCategory(MMS_Meeting_Category model)
        {
            bool successMsg;
            using (db = new MMSCGVAKDBEntities())
            {
                var category = db.MMS_Meeting_Category.Any(d => (d.CategoryName == model.CategoryName));

                if (category == false)
                {
                    db.MMS_Meeting_Category.Add(model);
                    db.SaveChanges();
                    successMsg = true;
                }
                else
                {
                    successMsg = false;
                }
                return successMsg;
            }
        }
        #endregion

        #region EditCategory
        public static bool EditCategory(MMS_Meeting_Category model)
        {
            bool successMsg;
            using (db = new MMSCGVAKDBEntities())
            {
                var category = db.MMS_Meeting_Category.Any(d => (d.CategoryName == model.CategoryName));

                if (category == false)
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    successMsg = true;
                }
                else
                {
                    successMsg = false;
                }
                return successMsg;
            }
        }
        #endregion

        #region deleteCategory

        public static MMS_Meeting_Category DeleteCategory(int? id, string method)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                if (string.Equals(method, "get"))
                {
                    return db.MMS_Meeting_Category.Find(id);
                }
                else
                {
                    int count = (from meetingDetails in db.MMS_Meeting_Details
                                 join
                                 meetingCat in db.MMS_Meeting_Category
                                 //on meetingDetails.Category equals meetingCat.CategoryName
                                 on meetingDetails.Category equals meetingCat.ID
                                 where meetingCat.ID == id
                                 select meetingCat).Count();
                    if (count > 0)
                    {
                        return null;
                    }
                    else
                    {
                        MMS_Meeting_Category model = db.MMS_Meeting_Category.Find(id);
                        db.MMS_Meeting_Category.Remove(model);
                        db.SaveChanges();
                        return model;
                    }


                }
            }

        }

        #endregion
    }
}