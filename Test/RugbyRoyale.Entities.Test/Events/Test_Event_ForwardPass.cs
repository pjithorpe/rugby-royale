using RugbyRoyale.Entities.Events;
using Xunit;

namespace RugbyRoyale.Entities.Test.Events
{
    public class Test_Event_ForwardPass
    {
        [Fact]
        public void ReturnsNonEmptyEventStrings()
        {
            var forwardPass = new Event_ForwardPass();

            TestHelper_MatchEvent.MatchEventTypeReturnsNonEmptyEventStrings(forwardPass);
        }

        [Fact]
        public void HasNonEmptyDisplayName()
        {
            var forwardPass = new Event_ForwardPass();

            TestHelper_MatchEvent.MatchEventTypeHasNonEmptyDisplayName(forwardPass);
        }
    }
}