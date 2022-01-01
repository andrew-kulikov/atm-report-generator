using System.Collections.Generic;
using AtmReportGenerator.Entities;

namespace AtmReportGenerator.Processors
{
    public class WorkingDayRemainingCashCollector
    {
        public IEnumerable<AtmWorkingDayStartReport> Process(AtmLog atmLog)
        {
            foreach (var log in atmLog.Logs)
                if (log.Time.Hour == 8 && log.Time.Minute == 0)
                    yield return new AtmWorkingDayStartReport
                    {
                        Time = log.Time,
                        AtmId = atmLog.AtmId,
                        Remaining = log.RemainingCash
                    };
        }
    }
}