using RugbyRoyale.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RugbyRoyale.Entities.Extensions
{
    public static class PositionExtensions
    {
        private static readonly Position[] forwardsPositions = new Position[] {
            Position.Prop, Position.Hooker, Position.Lock, Position.Flanker, Position.Number8,
        };

        private static readonly Position[] backsPositions = new Position[] {
            Position.ScrumHalf, Position.FlyHalf, Position.Centre, Position.Wing, Position.FullBack,
        };

        private static readonly Position[] spinePositions = new Position[]
        {
            Position.Hooker, Position.Number8, Position.ScrumHalf, Position.FlyHalf, Position.FullBack
        };

        private static readonly Dictionary<Position, Position[]> commonAltPositions = new Dictionary<Position, Position[]>()
        {
            { Position.Lock, new[]{ Position.Flanker } },
            { Position.Flanker, new[]{ Position.Number8 } },
            { Position.Number8, new[]{ Position.Flanker } },
            { Position.FlyHalf, new[]{ Position.Centre } },
            { Position.Wing, new[]{ Position.FullBack } },
            { Position.FullBack, new[]{ Position.Wing } },
        };

        private static readonly Dictionary<Position, Position[]> uncommonAltPositions = new Dictionary<Position, Position[]>()
        {
            { Position.Prop, new[]{ Position.Hooker } },
            { Position.Hooker, new[]{ Position.Flanker, Position.Number8, Position.Prop } },
            { Position.Lock, new[]{ Position.Flanker, Position.Number8 } },
            { Position.Flanker, new[]{ Position.Hooker } },
            { Position.Number8, new[]{ Position.Lock } },
            { Position.ScrumHalf, new[]{ Position.FlyHalf, Position.Wing } },
            { Position.FlyHalf, new[]{ Position.ScrumHalf, Position.Centre, Position.FullBack, Position.Wing } },
            { Position.Centre, new[]{ Position.FullBack, Position.Wing } },
            { Position.Wing, new[]{ Position.Centre } },
            { Position.FullBack, new[]{ Position.FlyHalf } }
        };

        public static bool IsForward(this Position position)
        {
            return forwardsPositions.Contains(position);
        }

        public static bool IsBack(this Position position)
        {
            return backsPositions.Contains(position);
        }

        public static bool IsSpine(this Position position)
        {
            return spinePositions.Contains(position);
        }

        public static Position[] CommonAlternatives(this Position position)
        {
            Position[] result;
            if (commonAltPositions.TryGetValue(position, out result))
            {
                return result;
            }
            return new Position[] { };
        }

        public static Position[] UncommonAlternatives(this Position position)
        {
            Position[] result;
            if (uncommonAltPositions.TryGetValue(position, out result))
            {
                return result;
            }
            return new Position[] { };
        }
    }
}