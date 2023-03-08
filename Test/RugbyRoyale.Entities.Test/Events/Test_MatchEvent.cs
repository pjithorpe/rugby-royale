using FluentAssertions;
using RugbyRoyale.Entities.Events;
using RugbyRoyale.Entities.Extensions.Event;
using Xunit;

namespace RugbyRoyale.Entities.Test.Events
{
    public class Test_MatchEvent
    {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(4801, 81)]
        public void CalculatesCorrectMinute(int seconds, int correctMinute)
        {
            var _matchEventType = MockHelper.SetupMockIMatchEventType();
            var matchEvent = new MatchEvent(_matchEventType, seconds);

            matchEvent.Minute.Should().Be(correctMinute);
        }

        [Theory]
        [InlineData(-1)]
        public void RejectsInvalidSeconds(int seconds)
        {
            var _matchEventType = MockHelper.SetupMockIMatchEventType();

            Action constructMatchEvent = () => new MatchEvent(_matchEventType, seconds);

            constructMatchEvent.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ReturnsEventString()
        {
            var _matchEventType = MockHelper.SetupMockIMatchEventType();
            var matchEvent = new MatchEvent(_matchEventType, 0);

            matchEvent.GetEventMessage(new Random()).Should().NotBeNullOrWhiteSpace();
        }
    }
}
