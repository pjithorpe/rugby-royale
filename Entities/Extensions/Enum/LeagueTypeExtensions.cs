using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.LeagueTypes;
using System;
using System.Linq;
using System.Reflection;

namespace RugbyRoyale.Entities.Extensions
{
    public static class LeagueTypeExtensions
    {
        public static LeagueRules GetRules(this LeagueType leagueTypeEnum)
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsSubclassOf(typeof(LeagueRules)) && !t.IsAbstract)
                .Select(t => Activator.CreateInstance(t))
                .Cast<LeagueRules>()
                .First(lt => lt.LeagueType == leagueTypeEnum);
        }
    }
}