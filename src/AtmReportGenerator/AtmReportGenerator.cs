using System.Linq;
using AtmReportGenerator.Entities;
using AtmReportGenerator.Logging;
using AtmReportGenerator.Parsers;
using AtmReportGenerator.Processors;

namespace AtmReportGenerator
{
    public class AtmReportGenerator
    {
        private readonly EventCollector _eventCollector;
        private readonly ILogger _logger;
        private readonly ILogParser _logParser;
        private readonly WorkingDayRemainingCashCollector _workingDayRemainingCashCollector;

        public AtmReportGenerator(ILogParser logParser, ILogger logger)
        {
            _logParser = logParser;
            _logger = logger;

            _eventCollector = new EventCollector();
            _workingDayRemainingCashCollector = new WorkingDayRemainingCashCollector();
        }

        public void GenerateReport(AtmReportRequest request)
        {
            foreach (var logFilePath in request.LogFilePaths)
            {
                var log = _logParser.ParseLog(logFilePath);

                var events = _eventCollector.CollectEvents(log).ToList();
                var workingDayCashStats = _workingDayRemainingCashCollector.Process(log).ToList();

                var report = new AtmReport
                {
                    AtmId = log.AtmId,
                    AtmEvents = events,
                    WorkingDayStartReports = workingDayCashStats
                };

                _logger.LogReport(report);
            }
        }
    }
}