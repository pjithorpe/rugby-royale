using System;
using System.Collections.Generic;
using System.Text;
using RugbyRoyale.Entities.Enums;

namespace RugbyRoyale.Entities.LeagueTypes
{
    public abstract class LeagueType
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        public virtual int Conferences => 1;
        public virtual int MinSize => 6;
        public virtual int MaxSize => 16;

        public abstract Enums.LeagueType Enumerate();
    }
}
