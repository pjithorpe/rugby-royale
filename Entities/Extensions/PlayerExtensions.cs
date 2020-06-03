using RugbyRoyale.Entities.Model;
using System.Linq;

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

        public static int TotalStats(this Player player)
        {
            return player.Attack + player.Defence + player.Physicality + player.Stamina + player.Handling + player.Kicking;
        }
    }
}