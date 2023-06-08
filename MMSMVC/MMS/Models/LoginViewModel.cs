using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MMS.Models
{
    public partial class LoginViewModel
    {

        private static MMSCGVAKDBEntities db;

        #region methods
        public static dynamic Login(LoginViewModel loginObj)
        {
            
            using (db = new MMSCGVAKDBEntities())
            {
                return  (from EmpMaster in db.Employee_Master
                           join MMSEmpMaster in db.MMS_Employee_Master on EmpMaster.EmployeeICode equals MMSEmpMaster.profile_employee_id
                           where EmpMaster.LoginUserName == loginObj.LoginUserName && EmpMaster.LoginPassword == loginObj.LoginPassword && EmpMaster.IsActive == true
                           select new
                           {
                               EmployeeIcode = EmpMaster.EmployeeICode

                           }).FirstOrDefault();
            }
        }

        public static string GetEmployeeName(string userName)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                return db.Employee_Master.Where(x => x.LoginUserName == userName).Select(x => x.EmployeeFirstName + " " + x.EmployeeLastName).FirstOrDefault();                                 
            }
        }

        public static string GetEmployeeMailId(int userId)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                return db.Employee_Master.Where(x => x.EmployeeICode == userId).Select(x => x.EmployeeCorporateEmailId).FirstOrDefault();                              
            }
        }


        public static dynamic ChangePassword(ChangePasswordViewModel model, int userId)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                return db.Employee_Master.FirstOrDefault(c => (c.EmployeeICode == userId && c.LoginPassword == model.OldPassword));
            }
        }

        public static bool ChangePasswordReset(ChangePasswordViewModel model, int userId)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                Employee_Master employeeMaster = new Employee_Master();
                employeeMaster = db.Employee_Master.Find(userId);
                employeeMaster.LoginPassword = model.NewPassword;
                db.Employee_Master.Add(employeeMaster);
                db.Entry(employeeMaster).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }

        }
        #endregion
    }
}