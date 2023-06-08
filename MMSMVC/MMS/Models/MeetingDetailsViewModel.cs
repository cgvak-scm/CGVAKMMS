using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using MMS.Models;
using System.Web.Security;
using MMS.Security;
using System.Net.Mail;
using MMS.Models.ApiModel;
using System.Runtime.Serialization.Formatters.Binary;

namespace MMS.Models
{
    public class MeetingDetailsViewModel
    {
        private static MMSCGVAKDBEntities db;

        #region Notification summary

        public static List<sp_get_notification_summary_Result> selectNotificationSummary(int chairPerson, int? id)
        {

            using(db = new MMSCGVAKDBEntities())
            {

                return db.sp_get_notification_summary(chairPerson,
                                                    0,                    //items.Employee_Id.ToString(),
                                                                          //"all",
                                                   0,
                                                   "all",
                                                   id
                                                   ).ToList();
            }
        }
        #endregion

        #region MeetingDetails By Id
        public static List<sp_select_mms_meeting_details_Result> SelectMeetingDetails(int? id)
        {
            using(db = new MMSCGVAKDBEntities())
            {
                return db.sp_select_mms_meeting_details(id).ToList();
            }
        }

        #endregion

        #region MeetingMasterDetails By Id
        public static List<sp_select_mms_meeting_master_Result> SelectMeetingMasterDetails(int? id)
        {
            using(db = new MMSCGVAKDBEntities())
            {
                return (from meetingmstr in db.MMS_Meeting_Master
                        where meetingmstr.Meeting_Id == id
                        select new sp_select_mms_meeting_master_Result
                        {
                            Meeting_Id = meetingmstr.Meeting_Id,
                            Meeting_Objective = meetingmstr.Meeting_Objective,
                            Meeting_Date = meetingmstr.Meeting_Date,
                            Meeting_Time = meetingmstr.Meeting_Time,
                            Meeting_Venue = meetingmstr.Meeting_Venue,
                            Meeting_Type = meetingmstr.Meeting_Type,
                            Meeting_Duration = meetingmstr.Meeting_Duration,
                            Meeting_Department = meetingmstr.Meeting_Department,
                            Meeting_Chairperson = db.Employee_Master.Where(x => x.EmployeeICode == meetingmstr.Meeting_Chairperson).Select(x => x.EmployeeDisplayName).FirstOrDefault(),
                            Minutes_Of_Meeting = meetingmstr.Minutes_Of_Meeting,
                            Meeting_No_Of_Participants = meetingmstr.Meeting_No_Of_Participants,
                            istemplate = meetingmstr.istemplate

                        }).ToList();
            }
        }
        #endregion

        #region select Participants
        public static List<ParticipantsViewModel> SelectParticipants(int? chairPerson)
        {
            using(db = new MMSCGVAKDBEntities())
            {
                return (from meetingDetails in db.MMS_Meeting_Details
                        join empMaster in db.Employee_Master on meetingDetails.Participant equals empMaster.EmployeeICode
                        where meetingDetails.Chairperson == chairPerson
                        select new ParticipantsViewModel
                        {
                            employee_id = meetingDetails.Employee_Id,
                            name = empMaster.EmployeeFirstName + " " + empMaster.EmployeeLastName,
                            Participant = meetingDetails.Participant
                        }).Distinct().OrderBy(x => x.name).ToList();
            }
        }

        #endregion

        //added newly For View Meeting
        public static dynamic ViewMeeting(int schairPerson, DateTime startDate, DateTime endDate, string MeetingDepartment)
        {

            using(db = new MMSCGVAKDBEntities())
            {
                var viewmeet = db.sp_get_meeting_master_viewByDept(schairPerson, startDate, endDate, MeetingDepartment).ToList();
                return viewmeet;
            }
        }

        #region MeetingDetails

        //meeting details will be returned
        public static dynamic MeetingFetchRes(int schairPerson, string taskPeriod, DateTime startDate, DateTime endDate, int category,
                                        int participants, int? status, string priority)
        {
            db = new MMSCGVAKDBEntities();

            List<getmeetingdetailsNew_Result> meetingFetchModel1 = new List<getmeetingdetailsNew_Result>();
            List<sp_get_MeetingSearch_Summary_13072018_Result> taskFetchModel1 = new List<sp_get_MeetingSearch_Summary_13072018_Result>();
            bool isNotClosed = false;
            int drp_status = Convert.ToInt16(status);

            try
            {
                meetingFetchModel1.AddRange(db.getmeetingdetailsNew(schairPerson, participants, drp_status, startDate, endDate, priority, isNotClosed, category).Distinct());
            }
            catch(Exception ex)
            {
                //TempData["Success"] = ex.ToString();
            }
            taskFetchModel1.AddRange(db.sp_get_MeetingSearch_Summary_13072018(schairPerson, participants, drp_status, startDate, endDate, priority, category).Distinct());

            var meetingModelResult = (from meetingCritera in meetingFetchModel1
                                      where meetingCritera.Participant != null && meetingCritera.Priority != null && meetingCritera.Status != null
                                      select new MeetingModel
                                      {
                                          Meeting_Id = Convert.ToInt32(meetingCritera.Meeting_Id),
                                          Employee_Id = meetingCritera.Employee_Id,
                                          Participant = meetingCritera.Participant,
                                          NoOfComments = meetingCritera.NoOfComments,
                                          Participantname = db.Employee_Master.Where(x => x.EmployeeICode == meetingCritera.Employee_Id).Select(x => x.EmployeeDisplayName).FirstOrDefault(),
                                          Task = meetingCritera.Task,
                                          Task_Id = Convert.ToInt16(meetingCritera.Task_Id),
                                          Status = meetingCritera.Status,
                                          Completion_date = Convert.ToDateTime(meetingCritera.Completion_date),
                                          Priority = meetingCritera.Priority,
                                          Meeting_Date = Convert.ToDateTime(meetingCritera.Meeting_Date),
                                          Category = meetingCritera.Category,

                                          Seen = meetingCritera.Seen,
                                          reviewFreq1 = meetingCritera.reviewFreq,
                                          reviewdate1 = Convert.ToDateTime(meetingCritera.reviewdate),
                                          Deleted = Convert.ToInt32(meetingCritera.deleted),
                                          review_status = Convert.ToBoolean(meetingCritera.ReviewStatus)

                                      }).ToList();

            List<int> name = new List<int>();
            List<int> name1 = new List<int>();
            List<string> name22 = new List<string>();
            List<string> name2 = new List<string>();
            //foreach (var item in meetingModelResult)
            //{
            //    name.Add(Convert.ToInt32(item.Participant));
            //}
            //foreach (var item in name.Distinct())
            //{
            //    name1.Add(item);
            //}

            //foreach (var item in taskFetchModel1.Distinct().Where(x => x.participant != null).Select(x => x.participant))
            //{
            //    name22.Add(item.ToLower());
            //}
            //foreach (var item in name22.Distinct())
            //{
            //    name2.Add(item);
            //}
            List<sp_get_MeetingSearch_Summary_13072018_Result> FetchModel = new List<sp_get_MeetingSearch_Summary_13072018_Result>();
            string[] participant = new string[] { };


            //foreach (var item in taskFetchModel1.ToList().Distinct())
            //{
            IEnumerable<sp_get_MeetingSearch_Summary_13072018_Result> taskFetchM = from result in db.sp_get_MeetingSearch_Summary_13072018(schairPerson, participants, status, startDate, endDate, priority, category).ToList()
                                                                                   select new sp_get_MeetingSearch_Summary_13072018_Result
                                                                                   {

                                                                                       assigned = result.assigned,
                                                                                       reopened = result.reopened,
                                                                                       closed = result.closed,
                                                                                       completed = result.completed,
                                                                                       employee_id = result.employee_id,
                                                                                       high = result.high,
                                                                                       inprogress = result.inprogress,
                                                                                       low = result.low,
                                                                                       medium = result.medium,
                                                                                       participant = result.participant,
                                                                                       total = result.total



                                                                                   };
            //}

            List<string> names = new List<string>();
            var taskFetchModel = default(IEnumerable<object>);
            taskFetchModel = taskFetchModel1.Where(x => x.participant != null).Select(x => new
            {
                ReOpened = x.reopened,
                Assigned = x.assigned,
                Closed = x.closed,
                Completed = x.completed,
                Employee_Id = x.employee_id,
                High = x.high,
                InProgress = x.inprogress,
                Low = x.low,
                Medium = x.medium,
                Participant = Convert.ToInt32(x.participant),
                total = x.total,
                ParticipantName = db.Employee_Master.Where(c => c.EmployeeICode == x.employee_id).Select(c => c.EmployeeDisplayName).FirstOrDefault()
            }).GroupBy(x => x.Employee_Id).Select(g => g.First()).OrderBy(x => x.Participant).Distinct();
            var ReturnJsonResult = new
            {
                FetchMeetingResult = meetingModelResult.Distinct(),
                //
                FetchTaskResult = taskFetchModel.Distinct()
            };

            return ReturnJsonResult;
        }

