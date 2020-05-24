using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.LeagueTypes
{
    public abstract class LeagueType
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        public virtual int Conferences => 1;
        public virtual int MinSize => 8;
        public virtual int MaxSize => 14;
    }
}
