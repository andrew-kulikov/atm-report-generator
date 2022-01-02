using System;
using System.Collections.Generic;

namespace AtmReportGenerator.Entities
{
    public class AggregatedAtmDailyReport
    {
        public DateTime Date { get; set; }
        public List<AtmDailyReport> DailyReports { get; set; } = new List<AtmDailyReport>();
    }
}