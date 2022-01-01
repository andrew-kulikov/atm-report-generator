using AtmReportGenerator.Utils;

namespace AtmReportGenerator.Entities
{
    public class AtmCashLoadEvent : AtmEvent
    {
        public double Amount { get; set; }

        public override string ToString() => $"{AtmId} | {Time.ToLongAtmFormat()} | Load {Amount}";
    }
}