        public static dynamic MeetingSummaryRes(int schairPerson, string taskPeriod, DateTime startDate, DateTime endDate, int template,
                                        int participants, int? status, string priority)
        {
            db = new MMSCGVAKDBEntities();

            List<getmeetingsummarydetailsNew_Result> meetingFetchModel1 = new List<getmeetingsummarydetailsNew_Result>();
            List<sp_get_MeetingSearch_Summary_01102019_Result> taskFetchModel1 = new List<sp_get_MeetingSearch_Summary_01102019_Result>();
            bool isNotClosed = false;
            int drp_status = Convert.ToInt16(status);

            //try
            //{
            meetingFetchModel1.AddRange(db.getmeetingsummarydetailsNew(schairPerson, participants, drp_status, startDate, endDate, priority, isNotClosed, template).Distinct());
            //}
            //catch (Exception ex)
            //{
            //    //TempData["Success"] = ex.ToString();
            //}
            taskFetchModel1.AddRange(db.sp_get_MeetingSearch_Summary_01102019(schairPerson, participants, drp_status, startDate, endDate, priority, template).Distinct());

            var meetingModelResult = (from meetingCritera in meetingFetchModel1
                                      where meetingCritera.Participant != null && meetingCritera.Priority != null && meetingCritera.Status != null
                                      select new MeetingModel
                                      {
                                          Meeting_Id = Convert.ToInt32(meetingCritera.Meeting_Id),
                                          Employee_Id = meetingCritera.Employee_Id,
                                          Participant = meetingCritera.Participant,
                                          NoOfComments = meetingCritera.NoOfComments,
                                          Participantname = db.Employee_Master.Where(x => x.EmployeeICode == meetingCritera.Employee_Id).Select(x => x.EmployeeDisplayName).FirstOrDefault(),
                                          Task = meetingCritera.Task,
                                          Task_Id = Convert.ToInt16(meetingCritera.Task_Id),
                                          Status = meetingCritera.Status,
                                          Completion_date = Convert.ToDateTime(meetingCritera.Completion_date),
                                          Priority = meetingCritera.Priority,
                                          Meeting_Date = Convert.ToDateTime(meetingCritera.Meeting_Date),
                                          Category = meetingCritera.TemplateName,

                                          Seen = meetingCritera.Seen,
                                          reviewFreq1 = meetingCritera.reviewFreq,
                                          reviewdate1 = Convert.ToDateTime(meetingCritera.reviewdate),
                                          Deleted = Convert.ToInt32(meetingCritera.deleted),
                                          review_status = Convert.ToBoolean(meetingCritera.ReviewStatus)

                                      }).ToList();

            List<int> name = new List<int>();
            List<int> name1 = new List<int>();
            List<string> name22 = new List<string>();
            List<string> name2 = new List<string>();
            //foreach (var item in meetingModelResult)
            //{
            //name.Add(Convert.ToInt32(item.Participant));
            //}
            //foreach (var item in name.Distinct())
            //{
            //    name1.Add(item);
            //}

            //foreach (var item in taskFetchModel1.Distinct().Where(x => x.participant != null).Select(x => x.participant))
            //{
            //    name22.Add(item.ToLower());
            //}
            //foreach (var item in name22.Distinct())
            //{
            //    name2.Add(item);
            //}
            List<sp_get_MeetingSearch_Summary_13072018_Result> FetchModel = new List<sp_get_MeetingSearch_Summary_13072018_Result>();
            string[] participant = new string[] { };


            //foreach(var item in taskFetchModel1.ToList().Distinct())
            //{
            IEnumerable<sp_get_MeetingSearch_Summary_13072018_Result> taskFetchM = from result in db.sp_get_MeetingSearch_Summary_13072018(schairPerson, participants, status, startDate, endDate, priority, template).ToList()
                                                                                   select new sp_get_MeetingSearch_Summary_13072018_Result
                                                                                   {

                                                                                       assigned = result.assigned,
                                                                                       reopened = result.reopened,
                                                                                       closed = result.closed,
                                                                                       completed = result.completed,
                                                                                       employee_id = result.employee_id,
                                                                                       high = result.high,
                                                                                       inprogress = result.inprogress,
                                                                                       low = result.low,
                                                                                       medium = result.medium,
                                                                                       participant = result.participant,
                                                                                       total = result.total
                                                                                   };
            //}

            List<string> names = new List<string>();
            var taskFetchModel = default(IEnumerable<object>);
            taskFetchModel = taskFetchModel1.Where(x => x.participant != null).Select(x => new
            {
                ReOpened = x.reopened,
                Assigned = x.assigned,
                Closed = x.closed,
                Completed = x.completed,
                Employee_Id = x.employee_id,
                High = x.high,
                InProgress = x.inprogress,
                Low = x.low,
                Medium = x.medium,
                Participant = Convert.ToInt32(x.participant),
                total = x.total,
                ParticipantName = db.Employee_Master.Where(c => c.EmployeeICode == x.employee_id).Select(c => c.EmployeeDisplayName).FirstOrDefault()
            }).GroupBy(x => x.Employee_Id).Select(g => g.First()).OrderBy(x => x.Participant).Distinct();

            var ReturnJsonResult = new
            {
                FetchMeetingResult = meetingModelResult.Distinct(),
                //
                FetchTaskResult = taskFetchModel.Distinct()
            };

            return ReturnJsonResult;
        }

