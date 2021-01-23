using Destructurama.Attributed;
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
        // PKs
        public Guid PlayerID { get; set; }

        // Fields
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Rating { get; set; }
        public PlayerFocus Focus { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public int Physicality { get; set; }
        public int Stamina { get; set; }
        public int Handling { get; set; }
        public int Kicking { get; set; }

        // Backing Fields
        private string _positionsPrimary;
        private string _positionsSecondary;
        private string _badges;

        // FKs

        // Navigation properties

        // Unmapped properties
        [NotMapped]
        [BackingField(nameof(_positionsPrimary))]
        public List<Position> Positions_Primary
        {
            get
            {
                if (_positionsPrimary != null)
                {
                    return _positionsPrimary
                        .Split(',')
                        .Select(s => (Position)Enum.Parse(typeof(Position), s))
                        .ToList();
                }
                return new List<Position>();
            }
            set
            {
                _positionsPrimary = string.Join(',', value.Select(p => p.ToString()));
            }
        }

        [NotMapped]
        [BackingField(nameof(_positionsSecondary))]
        public List<Position> Positions_Secondary
        {
            get
            {
                if (_positionsSecondary != null)
                {
                    return _positionsSecondary
                        .Split(',')
                        .Select(s => (Position)Enum.Parse(typeof(Position), s))
                        .ToList();
                }
                return new List<Position>();
            }
            set { _positionsSecondary = string.Join(',', value.Select(p => p.ToString())); }
        }

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