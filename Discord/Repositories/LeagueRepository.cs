﻿using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Discord.Context;
using RugbyRoyale.Entities.Model;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.Repositories
{
    public class LeagueRepository : Repository, ILeagueRepository
    {
        public LeagueRepository(DataContext db) : base(db)
        {
        }

        public async Task<bool> SaveAsync(League league)
        {
            try
            {
                db.Add(league);
                return (await db.SaveChangesAsync()) == 1;
            }
            catch (Exception e)
            {
                // TODO: LOG ERROR
                return false;
            }
        }
    }
}