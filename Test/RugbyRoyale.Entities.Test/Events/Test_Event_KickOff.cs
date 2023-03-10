using RugbyRoyale.Entities.Events;
using Xunit;

namespace RugbyRoyale.Entities.Test.Events
{
    public class Test_Event_KickOff
    {
        [Fact]
        public void ReturnsNonEmptyEventStrings()
        {
            var kickOff = new Event_KickOff();

            TestHelper_MatchEvent.MatchEventTypeReturnsNonEmptyEventStrings(kickOff);
        }

        [Fact]
        public void HasNonEmptyDisplayName()
        {
            var kickOff = new Event_KickOff();

            TestHelper_MatchEvent.MatchEventTypeHasNonEmptyDisplayName(kickOff);
        }
    }
}