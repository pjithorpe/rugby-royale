using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.GameObjects;
using RugbyRoyale.Entities.LeagueTypes;
using System.Linq;
using System.Reflection;

namespace RugbyRoyale.Entities.Extensions
{
    public static class LeagueTypeExtensions
    {
        public static LeagueRules GetObject(this LeagueType leagueTypeEnum)
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsSubclassOf(typeof(LeagueRules)) && !t.IsAbstract)
                .Cast<LeagueRules>()
                .First(lt => lt.LeagueType == leagueTypeEnum);
        }
    }
}