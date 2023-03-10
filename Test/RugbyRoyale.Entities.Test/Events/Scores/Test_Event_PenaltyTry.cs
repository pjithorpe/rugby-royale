using RugbyRoyale.Entities.Events;
using Xunit;

namespace RugbyRoyale.Entities.Test.Events
{
    public class Test_Event_PenaltyTry
    {
        [Fact]
        public void ReturnsNonEmptyEventStrings()
        {
            var penaltyTry = new Event_PenaltyTry();

            TestHelper_MatchEvent.MatchEventTypeReturnsNonEmptyEventStrings(penaltyTry);
        }

        [Fact]
        public void HasNonEmptyDisplayName()
        {
            var penaltyTry = new Event_PenaltyTry();

            TestHelper_MatchEvent.MatchEventTypeHasNonEmptyDisplayName(penaltyTry);
        }
    }
}