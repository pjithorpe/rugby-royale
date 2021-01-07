using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using System.Threading.Tasks;

namespace DSharpPlus.Helpers
{
    public class ReactionOrMessageTask
    {
        public ReactionOrMessageTask(Task reactionOrMessageTask)
        {
            if (reactionOrMessageTask is Task<InteractivityResult<MessageReactionAddEventArgs>> reactionResponse)
            {
                ReactionTask = reactionResponse;
                MessageTask = null;
            }
            else if (reactionOrMessageTask is Task<InteractivityResult<DiscordMessage>> messageResponse)
            {
                ReactionTask = null;
                MessageTask = messageResponse;
            }
        }

        public Task<InteractivityResult<MessageReactionAddEventArgs>> ReactionTask { get; private set; }
        public Task<InteractivityResult<DiscordMessage>> MessageTask { get; private set; }
    }
}
