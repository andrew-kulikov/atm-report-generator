using System.Linq;
using AtmReportGenerator.Entities;
using AtmReportGenerator.Exporters;
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
        private readonly IReportExporter _exporter;
        private readonly WorkingDayRemainingCashCollector _workingDayRemainingCashCollector;

        public AtmReportGenerator(ILogParser logParser, ILogger logger, IReportExporter exporter)
        {
            _logParser = logParser;
            _logger = logger;
            _exporter = exporter;

            _eventCollector = new EventCollector();
            _workingDayRemainingCashCollector = new WorkingDayRemainingCashCollector();
        }

        public void GenerateReport(ReportOptions options)
        {
            var reports = options.LogFilePaths.Select(BuildSingleAtmReport).ToList();

            var aggregatedReport = new AggregatedAtmReport { Reports = reports };

            _logger.LogAggregatedReport(aggregatedReport);

            _exporter.Export(aggregatedReport);
        }

        private AtmReport BuildSingleAtmReport(string logFilePath)
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

            //_logger.LogReport(report);

            return report;
        }
    }
}