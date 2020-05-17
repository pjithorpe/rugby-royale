﻿using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Discord.Context;
using RugbyRoyale.Discord.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord
{
    internal class Program
    {
        private static DiscordClient discord;
        private static CommandsNextExtension commands;
        private static InteractivityExtension interactivity;

        private static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            Settings settings = Settings.GetSettings();

            ServiceProvider dependencies = new ServiceCollection()
                .AddSingleton(settings)
                .AddDbContext<DataContext>(options => options.UseSqlite(settings.DBConnectionString))
                .AddScoped<IPlayerRepository, PlayerRepository>()
                .BuildServiceProvider();

            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = settings.BotToken,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug,
            });

            interactivity = discord.UseInteractivity(new InteractivityConfiguration
            {
                PaginationBehaviour = PaginationBehaviour.Ignore,
                PollBehaviour = PollBehaviour.KeepEmojis,
                Timeout = TimeSpan.FromMinutes(2)
            });

            // Use CommandsNext package for command management
            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new List<string>() { settings.CommandPrefix },
                Services = dependencies
            });

            // Register all commands in the Commands namespace
            var assembly = Assembly.GetExecutingAssembly();
            assembly.GetTypes()
                .Where(t => t.Namespace == $"{nameof(RugbyRoyale)}.Commands" && t.IsVisible).ToList()
                .ForEach(cmd => { commands.RegisterCommands(cmd); });

            // Connect and wait for events
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}