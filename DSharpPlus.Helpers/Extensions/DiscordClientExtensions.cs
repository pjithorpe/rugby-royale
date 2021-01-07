using DSharpPlus.Entities;

namespace DSharpPlus.Helpers
{
    public static class DiscordClientExtensions
    {
        public static DiscordEmoji AcceptEmoji(this DiscordClient client) => DiscordEmoji.FromName(client, ":white_check_mark:");
        public static DiscordEmoji RejectEmoji(this DiscordClient client) => DiscordEmoji.FromName(client, ":x:");
        public static DiscordEmoji SkipEmoji(this DiscordClient client) => DiscordEmoji.FromName(client, ":leftwards_arrow_with_hook:");
    }
}
