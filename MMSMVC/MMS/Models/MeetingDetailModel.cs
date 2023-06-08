using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMS.Models.ApiModel
{
    public class MeetingDetailModel
    {
        public int? MeetingId { get; set; }
        public string MeetingName { get; set; }
        public DateTime? MeetingDate { get; set; }
        public string MeetingTime { get; set; }
        public string MeetingVenue { get; set; }
        public string MeetingDuration { get; set; }
        public string[] files { get; set; }
        public string MeetingDepartment { get; set; }
        public string MeetingOtherDept { get; set; }
        public string Participants { get; set; }
        public string AddedParticipants { get; set; }
        public string Meetingtype { get; set; }
        public string MeetingChairperson { get; set; }
        public string MinutesOfMeeting { get; set; }
        
    }

    public class TaskDetailModel
    {
        public string MeetingTask { get; set; }
        public string MeetingParticipant { get; set; }
        public string MeetingPriority { get; set; }
        public string MeetingCompletionDate { get; set; }
        public string MeetingComments { get; set; }
        public string MeetingCategory { get; set; }

    }

    public class UpdateMeetingTask
    {       
        public string files { get; set; }
       
    }
}