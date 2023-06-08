using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMS.Models
{
    public class EmployeeInfoModel
    {
        private static MMSCGVAKDBEntities db;

        #region DepartmentMaster List
        public static List<Department_Master> DepartmentMasterList()
        {
            List<Department_Master> deptList;
            using (db = new MMSCGVAKDBEntities())
            {
                deptList = db.Department_Master.ToList();
            }
            return deptList;
        }
        #endregion
    }
}