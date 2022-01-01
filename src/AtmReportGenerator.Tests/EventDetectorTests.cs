using System.Linq;
using AtmReportGenerator.Entities;
using AtmReportGenerator.Processors;
using Xunit;

namespace AtmReportGenerator.Tests
{
    public class EventDetectorTests
    {
        [Fact]
        public void UnloadEvent_ShouldNotBeDetected_WhenRemainingAmountBecameLessZero()
        {
            var series = TestSeriesGenerator.FromMovements(new[]
            {
                (0d, 0d),
                (-590, 590),
                (-1450, 860),
                (52885, 1770)
            }).ToList();

            var unloadEventDetector = new CashUnloadEventDetector();

            for (int i = 0; i < series.Count - 1; i++)
            {
                Assert.False(unloadEventDetector.Match(series, i));
            }
        }

        [Fact]
        public void LoadEvent_ShouldProcessCorrectly_WhenRemainingAmountBecameLessZero()
        {
            var series = TestSeriesGenerator.FromMovements(new[]
            {
                (0d, 0d),
                (-590, 590),
                (-1450, 860),
                (52885, 1770)
            }).ToList();

            var loadEventDetector = new CashLoadEventDetector();

            Assert.False(loadEventDetector.Match(series, 0));
            Assert.False(loadEventDetector.Match(series, 1));
            Assert.True(loadEventDetector.Match(series, 2));

            var result = loadEventDetector.Produce(series, 2) as AtmCashLoadEvent;
            
            Assert.NotNull(result);
            Assert.Equal(52885 + 1770 + 860 + 590, result.Amount);
        }
    }
}