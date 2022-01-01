using System.Collections.Generic;
using AtmReportGenerator.Entities;
using AtmReportGenerator.Logging;
using AtmReportGenerator.Parsers;

namespace AtmReportGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var filePath = @"D:\git\atm-report-generator\data\in\12240.xls";

            var parser = new DefaultXlsLogParser();
            var logger = new DefaultConsoleLogger();
            var reportGenerator = new AtmReportGenerator(parser, logger);

            var request = new AtmReportRequest
            {
                LogFilePaths = new List<string> { filePath }
            };

            reportGenerator.GenerateReport(request);
        }
    }
}