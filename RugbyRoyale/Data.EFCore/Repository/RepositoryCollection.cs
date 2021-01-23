using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RugbyRoyale.Data.EFCore.Context;
using RugbyRoyale.Data.Repository;
using Serilog;

namespace RugbyRoyale.Data.EFCore.Repository
{
    public class RepositoryCollection : IRepositoryCollection
    {
        private DataContext db;
        private ILeagueRepository leagues;
        private IPlayerRepository players;
        private ITeamRepository teams;
        private IUserRepository users;

        public RepositoryCollection(DataContext db)
        {
            this.db = db;
        }

        public ILeagueRepository Leagues { get { if (leagues == null) leagues = new LeagueRepository(db); return leagues; } }
        public IPlayerRepository Players { get { if (players == null) players = new PlayerRepository(db); return players; } }
        public ITeamRepository Teams { get { if (teams == null) teams = new TeamRepository(db); return teams; } }
        public IUserRepository Users { get { if (users == null) users = new UserRepository(db); return users; } }

        public async Task<bool> Commit()
        {
            try
            {
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, "DB Error whilst committing changes.");
                return false;
            }
        }
    }
}
