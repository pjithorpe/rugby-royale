using System;

namespace RugbyRoyale.Entities.Model
{
    public class Teamsheet
    {
        public Guid TeamsheetID { get; set; }
        public Guid TightheadPropID { get; set; }
        public Guid HookerID { get; set; }
        public Guid LooseheadPropID { get; set; }
        public Guid Number4LockID { get; set; }
        public Guid Number5LockID { get; set; }
        public Guid BlindsideFlankerID { get; set; }
        public Guid OpensideFlankerID { get; set; }
        public Guid Number8ID { get; set; }
        public Guid ScrumHalfID { get; set; }
        public Guid FlyHalfID { get; set; }
        public Guid InsideCentreID { get; set; }
        public Guid OutsideCentreID { get; set; }
        public Guid LeftWingID { get; set; }
        public Guid RightWingID { get; set; }
        public Guid FullBackID { get; set; }
        public Guid Replacement16ID { get; set; }
        public Guid Replacement17ID { get; set; }
        public Guid Replacement18ID { get; set; }
        public Guid Replacement19ID { get; set; }
        public Guid Replacement20ID { get; set; }
        public Guid Replacement21ID { get; set; }
        public Guid Replacement22ID { get; set; }
        public Guid Replacement23ID { get; set; }
        public Guid TeamID { get; set; }

        public Player TightheadProp { get; set; }
        public Player Hooker { get; set; }
        public Player LooseheadProp { get; set; }
        public Player Number4Lock { get; set; }
        public Player Number5Lock { get; set; }
        public Player BlindsideFlanker { get; set; }
        public Player OpensideFlanker { get; set; }
        public Player Number8 { get; set; }
        public Player ScrumHalf { get; set; }
        public Player FlyHalf { get; set; }
        public Player InsideCentre { get; set; }
        public Player OutsideCentre { get; set; }
        public Player LeftWing { get; set; }
        public Player RightWing { get; set; }
        public Player FullBack { get; set; }
        public Player Replacement16 { get; set; }
        public Player Replacement17 { get; set; }
        public Player Replacement18 { get; set; }
        public Player Replacement19 { get; set; }
        public Player Replacement20 { get; set; }
        public Player Replacement21 { get; set; }
        public Player Replacement22 { get; set; }
        public Player Replacement23 { get; set; }
        public Team Team { get; set; }
    }
}