using RugbyRoyale.Entities.Events;
using Xunit;

namespace RugbyRoyale.Entities.Test.Events
{
    public class Test_Event_Turnover
    {
        [Fact]
        public void ReturnsNonEmptyEventStrings()
        {
            var turnover = new Event_Turnover();

            TestHelper_MatchEvent.MatchEventTypeReturnsNonEmptyEventStrings(turnover);
        }

        [Fact]
        public void HasNonEmptyDisplayName()
        {
            var turnover = new Event_Turnover();

            TestHelper_MatchEvent.MatchEventTypeHasNonEmptyDisplayName(turnover);
        }
    }
}