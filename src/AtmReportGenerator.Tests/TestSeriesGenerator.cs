using System;
using System.Collections.Generic;
using System.Linq;
using AtmReportGenerator.Entities;

namespace AtmReportGenerator.Tests
{
    public static class TestSeriesGenerator
    {
        public static AtmLog BuildLogFromMovements(params (double, double)[] logs) =>
            new AtmLog
            {
                AtmId = "Test1234",
                AtmInfo = "Test1234 Минск",
                Logs = FromMovements(logs)
            };

        public static List<AtmLogRecord> FromMovements(params (double, double)[] logs) => 
            EnumerateMovements(logs).ToList();

        private static IEnumerable<AtmLogRecord> EnumerateMovements(params (double, double)[] logs)
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