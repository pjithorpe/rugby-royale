using RugbyRoyale.Entities.Events;
using Xunit;

namespace RugbyRoyale.Entities.Test.Events
{
    public class Test_Event_Try
    {
        [Fact]
        public void ReturnsNonEmptyEventStrings()
        {
            var tryEvent = new Event_Try();

            TestHelper_MatchEvent.MatchEventTypeReturnsNonEmptyEventStrings(tryEvent);
        }

        [Fact]
        public void HasNonEmptyDisplayName()
        {
            var tryEvent = new Event_Try();

            TestHelper_MatchEvent.MatchEventTypeHasNonEmptyDisplayName(tryEvent);
        }
    }
}