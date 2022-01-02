using AtmReportGenerator.Entities;
using AtmReportGenerator.Utils;

namespace AtmReportGenerator.Logging
{
    public static class LoggerExtension
    {
        public static void LogAggregatedReport(this ILogger logger, AggregatedAtmReport report)
        {
            logger.LogInformation(new string('=', 30));

            var dailyReports = report.BuildAggregatedDailyReport();

            foreach (var aggregatedAtmDailyReport in dailyReports) LogAggregatedDailyReport(logger, aggregatedAtmDailyReport);

            logger.LogInformation(new string('=', 30));
        }

        public static void LogAggregatedDailyReport(this ILogger logger, AggregatedAtmDailyReport report)
        {
            logger.LogInformation($"{report.Date.ToShortAtmFormat()}");

            foreach (var dailyReport in report.DailyReports) logger.LogInformation($"{dailyReport.AtmId} | REM {dailyReport.WorkingDayStartReport.Remaining}");

            foreach (var dailyReport in report.DailyReports)
            {
                var cashLoad = dailyReport.CashLoad;
                var cashUnload = dailyReport.CashUnload;

                if (cashLoad > 0 || cashUnload > 0) logger.LogInformation($"{dailyReport.AtmId} | LOAD {cashLoad} | UNLOAD {cashUnload}");
            }

            logger.LogInformation("\n");
        }

        public static void LogReport(this ILogger logger, AtmReport report)
        {
            LogEvents(logger, report);
            logger.LogInformation("\n" + new string('=', 20) + "\n");
            LogDailyStats(logger, report);
        }

        public static void LogEvents(this ILogger logger, AtmReport report)
        {
            var eventsByDay = report.GetEventsByDay();

            foreach (var dailyEvents in eventsByDay)
            {
                logger.LogInformation($"Date: {dailyEvents.Key.ToShortAtmFormat()}");

                foreach (var atmEvent in dailyEvents.Value) logger.LogInformation(atmEvent.ToString());

                logger.LogInformation("\n");
            }
        }

        public static void LogDailyStats(this ILogger logger, AtmReport report)
        {
            foreach (var workingDayCashStat in report.WorkingDayStartReports) logger.LogInformation(workingDayCashStat.ToString());
        }
    }
}