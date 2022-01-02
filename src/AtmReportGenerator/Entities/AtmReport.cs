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

        public List<AtmDailyReport> GetDailyReports()
        {
            var dailyReports = GetStartReportsByDay();
            var eventsByDay = GetEventsByDay();

            var reports = new List<AtmDailyReport>();

            foreach (var dailyReport in dailyReports)
            {
                var events = eventsByDay.TryGetValue(dailyReport.Key, out var currentDayEvents) 
                    ? currentDayEvents 
                    : new List<AtmEvent>();

                reports.Add(new AtmDailyReport
                {
                    AtmId = AtmId,
                    Date = dailyReport.Key,
                    WorkingDayStartReport = dailyReport.Value,
                    AtmEvents = events
                });
            }

            return reports;
        }

        public Dictionary<DateTime, AtmWorkingDayStartReport> GetStartReportsByDay() =>
            WorkingDayStartReports.GroupBy(e => e.Time.Date).ToDictionary(group => group.Key, group => group.Single());

        public Dictionary<DateTime, List<AtmEvent>> GetEventsByDay() =>
            AtmEvents.GroupBy(e => e.Time.Date).ToDictionary(group => group.Key, group => group.ToList());
    }
}