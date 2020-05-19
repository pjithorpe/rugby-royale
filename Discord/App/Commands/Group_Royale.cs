using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using RugbyRoyale.Discord.App.Attributes;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    [Group("royale")]
    [MainChannel]
    internal class Group_Royale : BaseCommandModule
    {
        [Command("new"), Aliases("newteam")]
        public async Task New(CommandContext context)
        {
            await context.RespondAsync($"👋 Hi, {context.User.Mention}!");
        }
    }
}