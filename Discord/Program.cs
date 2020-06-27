using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RugbyRoyale.Discord.App;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Discord.Context;
using RugbyRoyale.Discord.Repositories;
using RugbyRoyale.GameEngine;
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

            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = settings.BotToken,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug,
            });

            // Connect and get match thread channels
            await discord.ConnectAsync();
            DiscordChannel[] matchChannels = settings.MatchChannels
                .Select(async mc => await discord.GetChannelAsync(ulong.Parse(mc)))
                .Select(t => t.Result)
                .ToArray();
            await discord.DisconnectAsync();

            MatchCoordinator coordinator = MatchCoordinator.GetCoordinator();
            coordinator.Initialise(matchChannels);

            ServiceProvider dependencies = new ServiceCollection()
                .AddSingleton(settings)
                .AddSingleton(coordinator)
                .AddScoped<IClient, Client>()
                .AddDbContext<DataContext>(options => options.UseSqlite($"Data Source={Environment.CurrentDirectory}/players.db"))
                .AddScoped<IPlayerRepository, PlayerRepository>()
                .AddScoped<ILeagueRepository, LeagueRepository>()
                .BuildServiceProvider();

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

            // Register all commands in the assembly
            commands.RegisterCommands(Assembly.GetExecutingAssembly());

            // Wait for events
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}