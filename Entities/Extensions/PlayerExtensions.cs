using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RugbyRoyale.Entities.Extensions
{
    public static class PlayerExtensions
    {
        public static bool IsAForward(this Player player)
        {
            return player.Positions_Primary.Any(x => x.IsForward());
        }

        public static bool IsABack(this Player player)
        {
            return player.Positions_Primary.Any(x => x.IsBack());
        }
    }
}
