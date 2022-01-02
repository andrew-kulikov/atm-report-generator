using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using AtmReportGenerator.Entities;
using AtmReportGenerator.Logging;
using AtmReportGenerator.Utils;
using Tababular;

namespace AtmReportGenerator.Exporters
{
    public class TxtFilesReportExporter : IReportExporter
    {
        private readonly ILogger _logger;
        private readonly ReportGeneratorOptions _generatorOptions;
        private readonly TableFormatter _tableFormatter;

        public TxtFilesReportExporter(ILogger logger, ReportGeneratorOptions generatorOptions)
        {
            _logger = logger;
            _generatorOptions = generatorOptions;
            _tableFormatter = new TableFormatter();
        }

        public void Export(AggregatedAtmReport report)
        {
            _logger.LogInformation($"Starting export to folder {_generatorOptions.DestinationFolder}");

            PrepareDirectory();

            var dailyReports = report.BuildAggregatedDailyReport();

            foreach (var aggregatedAtmDailyReport in dailyReports) LogAggregatedDailyReport(aggregatedAtmDailyReport);
        }

        public void LogAggregatedDailyReport(AggregatedAtmDailyReport report)
        {
            _logger.LogInformation($"Processing report for {report.Date.ToShortAtmFormat()}");

            PrepareReportFile(report);

            var dayStartReportsTable = report.DailyReports.Select(BuildDailyReportRow);
            var dayStartReportsTableText = _tableFormatter.FormatDictionaries(dayStartReportsTable);

            var eventsTable = BuildEventsTable(report);
            var eventsTableText = _tableFormatter.FormatDictionaries(eventsTable);

            var reportText = new StringBuilder();

            reportText.AppendLine("Остатки");
            reportText.AppendLine(dayStartReportsTableText);
            reportText.AppendLine();
            reportText.AppendLine("Загрузка/Выгрузка");
            reportText.AppendLine(eventsTableText);

            File.WriteAllText(GetReportFilePath(report), reportText.ToString(), Encoding.UTF8);

            _logger.LogInformation($"Exported report for {report.Date.ToShortAtmFormat()}!");
        }

        private void PrepareDirectory()
        {
            PrepareDirectory(_generatorOptions.DestinationFolder);
        }

        private void PrepareReportFile(AggregatedAtmDailyReport report)
        {
            PrepareReportDirectory(report);

            var reportFilePath = GetReportFilePath(report);

            if (File.Exists(reportFilePath))
            {
                _logger.LogInformation($"Report for {report.Date.ToShortAtmFormat()} already exists, deleting...");

                File.Delete(reportFilePath);
            }
        }

        private string GetReportFilePath(AggregatedAtmDailyReport report) => Path.Combine(
            _generatorOptions.DestinationFolder,
            report.Date.Year.ToString(),
            report.Date.Month.ToString(),
            $"{report.Date.Day}.txt");

        private void PrepareReportDirectory(AggregatedAtmDailyReport report)
        {
            var monthFolderPath = Path.Combine(_generatorOptions.DestinationFolder, report.Date.Year.ToString(), report.Date.Month.ToString());

            PrepareDirectory(monthFolderPath);
        }

        private void PrepareDirectory(string path)
        {
            if (Directory.Exists(path)) return;

            _logger.LogInformation($"Folder {path} not found, creating...");

            Directory.CreateDirectory(path);
        }

        private IEnumerable<Dictionary<string, string>> BuildEventsTable(AggregatedAtmDailyReport report)
        {
            foreach (var dailyReport in report.DailyReports)
            {
                var cashLoad = dailyReport.CashLoad;
                var cashUnload = dailyReport.CashUnload;

                if (cashLoad > 0 || cashUnload > 0)
                    yield return new Dictionary<string, string>
                    {
                        { "Устройство", dailyReport.AtmId },
                        { "Загрузка", cashLoad.ToString(CultureInfo.InvariantCulture) },
                        { "Выгрузка", cashUnload.ToString(CultureInfo.InvariantCulture) }
                    };
            }
        }

        private Dictionary<string, string> BuildDailyReportRow(AtmDailyReport dailyReport) =>
            new Dictionary<string, string>
            {
                { "Устройство", dailyReport.AtmId },
                { "Остаток BYN", dailyReport.WorkingDayStartReport.Remaining.ToString(CultureInfo.InvariantCulture) }
            };
    }
}