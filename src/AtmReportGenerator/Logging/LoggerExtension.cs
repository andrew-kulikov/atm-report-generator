using AtmReportGenerator.Entities;
using AtmReportGenerator.Utils;

namespace AtmReportGenerator.Logging
{
    public static class LoggerExtension
    {
        public static void LogReport(this ILogger logger, AtmReport report)
        {
            LogEvents(logger, report);
            logger.LogInformation("\n" + new string('=', 20) + "\n");
            LogDailyStats(logger, report);
        }

        public static void LogEvents(this ILogger logger, AtmReport report)
        {
            var eventsByDay = report.GetEventsByDay();

            logger.LogInformation("Events:");

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