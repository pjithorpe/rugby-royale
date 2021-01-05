using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
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

            if (!ulong.TryParse(settings.LogChannel, out ulong logChannelID))
            {
                throw new Exception("Failed to read LogChannel setting.");
            }

            if (!Enum.TryParse(settings.LogLevel, out LogLevel logLevel))
            {
                throw new Exception("Failed to read LogLevel setting.");
            }

            var loggingConfig = new DiscordLoggerConfiguration()
            {
                BotToken = settings.LoggingBotToken,
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
            };

            var startupLogger = new DiscordLogger(typeof(Program).FullName, loggingConfig);

            discord = new DSharpPlus.DiscordClient(new DSharpPlus.DiscordConfiguration
            {
                Token = settings.BotToken,
                TokenType = DSharpPlus.TokenType.Bot,
                LoggerFactory = new DiscordLoggerFactory(loggingConfig),
                MinimumLogLevel = LogLevel.Trace,
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

            startupLogger.LogInformation("Match coordinator initialised.", matchChannels.Select(x => x.Id));

            messageTracker = MessageTracker.GetMessageTracker();
            messageTracker.Initialise();

            startupLogger.LogInformation("Message tracker initialised.");

            dependencies = new ServiceCollection()
                .AddSingleton(settings)
                .AddSingleton(coordinator)
                .AddSingleton(messageTracker)
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                    loggingBuilder.AddProvider(new DiscordLoggerProvider(loggingConfig));
                })
                .AddScoped<IClient, Client>()
                .AddDbContext<DataContext>(options => options.UseSqlite($"Data Source={Environment.CurrentDirectory}/players.db"))
                .AddScoped<IPlayerRepository, PlayerRepository>()
                .AddScoped<ILeagueRepository, LeagueRepository>()
                .AddScoped<ITeamRepository, TeamRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ILeagueUserRepository, LeagueUserRepository>()
                .BuildServiceProvider();

            startupLogger.LogInformation("Service collection built.");

            discord.UseInteractivity(new InteractivityConfiguration
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

            startupLogger.LogInformation("Commands and events registered.");

            // Wait for events
            await discord.ConnectAsync();

            startupLogger.LogInformation("Finished startup. Ready to recieve commands.");

            await Task.Delay(-1);
        }

        private async Task Message_ReactionAdd(DiscordClient dc, MessageReactionAddEventArgs e)
        {
            if (e.User.IsBot) return;

            ITeamRepository teamRepo = dependencies.GetService<ITeamRepository>();
            ILeagueRepository leagueRepo = dependencies.GetService<ILeagueRepository>();
            ILeagueUserRepository leagueUserRepo = dependencies.GetService<ILeagueUserRepository>();

            await Handler_Message_ReactionAdd.ExecuteAsync(messageTracker, e, teamRepo, leagueRepo, leagueUserRepo);
        }
    }
}