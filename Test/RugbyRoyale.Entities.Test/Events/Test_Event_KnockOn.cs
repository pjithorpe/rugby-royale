using RugbyRoyale.Entities.Events;
using Xunit;

namespace RugbyRoyale.Entities.Test.Events
{
    public class Test_Event_KnockOn
    {
        [Fact]
        public void ReturnsNonEmptyEventStrings()
        {
            var knockOn = new Event_KnockOn();

            TestHelper_MatchEvent.MatchEventTypeReturnsNonEmptyEventStrings(knockOn);
        }

        [Fact]
        public void HasNonEmptyDisplayName()
        {
            var knockOn = new Event_KnockOn();

            TestHelper_MatchEvent.MatchEventTypeHasNonEmptyDisplayName(knockOn);
        }
    }
}