using Moq;
using RugbyRoyale.Entities.Events;

internal static class MockHelper
{
    /// <param name="eventMessages">Optionally provide a set of event messages. If null, will be automatically populated</param>
    public static IMatchEventType SetupMockIMatchEventType(string displayName = "Default Match Event", string[]? eventMessages = null)
    {
        var mock = new Mock<IMatchEventType>();
        mock.Setup(m => m.DisplayName)
            .Returns(displayName);
        mock.Setup(m => m.EventMessages)
            .Returns(eventMessages ?? new string[] { "An event happened", "There was an event" });

        return mock.Object;
    }
}