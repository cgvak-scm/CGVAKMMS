using MMS.Models;
using MMS.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MMS.Controllers
{

    [SessionExpire]
    [HandleError]

    public class SearchController : Controller
    {

        private MMSCGVAKDBEntities db = new MMSCGVAKDBEntities();

        public ActionResult Index()
        {
            ViewBag.Departments = DepartmentCategoryModel.DepartmentMasterList();
            ViewBag.Categories = DepartmentCategoryModel.CategoryMasterList();

            return View();
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult Index(MMS_Meeting_Template meetingviewObj, FormCollection frm, string command)
        {
            createMeetingTask meetingTaskObj = new createMeetingTask();
            string meetingDepartment = meetingviewObj.department == null ? Request["Meeting_other_dept"] : meetingviewObj.department;

            List<string> Added_Participants = new List<string>();
            //var split = new StringConverter().ConvertTo<List<string>>(names);




            List<string> ChairPerson = new List<string>();

            //int meeting_addedParticipant = Convert.ToInt32(Request["Added_Participants"]);

            // int meeting_addedParticipant = Convert.ToInt32(Added_Participants[0]);

            if (string.Equals(command, "Save"))
            {
                try
                {
                    foreach (var item in frm["Added_Participants"].Split(','))
                    {
                        // Added_Participants.Add(item.ToString());
                        Added_Participants.Add(item.ToString());

                    }
                    string[] the_array = Added_Participants.Select(i => i.ToString()).ToArray();

                    //for (int i = 0; i <= the_array.Length; i++)
                    //{
                    //int meeting_addedParticipant = Convert.ToInt32(the_array[i]);
                    int res = CreateMeetingViewModel.InsertMeetingTemplate(meetingviewObj, meetingDepartment, Session["LoggedUserId"].ToString(), the_array, Convert.ToInt32(frm["ChairPerson"]));
                    TempData["Success"] = "Meeting template has been created Successfully";
                    //return RedirectToAction("Index");
                    //}

                }
                catch (Exception ex)
                {
                    TempData["Success"] = ex.ToString();
                }
            }


            return RedirectToAction("Index");
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Search(string term)
        {
            List<string> returnResult;

            returnResult = (from meetingMaster in db.MMS_Meeting_Master
                            where meetingMaster.Meeting_Objective.Contains(term) && meetingMaster.istemplate == true
                            orderby meetingMaster.Meeting_Objective ascending
                            select meetingMaster.Meeting_Objective
                          ).ToList();

            return Json(returnResult, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        public JsonResult GetParticipants(string deptName)
        {
            //var deptId = CreateMeetingViewModel.GetParticipants(deptName);   //Commented. Getting users from Service. 

            var participants = CreateMeetingViewModel.GetParticipantsByDepartment(deptName);

            return Json(participants, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetSelectedParticipantId(int tid)
        {
            var id = CreateMeetingViewModel.GetParticipantId(tid);
            return Json(id, JsonRequestBehavior.AllowGet);
        }


        public int CheckMeetingName(string meetingName)
        {
            int count = db.MMS_Meeting_Template.Where(x => x.meeting_name == meetingName).Count();

            return count;

        }
    }
}