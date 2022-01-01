using System.Collections.Generic;
using AtmReportGenerator.Entities;

namespace AtmReportGenerator.Processors
{
    public interface IEventDetector
    {
        bool Match(List<AtmLogRecord> data, int index);
        AtmEvent Produce(List<AtmLogRecord> data, int index);
    }
}