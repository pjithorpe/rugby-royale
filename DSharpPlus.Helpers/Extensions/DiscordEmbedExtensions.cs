using DSharpPlus.Entities;
using System.Linq;

namespace DSharpPlus.Helpers
{
    public static class DiscordEmbedExtensions
    {
        /// <summary>
        /// Constructs an embed builder, cropping the title, description, author name and footer text if needed.
        /// </summary>
        /// <param name="embed"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="authorName"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static DiscordEmbedBuilder ConstructSafely(this DiscordEmbedBuilder embed, string title, string description, string authorName = null, string footer = null)
        {
            embed.Title = title?.Length > 256 ? title.Substring(0, 256) : title;
            embed.Description = description?.Length > 2048 ? description.Substring(0, 2048) : description;
            embed.Author = new DiscordEmbedBuilder.EmbedAuthor()
            {
                Name = authorName?.Length > 256 ? authorName.Substring(0, 256) : authorName,
            };
            embed.Footer = new DiscordEmbedBuilder.EmbedFooter()
            {
                Text = footer?.Length > 2048 ? footer.Substring(0, 2048) : footer,
            };

            return embed;
        }

        /// <summary>
        /// Adds a field to an embed builder, cropping its name and value if needed, and aborting the addition
        /// if it would cause the embed to be over the total character limit.
        /// </summary>
        /// <param name="embed"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="inline"></param>
        public static void AddFieldSafely(this DiscordEmbedBuilder embed, string name, string value, bool inline = false)
        {
            name = name.Length > 256 ? name = name.Substring(0, 256) : name;
            
            value = value.Length > 1024 ? value.Substring(0, 1024) : value;

            int totalChars = (embed.Title?.Length ?? 0) + (embed.Description?.Length ?? 0) + (embed.Author?.Name?.Length ?? 0) + (embed.Footer?.Text?.Length ?? 0);

            if (embed.Fields.Count > 0)
            {
                totalChars += embed.Fields.Select(x => x.Name.Length + x.Name.Length).Aggregate((x, y) => x + y);
            }

            if (totalChars + name?.Length + value?.Length > 6000) return;

            embed.AddField(name, value, inline);
        }
    }
}
