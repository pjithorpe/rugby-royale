using RugbyRoyale.Entities.Events;
using Xunit;

namespace RugbyRoyale.Entities.Test.Events
{
    public class Test_Event_FreeKick
    {
        [Fact]
        public void ReturnsNonEmptyEventStrings()
        {
            var freeKick = new Event_FreeKick();

            TestHelper_MatchEvent.MatchEventTypeReturnsNonEmptyEventStrings(freeKick);
        }

        [Fact]
        public void HasNonEmptyDisplayName()
        {
            var freeKick = new Event_FreeKick();

            TestHelper_MatchEvent.MatchEventTypeHasNonEmptyDisplayName(freeKick);
        }
    }
}