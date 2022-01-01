using AtmReportGenerator.Entities;

namespace AtmReportGenerator.Parsers
{
    public interface ILogParser
    {
        AtmLog ParseLog(string path);
    }
}