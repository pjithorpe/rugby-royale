using RugbyRoyale.Entities.Enums;

namespace RugbyRoyale.Entities.GameObjects
{
    public class PlayerBadgeProfile
    {
        public string Name { get; }
        public string Emoji { get; }
        public PlayerBadge Badge { get; }

        public PlayerBadgeProfile(PlayerBadge badge, string emoji, string name)
        {
            Name = name;
            Emoji = emoji;
            Badge = badge;
        }

        public PlayerBadgeProfile(PlayerBadge badge, string emoji) : this(badge, emoji, badge.ToString()) { }
    }
}
