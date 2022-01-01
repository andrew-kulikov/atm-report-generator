using System.Collections.Generic;
using AtmReportGenerator.Entities;

namespace AtmReportGenerator.Processors
{
    public class CashLoadEventDetector : IEventDetector
    {
        public bool Match(List<AtmLogRecord> data, int index)
        {
            var current = data[index];
            var next = data[index + 1];

            return current.RemainingCash - next.WithdrawAmount != next.RemainingCash &&
                   (current.RemainingCash < next.RemainingCash && current.RemainingCash != 0 || current.RemainingCash == 0);
        }

        public AtmEvent Produce(List<AtmLogRecord> data, int index)
        {
            var amount = GetAmount(data, index);

            return new AtmCashLoadEvent
            {
                Time = data[index].Time,
                Amount = amount
            };
        }

        // TODO: Add case when current is negative
        private double GetAmount(List<AtmLogRecord> data, int index)
        {
            var current = data[index];
            var next = data[index + 1];

            if (current.RemainingCash >= 0) return next.RemainingCash + next.WithdrawAmount;

            // When current.RemainingCash is less than zero we have some accumulated error, so we need to count this in load amount
            return next.RemainingCash + next.WithdrawAmount - current.RemainingCash;
        }
    }
}