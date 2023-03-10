using RugbyRoyale.Entities.Events;
using Xunit;

namespace RugbyRoyale.Entities.Test.Events
{
    public class Test_Event_Conversion
    {
        [Fact]
        public void ReturnsNonEmptyEventStrings()
        {
            var conversion = new Event_Conversion();

            TestHelper_MatchEvent.MatchEventTypeReturnsNonEmptyEventStrings(conversion);
        }

        [Fact]
        public void HasNonEmptyDisplayName()
        {
            var conversion = new Event_Conversion();

            TestHelper_MatchEvent.MatchEventTypeHasNonEmptyDisplayName(conversion);
        }
    }
}