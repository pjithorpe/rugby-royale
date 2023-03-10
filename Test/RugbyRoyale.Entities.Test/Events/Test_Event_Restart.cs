using RugbyRoyale.Entities.Events;
using Xunit;

namespace RugbyRoyale.Entities.Test.Events
{
    public class Test_Event_Restart
    {
        [Fact]
        public void ReturnsNonEmptyEventStrings()
        {
            var restart = new Event_Restart();

            TestHelper_MatchEvent.MatchEventTypeReturnsNonEmptyEventStrings(restart);
        }

        [Fact]
        public void HasNonEmptyDisplayName()
        {
            var restart = new Event_Restart();

            TestHelper_MatchEvent.MatchEventTypeHasNonEmptyDisplayName(restart);
        }
    }
}