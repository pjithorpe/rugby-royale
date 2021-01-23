using Destructurama.Attributed;
using System;

namespace RugbyRoyale.Entities.Model
{
    public class Teamsheet
    {
        // PKs
        public Guid TeamsheetID { get; set; }

        // Fields

        // FKs
        public Guid TeamID { get; set; }
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

        // Navigation properties
        [NotLogged]
        public Team Team { get; set; }
        [NotLogged]
        public Player TightheadProp { get; set; }
        [NotLogged]
        public Player Hooker { get; set; }
        [NotLogged]
        public Player LooseheadProp { get; set; }
        [NotLogged]
        public Player Number4Lock { get; set; }
        [NotLogged]
        public Player Number5Lock { get; set; }
        [NotLogged]
        public Player BlindsideFlanker { get; set; }
        [NotLogged]
        public Player OpensideFlanker { get; set; }
        [NotLogged]
        public Player Number8 { get; set; }
        [NotLogged]
        public Player ScrumHalf { get; set; }
        [NotLogged]
        public Player FlyHalf { get; set; }
        [NotLogged]
        public Player InsideCentre { get; set; }
        [NotLogged]
        public Player OutsideCentre { get; set; }
        [NotLogged]
        public Player LeftWing { get; set; }
        [NotLogged]
        public Player RightWing { get; set; }
        [NotLogged]
        public Player FullBack { get; set; }
        [NotLogged]
        public Player Replacement16 { get; set; }
        [NotLogged]
        public Player Replacement17 { get; set; }
        [NotLogged]
        public Player Replacement18 { get; set; }
        [NotLogged]
        public Player Replacement19 { get; set; }
        [NotLogged]
        public Player Replacement20 { get; set; }
        [NotLogged]
        public Player Replacement21 { get; set; }
        [NotLogged]
        public Player Replacement22 { get; set; }
        [NotLogged]
        public Player Replacement23 { get; set; }
    }
}