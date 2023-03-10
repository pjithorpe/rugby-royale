using FluentAssertions;
using RugbyRoyale.Entities.Events;

namespace RugbyRoyale.Entities.Test.Events
{
    internal static class TestHelper_MatchEvent
    {
        public static void MatchEventTypeReturnsNonEmptyEventStrings(IMatchEventType matchEventType)
        {
            matchEventType.EventMessages.Should().NotBeEmpty();

            foreach (var message in matchEventType.EventMessages)
            {
                message.Should().NotBeNullOrWhiteSpace();
            }
        }

        public static void MatchEventTypeHasNonEmptyDisplayName(IMatchEventType matchEventType)
        {
            matchEventType.DisplayName.Should().NotBeNullOrWhiteSpace();
        }
    }
}
