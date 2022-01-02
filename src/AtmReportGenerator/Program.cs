using System.Collections.Generic;
using AtmReportGenerator.Entities;
using AtmReportGenerator.Exporters;
using AtmReportGenerator.Logging;
using AtmReportGenerator.Parsers;

namespace AtmReportGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var options = new ReportOptions
            {
                LogFilePaths = new List<string>
                {
                    @"D:\git\atm-report-generator\data\in\12240.xls",
                    @"D:\git\atm-report-generator\data\in\12494.xls",
                    @"D:\git\atm-report-generator\data\in\12833.xls",
                    @"D:\git\atm-report-generator\data\in\12909.xls",
                    @"D:\git\atm-report-generator\data\in\13650.xls"
                },
                DestinationFolder = @"D:\git\atm-report-generator\data\out"
            };

            var parser = new DefaultXlsLogParser();
            var logger = new DefaultConsoleLogger();
            var exporter = new TxtFilesReportExporter(logger, options);
            var reportGenerator = new AtmReportGenerator(parser, logger, exporter);

            reportGenerator.GenerateReport(options);
        }
    }
}