using DSharpPlus.Entities;
using System.Linq;

namespace DSharpPlus.Helpers
{
    public static class DiscordEmbedExtensions
    {
        public static DiscordEmbedBuilder ConstructSafely(this DiscordEmbedBuilder embed, string title, string description, string authorName = null, string footer = null)
        {
            var croppedEmbed = new DiscordEmbedBuilder()
            {
                Title = title?.Length > 256 ? title.Substring(0, 256) : title,
                Description = description?.Length > 2048 ? description.Substring(0, 2048) : description,
                Author = new DiscordEmbedBuilder.EmbedAuthor()
                {
                    Name = authorName?.Length > 256 ? authorName.Substring(0, 256) : authorName,
                },
                Footer = new DiscordEmbedBuilder.EmbedFooter()
                {
                    Text = footer?.Length > 2048 ? footer.Substring(0, 2048) : footer,
                }
            };

            return croppedEmbed;
        }

        public static void AddFieldSafely(this DiscordEmbedBuilder embed, string name, string value)
        {
            name = name.Length > 256 ? name.Substring(0, 256) : name;
            value = value.Length > 1024 ? value.Substring(0, 1024) : value;

            int totalChars = (embed.Title?.Length ?? 0) + (embed.Description?.Length ?? 0) + (embed.Author?.Name?.Length ?? 0) + (embed.Footer?.Text?.Length ?? 0);

            if (embed.Fields.Count > 0)
            {
                totalChars += embed.Fields.Select(x => x.Name.Length + x.Name.Length).Aggregate((x, y) => x + y);
            }

            if (totalChars + name?.Length + value?.Length > 6000) return;

            embed.AddField(name, value);
        }
    }
}
