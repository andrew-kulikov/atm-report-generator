using System.Collections.Generic;
using System.Linq;

namespace AtmReportGenerator.Entities
{
    public class AggregatedAtmReport
    {
        public List<AtmReport> Reports { get; set; }

        public List<AggregatedAtmDailyReport> BuildAggregatedDailyReport()
        {
            var aggregatedReports = new List<AggregatedAtmDailyReport>();

            foreach (var atmReport in Reports)
            {
                var dailyReports = atmReport.GetDailyReports();

                foreach (var dailyReport in dailyReports)
                {
                    Merge(aggregatedReports, dailyReport);
                }
            }

            return aggregatedReports;
        }

        private void Merge(List<AggregatedAtmDailyReport> aggregatedReports, AtmDailyReport dailyReport)
        {
            if (aggregatedReports.All(r => r.Date != dailyReport.Date)) aggregatedReports.Add(new AggregatedAtmDailyReport
            {
                Date = dailyReport.Date,
                DailyReports = new List<AtmDailyReport>()
            });

            var aggregatedReport = aggregatedReports.Single(report => report.Date == dailyReport.Date);
            aggregatedReport.DailyReports.Add(dailyReport);
        }

    }
}