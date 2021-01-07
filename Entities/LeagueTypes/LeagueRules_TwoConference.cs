using RugbyRoyale.Entities.Enums;

namespace RugbyRoyale.Entities.LeagueTypes
{
    public class LeagueRules_TwoConference : LeagueRules
    {
        public override string Name => "Two Conference";
        public override string Description => "Over the course of a season, each team plays all other teams in their conference both home and away, and once against each team in the other conference, either home or away.";
        public override LeagueType LeagueType => LeagueType.TwoConference;

        public override int Conferences => 2;
        public override int MaxSize => 20;

    }
}