        /// <summary>
        /// FIRST REPORT
        /// </summary>
        /// <param name="ChairPerson"></param>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <param name="Participant"></param>
        /// <returns></returns>
        public static dynamic ReportFetchRes(int? ChairPerson, DateTime? SDate, DateTime? EDate, int? Participant)
        {
            db = new MMSCGVAKDBEntities();
            List<sp_getCompReport_2442018_Result> completionreport = new List<sp_getCompReport_2442018_Result>();

            for(int i = 0; i >= completionreport.Count();)
            {
                completionreport.AddRange(db.sp_getCompReport_2442018(SDate, EDate, ChairPerson, Convert.ToInt16(Participant)));
            }

            foreach(var item in completionreport.ToList())
            {
                IEnumerable<sp_getCompReport_2442018_Result> completion_report = from result in db.sp_getCompReport_2442018(SDate, EDate, ChairPerson, Convert.ToInt16(Participant)).ToList()
                                                                                 select new sp_getCompReport_2442018_Result
                                                                                 {
                                                                                     HighAssigned = result.HighAssigned,
                                                                                     MediumAssigned = result.MediumAssigned,
                                                                                     LowAssigned = result.LowAssigned,
                                                                                     HighCompleted = result.HighCompleted,
                                                                                     MediumCompleted = result.MediumCompleted,
                                                                                     LowCompleted = result.LowCompleted,
                                                                                     HighInProgress = result.HighInProgress,
                                                                                     MediumInProgress = result.MediumInProgress,
                                                                                     LowInProgress = result.LowInProgress,
                                                                                     HighYetToBegin = result.HighYetToBegin,
                                                                                     MediumYetToBegin = result.MediumYetToBegin,
                                                                                     LowYetToBegin = result.LowYetToBegin,
                                                                                     HighAVERAGEDays = result.HighAVERAGEDays,
                                                                                     MediumAVERAGEDays = result.MediumAVERAGEDays,
                                                                                     LowAVERAGEDays = result.LowAVERAGEDays,
                                                                                     HighMAXIMUMDays = result.HighMAXIMUMDays,
                                                                                     MediumMAXIMUMDays = result.MediumMAXIMUMDays,
                                                                                     LowMAXIMUMDays = result.LowMAXIMUMDays,
                                                                                     HighMINIMUMDays = result.HighMINIMUMDays,
                                                                                     MediumMINIMUMDays = result.MediumMINIMUMDays,
                                                                                     LowMINIMUMDays = result.LowMINIMUMDays,
                                                                                     HighClosed = result.HighClosed,
                                                                                     HighReOpened = result.HighReOpened,
                                                                                     MediumClosed = result.MediumClosed,
                                                                                     MediumReOpened = result.MediumReOpened,
                                                                                     LowClosed = result.LowClosed,
                                                                                     LowReOpened = result.LowReOpened
                                                                                 };
            }

            var ReturnJsonResult = new
            {
                FetchReportResult = completionreport.OrderBy(x => x.HighAssigned)
            };
            return ReturnJsonResult.FetchReportResult;
        }
        /// <summary>
        /// CompletionEffectivenessReport Report 2
        /// </summary>
        /// <param name="ChairPerson"></param>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <param name="Participant"></param>
        /// <returns></returns>
        public static dynamic EffectivenessFetchRes(int ChairPerson, DateTime? SDate, DateTime? EDate, string Participant)
        {
            db = new MMSCGVAKDBEntities();
            List<sp_getCompletionEffectivenessReport_Result> Effectivenessreport = new List<sp_getCompletionEffectivenessReport_Result>();
            List<int> Task_Id = db.MMS_Meeting_Details.Where(x => x.Chairperson == ChairPerson && x.ReviewFrequencyID != null).Select(x => x.Task_Id).ToList();
            string ReviewFrequencyID = string.Empty;
            foreach(int ID in Task_Id)
            {
                for(int i = 0; i >= Effectivenessreport.Count();)
                {

                    ReviewFrequencyID = db.MMS_Meeting_Details.Where(x => x.Task_Id == ID).Select(x => x.ReviewFrequencyID).Single().ToString();
                    if(ReviewFrequencyID != null)
                    {
                        Effectivenessreport.AddRange(db.sp_getCompletionEffectivenessReport(SDate, EDate, Convert.ToInt16(ChairPerson), Convert.ToInt16(Participant), ID, Convert.ToInt16(ReviewFrequencyID)));
                    }
                }
            }

            effectivenessresultvm Effectiveness = new effectivenessresultvm();

            foreach(var item in Effectivenessreport.ToList())
            {
                Effectiveness.HighCompleted = item.HighCompleted.Value;
                Effectiveness.LowCompleted = item.LowCompleted.Value;
                Effectiveness.MediumCompleted = item.MediumCompleted.Value;
                Effectiveness.HighTotal = item.HighTotal.Value;
                Effectiveness.MediumTotal = item.MediumTotal.Value;
                Effectiveness.LowTotal = item.LowTotal.Value;
                Effectiveness.HighDelay = Effectiveness.HighDelay + item.HighDelay.Value;
                Effectiveness.MediumDelay = Effectiveness.MediumDelay + item.MediumDelay.Value;
                Effectiveness.LowDelay = Effectiveness.LowDelay + item.LowDelay.Value;

            }
            var ReturnJsonResults = new
            {
                FetchReportResult = Effectiveness
            };
            return ReturnJsonResults.FetchReportResult;
        }


        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static dynamic MeetingFetchIndividualRes(int schairPerson, string taskPeriod, DateTime startDate, DateTime endDate, int category,
                                       int participants, int status, string priority)
        {
            db = new MMSCGVAKDBEntities();
            List<getmeetingdetailsNew_Result> meetingFetchModel1 = new List<getmeetingdetailsNew_Result>();
            List<getmeetingdetailsNew_Result> meetingFetcher = new List<getmeetingdetailsNew_Result>();
            List<sp_get_MeetingSearch_Summary_13072018_Result> taskFetchModel1 = new List<sp_get_MeetingSearch_Summary_13072018_Result>();
            bool isNotClosed = false;
            int drp_status = status;

            meetingFetchModel1.AddRange(db.getmeetingdetailsNew(schairPerson, participants, drp_status, startDate, endDate, priority, isNotClosed, category));
            meetingFetcher.AddRange(db.getmeetingdetailsNew(schairPerson, participants, drp_status, startDate, endDate, priority, isNotClosed, category));
            taskFetchModel1.AddRange(db.sp_get_MeetingSearch_Summary_13072018(schairPerson, participants, drp_status, startDate, endDate, priority, category));

            MeetingModel md = new MeetingModel();
            var meetingFetcherModelResult = (from meetingCritera in meetingFetcher
                                             where meetingCritera.Participant != null && meetingCritera.Priority != null && meetingCritera.Status != null
                                             select new MeetingModel
                                             {

                                                 Meeting_Id = Convert.ToInt32(meetingCritera.Meeting_Id),
                                                 Employee_Id = meetingCritera.Employee_Id,
                                                 Participant = meetingCritera.Participant,
                                                 Participantname = db.Employee_Master.Where(x => x.EmployeeICode == meetingCritera.Employee_Id).Select(x => x.EmployeeDisplayName).FirstOrDefault(),
                                                 Task = meetingCritera.Task,
                                                 Task_Id = Convert.ToInt16(meetingCritera.Task_Id),
                                                 Status = meetingCritera.Status,
                                                 Completion_date = Convert.ToDateTime(meetingCritera.Completion_date),
                                                 Priority = meetingCritera.Priority,
                                                 Meeting_Date = Convert.ToDateTime(meetingCritera.Meeting_Date),
                                                 Category = meetingCritera.Category,
                                                 Seen = meetingCritera.Seen,
                                                 reviewFreq1 = meetingCritera.reviewFreq,
                                                 reviewdate1 = Convert.ToDateTime(meetingCritera.reviewdate),
                                                 Deleted = Convert.ToInt32(meetingCritera.deleted)


                                                 ,

                                                 review_status = Convert.ToBoolean(meetingCritera.ReviewStatus)


                                             }).ToList().Distinct();

            var meetingModelResult = (from meetingCritera in meetingFetchModel1
                                      where meetingCritera.Participant != null && meetingCritera.Priority != null && meetingCritera.Status != null
                                      select new MeetingModel
                                      {
                                          Meeting_Id = Convert.ToInt32(meetingCritera.Meeting_Id),
                                          Employee_Id = meetingCritera.Employee_Id,
                                          Participant = meetingCritera.Participant,
                                          Participantname = db.Employee_Master.Where(x => x.EmployeeICode == meetingCritera.Employee_Id).Select(x => x.EmployeeDisplayName).FirstOrDefault(),
                                          Task = meetingCritera.Task,
                                          Task_Id = meetingCritera.Task_Id,
                                          Status = meetingCritera.Status,
                                          Completion_date = meetingCritera.Completion_date,
                                          Priority = meetingCritera.Priority,
                                          Meeting_Date = Convert.ToDateTime(meetingCritera.Meeting_Date),
                                          Category = meetingCritera.Category,
                                          Seen = meetingCritera.Seen,
                                          reviewFreq1 = meetingCritera.reviewFreq,
                                          reviewdate1 = Convert.ToDateTime(meetingCritera.reviewdate),
                                          Deleted = Convert.ToInt32(meetingCritera.deleted)


                                          ,

                                          review_status = Convert.ToBoolean(meetingCritera.ReviewStatus)

                                      }).ToList().Distinct();

            List<sp_get_MeetingSearch_Summary_13072018_Result> taskFetchModel = new List<sp_get_MeetingSearch_Summary_13072018_Result>();

            foreach(var item in taskFetchModel1.ToList())
            {
                IEnumerable<sp_get_MeetingSearch_Summary_13072018_Result> taskFetchM = from result in db.sp_get_MeetingSearch_Summary_13072018(schairPerson, participants, status, startDate, endDate, priority, category).Distinct().ToList()
                                                                                       select new sp_get_MeetingSearch_Summary_13072018_Result
                                                                                       {
                                                                                           assigned = result.assigned,
                                                                                           reopened = result.reopened,
                                                                                           closed = result.closed,
                                                                                           completed = result.completed,
                                                                                           employee_id = result.employee_id,
                                                                                           high = result.high,
                                                                                           inprogress = result.inprogress,
                                                                                           low = result.low,
                                                                                           medium = result.medium,
                                                                                           participant = result.participant,
                                                                                           total = result.total
                                                                                       };
            }

            var taskFetchModels = default(IEnumerable<object>);
            taskFetchModels = taskFetchModel1.Where(x => x.participant != null).Select(x => new
            {
                ReOpened = x.reopened,
                Assigned = x.assigned,
                Closed = x.closed,
                Completed = x.completed,
                Employee_Id = x.employee_id,
                High = x.high,
                InProgress = x.inprogress,
                Low = x.low,
                Medium = x.medium,
                Participant = db.Employee_Master.Where(c => c.EmployeeICode == x.employee_id).Select(c => c.EmployeeDisplayName).FirstOrDefault(),
                total = x.total
            }).Distinct().GroupBy(x => x.Employee_Id).Select(g => g.First()).OrderBy(x => x.Participant);

            if(priority.ToLower().Trim() == "all")
            {
                var ReturnJsonResult = new
                {
                    FetchMeetingResult = meetingModelResult.Distinct(),
                    FetchTaskResult = taskFetchModels//.Where(x => x.Participant != null).GroupBy(x => x.Employee_Id).Select(g => g.First()).OrderBy(x => x.Participant)
                };
                return ReturnJsonResult;
            }
            else
            {
                var ReturnJsonResult = new
                {
                    FetchMeetingResult = meetingFetcherModelResult.Distinct().OrderBy(x => x.Task),
                    FetchTaskResult = taskFetchModels//.Where(x => x.Participant != null).GroupBy(x => x.Employee_Id).Select(g => g.First()).OrderBy(x => x.Participant).Distinct()
                    // FetchTaskResult = taskFetchModel1.Distinct()
                };
                return ReturnJsonResult;
            }
        }

