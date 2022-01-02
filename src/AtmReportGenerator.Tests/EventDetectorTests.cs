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
            var series = TestSeriesGenerator.FromMovements(
                (0d, 0d),
                (-590, 590),
                (-1450, 860),
                (52885, 1770));

            var unloadEventDetector = new CashUnloadEventDetector();

            for (var i = 0; i < series.Count - 1; i++) Assert.False(unloadEventDetector.Match(series, i));
        }

        [Fact]
        public void UnloadEvent_ShouldDetectEvent_WhenNextIsZero()
        {
            var series = TestSeriesGenerator.FromMovements(
                (10000d, 100d),
                (0, 0),
                (70000d, 860));

            var unloadEventDetector = new CashUnloadEventDetector();

            Assert.True(unloadEventDetector.Match(series, 0));
            Assert.False(unloadEventDetector.Match(series, 1));

            var result = unloadEventDetector.Produce(series, 0) as AtmCashUnloadEvent;

            Assert.NotNull(result);
            Assert.Equal(10000d, result.Amount);
        }

        [Fact]
        public void UnloadEvent_ShouldDetectEvent_WhenAmountIncreasedImmediately()
        {
            var series = TestSeriesGenerator.FromMovements(
                (10000d, 100d),
                (70000d, 860));

            var unloadEventDetector = new CashUnloadEventDetector();

            Assert.True(unloadEventDetector.Match(series, 0));

            var result = unloadEventDetector.Produce(series, 0) as AtmCashUnloadEvent;

            Assert.NotNull(result);
            Assert.Equal(10000d, result.Amount);
        }

        [Fact]
        public void LoadEvent_ShouldProcessCorrectly_WhenRemainingAmountBecameLessZero()
        {
            var series = TestSeriesGenerator.FromMovements(
                (0d, 0d),
                (-590, 590),
                (-1450, 860),
                (52885, 1770));

            var loadEventDetector = new CashLoadEventDetector();

            Assert.False(loadEventDetector.Match(series, 0));
            Assert.False(loadEventDetector.Match(series, 1));
            Assert.True(loadEventDetector.Match(series, 2));

            var result = loadEventDetector.Produce(series, 2) as AtmCashLoadEvent;

            Assert.NotNull(result);
            Assert.Equal(52885 + 1770 + 860 + 590, result.Amount);
        }

        [Fact]
        public void LoadEvent_ShouldDetectEvent_WhenCurrentIsZero()
        {
            var series = TestSeriesGenerator.FromMovements(
                (10000d, 100d),
                (0, 0),
                (70000d, 860));

            var loadEventDetector = new CashLoadEventDetector();

            Assert.False(loadEventDetector.Match(series, 0));
            Assert.True(loadEventDetector.Match(series, 1));

            var result = loadEventDetector.Produce(series, 1) as AtmCashLoadEvent;

            Assert.NotNull(result);
            Assert.Equal(70860d, result.Amount);
        }

        [Fact]
        public void LoadEvent_ShouldDetectEvent_WhenAmountIncreasedImmediately()
        {
            var series = TestSeriesGenerator.FromMovements(
                (10000d, 100d),
                (70000d, 860));

            var loadEventDetector = new CashLoadEventDetector();

            Assert.True(loadEventDetector.Match(series, 0));

            var result = loadEventDetector.Produce(series, 0) as AtmCashLoadEvent;

            Assert.NotNull(result);
            Assert.Equal(70860d, result.Amount);
        }
    }
}