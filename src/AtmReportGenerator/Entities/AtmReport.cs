using System;
using System.Collections.Generic;
using System.Linq;

namespace AtmReportGenerator.Entities
{
    public class AtmReport
    {
        public string AtmId { get; set; }
        public List<AtmEvent> AtmEvents { get; set; }
        public List<AtmWorkingDayStartReport> WorkingDayStartReports { get; set; }

        public Dictionary<DateTime, List<AtmEvent>> GetEventsByDay() =>
            AtmEvents.GroupBy(e => e.Time.Date).ToDictionary(group => group.Key, group => group.ToList());
    }
}