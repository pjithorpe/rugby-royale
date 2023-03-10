using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Events;
using Xunit;

namespace RugbyRoyale.Entities.Test.Events
{
    public class Test_Event_PenaltyAwarded
    {
        [Fact]
        public void ReturnsNonEmptyEventStrings()
        {
            var penaltyAwarded = new Event_PenaltyAwarded(PenaltyOffence.FoulPlay);

            TestHelper_MatchEvent.MatchEventTypeReturnsNonEmptyEventStrings(penaltyAwarded);
        }

        [Fact]
        public void HasNonEmptyDisplayName()
        {
            var penaltyAwarded = new Event_PenaltyAwarded(PenaltyOffence.FoulPlay);

            TestHelper_MatchEvent.MatchEventTypeHasNonEmptyDisplayName(penaltyAwarded);
        }
    }
}