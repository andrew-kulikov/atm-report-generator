using AtmReportGenerator.Entities;
using AtmReportGenerator.Exporters;
using AtmReportGenerator.Logging;
using AtmReportGenerator.Parsers;
using CommandLine;

namespace AtmReportGenerator
{
    public class ReportGeneratorConsoleOptions
    {
        [Option('i', "input", Required = true, HelpText = "Path to folder with input Excel files. Example: D:\\Temp\\AtmLogs\\input")]
        public string LogFileDirectory { get; set; }

        [Option('o', "out", Required = false, HelpText = "Path to folder with input Excel files. Example: D:\\Temp\\AtmLogs\\out", Default = "out")]
        public string DestinationFolder { get; set; }

        [Option('f', "date-format", Required = false, HelpText = "Date format. Example: dd.MM.yyyy HH:mm", Default = "dd.MM.yyyy HH:mm")]
        public string DateFormat { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<ReportGeneratorConsoleOptions>(args)
                .WithParsed(consoleOptions =>
                {
                    var options = new ReportGeneratorOptions
                    {
                        LogFileDirectory = consoleOptions.LogFileDirectory,
                        DestinationFolder = consoleOptions.DestinationFolder,
                        DateFormat = consoleOptions.DateFormat
                    };

                    RunApplication(options);
                });
        }

        private static void RunApplication(ReportGeneratorOptions options)
        {
            var logger = new DefaultConsoleLogger();

            var parser = new DefaultXlsLogParser(options);
            var exporter = new TxtFilesReportExporter(logger, options);
            var reportGenerator = new AtmReportGenerator(parser, logger, exporter);

            reportGenerator.GenerateReport(options);
        }
    }
}