using RugbyRoyale.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RugbyRoyale.Entities.Extensions
{
    public static class PositionExtensions
    {
        private static readonly Position[] forwardsPositions = new Position[] {
            Position.LooseheadProp, Position.Hooker, Position.TightheadProp,
            Position.Number4Lock, Position.Number5Lock,
            Position.BlindsideFlanker, Position.Number8, Position.OpensideFlanker
        };

        private static readonly Position[] backsPositions = new Position[] {
            Position.ScrumHalf, Position.FlyHalf,
            Position.InsideCentre, Position.OutsideCentre,
            Position.LeftWing, Position.FullBack, Position.RightWing
        };

        private static readonly Position[] spinePositions = new Position[]
        {
            Position.Hooker, Position.Number8, Position.ScrumHalf, Position.FlyHalf, Position.FullBack
        };

        private static readonly Dictionary<Position, Position[]> altPositions = new Dictionary<Position, Position[]>()
        {
            { Position.Number4Lock, new[]{ Position.Number5Lock } },
            { Position.Number5Lock, new[]{ Position.Number4Lock } },
            { Position.BlindsideFlanker, new[]{ Position.OpensideFlanker } },
            { Position.OpensideFlanker, new[]{ Position.BlindsideFlanker } },
            { Position.LeftWing, new[]{ Position.RightWing } },
            { Position.RightWing, new[]{ Position.LeftWing } },
        };

        private static readonly Dictionary<Position, Position[]> commonAltPositions = new Dictionary<Position, Position[]>()
        {
            { Position.LooseheadProp, new[]{ Position.TightheadProp } },
            { Position.TightheadProp, new[]{ Position.LooseheadProp } },
            { Position.Number4Lock, new[]{ Position.BlindsideFlanker } },
            { Position.Number5Lock, new[]{ Position.BlindsideFlanker } },
            { Position.BlindsideFlanker, new[]{ Position.OpensideFlanker, Position.Number8 } },
            { Position.OpensideFlanker, new[]{ Position.BlindsideFlanker, Position.Number8 } },
            { Position.Number8, new[]{ Position.BlindsideFlanker, Position.OpensideFlanker } },
            { Position.FlyHalf, new[]{ Position.InsideCentre } },
            { Position.InsideCentre, new[]{ Position.OutsideCentre } },
            { Position.OutsideCentre, new[]{ Position.InsideCentre } },
            { Position.LeftWing, new[]{ Position.FullBack } },
            { Position.RightWing, new[]{ Position.FullBack } },
            { Position.FullBack, new[]{ Position.LeftWing, Position.RightWing } },
        };

        private static readonly Dictionary<Position, Position[]> uncommonAltPositions = new Dictionary<Position, Position[]>()
        {
            { Position.LooseheadProp, new[]{ Position.Hooker } },
            { Position.TightheadProp, new[]{ Position.Hooker } },
            { Position.Hooker, new[]{ Position.BlindsideFlanker, Position.Number8, Position.TightheadProp, Position.LooseheadProp } },
            { Position.Number4Lock, new[]{ Position.OpensideFlanker, Position.Number8 } },
            { Position.Number5Lock, new[]{ Position.OpensideFlanker, Position.Number8 } },
            { Position.BlindsideFlanker, new[]{ Position.Hooker } },
            { Position.OpensideFlanker, new[]{ Position.Hooker } },
            { Position.Number8, new[]{ Position.Number4Lock, Position.Number5Lock } },
            { Position.ScrumHalf, new[]{ Position.FlyHalf, Position.LeftWing, Position.RightWing } },
            { Position.FlyHalf, new[]{ Position.ScrumHalf, Position.OutsideCentre, Position.FullBack, Position.LeftWing, Position.RightWing } },
            { Position.InsideCentre, new[]{ Position.FullBack, Position.LeftWing, Position.RightWing } },
            { Position.OutsideCentre, new[]{ Position.FullBack, Position.LeftWing, Position.RightWing } },
            { Position.LeftWing, new[]{ Position.OutsideCentre } },
            { Position.RightWing, new[]{ Position.OutsideCentre } },
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

        public static Position[] Alternatives(this Position position)
        {
            return altPositions[position];
        }

        public static Position[] CommonAlternatives(this Position position)
        {
            return commonAltPositions[position];
        }

        public static Position[] UncommonAlternatives(this Position position)
        {
            return uncommonAltPositions[position];
        }
    }
}