using AtmReportGenerator.Entities;

namespace AtmReportGenerator.Exporters
{
    public interface IReportExporter
    {
        void Export(AggregatedAtmReport report);
    }
}