using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMS.Models
{
    public class effectivenessresultvm
    {
       
            public int HighTotal { get; set; }
            public int MediumTotal { get; set; }
            public int LowTotal { get; set; }
            public int HighCompleted { get; set; }
            public int MediumCompleted { get; set; }
            public int LowCompleted { get; set; }
            public int HighDelay { get; set; }
            public int MediumDelay { get; set; }
            public int LowDelay { get; set; }
        
    }
}