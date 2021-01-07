using RugbyRoyale.Entities.Enums;

namespace RugbyRoyale.Entities.LeagueTypes
{
    public class LeagueRules_RoundRobin : LeagueRules
    {
        public override string Name => "Round-Robin";
        public override string Description => "Over the course of a season, each team plays all other teams once, either home or away.";
        public override LeagueType LeagueType => LeagueType.RoundRobin;

        public override int MaxSize => 24;
    }
}
