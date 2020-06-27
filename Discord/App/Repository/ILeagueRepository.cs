﻿using RugbyRoyale.Entities.Model;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Repository
{
    public interface ILeagueRepository
    {
        Task<bool> SaveAsync(League league);
    }
}