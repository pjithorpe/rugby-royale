using RugbyRoyale.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RugbyRoyale.Data.Repository
{
    public interface IRepositoryCollection
    {
        ILeagueRepository Leagues { get; }
        IPlayerRepository Players { get; }
        ITeamRepository Teams { get; }
        IUserRepository Users { get; }

        Task<bool> Commit();
    }
}
