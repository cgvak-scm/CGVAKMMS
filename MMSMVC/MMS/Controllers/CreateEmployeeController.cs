using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Mvc;
using MMS.Models;
using MMS.Security;

namespace MMS.Controllers
{
    [SessionExpire]
    [HandleError]

    public class CreateEmployeeController : Controller
    {
        private MMSCGVAKDBEntities db = new MMSCGVAKDBEntities();
        // GET: Create Employee
        public ActionResult Index()
        {            
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Index(EmpPersonalViewDetails PersonalDetails)
        {
            if (ModelState.IsValid)
            {
                TempData["EmpPersonalData"] = PersonalDetails;
                TempData.Keep("EmpPersonalData");

                return RedirectToAction("CreateEmployeePage2");
            }
            return View();
        }

        // GET: Create Employee Page 2
        public ActionResult CreateEmployeePage2()
        {

            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateEmployeePage2(EmpOthersViewDetails empOtherDetails)
        {
            EmpPersonalViewDetails empPersonal = TempData["EmpPersonalData"] as EmpPersonalViewDetails;
            TempData.Keep("EmpPersonalData");

            string EmpFirstName = "";
            if (!string.IsNullOrEmpty(empPersonal.EmployeeFirstName))
                EmpFirstName = empPersonal.EmployeeFirstName;

            string EmpLastName = "";
            if (!string.IsNullOrEmpty(empPersonal.EmployeeLastName))
                EmpLastName = empPersonal.EmployeeLastName;

            string EmpDisplayName = empPersonal.EmployeeDisplayName;
            string EmpFatherName = empPersonal.EmployeeFatherName;
            string EmpCurrentAddress1 = empPersonal.EmployeeCurrentAddress1;
            string EmpCurrentAddress2 = empPersonal.EmployeeCurrentAddress2;
            string EmpCurrentCity = empPersonal.EmployeeCurrentCity;
            string EmpCurrentState = empPersonal.EmployeeCurrentState;
            string EmpCurrentCountry = empPersonal.EmployeeCurrentCountry;
            string EmpCurrentPinCode = empPersonal.EmployeeCurrentPinCode;
            string EmpCurrentPhoneNo = empPersonal.EmployeeCurrentPhoneNo;

            string EmpPermanentAddress1 = "";
            string EmpPermanentAddress2 = "";
            string EmpPermanentCity = "";
            string EmpPermanentState = "";
            string EmpPermanentCountry = "";
            string EmpPermanentPinCode = "";
            string EmpPermanentPhoneNo = "";

            int? IsSameAsAddress = 0;
            bool? IsSameAsCurrentAddress = empPersonal.IsSameAsCurrentAddress;

            IsSameAsAddress = Convert.ToInt16(empPersonal.IsSameAsCurrentAddress);
            if (IsSameAsAddress == 1)
            {
                EmpPermanentAddress1 = empPersonal.EmployeeCurrentAddress1;
                EmpPermanentAddress2 = empPersonal.EmployeePermanentAddress2;
                EmpPermanentCity = empPersonal.EmployeeCurrentCity;
                EmpPermanentState = empPersonal.EmployeeCurrentState;
                EmpPermanentCountry = empPersonal.EmployeeCurrentCountry;
                EmpPermanentPinCode = empPersonal.EmployeeCurrentPinCode;
                EmpPermanentPhoneNo = empPersonal.EmployeeCurrentPhoneNo;
            }
            else
            {
                EmpPermanentAddress1 = empPersonal.EmployeePermanentAddress1;
                EmpPermanentAddress2 = empPersonal.EmployeePermanentAddress2;
                EmpPermanentCity = empPersonal.EmployeePermanentCity;
                EmpPermanentState = empPersonal.EmployeePermanentState;
                EmpPermanentCountry = empPersonal.EmployeePermanentCountry;
                EmpPermanentPinCode = empPersonal.EmployeePermanentPinCode;
                EmpPermanentPhoneNo = empPersonal.EmployeePermanentPhoneNo;
            }
            string EmpMobileNo = empPersonal.EmployeeMobileNo;


            System.DateTime? EmpDateofBirth = null;
            if (!string.IsNullOrEmpty(empPersonal.EmployeeDateofBirth))
            {
                EmpDateofBirth = Convert.ToDateTime(empPersonal.EmployeeDateofBirth);
            }

            string EmpPersonalEmailId = empPersonal.EmployeePersonalEmailId;            
            bool EmpMaritalStatus = Convert.ToBoolean(empPersonal.EmployeeMaritalStatus);
            string EmpLocation = "";

            if (empPersonal.Location != null)
            {
                string location = empPersonal.Location;
                EmpLocation = db.MMS_Employee_Location.Where(x => x.Name == location).Select(x => x.Location).FirstOrDefault();
            }
            int? EmpBloodGroupId = 0;
            if (empPersonal.EmployeeBloodGroupId != null)
            {
                string BGroupId = empPersonal.EmployeeBloodGroupId;
                EmpBloodGroupId = db.MMS_Blood_Details.Where(x => x.Descriptions == BGroupId).Select(x => x.Values).FirstOrDefault();
            }

            int CompanyICode = Convert.ToInt32(empOtherDetails.CompanyICode);
            int DepartmentICode = Convert.ToInt32(empOtherDetails.DepartmentICode);
            int DesignationICode = Convert.ToInt32(empOtherDetails.DesignationICode);
            string EmployeeNumber = empOtherDetails.EmployeeNumber;

            DateTime? EmpDateOfJoining = null;
            if (!string.IsNullOrEmpty(empOtherDetails.EmployeeDateOfJoining))
            {
                EmpDateOfJoining = Convert.ToDateTime(empOtherDetails.EmployeeDateOfJoining);
            }
            string EmpCorporateEmailId = empOtherDetails.EmployeeCorporateEmailId;
            string EmpPrimarySkill = empOtherDetails.EmployeePrimarySkill;
            string EmpPreviousEmployer = empOtherDetails.EmployeePreviousEmployer;
            int EmpPreviousExperienceYears = Convert.ToInt32(empOtherDetails.EmployeePreviousExperienceYears);
            int EmpPreviousExperienceMonths = Convert.ToInt32(empOtherDetails.EmployeePreviousExperienceMonths);
            string EmployeeReference1 = empOtherDetails.EmployeeReference1Remarks;
            string EmployeeReference2 = empOtherDetails.EmployeeReference2Remarks;

            int? SwipeICode = Convert.ToInt32(empOtherDetails.SwipeICode);
            int? GroupId = empOtherDetails.GroupId;
            int? ShiftId = empOtherDetails.ShiftId;

            string[] EmployeeBioData = null;
            byte[] EmployeeBioData1 = null;
            string[] EmployeePhoto = null;
            byte[] EmployeePhoto1 = null;

            EmployeeBioData = (empOtherDetails.files);
            EmployeePhoto = empOtherDetails.EmployeePhoto;

            DateTime? EmpDateofLeaving = null;
            if (!string.IsNullOrEmpty(empOtherDetails.EmployeeDateofLeaving))
            {
                EmpDateofLeaving = Convert.ToDateTime(empOtherDetails.EmployeeDateofLeaving);
            }
            string EmpReasonforLeaving = empOtherDetails.EmployeeReasonforLeaving;


            string EmpIM = empOtherDetails.EmployeeIM;
            string EmpLoginUserName = empOtherDetails.LoginUserName;
            string EmpLoginPassword = empOtherDetails.LoginPassword;
            string EmpPANNo = empOtherDetails.EmployeePANNo;
            string EmpPFNo = empOtherDetails.EmployeePFNo;
            string EmpESINo = empOtherDetails.EmployeeESINo;
            string EmpBankName = empOtherDetails.EmployeeBankName;
            string EmpBankAccountNo = empOtherDetails.EmployeeBankAccountNo;
            int ModifiedBy = 0;
            bool? IsActive = empOtherDetails.IsActive;
            bool? pseudo = empOtherDetails.pseudo;
            byte[] picture = empOtherDetails.picture;
            bool? nightshift = empOtherDetails.nightshift;
            //int? res = 0;

            if (ModelState.IsValid)
            {
                try
                {
                    db.sp_insert_EmployeeMaster(CompanyICode, DepartmentICode, DesignationICode, EmployeeNumber, EmpFirstName, EmpLastName, EmpDisplayName, EmpCurrentAddress1, EmpCurrentAddress2, EmpCurrentCity, EmpCurrentState, EmpCurrentCountry, EmpCurrentPinCode, EmpCurrentPhoneNo, IsSameAsCurrentAddress
                        , EmpPermanentAddress1, EmpPermanentAddress2, EmpPermanentCity, EmpPermanentState, EmpPermanentCountry, EmpPermanentPinCode, EmpPermanentPhoneNo, EmpMobileNo, EmpFatherName, EmpDateofBirth, EmpPersonalEmailId, EmpDateOfJoining, EmpCorporateEmailId
                        , EmpIM, EmpLoginUserName, EmpLoginPassword, EmpBloodGroupId, EmpPANNo, EmpPFNo, EmpESINo, EmpBankName, EmpBankAccountNo, EmpMaritalStatus, EmpPrimarySkill, EmpPreviousEmployer, EmpPreviousExperienceYears, EmpPreviousExperienceMonths, EmployeeReference1, EmployeeReference2, EmployeePhoto1,
                        "", EmployeeBioData1, "", EmpDateofLeaving, EmpReasonforLeaving, IsActive, 0, DateTime.Now, ModifiedBy, DateTime.Now, SwipeICode, EmpLocation, GroupId, ShiftId, "", pseudo, 0, picture, 0, "", nightshift);

                    int? EmpICode = db.Employee_Master.Where(x => x.LoginUserName == EmpLoginUserName).Select(x => x.EmployeeICode).FirstOrDefault();
                    int? EmpId = db.MMS_Employee_Master.Where(x => x.profile_employee_id == EmpICode).Select(x => x.Employee_Id).FirstOrDefault();

                    if (!string.IsNullOrEmpty(EmployeeBioData[0]))
                    {
                        string name = EmployeeBioData[0];
                        string[] attached = name.Split(',');

                        for (int i = 0; i < attached.Count(); i++)
                        {
                            byte[] fileContents = null;
                            BinaryFormatter bf = new BinaryFormatter();
                            MemoryStream ms = new MemoryStream();
                            bf.Serialize(ms, attached[i]);
                            fileContents = ms.ToArray();
                            string extension = Path.GetExtension(attached[i]);
                            string fileName = attached[i];

                            int? attachres = 0;
                            attachres = db.MMS_Employee_Attachment.Where(x => x.Id == EmpId && x.MMS_FileExtension == extension).Count();                           
                            if (attachres > 0)
                                db.Sp_Update_EmployeeAttachments(EmpId, fileName, extension, fileContents, false);

                            db.sp_insert_EmployeeAttachments(EmpId, fileName, extension, fileContents, true);
                        }
                    }
                    
                    TempData["Success"] = "Inserted and Mailed Successfully";

                    return RedirectToAction("ViewEmployee");
                }
                catch (Exception ex)
                {
                    TempData["Success"] = ex.ToString();
                }
            }

            return View();
        }

        public ActionResult ViewEmployee()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewAndUpdateEmployeePage1(int? EmployeeICode)
        {
            EmpPersonalViewDetails EmpIndexView = new EmpPersonalViewDetails();
            var EmpIndexDetails = CreateEmployeeViewModel.EditIndexDetails(EmployeeICode);

            foreach (var resIndex in EmpIndexDetails)
            {
                EmpIndexView.EmployeeICode = EmployeeICode;
                EmpIndexView.EmployeeFirstName = resIndex.employeeFirstName;
                EmpIndexView.EmployeeLastName = resIndex.employeeLastName;
                EmpIndexView.EmployeeDisplayName = resIndex.employeeDisplayName;
                EmpIndexView.EmployeeFatherName = resIndex.employeeFatherName;
                
                if (resIndex.isSameAsCurrentAddress != null)
                {
                    string isSameAsCurrentAddress = resIndex.isSameAsCurrentAddress.ToString();
                    if (isSameAsCurrentAddress == "True")
                        EmpIndexView.IsSameAsCurrentAddress = true;
                    else if (resIndex.isSameAsCurrentAddress.ToString() == "false")
                        EmpIndexView.IsSameAsCurrentAddress = false;
                }
                else                
                    EmpIndexView.IsSameAsCurrentAddress = false;
                

                EmpIndexView.EmployeeCurrentAddress1 = resIndex.employeeCurrentAddress1;
                EmpIndexView.EmployeeCurrentAddress2 = resIndex.employeeCurrentAddress2;
                EmpIndexView.EmployeeCurrentCity = resIndex.employeeCurrentCity;
                EmpIndexView.EmployeeCurrentState = resIndex.employeeCurrentState;
                EmpIndexView.EmployeeCurrentCountry = resIndex.employeeCurrentCountry;
                EmpIndexView.EmployeeCurrentPinCode = resIndex.employeeCurrentPinCode;
                EmpIndexView.EmployeeCurrentPhoneNo = resIndex.employeeCurrentPhoneNo;
                EmpIndexView.EmployeeMobileNo = resIndex.employeeMobileNo;
                if ((resIndex.employeeDateofBirth) != null)
                    EmpIndexView.EmployeeDateofBirth = Convert.ToDateTime(resIndex.employeeDateofBirth).ToString("dd-MM-yyyy");
                EmpIndexView.EmployeePersonalEmailId = resIndex.employeePersonalEmailId;
                if (resIndex.location != null)
                {
                    string location = resIndex.location;
                    string Emplocation = db.MMS_Employee_Location.Where(x => x.Location == location).Select(x => x.Name).FirstOrDefault();
                    EmpIndexView.Location = Emplocation;
                }
                if (resIndex.employeeBloodGroupId != null && resIndex.employeeBloodGroupId != 0)
                {
                    int BGroupId = resIndex.employeeBloodGroupId;
                    string EmpBGroup = db.MMS_Blood_Details.Where(x => x.Values == BGroupId).Select(x => x.Descriptions).FirstOrDefault();

                    EmpIndexView.EmployeeBloodGroupId = EmpBGroup;
                }

                if (resIndex.employeeMaritalStatus != null)
                {
                    string employeeMaritalStatus = resIndex.employeeMaritalStatus.ToString();
                    if (employeeMaritalStatus == "1")
                        EmpIndexView.EmployeeMaritalStatus = true;
                    else
                        EmpIndexView.EmployeeMaritalStatus = false;
                }
                else                
                    EmpIndexView.EmployeeMaritalStatus = false;
                            
            }

            return View(EmpIndexView);
        }

        [HttpPost]
        public ActionResult ViewAndUpdateEmployeePage1(EmpPersonalViewDetails resEmpIndexView)
        {
            TempData["RowId"] = resEmpIndexView.EmployeeICode;
            TempData.Keep("RowId");

            if (ModelState.IsValid)
            {
                TempData["EmpPersonalViewData"] = resEmpIndexView;
                TempData.Keep("EmpPersonalViewData");

                return RedirectToAction("ViewAndUpdateEmployeePage2");
            }

            return View();
        }

        [HttpGet]
        public ActionResult ViewAndUpdateEmployeePage2()
        {
            int EmployeeICode = Convert.ToInt32(TempData["RowId"]);
            TempData.Keep("RowId");

            ViewBag.employeeICode = EmployeeICode;
            EmpOthersViewDetails empOthersView = new EmpOthersViewDetails();

            int? EmployeeId = db.MMS_Employee_Master.Where(x => x.profile_employee_id == EmployeeICode).Select(x => x.Employee_Id).FirstOrDefault();
            var EmpViewDetails = CreateEmployeeViewModel.EditEmpOtherDetails(EmployeeICode);
            bool active = true;
            var UpdatedFile = (from files in db.MMS_Employee_Attachment
                               where files.Id == EmployeeId && (files.MMS_FileExtension == ".pdf" || files.MMS_FileExtension == ".docx") && (files.IsActive == active)
                               select new
                               {
                                   FileName = files.MMS_FileName
                               });
            TempData["AttachFiles"] = UpdatedFile;
            TempData.Keep("AttachFiles");

            var UpdatedImage = (from files in db.MMS_Employee_Attachment
                                where files.Id == EmployeeId && (files.MMS_FileExtension == ".png" || files.MMS_FileExtension == ".jpg") && (files.IsActive == active)
                                select new
                                {
                                    FileName = files.MMS_FileName
                                });
            TempData["AttachImages"] = UpdatedImage;
            TempData.Keep("AttachImages");

            foreach (var resEmp in EmpViewDetails)
            {
                empOthersView.CompanyICode = resEmp.CompanyICode;
                empOthersView.DepartmentICode = resEmp.DepartmentICode;
                empOthersView.DesignationICode = resEmp.DesignationICode;
                empOthersView.EmployeeNumber = resEmp.EmployeeNumber;
                if (resEmp.EmployeeDateOfJoining != null)
                    empOthersView.EmployeeDateOfJoining = Convert.ToDateTime(resEmp.EmployeeDateOfJoining).ToString("dd-MM-yyyy");
                empOthersView.EmployeeCorporateEmailId = resEmp.EmployeeCorporateEmailId;
                empOthersView.EmployeePrimarySkill = resEmp.EmployeePrimarySkill;
                empOthersView.EmployeePreviousExperienceYears = resEmp.EmployeePreviousExperienceYears;
                empOthersView.EmployeePreviousExperienceMonths = resEmp.EmployeePreviousExperienceMonths;
                empOthersView.SwipeICode = resEmp.SwipeICode;
                empOthersView.GroupId = resEmp.GroupId;
                empOthersView.ShiftId = resEmp.ShiftId;
                if (resEmp.EmployeeDateofLeaving != null)
                    empOthersView.EmployeeDateofLeaving = Convert.ToDateTime(resEmp.EmployeeDateofLeaving).ToString("dd-MM-yyyy");
                empOthersView.EmployeeReasonforLeaving = resEmp.EmployeeReasonforLeaving;
                empOthersView.EmployeeReference1Remarks = resEmp.EmployeeReference1Remarks;
                empOthersView.EmployeeReference2Remarks = resEmp.EmployeeReference2Remarks;
                empOthersView.IsActive = resEmp.IsActive;
                empOthersView.EmployeeBankName = resEmp.EmployeeBankName;
                empOthersView.EmployeeBankAccountNo = resEmp.EmployeeBankAccountNo;
                empOthersView.EmployeeIM = resEmp.EmployeeIM;
                empOthersView.EmployeePANNo = resEmp.EmployeePANNo;
                empOthersView.EmployeePFNo = resEmp.EmployeePFNo;
                empOthersView.EmployeeESINo = resEmp.EmployeeESINo;
                empOthersView.LoginUserName = resEmp.LoginUserName;
                empOthersView.LoginPassword = resEmp.LoginPassword;
            }

            return View(empOthersView);
        }

        [HttpPost]
        public ActionResult ViewAndUpdateEmployeePage2(EmpOthersViewDetails resEmpOthersView)
        {
            EmpPersonalViewDetails empViewPersonal = TempData["EmpPersonalViewData"] as EmpPersonalViewDetails;
            TempData.Keep("EmpPersonalViewData");

            string resMeetingMsg = string.Empty;
            string EmpFirstName = empViewPersonal.EmployeeFirstName;
            string EmpLastName = empViewPersonal.EmployeeLastName;
            string EmpDisplayName = empViewPersonal.EmployeeDisplayName;
            string EmpFatherName = empViewPersonal.EmployeeFatherName;
            string EmpCurrentAddress1 = empViewPersonal.EmployeeCurrentAddress1;
            string EmpCurrentAddress2 = empViewPersonal.EmployeeCurrentAddress2;
            string EmpCurrentCity = empViewPersonal.EmployeeCurrentCity;
            string EmpCurrentState = empViewPersonal.EmployeeCurrentState;
            string EmpCurrentCountry = empViewPersonal.EmployeeCurrentCountry;
            string EmpCurrentPinCode = empViewPersonal.EmployeeCurrentPinCode;
            string EmpCurrentPhoneNo = empViewPersonal.EmployeeCurrentPhoneNo;

            string EmpPermanentAddress1 = "";
            string EmpPermanentAddress2 = "";
            string EmpPermanentCity = "";
            string EmpPermanentState = "";
            string EmpPermanentCountry = "";
            string EmpPermanentPinCode = "";
            string EmpPermanentPhoneNo = "";

            int? IsSameAsAddress = 0;
            bool? IsSameAsCurrentAddress = empViewPersonal.IsSameAsCurrentAddress;

            IsSameAsAddress = Convert.ToInt16(empViewPersonal.IsSameAsCurrentAddress);
            if (IsSameAsAddress == 1)
            {
                EmpPermanentAddress1 = empViewPersonal.EmployeeCurrentAddress1;
                EmpPermanentAddress2 = empViewPersonal.EmployeeCurrentAddress2;
                EmpPermanentCity = empViewPersonal.EmployeeCurrentCity;
                EmpPermanentState = empViewPersonal.EmployeeCurrentState;
                EmpPermanentCountry = empViewPersonal.EmployeeCurrentCountry;
                EmpPermanentPinCode = empViewPersonal.EmployeeCurrentPinCode;
                EmpPermanentPhoneNo = empViewPersonal.EmployeeCurrentPhoneNo;
            }
            else
            {
                EmpPermanentAddress1 = empViewPersonal.EmployeePermanentAddress1;
                EmpPermanentAddress2 = empViewPersonal.EmployeePermanentAddress2;
                EmpPermanentCity = empViewPersonal.EmployeePermanentCity;
                EmpPermanentState = empViewPersonal.EmployeePermanentState;
                EmpPermanentCountry = empViewPersonal.EmployeePermanentCountry;
                EmpPermanentPinCode = empViewPersonal.EmployeePermanentPinCode;
                EmpPermanentPhoneNo = empViewPersonal.EmployeePermanentPhoneNo;
            }
            string EmpMobileNo = empViewPersonal.EmployeeMobileNo;

            System.DateTime? EmpDateofBirth = null;
            if (!string.IsNullOrEmpty(empViewPersonal.EmployeeDateofBirth))
                EmpDateofBirth = Convert.ToDateTime(empViewPersonal.EmployeeDateofBirth);

            string EmpPersonalEmailId = empViewPersonal.EmployeePersonalEmailId;
            int? EmpBloodGroupId = 0;
            if (!string.IsNullOrEmpty(empViewPersonal.EmployeeBloodGroupId))
            {
                string BGroup = empViewPersonal.EmployeeBloodGroupId;
                EmpBloodGroupId = db.MMS_Blood_Details.Where(x => x.Descriptions == BGroup).Select(x => x.Values).FirstOrDefault();

            }
            bool EmpMaritalStatus = Convert.ToBoolean(empViewPersonal.EmployeeMaritalStatus);

            string EmpLocation = "";
            if (!string.IsNullOrEmpty(empViewPersonal.Location))
            {
                string Location = empViewPersonal.Location.Trim();
                EmpLocation = db.MMS_Employee_Location.Where(x => x.Name == Location).Select(x => x.Location).FirstOrDefault();
            }


            int CompanyICode = Convert.ToInt32(resEmpOthersView.CompanyICode);
            int DepartmentICode = Convert.ToInt32(resEmpOthersView.DepartmentICode);
            int DesignationICode = Convert.ToInt32(resEmpOthersView.DesignationICode);
            string EmployeeNumber = resEmpOthersView.EmployeeNumber;

            DateTime? EmpDateOfJoining = null;
            if (!string.IsNullOrEmpty(resEmpOthersView.EmployeeDateOfJoining))
                EmpDateOfJoining = Convert.ToDateTime(resEmpOthersView.EmployeeDateOfJoining);


            string EmpCorporateEmailId = resEmpOthersView.EmployeeCorporateEmailId;
            string EmpPrimarySkill = resEmpOthersView.EmployeePrimarySkill;
            string EmpPreviousEmployer = resEmpOthersView.EmployeePreviousEmployer;

            int EmpPreviousExperienceYears = Convert.ToInt32(resEmpOthersView.EmployeePreviousExperienceYears);
            int EmpPreviousExperienceMonths = Convert.ToInt32(resEmpOthersView.EmployeePreviousExperienceMonths);

            string EmployeeReference1 = resEmpOthersView.EmployeeReference1Remarks;
            string EmployeeReference2 = resEmpOthersView.EmployeeReference2Remarks;

            int SwipeICode = Convert.ToInt32(resEmpOthersView.SwipeICode);
            int? GroupId = resEmpOthersView.GroupId;
            int? ShiftId = resEmpOthersView.ShiftId;

            string[] EmployeeBioData = null;
            byte[] EmployeeBioData1 = null;
            string[] EmployeePhoto = null;
            byte[] EmployeePhoto1 = null;

            EmployeeBioData = (resEmpOthersView.files);
            EmployeePhoto = resEmpOthersView.EmployeePhoto;

            DateTime? EmpDateofLeaving = null;
            if (!string.IsNullOrEmpty(resEmpOthersView.EmployeeDateofLeaving))
                EmpDateofLeaving = Convert.ToDateTime(resEmpOthersView.EmployeeDateofLeaving);

            string EmpReasonforLeaving = resEmpOthersView.EmployeeReasonforLeaving;
            string EmpIM = resEmpOthersView.EmployeeIM;
            string EmpLoginUserName = resEmpOthersView.LoginUserName;
            string EmpLoginPassword = resEmpOthersView.LoginPassword;
            string EmpPANNo = resEmpOthersView.EmployeePANNo;
            string EmpPFNo = resEmpOthersView.EmployeePFNo;
            string EmpESINo = resEmpOthersView.EmployeeESINo;
            string EmpBankName = resEmpOthersView.EmployeeBankName;
            string EmpBankAccountNo = resEmpOthersView.EmployeeBankAccountNo;
            int ModifiedBy = 0;

            bool? IsActive = resEmpOthersView.IsActive;
            bool? pseudo = resEmpOthersView.pseudo;
            byte[] picture = resEmpOthersView.picture;
            bool? nightshift = resEmpOthersView.nightshift;

            if (ModelState.IsValid)
            {
                int EmployeeICode = db.Employee_Master.Where(x => x.LoginUserName == EmpLoginUserName).Select(x => x.EmployeeICode).FirstOrDefault();
                int EmployeeId = db.MMS_Employee_Master.Where(x => x.profile_employee_id == EmployeeICode).Select(x => x.Employee_Id).FirstOrDefault();

                try
                {
                    db.Sp_Update_EmployeeMaster(EmployeeICode, CompanyICode, DepartmentICode, DesignationICode, EmployeeNumber, EmpFirstName, EmpLastName, EmpDisplayName, EmpCurrentAddress1, EmpCurrentAddress2, EmpCurrentCity, EmpCurrentState, EmpCurrentCountry, EmpCurrentPinCode, EmpCurrentPhoneNo, IsSameAsCurrentAddress
                        , EmpPermanentAddress1, EmpPermanentAddress2, EmpPermanentCity, EmpPermanentState, EmpPermanentCountry, EmpPermanentPinCode, EmpPermanentPhoneNo, EmpMobileNo, EmpFatherName, EmpDateofBirth, EmpPersonalEmailId, EmpDateOfJoining, EmpCorporateEmailId
                        , EmpIM, EmpLoginUserName, EmpLoginPassword, EmpBloodGroupId, EmpPANNo, EmpPFNo, EmpESINo, EmpBankName, EmpBankAccountNo, EmpMaritalStatus, EmpPrimarySkill, EmpPreviousEmployer, EmpPreviousExperienceYears, EmpPreviousExperienceMonths, EmployeeReference1, EmployeeReference2, EmployeePhoto1,
                        "", EmployeeBioData1, "", EmpDateofLeaving, EmpReasonforLeaving, IsActive, 0, DateTime.Now, ModifiedBy, DateTime.Now, SwipeICode, EmpLocation, GroupId, ShiftId, "", pseudo, 0, picture, 0, "", nightshift);


                    if (EmployeeBioData[0] != "")
                    {
                        byte[] fileContents = null;
                        BinaryFormatter bf = new BinaryFormatter();
                        MemoryStream ms = new MemoryStream();
                        if (EmployeeBioData[0] != null)
                        {
                            string name = EmployeeBioData[0];
                            string[] attached = name.Split(',');
                            for (int i = 0; i < attached.Count(); i++)
                            {
                                bf.Serialize(ms, attached[i]);
                                fileContents = ms.ToArray();
                                string extension = Path.GetExtension(attached[i].ToString());
                                string fileName = Path.GetFileName(attached[i].ToString());

                                int? attachres = 0;
                                attachres = db.MMS_Employee_Attachment.Where(x => x.Id == EmployeeId && x.MMS_FileExtension == extension).Count();
                                if (attachres > 0)
                                    db.Sp_Update_EmployeeAttachments(EmployeeId, fileName, extension, fileContents, true);
                                else
                                    db.sp_insert_EmployeeAttachments(EmployeeId, fileName, extension, fileContents, true);
                            }
                        }
                    }

                    TempData["Success"] = "Updated Successfully";
                }
                catch (Exception ex)
                {
                    TempData["Success"] = ex.ToString();
                }
            }
            return View("ViewEmployee");
        }

        [HttpPost]
        public JsonResult FindoutDupliEmpPage1(int EmployeeICode, string EmpDisplayName, string EmpMobileNumber)
        {
            string DisplayName = "";
            var MobileNumber = "";

            if (EmployeeICode != 0)
            {
                DisplayName = Convert.ToString(db.Employee_Master.Where(x => x.EmployeeDisplayName == EmpDisplayName && x.EmployeeICode != EmployeeICode).FirstOrDefault());
                if (!string.IsNullOrEmpty(EmpMobileNumber))
                    MobileNumber = Convert.ToString(db.Employee_Master.Where(x => x.EmployeeMobileNo == EmpMobileNumber && x.EmployeeICode != EmployeeICode).FirstOrDefault());
            }
            else
            {
                DisplayName = Convert.ToString(db.Employee_Master.Where(x => x.EmployeeDisplayName == EmpDisplayName).FirstOrDefault());
                if (!string.IsNullOrEmpty(EmpMobileNumber))
                    MobileNumber = Convert.ToString(db.Employee_Master.Where(x => x.EmployeeMobileNo == EmpMobileNumber).FirstOrDefault());
            }

            if (!string.IsNullOrEmpty(DisplayName))
                return Json(2);
            else if (!string.IsNullOrEmpty(MobileNumber))
                return Json(1);
            else
                return Json(0);
        }

        [HttpPost]
        public JsonResult FindoutDupliEmpPage2(int EmployeeICode, string EmpLoginUserName, string EmployeeNumber, string EmpCorpEmailId, string EmpBankAccountNo, string EmpPFNo)
        {
            var LoginUserName = "";
            var EmpNumber = "";
            var CorpEmailId = "";
            var BankAccountNo = "";
            var PFNo = "";

            if (EmployeeICode != 0)
            {
                LoginUserName = Convert.ToString(db.Employee_Master.Where(x => x.LoginUserName == EmpLoginUserName && x.EmployeeICode != EmployeeICode).FirstOrDefault());
                if (!string.IsNullOrEmpty(EmployeeNumber))
                    EmpNumber = Convert.ToString(db.Employee_Master.Where(x => x.EmployeeNumber == EmployeeNumber && x.EmployeeICode != EmployeeICode).FirstOrDefault());

                if (!string.IsNullOrEmpty(EmpCorpEmailId))
                    CorpEmailId = Convert.ToString(db.Employee_Master.Where(x => x.EmployeeCorporateEmailId == EmpCorpEmailId && x.EmployeeICode != EmployeeICode).FirstOrDefault());

                if (!string.IsNullOrEmpty(EmpBankAccountNo))
                    BankAccountNo = Convert.ToString(db.Employee_Master.Where(x => x.EmployeeBankAccountNo == EmpBankAccountNo && x.EmployeeICode != EmployeeICode).FirstOrDefault());

                if (!string.IsNullOrEmpty(EmpPFNo))
                    PFNo = Convert.ToString(db.Employee_Master.Where(x => x.EmployeePFNo == EmpPFNo && x.EmployeeICode != EmployeeICode).FirstOrDefault());
            }
            else
            {
                LoginUserName = Convert.ToString(db.Employee_Master.Where(x => x.LoginUserName == EmpLoginUserName).FirstOrDefault());
                if (!string.IsNullOrEmpty(EmployeeNumber))
                    EmpNumber = Convert.ToString(db.Employee_Master.Where(x => x.EmployeeNumber == EmployeeNumber).FirstOrDefault());

                if (!string.IsNullOrEmpty(EmpCorpEmailId))
                    CorpEmailId = Convert.ToString(db.Employee_Master.Where(x => x.EmployeeCorporateEmailId == EmpCorpEmailId).FirstOrDefault());

                if (!string.IsNullOrEmpty(EmpBankAccountNo))
                    BankAccountNo = Convert.ToString(db.Employee_Master.Where(x => x.EmployeeBankAccountNo == EmpBankAccountNo).FirstOrDefault());

                if (!string.IsNullOrEmpty(EmpPFNo))
                    PFNo = Convert.ToString(db.Employee_Master.Where(x => x.EmployeePFNo == EmpPFNo).FirstOrDefault());
            }


            if (!string.IsNullOrEmpty(LoginUserName))
                return Json(5);
            else if (!string.IsNullOrEmpty(EmpNumber))
                return Json(4);
            else if (!string.IsNullOrEmpty(CorpEmailId))
                return Json(3);
            else if (!string.IsNullOrEmpty(BankAccountNo))
                return Json(2);
            else if (!string.IsNullOrEmpty(PFNo))
                return Json(1);
            else
                return Json(0);
        }
    }
}