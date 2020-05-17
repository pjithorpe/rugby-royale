using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    [Group("royale")]
    internal class Group_Royale : BaseCommandModule
    {
        [Command("hi"), Aliases("hello", "howdy")]
        public async Task Hi(CommandContext context)
        {
            await context.RespondAsync($"👋 Hi, {context.User.Mention}!");
        }
    }
}