        public static dynamic MeetingSummaryIndividualRes(int schairPerson, string taskPeriod, DateTime startDate, DateTime endDate, int template,
                                       int participants, int status, string priority)
        {
            db = new MMSCGVAKDBEntities();
            List<getmeetingdetailsNew_Result> meetingFetchModel1 = new List<getmeetingdetailsNew_Result>();
            List<getmeetingdetailsNew_Result> meetingFetcher = new List<getmeetingdetailsNew_Result>();
            List<sp_get_MeetingSearch_Summary_13072018_Result> taskFetchModel1 = new List<sp_get_MeetingSearch_Summary_13072018_Result>();
            bool isNotClosed = false;
            int drp_status = status;

            meetingFetchModel1.AddRange(db.getmeetingdetailsNew(schairPerson, participants, drp_status, startDate, endDate, priority, isNotClosed, template));
            meetingFetcher.AddRange(db.getmeetingdetailsNew(schairPerson, participants, drp_status, startDate, endDate, priority, isNotClosed, template));
            taskFetchModel1.AddRange(db.sp_get_MeetingSearch_Summary_13072018(schairPerson, participants, drp_status, startDate, endDate, priority, template));

            MeetingModel md = new MeetingModel();
            var meetingFetcherModelResult = (from meetingCritera in meetingFetcher
                                             where meetingCritera.Participant != null && meetingCritera.Priority != null && meetingCritera.Status != null
                                             select new MeetingModel
                                             {

                                                 Meeting_Id = Convert.ToInt32(meetingCritera.Meeting_Id),
                                                 Employee_Id = meetingCritera.Employee_Id,
                                                 Participant = meetingCritera.Participant,
                                                 Participantname = db.Employee_Master.Where(x => x.EmployeeICode == meetingCritera.Employee_Id).Select(x => x.EmployeeDisplayName).FirstOrDefault(),
                                                 Task = meetingCritera.Task,
                                                 Task_Id = Convert.ToInt16(meetingCritera.Task_Id),
                                                 Status = meetingCritera.Status,
                                                 Completion_date = Convert.ToDateTime(meetingCritera.Completion_date),
                                                 Priority = meetingCritera.Priority,
                                                 Meeting_Date = Convert.ToDateTime(meetingCritera.Meeting_Date),
                                                 Category = meetingCritera.Category,
                                                 Seen = meetingCritera.Seen,
                                                 reviewFreq1 = meetingCritera.reviewFreq,
                                                 reviewdate1 = Convert.ToDateTime(meetingCritera.reviewdate),
                                                 Deleted = Convert.ToInt32(meetingCritera.deleted)


                                                 ,

                                                 review_status = Convert.ToBoolean(meetingCritera.ReviewStatus)


                                             }).ToList().Distinct();

            var meetingModelResult = (from meetingCritera in meetingFetchModel1
                                      where meetingCritera.Participant != null && meetingCritera.Priority != null && meetingCritera.Status != null
                                      select new MeetingModel
                                      {
                                          Meeting_Id = Convert.ToInt32(meetingCritera.Meeting_Id),
                                          Employee_Id = meetingCritera.Employee_Id,
                                          Participant = meetingCritera.Participant,
                                          Participantname = db.Employee_Master.Where(x => x.EmployeeICode == meetingCritera.Employee_Id).Select(x => x.EmployeeDisplayName).FirstOrDefault(),
                                          Task = meetingCritera.Task,
                                          Task_Id = meetingCritera.Task_Id,
                                          Status = meetingCritera.Status,
                                          Completion_date = meetingCritera.Completion_date,
                                          Priority = meetingCritera.Priority,
                                          Meeting_Date = Convert.ToDateTime(meetingCritera.Meeting_Date),
                                          Category = meetingCritera.Category,
                                          Seen = meetingCritera.Seen,
                                          reviewFreq1 = meetingCritera.reviewFreq,
                                          reviewdate1 = Convert.ToDateTime(meetingCritera.reviewdate),
                                          Deleted = Convert.ToInt32(meetingCritera.deleted)


                                          ,

                                          review_status = Convert.ToBoolean(meetingCritera.ReviewStatus)

                                      }).ToList().Distinct();

            List<sp_get_MeetingSearch_Summary_13072018_Result> taskFetchModel = new List<sp_get_MeetingSearch_Summary_13072018_Result>();

            foreach(var item in taskFetchModel1.ToList())
            {
                IEnumerable<sp_get_MeetingSearch_Summary_13072018_Result> taskFetchM = from result in db.sp_get_MeetingSearch_Summary_13072018(schairPerson, participants, status, startDate, endDate, priority, template).Distinct().ToList()
                                                                                       select new sp_get_MeetingSearch_Summary_13072018_Result
                                                                                       {
                                                                                           assigned = result.assigned,
                                                                                           reopened = result.reopened,
                                                                                           closed = result.closed,
                                                                                           completed = result.completed,
                                                                                           employee_id = result.employee_id,
                                                                                           high = result.high,
                                                                                           inprogress = result.inprogress,
                                                                                           low = result.low,
                                                                                           medium = result.medium,
                                                                                           participant = result.participant,
                                                                                           total = result.total
                                                                                       };
            }

            var taskFetchModels = default(IEnumerable<object>);
            taskFetchModels = taskFetchModel1.Where(x => x.participant != null).Select(x => new
            {
                ReOpened = x.reopened,
                Assigned = x.assigned,
                Closed = x.closed,
                Completed = x.completed,
                Employee_Id = x.employee_id,
                High = x.high,
                InProgress = x.inprogress,
                Low = x.low,
                Medium = x.medium,
                Participant = db.Employee_Master.Where(c => c.EmployeeICode == x.employee_id).Select(c => c.EmployeeDisplayName).FirstOrDefault(),
                total = x.total
            }).Distinct().GroupBy(x => x.Employee_Id).Select(g => g.First()).OrderBy(x => x.Participant);

            if(priority.ToLower().Trim() == "all")
            {
                var ReturnJsonResult = new
                {
                    FetchMeetingResult = meetingModelResult.Distinct(),
                    FetchTaskResult = taskFetchModels//.Where(x => x.Participant != null).GroupBy(x => x.Employee_Id).Select(g => g.First()).OrderBy(x => x.Participant)
                };
                return ReturnJsonResult;
            }
            else
            {
                var ReturnJsonResult = new
                {
                    FetchMeetingResult = meetingFetcherModelResult.Distinct().OrderBy(x => x.Task),
                    FetchTaskResult = taskFetchModels//.Where(x => x.Participant != null).GroupBy(x => x.Employee_Id).Select(g => g.First()).OrderBy(x => x.Participant).Distinct()
                    // FetchTaskResult = taskFetchModel1.Distinct()
                };
                return ReturnJsonResult;
            }
        }

        #endregion

        #region EditMeetingDetails
        public static dynamic editMeetingDetails(int taskId, int? Meeting_id)
        {
            using(db = new MMSCGVAKDBEntities())
            {
                var meetingResult = (from meetingDetailstemp in db.MMS_Meeting_Details
                                     where meetingDetailstemp.Meeting_Id == Meeting_id
                                     && meetingDetailstemp.Task_Id == taskId

                                     select new
                                     {
                                         reviewFrequencyId = meetingDetailstemp.ReviewFrequencyID,
                                         meetingpriority = meetingDetailstemp.Priority

                                     }).FirstOrDefault();

                if(meetingResult.reviewFrequencyId != 0 && meetingResult.meetingpriority != "undefined")
                {
                    var res = (from meetingDetails in db.MMS_Meeting_Details
                               join reviewfrequency in db.MMS_Review_Frequency on meetingDetails.ReviewFrequencyID equals reviewfrequency.Freq_In_Days
                               where meetingDetails.Meeting_Id == Meeting_id
                                          && meetingDetails.Task_Id == taskId                   //Added this condition 
                               select new
                               {
                                   meetingId = meetingDetails.Meeting_Id,
                                   task = meetingDetails.Task,
                                   participant = meetingDetails.Participant,
                                   employee_id = meetingDetails.Employee_Id,
                                   completion_date = meetingDetails.Completion_Date,/*.ToString("dd/MMM/yyyy hh:mm:ss tt")*/
                                   priority = meetingDetails.Priority,
                                   category = meetingDetails.Category,
                                   status = meetingDetails.Status,
                                   comments = meetingDetails.Comments,
                                   task_id = meetingDetails.Task_Id,
                                   reviewDate = meetingDetails.NextReviewDate,
                                   reviewfreq = reviewfrequency.Freq_In_Days
                               }).FirstOrDefault();
                    return res;
                }
                else
                {
                    DateTime tempreviewDate = DateTime.Now.AddDays(1);
                    var result = (from meetingDetails in db.MMS_Meeting_Details
                                  where meetingDetails.Meeting_Id == Meeting_id
                                             && meetingDetails.Task_Id == taskId                   //Added this condition 
                                  select new
                                  {
                                      meetingId = meetingDetails.Meeting_Id,
                                      task = meetingDetails.Task,
                                      participant = meetingDetails.Participant,
                                      employee_id = meetingDetails.Employee_Id,
                                      completion_date = meetingDetails.Completion_Date,/*.ToString("dd/MMM/yyyy hh:mm:ss tt")*/
                                      priority = "High",
                                      category = meetingDetails.Category,
                                      status = meetingDetails.Status,
                                      comments = meetingDetails.Comments,
                                      task_id = meetingDetails.Task_Id,
                                      reviewDate = tempreviewDate,
                                      reviewfreq = 1
                                  }).FirstOrDefault();

                    return result;
                }
            }
        }
        #endregion

