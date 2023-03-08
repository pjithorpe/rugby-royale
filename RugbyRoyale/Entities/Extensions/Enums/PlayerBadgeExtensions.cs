using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.GameObjects;
using System.Collections.Generic;

namespace RugbyRoyale.Entities.Extensions
{
    public static class PlayerBadgeExtensions
    {
        private static readonly Dictionary<PlayerBadge, PlayerBadgeProfile> badgeProfileMappings = new()
        {
            { PlayerBadge.Fast, new PlayerBadgeProfile(PlayerBadge.Fast, "🏃") },
            { PlayerBadge.Finisher, new PlayerBadgeProfile(PlayerBadge.Finisher, "🚩", "Born Finisher") },
            { PlayerBadge.HardHitter, new PlayerBadgeProfile(PlayerBadge.HardHitter, "💢", "Hard Hitter") },
            { PlayerBadge.Jackler, new PlayerBadgeProfile(PlayerBadge.Jackler, "🦊") },
            { PlayerBadge.Leader, new PlayerBadgeProfile(PlayerBadge.Leader, "🎖") },
            { PlayerBadge.LineoutSpecialist, new PlayerBadgeProfile(PlayerBadge.LineoutSpecialist, "🙌", "Lineout Specialist") },
            { PlayerBadge.Magician, new PlayerBadgeProfile(PlayerBadge.Magician, "🧙") },
            { PlayerBadge.PlaceKicker, new PlayerBadgeProfile(PlayerBadge.PlaceKicker, "🎯", "Accurate Kicker") },
            { PlayerBadge.ScrumSpecialist, new PlayerBadgeProfile(PlayerBadge.ScrumSpecialist, "🤼", "Scrum Specialist") },
            { PlayerBadge.TacticalKicker, new PlayerBadgeProfile(PlayerBadge.TacticalKicker, "↕", "Tactical Kicker") },
            { PlayerBadge.Versatile, new PlayerBadgeProfile(PlayerBadge.Versatile, "🌐") },
            { PlayerBadge.Workhorse, new PlayerBadgeProfile(PlayerBadge.Workhorse, "🐴") },
        };

        public static PlayerBadgeProfile GetProfile(this PlayerBadge playerBadge)
        {
            return badgeProfileMappings[playerBadge];
        }
    }
}