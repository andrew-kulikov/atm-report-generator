using System.Collections.Generic;
using AtmReportGenerator.Entities;

namespace AtmReportGenerator.Processors
{
    public class EventCollector
    {
        private readonly List<IEventDetector> _detectors = new List<IEventDetector>
        {
            new CashUnloadEventDetector(),
            new CashLoadEventDetector()
        };

        public IEnumerable<AtmEvent> CollectEvents(AtmLog atmLog)
        {
            for (var i = 0; i < atmLog.Logs.Count - 1; i++)
                foreach (var eventDetector in _detectors)
                    if (eventDetector.Match(atmLog.Logs, i))
                    {
                        var atmEvent = eventDetector.Produce(atmLog.Logs, i);
                        atmEvent.AtmId = atmLog.AtmId;

                        yield return atmEvent;
                    }
        }
    }
}