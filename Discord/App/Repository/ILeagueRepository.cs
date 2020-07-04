﻿using RugbyRoyale.Entities.Model;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Repository
{
    public interface ILeagueRepository
    {
        Task<bool> SaveAsync(League league);

        Task<League> GetAsync(Guid leagueID);

        Task<League> GetAsync(string userID);
    }
}