using System.Collections.Generic;
using AtmReportGenerator.Entities;

namespace AtmReportGenerator.Processors
{
    public class CashUnloadEventDetector : IEventDetector
    {
        public bool Match(List<AtmLogRecord> data, int index)
        {
            var current = data[index];
            var next = data[index + 1];

            return current.RemainingCash - next.WithdrawAmount != next.RemainingCash &&
                   current.RemainingCash >= 0 &&
                   (current.RemainingCash < next.RemainingCash && current.RemainingCash != 0 || next.RemainingCash == 0);
        }

        public AtmEvent Produce(List<AtmLogRecord> data, int index)
        {
            var current = data[index];
            var next = data[index + 1];

            var amount = GetAmount(current, next);

            return new AtmCashUnloadEvent
            {
                Time = current.Time,
                Amount = amount
            };
        }

        private double GetAmount(AtmLogRecord current, AtmLogRecord next) => current.RemainingCash;
    }
}