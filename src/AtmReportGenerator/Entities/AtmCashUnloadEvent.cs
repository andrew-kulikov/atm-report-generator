using AtmReportGenerator.Utils;

namespace AtmReportGenerator.Entities
{
    public class AtmCashUnloadEvent : AtmEvent
    {
        public double Amount { get; set; }

        public override string ToString() => $"{AtmId} | {Time.ToLongAtmFormat()} | Unload {Amount}";
    }
}