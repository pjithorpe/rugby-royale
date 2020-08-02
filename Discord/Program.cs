using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RugbyRoyale.Discord.App;
using RugbyRoyale.Discord.App.EventHandlers;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Discord.Context;
using RugbyRoyale.Discord.Logging;
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
        private CommandsNextExtension commands;
        private InteractivityExtension interactivity;

        private DSharpPlus.DiscordClient discord;
        private MessageTracker messageTracker;

        private ServiceProvider dependencies;

        private static void Main(string[] args)
        {
            new Program().MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private async Task MainAsync(string[] args)
        {
            Settings settings = Settings.GetSettings();
            discord = new DSharpPlus.DiscordClient(new DSharpPlus.DiscordConfiguration
            {
                Token = settings.BotToken,
                TokenType = DSharpPlus.TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = DSharpPlus.LogLevel.Debug,
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

            messageTracker = MessageTracker.GetMessageTracker();
            messageTracker.Initialise();

            if (!ulong.TryParse(settings.LogChannel, out ulong logChannelID))
            {
                throw new Exception("Failed to read LogChannel setting.");
            }

            if (!Enum.TryParse(settings.LogLevel, out Microsoft.Extensions.Logging.LogLevel logLevel))
            {
                throw new Exception("Failed to read LogLevel setting.");
            }

            dependencies = new ServiceCollection()
                .AddSingleton(settings)
                .AddSingleton(coordinator)
                .AddSingleton(messageTracker)
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddProvider(new DiscordLoggerProvider(new DiscordLoggerConfiguration()
                    {
                        BotToken = settings.BotToken,
                        LogChannelID = logChannelID,
                        LogLevel = logLevel,
                        LogLevelColours = new Dictionary<LogLevel, DiscordColor>()
                        {
                            { LogLevel.Trace, DiscordColor.White },
                            { LogLevel.Debug, DiscordColor.SpringGreen },
                            { LogLevel.Information, DiscordColor.CornflowerBlue },
                            { LogLevel.Warning, DiscordColor.Yellow },
                            { LogLevel.Error, DiscordColor.Orange },
                            { LogLevel.Critical, DiscordColor.Red },
                        }
                    }));
                })
                .AddScoped<IClient, Client>()
                .AddDbContext<DataContext>(options => options.UseSqlite($"Data Source={Environment.CurrentDirectory}/players.db"))
                .AddScoped<IPlayerRepository, PlayerRepository>()
                .AddScoped<ILeagueRepository, LeagueRepository>()
                .AddScoped<ITeamRepository, TeamRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ILeagueUserRepository, LeagueUserRepository>()
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

            // Register events and required services
            discord.MessageReactionAdded += Message_ReactionAdd;

            // Wait for events
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        private async Task Message_ReactionAdd(MessageReactionAddEventArgs e)
        {
            if (e.User.IsBot) return;

            ITeamRepository teamRepo = dependencies.GetService<ITeamRepository>();
            ILeagueRepository leagueRepo = dependencies.GetService<ILeagueRepository>();
            ILeagueUserRepository leagueUserRepo = dependencies.GetService<ILeagueUserRepository>();

            await Handler_Message_ReactionAdd.ExecuteAsync(messageTracker, e, teamRepo, leagueRepo, leagueUserRepo);
        }
    }
}