using RugbyRoyale.Entities.Model;
using Serilog;
using System;
using System.Linq;

namespace RugbyRoyale.Entities.Extensions
{
    public static class LeagueExtensions
    {
        /// <summary>
        /// Checks whether the number of participants equals or exceeds the maximum size of the League. (League.Users must be loaded)
        /// </summary>
        /// <param name="league">League with loaded Users</param>
        /// <returns></returns>
        public static bool IsFull(this League league)
        {
            if (league.Users == null)
            {
                Log.Warning("Attempted to use IsFull() method on League with no loaded Users. {@League}", league);
                return true;
            }

            return league.Users.Count >= league.Size_Max;
        }
    }
}