﻿using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Model;
using System.Collections.Generic;

namespace RugbyRoyale.Entities.Extensions
{
    public static class TeamsheetExtensions
    {
        public static Dictionary<TeamsheetPosition, Player> GetStartersDict(this Teamsheet teamsheet)
        {
            return new Dictionary<TeamsheetPosition, Player>()
            {
                { TeamsheetPosition.LooseheadProp, teamsheet.LooseheadProp },
                { TeamsheetPosition.Hooker, teamsheet.Hooker },
                { TeamsheetPosition.TightheadProp, teamsheet.TightheadProp },
                { TeamsheetPosition.Number4Lock, teamsheet.Number4Lock },
                { TeamsheetPosition.Number5Lock, teamsheet.Number5Lock },
                { TeamsheetPosition.BlindsideFlanker, teamsheet.BlindsideFlanker },
                { TeamsheetPosition.OpensideFlanker, teamsheet.OpensideFlanker },
                { TeamsheetPosition.Number8, teamsheet.Number8 },
                { TeamsheetPosition.ScrumHalf, teamsheet.ScrumHalf },
                { TeamsheetPosition.FlyHalf, teamsheet.FlyHalf },
                { TeamsheetPosition.LeftWing, teamsheet.LeftWing },
                { TeamsheetPosition.InsideCentre, teamsheet.InsideCentre },
                { TeamsheetPosition.OutsideCentre, teamsheet.OutsideCentre },
                { TeamsheetPosition.RightWing, teamsheet.RightWing },
                { TeamsheetPosition.FullBack, teamsheet.FullBack },
            };
        }

        public static List<Player> GetPlayers(this Teamsheet teamsheet)
        {
            return new List<Player>()
            {
                teamsheet.LooseheadProp,
                teamsheet.Hooker,
                teamsheet.TightheadProp,
                teamsheet.Number4Lock,
                teamsheet.Number5Lock,
                teamsheet.BlindsideFlanker,
                teamsheet.OpensideFlanker,
                teamsheet.Number8,
                teamsheet.ScrumHalf,
                teamsheet.FlyHalf,
                teamsheet.LeftWing,
                teamsheet.InsideCentre,
                teamsheet.OutsideCentre,
                teamsheet.RightWing,
                teamsheet.FullBack,
            };
        }
    }
}