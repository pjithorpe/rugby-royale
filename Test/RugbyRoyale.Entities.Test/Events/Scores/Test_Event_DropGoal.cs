using RugbyRoyale.Entities.Events;
using Xunit;

namespace RugbyRoyale.Entities.Test.Events
{
    public class Test_Event_DropGoal
    {
        [Fact]
        public void ReturnsNonEmptyEventStrings()
        {
            var dropGoal = new Event_DropGoal();

            TestHelper_MatchEvent.MatchEventTypeReturnsNonEmptyEventStrings(dropGoal);
        }

        [Fact]
        public void HasNonEmptyDisplayName()
        {
            var dropGoal = new Event_DropGoal();

            TestHelper_MatchEvent.MatchEventTypeHasNonEmptyDisplayName(dropGoal);
        }
    }
}