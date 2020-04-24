using Microsoft.EntityFrameworkCore;
using RugbyRoyale.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace RugbyRoyale.Entities.Model
{
    public class Player
    {
        public Guid PlayerID { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Rating { get; set; }
        public PlayerFocus Focus { get; set; }

        private string _positionsPrimary;
        [NotMapped]
        [BackingField(nameof(_positionsPrimary))]
        public List<Position> Positions_Primary
        {
            get
            {
                return _positionsPrimary
                    .Split(',')
                    .Select(s => (Position)Enum.Parse(typeof(Position), s))
                    .ToList();
            }
            set { _positionsPrimary = string.Join(',', value.Select(p => p.ToString())); }
        }

        private string _positionsSecondary;
        [NotMapped]
        [BackingField(nameof(_positionsSecondary))]
        public List<Position> Positions_Secondary
        {
            get
            {
                return _positionsSecondary
                    .Split(',')
                    .Select(s => (Position)Enum.Parse(typeof(Position), s))
                    .ToList();
            }
            set { _positionsSecondary = string.Join(',', value.Select(p => p.ToString())); }
        }

        private string _badges;
        [NotMapped]
        [BackingField(nameof(_badges))]
        public List<PlayerBadge> Badges
        {
            get
            {
                return _badges
                    .Split(',')
                    .Select(s => (PlayerBadge)Enum.Parse(typeof(PlayerBadge), s))
                    .ToList();
            }
            set { _badges = string.Join(',', value.Select(p => p.ToString())); }
        }
    }
}
