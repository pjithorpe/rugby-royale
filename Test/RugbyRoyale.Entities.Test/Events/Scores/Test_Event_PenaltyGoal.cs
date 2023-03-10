using RugbyRoyale.Entities.Events;
using Xunit;

namespace RugbyRoyale.Entities.Test.Events
{
    public class Test_Event_PenaltyGoal
    {
        [Fact]
        public void ReturnsNonEmptyEventStrings()
        {
            var penaltyGoal = new Event_PenaltyGoal();

            TestHelper_MatchEvent.MatchEventTypeReturnsNonEmptyEventStrings(penaltyGoal);
        }

        [Fact]
        public void HasNonEmptyDisplayName()
        {
            var penaltyGoal = new Event_PenaltyGoal();

            TestHelper_MatchEvent.MatchEventTypeHasNonEmptyDisplayName(penaltyGoal);
        }
    }
}