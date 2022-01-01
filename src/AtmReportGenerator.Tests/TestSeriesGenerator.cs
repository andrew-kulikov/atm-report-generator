using System;
using System.Collections.Generic;
using System.Linq;
using AtmReportGenerator.Entities;

namespace AtmReportGenerator.Tests
{
    public static class TestSeriesGenerator
    {
        public static AtmLog BuildLogFromMovements((double, double)[] logs) =>
            new AtmLog
            {
                AtmId = "Test1234",
                AtmInfo = "Test1234 Минск",
                Logs = FromMovements(logs).ToList()
            };

        public static IEnumerable<AtmLogRecord> FromMovements((double, double)[] logs)
        {
            var currentTime = new DateTime(2021, 11, 1, 7, 0, 0);
            var measureStep = TimeSpan.FromMinutes(15);

            foreach (var (remainingCash, withdrawAmount) in logs)
            {
                yield return new AtmLogRecord
                {
                    Time = currentTime,
                    RemainingCash = remainingCash,
                    WithdrawAmount = withdrawAmount
                };

                currentTime += measureStep;
            }
        }
    }
}