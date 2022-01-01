using System;
using AtmReportGenerator.Utils;

namespace AtmReportGenerator.Entities
{
    public class AtmWorkingDayStartReport
    {
        public string AtmId { get; set; }
        public DateTime Time { get; set; }
        public double Remaining { get; set; }

        public override string ToString() => $"{AtmId} | {Time.ToLongAtmFormat()} | REM {Remaining}";
    }
}