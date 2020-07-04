using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Repository
{
    public interface ILeagueUserRepository
    {
        Task<bool> SaveAsync(LeagueUser league);

        Task<LeagueUser> GetAsync(Guid leagueID, string userID);

        Task<int> CountAsync(Guid leagueID);
    }
}