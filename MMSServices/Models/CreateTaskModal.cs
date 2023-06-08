using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMSServices.Models
{
  

    public class MMS_Meeting_Master
    {
    public string  ObjectiveIn { get; set; }
    public Nullable<System.DateTime> DateIn { get; set; }
    public Nullable<System.DateTime> TimeIn { get; set; }
    public string VenueIn { get; set; }
    public string TypeIn { get; set; }
    public Nullable<System.DateTime> DurationIn { get; set; }
    public string DepartmentIn { get; set; }
    public int ChairpersonIn { get; set; }
    public string MinutesOfMeetingIn { get; set; }
    public int ParticipantsIn { get; set; }
    public Nullable<bool> Istemplate { get; set; }
    public int TemplateID { get; set; }
    }
}