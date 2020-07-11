using RugbyRoyale.Entities.Model;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Repository
{
    public interface ITeamRepository
    {
        Task<Team> GetAsync(Guid teamID);

        Task<bool> SaveAsync(Team team);
    }
}