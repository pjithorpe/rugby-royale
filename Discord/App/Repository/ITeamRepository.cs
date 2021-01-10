using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Repository
{
    public interface ITeamRepository
    {
        Task<Team> GetAsync(Guid teamID);

        Task<Team> GetAsync(string userID);

        Task<bool> ExistsAsync(string userID);

        Task<bool> EditAsync(Team team);

        Task<bool> SaveAsync(Team team);

        Task<List<Team>> ListAllAsync();
    }
}