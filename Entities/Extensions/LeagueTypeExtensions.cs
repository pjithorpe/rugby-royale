using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.GameObjects;
using System.Linq;
using System.Reflection;

namespace RugbyRoyale.Entities.Extensions
{
    public static class LeagueTypeExtensions
    {
        public static LeagueTypes.LeagueType GetObject(this LeagueType leagueTypeEnum)
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsSubclassOf(typeof(LeagueTypes.LeagueType)) && !t.IsAbstract)
                .Cast<LeagueTypes.LeagueType>()
                .First(lt => lt.Enumerate() == leagueTypeEnum);
        }
    }
}