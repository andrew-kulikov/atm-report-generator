using System;
using AtmReportGenerator.Utils;

namespace AtmReportGenerator.Entities
{
    public class AtmLogRecord
    {
        public DateTime Time { get; set; }

        public double RemainingCash { get; set; }
        public double WithdrawAmount { get; set; }

        public double ExpectedRemaining { get; set; }
        public double ExpectedWithdrawAmount { get; set; }

        public override string ToString() => $"{Time.ToLongAtmFormat()} | OUT {WithdrawAmount} | REM {RemainingCash}";
    }
}