namespace AtmReportGenerator.Entities
{
    public class ReportGeneratorOptions
    {
        public string LogFileDirectory { get; set; }
        public string DestinationFolder { get; set; }
        public string DateFormat { get; set; } = "dd.MM.yyyy HH:mm";
    }
}