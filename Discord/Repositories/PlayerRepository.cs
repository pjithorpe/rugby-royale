﻿using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Discord.Context;
using RugbyRoyale.Entities.Model;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.Repositories
{
    public class PlayerRepository : Repository, IPlayerRepository
    {
        public PlayerRepository(DataContext db) : base(db)
        {
        }

        public Task<bool> ExistsAsync(string playerId)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(Player player)
        {
            throw new NotImplementedException();
        }
    }
}