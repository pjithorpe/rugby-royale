using RugbyRoyale.Entities.Enums;
using System.Collections.Generic;

namespace RugbyRoyale.Entities.Extensions
{
    public static class TeamsheetPositionExtensions
    {
        private static readonly Dictionary<TeamsheetPosition, TeamsheetPosition[]> altPositions =
            new Dictionary<TeamsheetPosition, TeamsheetPosition[]>()
            {
                { TeamsheetPosition.Number4Lock, new[]{ TeamsheetPosition.Number5Lock } },
                { TeamsheetPosition.Number5Lock, new[]{ TeamsheetPosition.Number4Lock } },
                { TeamsheetPosition.BlindsideFlanker, new[]{ TeamsheetPosition.OpensideFlanker } },
                { TeamsheetPosition.OpensideFlanker, new[]{ TeamsheetPosition.BlindsideFlanker } },
                { TeamsheetPosition.LeftWing, new[]{ TeamsheetPosition.RightWing } },
                { TeamsheetPosition.RightWing, new[]{ TeamsheetPosition.LeftWing } },
            };

        private static readonly Dictionary<TeamsheetPosition, Position> positionMappings =
            new Dictionary<TeamsheetPosition, Position>()
            {
                { TeamsheetPosition.LooseheadProp, Position.Prop },
                { TeamsheetPosition.TightheadProp, Position.Prop },
                { TeamsheetPosition.Hooker, Position.Hooker },
                { TeamsheetPosition.Number4Lock, Position.Lock },
                { TeamsheetPosition.Number5Lock, Position.Lock },
                { TeamsheetPosition.BlindsideFlanker, Position.Flanker },
                { TeamsheetPosition.OpensideFlanker, Position.Flanker },
                { TeamsheetPosition.Number8, Position.Number8 },
                { TeamsheetPosition.ScrumHalf, Position.ScrumHalf },
                { TeamsheetPosition.FlyHalf, Position.FlyHalf },
                { TeamsheetPosition.InsideCentre, Position.Centre },
                { TeamsheetPosition.OutsideCentre, Position.Centre },
                { TeamsheetPosition.LeftWing, Position.Wing },
                { TeamsheetPosition.RightWing, Position.Wing },
                { TeamsheetPosition.FullBack, Position.FullBack },
            };

        public static TeamsheetPosition[] Alternatives(this TeamsheetPosition position)
        {
            TeamsheetPosition[] result;
            if (altPositions.TryGetValue(position, out result))
            {
                return result;
            }
            return new TeamsheetPosition[] { };
        }

        public static Position ToPosition(this TeamsheetPosition position)
        {
            return positionMappings[position];
        }
    }
}