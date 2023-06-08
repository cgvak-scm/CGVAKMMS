using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMS.Models
{
    public class DepartmentCategoryModel
    {

        private static MMSCGVAKDBEntities db;

        #region DepartmentMaster List
        public static List<Department_Master> DepartmentMasterList()
        {
            List<Department_Master> deptList;
            using (db = new MMSCGVAKDBEntities())
            {
                deptList = db.Department_Master.OrderBy(d => d.DepartmentName).ToList();
            }
            return deptList;
        }
        #endregion

        #region CategoryMaster List
        public static List<MMS_Meeting_Category> CategoryMasterList()
        {
            List<MMS_Meeting_Category> categoryList;
            using (db = new MMSCGVAKDBEntities())
            {
                categoryList = db.MMS_Meeting_Category.OrderBy(x => x.CategoryName).ToList();
            }

            return categoryList;
        }
        #endregion

        #region TemplateMaster List
        public static List<MMS_Meeting_Template> TemplateMasterList()
        {
            List<MMS_Meeting_Template> TemplateList;
            using (db = new MMSCGVAKDBEntities())
            {
                TemplateList = db.MMS_Meeting_Template.OrderBy(x => x.objective).ToList();
            }

            return TemplateList;
        }
        #endregion

        #region ReviewFrequency List
        public static List<MMS_Review_Frequency> ReviewFrequency()
        {
            List<MMS_Review_Frequency> FrequencyList;            
            using (db = new MMSCGVAKDBEntities())
            {
                FrequencyList = db.MMS_Review_Frequency.ToList();
            }
            return FrequencyList;
        }
        #endregion

        #region Priority
        public static List<MMS_Priority> TaskPriority()
        {
            List<MMS_Priority> TaskPriority;
            using (db = new MMSCGVAKDBEntities())
            {
                TaskPriority = db.MMS_Priority.ToList();
            }
            return TaskPriority;
        }
        #endregion

        #region Status
        public static List<Task_Status> TaskStatus()
        {
            List<Task_Status> TaskStatus;
            using (db = new MMSCGVAKDBEntities())
            {
                TaskStatus = db.Task_Status.ToList();
            }
            return TaskStatus;
        }
        #endregion


    }
}