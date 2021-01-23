using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RugbyRoyale.Data.Repository
{
    public interface ITeamRepository : IRepository<Team>
    {
        Task<Team> GetAsync(Guid teamID);

        Task<Team> GetAsync(string userID);

        Task<bool> ExistsAsync(string userID);
    }
}