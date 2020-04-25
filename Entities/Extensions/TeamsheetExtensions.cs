using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Model;
using System.Collections.Generic;

namespace RugbyRoyale.Entities.Extensions
{
    public static class TeamsheetExtensions
    {
        public static Dictionary<Position, Player> GetStartersDict(this Teamsheet teamsheet)
        {
            return new Dictionary<Position, Player>()
            {
                { Position.LooseheadProp, teamsheet.LooseheadProp },
                { Position.Hooker, teamsheet.Hooker },
                { Position.TightheadProp, teamsheet.TightheadProp },
                { Position.Number4Lock, teamsheet.Number4Lock },
                { Position.Number5Lock, teamsheet.Number5Lock },
                { Position.BlindsideFlanker, teamsheet.BlindsideFlanker },
                { Position.OpensideFlanker, teamsheet.OpensideFlanker },
                { Position.Number8, teamsheet.Number8 },
                { Position.ScrumHalf, teamsheet.ScrumHalf },
                { Position.FlyHalf, teamsheet.FlyHalf },
                { Position.LeftWing, teamsheet.LeftWing },
                { Position.InsideCentre, teamsheet.InsideCentre },
                { Position.OutsideCentre, teamsheet.OutsideCentre },
                { Position.RightWing, teamsheet.RightWing },
                { Position.FullBack, teamsheet.FullBack },
            };
        }
    }
}
