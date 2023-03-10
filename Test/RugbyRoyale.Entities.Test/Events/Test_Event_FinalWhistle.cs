using RugbyRoyale.Entities.Events;
using Xunit;

namespace RugbyRoyale.Entities.Test.Events
{
    public class Test_Event_FinalWhistle
    {
        [Fact]
        public void ReturnsNonEmptyEventStrings()
        {
            var finalWhistle = new Event_FinalWhistle();

            TestHelper_MatchEvent.MatchEventTypeReturnsNonEmptyEventStrings(finalWhistle);
        }

        [Fact]
        public void HasNonEmptyDisplayName()
        {
            var finalWhistle = new Event_FinalWhistle();

            TestHelper_MatchEvent.MatchEventTypeHasNonEmptyDisplayName(finalWhistle);
        }
    }
}