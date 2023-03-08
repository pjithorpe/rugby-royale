using FluentAssertions;
using RugbyRoyale.Entities.Events;
using System.Reflection;
using Xunit;

namespace RugbyRoyale.Entities.Test.Events
{
    public class Test_Event_DropOut
    {
        [Fact]
        public void ReturnsNonEmptyEventStrings()
        {
            var dropOut = new Event_DropOut();

            dropOut.EventMessages.Should().NotBeEmpty();

            foreach (var message in dropOut.EventMessages)
            {
                message.Should().NotBeNullOrWhiteSpace();
            }
        }

        [Fact]
        public void HasNonEmptyDisplayName()
        {
            var dropOut = new Event_DropOut();

            dropOut.DisplayName.Should().NotBeNullOrWhiteSpace();
        }
    }
}