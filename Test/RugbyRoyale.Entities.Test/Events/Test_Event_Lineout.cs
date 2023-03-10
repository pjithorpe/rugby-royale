using RugbyRoyale.Entities.Events;
using Xunit;

namespace RugbyRoyale.Entities.Test.Events
{
    public class Test_Event_Lineout
    {
        [Fact]
        public void ReturnsNonEmptyEventStrings()
        {
            var lineout = new Event_Lineout();

            TestHelper_MatchEvent.MatchEventTypeReturnsNonEmptyEventStrings(lineout);
        }

        [Fact]
        public void HasNonEmptyDisplayName()
        {
            var lineout = new Event_Lineout();

            TestHelper_MatchEvent.MatchEventTypeHasNonEmptyDisplayName(lineout);
        }
    }
}