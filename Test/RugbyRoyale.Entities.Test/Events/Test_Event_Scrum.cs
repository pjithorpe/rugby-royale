using RugbyRoyale.Entities.Events;
using Xunit;

namespace RugbyRoyale.Entities.Test.Events
{
    public class Test_Event_Scrum
    {
        [Fact]
        public void ReturnsNonEmptyEventStrings()
        {
            var scrum = new Event_Scrum();

            TestHelper_MatchEvent.MatchEventTypeReturnsNonEmptyEventStrings(scrum);
        }

        [Fact]
        public void HasNonEmptyDisplayName()
        {
            var scrum = new Event_Scrum();

            TestHelper_MatchEvent.MatchEventTypeHasNonEmptyDisplayName(scrum);
        }
    }
}