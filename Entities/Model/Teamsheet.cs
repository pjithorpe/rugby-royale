using System;

namespace RugbyRoyale.Entities.Model
{
    public class Teamsheet
    {
        public Guid TeamsheetID { get; set; }
        public Guid TightheadProp { get; set; }
        public Guid Hooker { get; set; }
        public Guid LooseheadProp { get; set; }
        public Guid Number4Lock { get; set; }
        public Guid Number5Lock { get; set; }
        public Guid BlindsideFlanker { get; set; }
        public Guid OpensideFlanker { get; set; }
        public Guid Number8 { get; set; }
        public Guid ScrumHalf { get; set; }
        public Guid FlyHalf { get; set; }
        public Guid InsideCentre { get; set; }
        public Guid OutsideCentre { get; set; }
        public Guid LeftWing { get; set; }
        public Guid RightWing { get; set; }
        public Guid FullBack { get; set; }
        public Guid Replacement16 { get; set; }
        public Guid Replacement17 { get; set; }
        public Guid Replacement18 { get; set; }
        public Guid Replacement19 { get; set; }
        public Guid Replacement20 { get; set; }
        public Guid Replacement21 { get; set; }
        public Guid Replacement22 { get; set; }
        public Guid Replacement23 { get; set; }

        public Guid TeamID { get; set; }

        public Team Team { get; set; }
    }
}