        public static string deleteMeetingDetails(int taskId, int? Meeting_id)
        {
            MMS_Meeting_Details meetingDetail = new MMS_Meeting_Details();
            string message = string.Empty;
            using(db = new MMSCGVAKDBEntities())
            {
                try
                {
                    var delres = db.MMS_Meeting_Details.Where(s => s.Task_Id == taskId).FirstOrDefault();
                    delres.IsDeleted = 1;
                    db.SaveChanges();
                    message = "Delete Successful";
                }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
                catch(Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
                {
                    message = "Try Again";
                }

                return message;
            }

        }

        #region  Review



        public static string Review(int taskId, int? Meeting_id)
        {
            MMS_Meeting_Details meetingDetail = new MMS_Meeting_Details();
            string message = string.Empty;

            using(db = new MMSCGVAKDBEntities())
            {
                try
                {
                    // var res = db.MMS_Track_ReviewTasks.Where(s => s.TaskId == taskId).FirstOrDefault();
                    MMS_Track_ReviewTasks reviewTasks = new MMS_Track_ReviewTasks();
                    reviewTasks.TaskId = taskId;
                    reviewTasks.ActualReviewedDate = DateTime.Now;
                    reviewTasks.Review_Status = true;

                    reviewTasks.NextReviewDate = db.MMS_Meeting_Details.Where(x => x.Task_Id == taskId).Select(x => x.NextReviewDate).FirstOrDefault();

                    db.MMS_Track_ReviewTasks.Add(reviewTasks);
                    //  db.Entry(meetingDetail).State = EntityState.Deleted;
                    db.SaveChanges();
                    message = "Review Details Has Been Inserted Successfully";
                }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
                catch(Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
                {
                    message = "Try Again";
                }

                return message;
            }

        }


        #endregion

        #region UpdateMeetingDetails
        public static string UpdateMeetingDetails(int respPerson, DateTime? date, int taskStatus, int category, string comments, DateTime? revdate, int meetingId, int empId, string task, int taskId, int commented_by, int ReviewFrequency, string Priority)
        {
            StringBuilder strHtml = new StringBuilder();
            string resMeetingMsg = string.Empty;
            int? resStatus = 0;
            string resPriority = string.Empty;
            string resSubject = string.Empty;



            using(db = new MMSCGVAKDBEntities())
            {
                var updateCount = db.Sp_Update_MMS_Meeting_Details_Change(empId, taskId, date, taskStatus, category, meetingId, comments, revdate, ReviewFrequency, commented_by, task, Priority);
                var meetingStatus = db.sp_select_meeting_status(taskId).ToList();

                var employeeMail = (String)null;
                string lastSavedComment = db.MMS_Meeting_Details.Where(x => x.Employee_Id == empId && x.Task_Id == taskId && x.Meeting_Id == meetingId && x.Task == task).Select(x => x.Comments).FirstOrDefault();
                employeeMail = db.sp_get_EmailId(empId).First();
                string empName = db.Employee_Master.Where(x => x.EmployeeICode == respPerson).Select(x => x.EmployeeDisplayName).FirstOrDefault();
                int mailresp = 0;
                if(taskStatus == 4)
                {
                    var MeetingDetails = db.sp_get_meeting_details(meetingId).ToList();
                    var meetingDetails = db.MMS_Meeting_Details.Where(x => x.Task_Id == taskId).FirstOrDefault();
                    meetingDetails.Percentage_Completed = 0;
                    db.Entry(meetingDetails).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    foreach(var item in MeetingDetails)
                    {
                        resStatus = item.Status;
                        resPriority = item.Priority;
                    }

                    strHtml.Append("<div style='margin-top:10px; Font-Weight:Bold;Font-Size:12px;'>Task: <i> " + task + "</i></div>");
                    strHtml.Append("<div style='Font-Weight:Bold;Font-Size:12px;'>Completion date: <i> " + date + "</i></div>");
                    strHtml.Append("<div style='Font-Weight:Bold;Font-Size:12px;'>Responsible Person: <i> " + empName + "</i></div>");
                    strHtml.Append("<div style='margin-bottom:10px;Font-Weight:Bold;Font-Size:12px;'>Priority: <i> " + resPriority + "</i></div>");

                    // Getcommentslist will be appended
                    strHtml.Append(GetCommentsListByMail(meetingId, taskId, respPerson, empId, "Re-open"));

                    resSubject = "Task Re-opended: " + task;

                    //mail function to be called  
                    //mailresp return 1 the Mail Send Sucess or return -1 Not success
                    mailresp = MailFunctionModel.MailUpdateDetails(employeeMail.ToString(), resSubject, strHtml, empName, taskStatus, HttpContext.Current.Session["LoggedEmail"].ToString(), task);
                }
                else
                {
                    //CHECK
                    strHtml.Append(comments);
                    lastSavedComment = db.MMS_Meeting_Details.Where(x => x.Employee_Id == empId && x.Task_Id == taskId && x.Meeting_Id == meetingId && x.Task == task).Select(x => x.Comments).FirstOrDefault();
                    if(lastSavedComment != comments)
                    {
                        //mailresp return 1 the Mail Send Success or return -1 Not sucess
                        mailresp = MailFunctionModel.MailUpdateDetails(employeeMail.ToString(), resSubject, strHtml, empName, taskStatus, HttpContext.Current.Session["LoggedEmail"].ToString(), task);
                    }
                }
            }
            if(taskStatus == 4)
            {
                return "Mailed and Updated Successfully";
            }
            else
            {
                return "Updated Successfully";
            }
        }
        public static string UpdateMeetingDetails1(int respPerson, DateTime? date, int taskStatus, int category, string comments, int meetingId, int empId, string task, int taskId, int percentage, int commented_by, int ReviewFrequency, string Priority)
        {
            StringBuilder strHtml = new StringBuilder();
            string resMeetingMsg = string.Empty;
            int? resStatus = 0;
            string resPriority = string.Empty;
            string resSubject = string.Empty;
            string empName = db.Employee_Master.Where(x => x.EmployeeICode == respPerson).Select(x => x.EmployeeDisplayName).FirstOrDefault();
            int mailresp = 0;
            using(db = new MMSCGVAKDBEntities())
            {
                meetingId = Convert.ToInt32(db.sp_get_meeting_id().FirstOrDefault());

                var updateCount = db.Sp_Update_MMS_Meeting_Details_Change(empId, taskId, DateTime.Now, taskStatus, Convert.ToInt16(category), meetingId, comments, DateTime.Now, ReviewFrequency, commented_by, task, Priority);
                var meetingStatus = db.sp_select_meeting_status(taskId).ToList();

                //getting employee mail id
                var employeeMail = db.sp_get_EmailId(empId).First();
                if(taskStatus == 3)
                {
                    var meetingDetails = db.MMS_Meeting_Details.Where(x => x.Task_Id == taskId).FirstOrDefault();
                    meetingDetails.Percentage_Completed = percentage;
                    db.Entry(meetingDetails).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                if(taskStatus == 4)
                {
                    var MeetingDetails = db.sp_get_meeting_details(meetingId).ToList();
                    //to change the reopened tasks completed percentage value
                    var meetingDetails = db.MMS_Meeting_Details.Where(x => x.Task_Id == taskId).FirstOrDefault();
                    meetingDetails.Percentage_Completed = 0;
                    db.Entry(meetingDetails).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    foreach(var item in MeetingDetails)
                    {
                        resStatus = item.Status;
                        resPriority = item.Priority;
                    }

                    strHtml.Append("<div style='margin-top:10px; Font-Weight:Bold;Font-Size:12px;'>Task: <i> " + task + "</i></div>");
                    strHtml.Append("<div style='Font-Weight:Bold;Font-Size:12px;'>Completion date: <i> " + date + "</i></div>");
                    strHtml.Append("<div style='Font-Weight:Bold;Font-Size:12px;'>Responsible Person: <i> " + respPerson + "</i></div>");
                    strHtml.Append("<div style='margin-bottom:10px;Font-Weight:Bold;Font-Size:12px;'>Priority: <i> " + resPriority + "</i></div>");

                    // Getcommentslist will be appended
                    strHtml.Append(GetCommentsListByMail(meetingId, taskId, respPerson, empId, "Re-open"));
                    resSubject = "Task Re-opended: " + task;

                    //mail function to be called
                    //mailresp return 1 the Mail Send Sucess or return -1 Not success
                    mailresp = MailFunctionModel.MailUpdateDetails(employeeMail.ToString(), resSubject, strHtml, empName, taskStatus, HttpContext.Current.Session["LoggedEmail"].ToString(), task);
                }
                else
                {
                    strHtml.Append(comments);
                    //mailresp return 1 the Mail Send Sucess or return -1 Not success
                    mailresp = MailFunctionModel.MailUpdateDetails(employeeMail.ToString(), resSubject, strHtml, empName, taskStatus, HttpContext.Current.Session["LoggedEmail"].ToString(), task);
                }
            }
            if(string.Equals(taskStatus, "Re-opened"))
            {
                return "Mailed and Updated Successfully";
            }
            else
            {
                return "Updated Successfully";
            }
        }

        public static string UpdateMainMeetingDetails(MeetingDetailModel MeetinDetails, List<TaskDetailModel> taskdetails, List<UpdateMeetingTask> updateddetails)
        {
            StringBuilder strHtml = new StringBuilder();
            string resMeetingMsg = string.Empty;

            string resPriority = string.Empty;
            string resSubject = string.Empty;

            int? meetingId = 0;
            int? empId = 0;
            int? taskId = 0;
            int? commented_by = 0;
            int? res = 0;

            string task = string.Empty;
            string Priority = string.Empty;
            DateTime? CompletionDate = null;
            string comments = string.Empty;
            string category = string.Empty;

            DateTime? MeetingDate = null;
            string MeetingTime = string.Empty;
            string MeetingDuration = string.Empty;
            string MeetingName = string.Empty;
            string MeetingVenue = string.Empty;
            string MeetingDepartment = string.Empty;
            string MeetingOtherDept = string.Empty;
            string Participants = string.Empty;
            string MeetingType = string.Empty;
            string chairpers = string.Empty;
            string MinutesOfMeeting = string.Empty;
            int? MeetingNoOfParticipants = 0;
            int? MeetingChairperson = 0;

            int respPerson = Convert.ToInt32(MeetinDetails.Participants);

            meetingId = (MeetinDetails.MeetingId);

            if(MeetinDetails.MeetingDate != null)
            { MeetingDate = Convert.ToDateTime(MeetinDetails.MeetingDate); }
            MeetingTime = MeetinDetails.MeetingTime;
            MeetingDuration = MeetinDetails.MeetingDuration;
            MeetingType = MeetinDetails.Meetingtype;
            MeetingName = MeetinDetails.MeetingName;
            MeetingVenue = MeetinDetails.MeetingVenue;
            MeetingDepartment = MeetinDetails.MeetingDepartment;
            MeetingOtherDept = MeetinDetails.MeetingOtherDept;
            Participants = MeetinDetails.Participants;
            chairpers = MeetinDetails.MeetingChairperson;
            MinutesOfMeeting = MeetinDetails.MinutesOfMeeting;
            MeetingNoOfParticipants = Convert.ToInt16(MeetinDetails.AddedParticipants);

            using(db = new MMSCGVAKDBEntities())
            {
                var chairpersonid = (from x in db.Employee_Master where x.EmployeeDisplayName == chairpers select x.EmployeeICode).FirstOrDefault();

                if(chairpersonid == 0)
                {
                    chairpersonid = Convert.ToInt32(chairpers);
                }

                MeetingChairperson = Convert.ToInt32(chairpersonid);

                string empName = db.Employee_Master.Where(x => x.EmployeeICode == respPerson).Select(x => x.EmployeeDisplayName).FirstOrDefault();

                for(int i = 0; i < taskdetails.Count; i++)
                {
                    if(taskdetails[i].MeetingTask != null)
                    {
                        var meetingdetails = db.MMS_Meeting_Details.Where(x => x.Meeting_Id == meetingId).Select(x => new { x.Employee_Id, x.Task_Id, x.Chairperson }).ToArray();

                        empId = meetingdetails[i].Employee_Id;
                        taskId = meetingdetails[i].Task_Id;
                        commented_by = meetingdetails[i].Chairperson;

                        task = taskdetails[i].MeetingTask;
                        Priority = taskdetails[i].MeetingPriority;
                        if(taskdetails[i].MeetingCompletionDate != null)
                        { CompletionDate = Convert.ToDateTime(taskdetails[i].MeetingCompletionDate); }
                        comments = taskdetails[i].MeetingComments;
                        category = taskdetails[i].MeetingCategory;

                        DateTime ReviewDate = DateTime.Now.AddDays(1);

                        var updateCount1 = db.Sp_Update_MMS_Meeting_Details_Change1(meetingId, MeetingName, MeetingDate, MeetingTime, MeetingDuration, MeetingVenue, MeetingType, MeetingDepartment, MeetingChairperson, MinutesOfMeeting);
                        var updateCount = db.Sp_Update_MMS_Meeting_Details_Change(empId, taskId, CompletionDate, 5, Convert.ToInt16(category), meetingId, comments, ReviewDate, 1, commented_by, task, Priority);
                    }
                }
                if(updateddetails != null)
                {
                    for(int i = 0; i < updateddetails.Count(); i++)
                    {
                        byte[] fileContents = null;
                        BinaryFormatter bf = new BinaryFormatter();
                        MemoryStream ms = new MemoryStream();
                        if(updateddetails[i].files != null)
                        {
                            bf.Serialize(ms, updateddetails[i].files);
                            fileContents = ms.ToArray();
                            string extension = Path.GetExtension(updateddetails[i].files.ToString());
                            string fileName = Path.GetFileName(updateddetails[i].files.ToString());
                            var attachres = db.MMS_Attachment.Where(x => x.Meeting_Id == meetingId).FirstOrDefault();
                            if(attachres != null)
                                res = db.Sp_Update_Attachment(meetingId, fileName, extension, fileContents);
                            else
                                res = db.sp_insert_Attachment(meetingId, fileName, extension, fileContents);
                        }
                    }
                }
            }
            return "Updated Successfully";
        }

        #endregion

        #region GetCommentsList Display
        public static StringBuilder GetCommentsListByMail(int meetingId, int taskId, int respPerson, int employeeID, string meetingStatus)
        {
            StringBuilder strHtml = new StringBuilder();
            var meetingTask = db.MMS_Meeting_Details.Where(x => x.Task_Id == taskId).Select(x => x.Task).FirstOrDefault();
            var ListMeetingdetails = db.sp_select_meeting_status(taskId).ToList();
            //var TaskName = from taskres in db.sp_select_meeting_status(meetingId, meetingTask, respPerson)
            //               select taskres.Meeting_Status_Task;
            string empName = db.Employee_Master.Where(x => x.EmployeeICode == respPerson).Select(x => x.EmployeeDisplayName).FirstOrDefault();

            strHtml.Append("<table style='width:100%'");


            if(ListMeetingdetails.Count() > 0)
            {
                var EmployeeEmailAddress = db.Employee_Master.Where(x => x.EmployeeICode == employeeID).Select(x => x.EmployeeCorporateEmailId).FirstOrDefault();

                strHtml.Append("<tr><td style='Height:8px;'></td></tr>");
                strHtml.Append("<tr><td style='Font-Weight:Bold;Font-Size:12px;'>Tasks: <i> " + ListMeetingdetails.Select(x => x.Meeting_Status_Task).First() + "</i></td></tr>");
                strHtml.Append("<tr><td style='Height:1px;' bgcolor='#070719'></td></tr>");
                foreach(var Meetingres in ListMeetingdetails)
                {


                    strHtml.Append("<tr><td style='width:40%'><table class='tablecomments'><tr><td>" +
                        "<div class='comment' name=text style=background-color:Lightgreen;><b>" + Meetingres.Meeting_Status_Comments + "</b></div></td></tr><tr><td style='' align='Right'>&nbsp<i>" +
                       "Commented By: " + empName + " On " + Meetingres.Meeting_Status_Date + "</i></td>" +
                       "</tr><tr><td style='Height:20px;'></td></tr></table></td></tr>");

                }

                strHtml.Append("<input type='hidden' name='Mtg_Status_EmpID' id='Mtg_Status_EmpID' value=" + employeeID + ">");
                strHtml.Append("<input type='hidden' name='Mtg_Task' id='Mtg_Task' value='" + meetingTask + "'>");
                strHtml.Append("<input type='hidden' name='RespPerson' id='RespPerson' value='" + respPerson + "'>");
                strHtml.Append("<input type='hidden' name='Mtg_ID' id='Mtg_ID' value=" + meetingId + ">");
                strHtml.Append("<input type='hidden' name='Mtg_Status' id='Mtg_Status' value='" + meetingStatus + "'>");


            }
            else
            {
                strHtml.Append("<tr><td style='Font-Weight:Bold;Color:Red;Font-Size:12px;'>No comments found for the selected task...!</td></tr>");
                strHtml.Append("<tr><td style='Height:1px;' bgcolor='#070719'></td></tr>");

                strHtml.Append("<input type='hidden' name='Mtg_Status_EmpID' id='Mtg_Status_EmpID' value=" + employeeID + ">");
                strHtml.Append("<input type='hidden' name='Mtg_Task' id='Mtg_Task' value='" + meetingTask + "'>");
                strHtml.Append("<input type='hidden' name='RespPerson' id='RespPerson' value='" + respPerson + "'>");
                strHtml.Append("<input type='hidden' name='Mtg_ID' id='Mtg_ID' value=" + meetingId + ">");
                strHtml.Append("<input type='hidden' name='Mtg_Status' id='Mtg_Status' value='" + meetingStatus + "'>");


            }

            return strHtml;

        }
        #endregion


        #region GetCommentsList Mail
        public static StringBuilder GetCommentsList(int meetingId, string respPerson, int empId, string meetingStatus, int taskId)
        {
            StringBuilder strHtml = new StringBuilder();

            using(db = new MMSCGVAKDBEntities())
            {

                var ListMeetingdetails = (from val in db.sp_select_meeting_status(taskId)
                                          select val).ToList();      //Commented Gokul Thangavel. 18-Aug-2017

                // var ListMeetingdetails = db.sp_select_meeting_status(meetingId, meetingTask, respPerson).OrderByDescending(x => x.MeetingStatusAutoId).ToList();     //Added Gokul Thangavel. 18-Aug-2017



                //var TaskName = from taskres in db.sp_select_meeting_status(meetingId, meetingTask, respPerson)
                //               select taskres.Meeting_Status_Task;

                strHtml.Append("<table style='width:100%'");
                int cout = 1;
                var meetingTask = db.MMS_Meeting_Details.Where(x => x.Task_Id == taskId).Select(x => x.Task).FirstOrDefault();
                foreach(var meet in ListMeetingdetails)
                {
                    if(meet.Meeting_Status_Comments != null && meet.Meeting_Status_Comments != "")
                    {
                        cout = 0;
                    }
                }

                if(ListMeetingdetails.Count() > 0 && cout < 1)
                {
                    //strHtml.Append("<div class='modal - content'><div class='modal - header'><button type='button' id='command-close' class='close' data-dismiss='modal'>&times;</button></div>");
                    strHtml.Append("<tr><td style='Height:8px;'></td></tr>");
                    strHtml.Append("<tr><td style='Font-Weight:Bold;Font-Size:12px;'>Task: <i> " + ListMeetingdetails.Select(x => x.Meeting_Status_Task).FirstOrDefault() + "</i></td></tr>");
                    strHtml.Append("<tr><td style='Height:1px;' bgcolor='#070719'></td></tr>");
                    strHtml.Append("<tr><td align='right'><input type='button' class='btn btn-primary btn-sm btnaddcmt' name='btnAddComment' onclick='fncOpen();' value='Add Comment' id='btnAddComment'/></td></tr>");

                    foreach(var Meetingres in ListMeetingdetails)
                    {
                        if(Meetingres.Meeting_Status_Comments != null && Meetingres.Meeting_Status_Comments != "")
                        {
                            if(Meetingres.Commented_By != null)
                            {
                                if(Meetingres.Commented_By.ToLower().Trim() != "suresh (gs) g")
                                {
                                    var EmployeeEmailAddress = db.Employee_Master.Where(x => x.EmployeeICode == Meetingres.Meeting_Status_Employee_Id).Select(x => x.EmployeeCorporateEmailId).FirstOrDefault();
                                    var ccEmailAddress = db.Employee_Master.Where(x => x.EmployeeICode == Meetingres.Commented_By_ID).Select(x => x.EmployeeCorporateEmailId).FirstOrDefault();

                                    int responsible_comment = Convert.ToInt16(Meetingres.Commented_By_ID);
                                    Meetingres.Commented_By = db.Employee_Master.Where(x => x.EmployeeICode == responsible_comment).Select(x => x.EmployeeDisplayName).FirstOrDefault();

                                    strHtml.Append("<tr><td style='width:40%'><table class='tablecomments'><tr><td>" +
                                           "<div class='comment' name=text style=background-color:Lightgreen;>" + Meetingres.Meeting_Status_Comments + "</div></td></tr><tr><td style='' align='left'> <a href='javascript:{}' data-subject_task=" +
                                            Meetingres.Meeting_Status_Task + " data-to_email='" + EmployeeEmailAddress + "'" +
                                            "data-cc_mail='" + ccEmailAddress + "'" +
                                            " data-comment='" + Meetingres.Meeting_Status_Comments + "'" + "class='emailcomment'>Send to email</a>&nbsp<i>" +
                                          "Commented By: " + Meetingres.Commented_By + " On " + Meetingres.Meeting_Status_Date + "</i></td>" +
                                          "</tr><tr><td style='Height:20px;'></td></tr></table></td></tr>");
                                }
                                else
                                {
                                    var EmployeeEmailAddress = db.Employee_Master.Where(x => x.EmployeeICode == Meetingres.Meeting_Status_Employee_Id).Select(x => x.EmployeeCorporateEmailId).FirstOrDefault();
                                    var ccEmailAddress = db.Employee_Master.Where(x => x.EmployeeICode == Meetingres.Commented_By_ID).Select(x => x.EmployeeCorporateEmailId).FirstOrDefault();


                                    int responsible_comment = Convert.ToInt16(Meetingres.Commented_By);
                                    Meetingres.Commented_By = db.Employee_Master.Where(x => x.EmployeeICode == responsible_comment).Select(x => x.EmployeeDisplayName).FirstOrDefault();

                                    strHtml.Append("<tr><td style='width:40%'><table class='tablecomments'><tr><td>" +
                                           "<div class='comment' name=text style=background-color:skyblue;>" + Meetingres.Meeting_Status_Comments + "</div></td></tr><tr><td style='' align='left'> <a href='javascript:{}' data-subject_task=" +
                                            Meetingres.Meeting_Status_Task + " data-to_email='" + EmployeeEmailAddress + "'" +
                                            "data-cc_mail='" + ccEmailAddress + "'" +
                                            " data-comment='" + Meetingres.Meeting_Status_Comments + "'" + "class='emailcomment'>Send to email</a>&nbsp<i>" +
                                          "Commented By: " + Meetingres.Commented_By + " On " + Meetingres.Meeting_Status_Date + "</i></td>" +
                                          "</tr><tr><td style='Height:20px;'></td></tr></table></td></tr>");
                                }
                            }
                            else
                            {
                                var EmployeeEmailAddress = db.Employee_Master.Where(x => x.EmployeeICode == Meetingres.Meeting_Status_Employee_Id).Select(x => x.EmployeeCorporateEmailId).FirstOrDefault();
                                var ccEmailAddress = db.Employee_Master.Where(x => x.EmployeeICode == Meetingres.Commented_By_ID).Select(x => x.EmployeeCorporateEmailId).FirstOrDefault();

                                int responsible_comment = Convert.ToInt16(Meetingres.Commented_By);
                                Meetingres.Commented_By = db.Employee_Master.Where(x => x.EmployeeICode == responsible_comment).Select(x => x.EmployeeDisplayName).FirstOrDefault();


                                strHtml.Append("<tr><td style='width:40%'><table class='tablecomments'><tr><td>" +
                                       "<div class='comment' name=text style=background-color:skyblue;>" + Meetingres.Meeting_Status_Comments + "</div></td></tr><tr><td style='' align='left'> <a href='javascript:{}' data-subject_task=" +
                                        Meetingres.Meeting_Status_Task + " data-to_email='" + EmployeeEmailAddress + "'" +
                                        "data-cc_mail='" + ccEmailAddress + "'" +
                                        " data-comment='" + Meetingres.Meeting_Status_Comments + "'" + "class='emailcomment'>Send to email</a>&nbsp<i>" +
                                      "Commented By: " + Meetingres.Commented_By + " On " + Meetingres.Meeting_Status_Date + "</i></td>" +
                                      "</tr><tr><td style='Height:20px;'></td></tr></table></td></tr>");
                            }
                        }

                    }


                    strHtml.Append("<input type='hidden' name='Mtg_Status_EmpID' id='Mtg_Status_EmpID' value=" + empId + ">");
                    strHtml.Append("<input type='hidden' name='Mtg_Task' id='Mtg_Task' value='" + meetingTask + "'>");

                    strHtml.Append("<input type='hidden' name='RespPerson' id='RespPerson' value='" + respPerson + "'>");
                    strHtml.Append("<input type='hidden' name='Mtg_ID' id='Mtg_ID' value=" + meetingId + ">");
                    strHtml.Append("<input type='hidden' name='Mtg_Status' id='Mtg_Status' value='" + meetingStatus + "'>");
                    strHtml.Append("<input type='hidden' name='Mtg_TaskId' id='Mtg_TaskId' value='" + taskId + "'>");


                }
                else
                {

                    // strHtml.Append("<input type='hidden' name='Mtg_Task' id='Mtg_Task' value='" + meetingTask + "'>");
                    strHtml.Append("<tr><td style='Font-Weight:Bold;Font-Size:12px;'>Task: <i> " + meetingTask + "</i></td></tr>");
                    //strHtml.Append("<tr><td style='Font-Weight:Bold;Color:Red;Font-Size:12px;'>No comments found for the selected task...!</td></tr>");
                    strHtml.Append("<tr><td style='Height:1px;' bgcolor='#070719'></td></tr>");
                    strHtml.Append("<tr><td align='right'><input type='button' class='btn btn-primary btn-sm btnaddcmt' name='btnAddComment' onclick='fncOpen();' value='Add Comment' id='btnAddComment'/></td></tr>");
                    strHtml.Append("<tr><td style='Font-Weight:Bold;Color:Red;Font-Size:12px;'>No comments found for the selected task...!</td></tr>");

                    strHtml.Append("<input type='hidden' name='Mtg_Status_EmpID' id='Mtg_Status_EmpID' value=" + empId + ">");
                    strHtml.Append("<input type='hidden' name='Mtg_Task' id='Mtg_Task' value='" + meetingTask + "'>");
                    strHtml.Append("<input type='hidden' name='RespPerson' id='RespPerson' value='" + respPerson + "'>");
                    strHtml.Append("<input type='hidden' name='Mtg_ID' id='Mtg_ID' value=" + meetingId + ">");
                    strHtml.Append("<input type='hidden' name='Mtg_Status' id='Mtg_Status' value='" + meetingStatus + "'>");
                    strHtml.Append("<input type='hidden' name='Mtg_TaskId' id='Mtg_TaskId' value='" + taskId + "'>");

                }
            }
            return strHtml;
        }
        #endregion


        #region AddingComments
        public static int AddComments(int? meetingStatusEmpID, string meetingTask, string meetingComments, int? respPerson, int? meetingId, int? meetingStatus, string commentedBy, int? commentedByID, string currentMail, int? taskId)
        {
            int cnt = 0;

            using(db = new MMSCGVAKDBEntities())
            {
                int mailresp = 0;
                string ss = DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt");
                DateTime Datee;
                Datee = DateTime.Parse(ss);

                //  cnt = db.sp_insert_Meeting_Status(meetingStatusEmpID, meetingTask, respPerson, DateTime.Now.ToString(), meetingComments, meetingStatus, "HH:MM", meetingId, commentedBy, commentedByID, taskId);

                cnt = db.sp_insert_Meeting_Status(meetingStatusEmpID, meetingTask, respPerson, DateTime.Now, meetingComments, meetingStatus, "HH:MM", meetingId, Convert.ToInt32(commentedBy), commentedByID, taskId);



                var employeeMail = db.sp_get_EmailId(Convert.ToInt32(meetingStatusEmpID)).First();

                var MailPerson = db.Employee_Master.Where(x => x.EmployeeICode == respPerson).Select(x => x.EmployeeDisplayName).FirstOrDefault();
                string a = string.Empty;
                int mailreceiver = Convert.ToInt16(commentedBy);
                commentedBy = db.Employee_Master.Where(x => x.EmployeeICode == mailreceiver).Select(x => x.EmployeeDisplayName).FirstOrDefault();
                //mailresp return 1 the Mail Send Sucess or return -1 Not success
                mailresp = MailFunctionModel.CommentsMail(employeeMail, meetingTask, meetingComments, MailPerson, commentedBy);
            }
            return cnt;
        }

        #endregion

        #region ClosingComments
        public static string CloseComments(int taskId, int? meetingId)
        {
            //    string MeetingStatus = "Closed";
            //    string MeetingComments = "Task Closed";

            //    using (db = new MMSCGVAKDBEntities())
            //    {





            //            db.sp_insert_Meeting_Status(employeeId, task, respPerson, DateTime.Now.ToShortDateString(), MeetingComments, MeetingStatus, "HH:MM", meetingId, HttpContext.Current.Session["LoggedEmployeeName"].ToString(), Convert.ToInt16(HttpContext.Current.Session["LoggedUserId"]), taskId);

            //        string Status = "Closed";

            //        //db.sp_update_status_update(meetingId, employeeId, task, Status).ToString();       //Commented. Gokul Thangavel 18-Aug-2017
            //        db.UpdateMeetingTaskStatus(taskId, Status);                                         //Added. Gokul Thangavel 18-Aug-2017

            //    }
            //    return string.Empty;
            //}


            MMS_Meeting_Details meetingDetail = new MMS_Meeting_Details();
            string message = string.Empty;
            using(db = new MMSCGVAKDBEntities())
            {

                try
                {
                    var closeres = db.MMS_Meeting_Status_Update.Where(s => s.TaskId == taskId).FirstOrDefault();
                    closeres.status = 1;

                    var closeres1 = db.MMS_Meeting_Details.Where(s => s.Task_Id == taskId).FirstOrDefault();
                    closeres1.Status = 1;


                    // db.Entry(meetingDetail).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();


                    message = "Successful";
                }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
                catch(Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
                {
                    message = "Try Again";
                }

                return message;
            }

        }


        #endregion

        #region Get Meeting Total Counts
        public static int GetCounts(int meetingId, int employeeId, int participant, int taskId)
        {
            using(db = new MMSCGVAKDBEntities())
            {
                //int count = db.sp_select_meeting_status(meetingId, task, participant).Count();*/

                var list = db.sp_select_meeting_status(taskId).ToList();
                int count = 0;
                foreach(var li in list)
                {
                    if(li.Meeting_Status_Comments != null && li.Meeting_Status_Comments != "")
                    {
                        count = count + 1;
                    }
                }
                return count;
            }
        }


        #endregion

        #region updateNotification

        public static void UpdateNotification(int id)
        {
            using(db = new MMSCGVAKDBEntities())
            {
                db.sp_update_read_notification(id);
            }
        }

        #endregion


        #region TimeTakenforcompletionreport_TotatReports
        //public static List<sp_TimeTakenReport_Result> TimeTakenFetchRes_Total(string ChairPerson, string startDate, string endDate)
        //{
        //    using (db = new MMSCGVAKDBEntities())
        //    {
        //        var result = (from res in db.sp_TimeTakenReport(ChairPerson, startDate, endDate)
        //                      select res
        //                      ).ToList();

        //        return result;
        //    }
        //}

        #endregion

        #region Get Meeting Template
        public static List<string> TemplateDetails()
        {
            using(db = new MMSCGVAKDBEntities())
            {
                var template_list = db.MMS_Meeting_Template.Select(x => x.objective).ToList();
                return template_list;
            }
        }

        #endregion

        #region Get Meeting Template ID
        public static int MeetingTemplateID(string meetingName)
        {
            int? template_ID = 0;
            using(db = new MMSCGVAKDBEntities())
            {
                template_ID = db.MMS_Meeting_Template.Where(x => x.objective == meetingName).Select(x => x.tid).FirstOrDefault();
                return Convert.ToInt32(template_ID);
            }

        }
        #endregion

    }

    public class Priority
    {
        [Key]
        public int PriorityID { get; set; }
        public string High { get; set; }
        public string Medium { get; set; }
        public string Low { get; set; }
    }


    public class AddComment
    {
        public int meetingStatusEmpID { get; set; }
        public string meetingTask { get; set; }
        public string meetingComments { get; set; }
        public string respPerson { get; set; }
        public int meetingId { get; set; }
        public string meetingStatus { get; set; }
        public string commentedBy { get; set; }
        public int commentedByID { get; set; }
        public string currentMail { get; set; }
    }

}


