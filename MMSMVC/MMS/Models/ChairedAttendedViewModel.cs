using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMS.Models
{
    public class ChairedAttendedViewModel
    {

        #region dbcontext
        private static MMSCGVAKDBEntities db;

        #endregion


        #region MeetingDetails By Id

        public static dynamic GetMeetingDetails(int? meetingId)
        {
            using (db = new MMSCGVAKDBEntities())
            {
                try
                {
                    var ParticipantRes = db.sp_select_Meeting_Participant(meetingId).ToList();
                    var participantIds = ParticipantRes.Select(x => x.Participant_Id).ToList();
                    List<ChairedAttendedParticipantViewModel> MeetParticipants = new List<ChairedAttendedParticipantViewModel>();
                    var empl = db.Employee_Master.Where(x => participantIds.Contains(x.EmployeeICode)).ToList();

                    //foreach (var data in ParticipantRes)
                    //{
                    //    var part = db.Employee_Master.Where(c => c.EmployeeICode == data.Participant_Id).Select(c => c.EmployeeDisplayName).FirstOrDefault();
                    //}
                    var MeetingRes = db.sp_select_mms_meeting_master(meetingId).ToList();


                    var MeetingDetails = db.sp_select_mms_meeting_details(meetingId).ToList();

                    return new { MeetingViewResult = MeetingRes, ParticipantViewResult = empl, MeetingDetails = MeetingDetails };
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
                //
            }
        }
        #endregion

        #region MeetingTask

        public static dynamic MeetingTask(DateTime startDate, DateTime endDate, int selectedVal)
        {


            using (db = new MMSCGVAKDBEntities())
            {
                if (selectedVal == 1)
                {
                    return db.sp_select_Meeting_Id(Convert.ToInt16(HttpContext.Current.Session["LoggedUserId"]), startDate, endDate).ToList();
                }
                else
                {
                    return db.sp_select_AttendedMeeting_Name(Convert.ToInt16(HttpContext.Current.Session["LoggedUserId"]), startDate, endDate).ToList();

                }

            }

        }
        #endregion


    }
}