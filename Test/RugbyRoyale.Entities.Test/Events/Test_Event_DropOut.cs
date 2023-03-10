using RugbyRoyale.Entities.Events;
using Xunit;

namespace RugbyRoyale.Entities.Test.Events
{
    public class Test_Event_DropOut
    {
        [Fact]
        public void ReturnsNonEmptyEventStrings()
        {
            var dropOut = new Event_DropOut();

            TestHelper_MatchEvent.MatchEventTypeReturnsNonEmptyEventStrings(dropOut);
        }

        [Fact]
        public void HasNonEmptyDisplayName()
        {
            var dropOut = new Event_DropOut();

            TestHelper_MatchEvent.MatchEventTypeHasNonEmptyDisplayName(dropOut);
        }
    }
}