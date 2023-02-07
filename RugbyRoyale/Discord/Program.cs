using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using Serilog;
using Serilog.Events;
using CXuesong.Uel.Serilog.Sinks.Discord;
using RugbyRoyale.Discord.App;
using RugbyRoyale.Discord.App.EventHandlers;
using RugbyRoyale.GameEngine;
using RugbyRoyale.Data.EFCore.Context;
using RugbyRoyale.Data.Repository;
using RugbyRoyale.Data.EFCore.Repository;

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

            if (!ulong.TryParse(settings.WebhookID, out ulong webhookID))
            {
                Log.Error("Failed to read WebhookID setting.");
            }

            if (!Enum.TryParse(settings.LogLevel, out LogEventLevel logLevel))
            {
                Log.Warning("Failed to read WebhookID setting.");
            }

            var logMessenger = new DiscordWebhookMessenger(webhookID, settings.WebhookToken);
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Discord(logMessenger, logLevel)
                .CreateLogger();

            var logFactory = new LoggerFactory().AddSerilog();

            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = settings.BotToken,
                TokenType = TokenType.Bot,
                LoggerFactory = logFactory,
                MinimumLogLevel = LogLevel.Trace,
                Intents = DiscordIntents.AllUnprivileged.AddIntent(DiscordIntents.MessageContents)
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

            Log.Information("Match coordinator initialised.", matchChannels.Select(x => x.Id));

            messageTracker = MessageTracker.GetMessageTracker();
            messageTracker.Initialise();

            Log.Information("Message tracker initialised.");

            dependencies = new ServiceCollection()
                .AddSingleton(settings)
                .AddSingleton(coordinator)
                .AddSingleton(messageTracker)
                .AddSingleton(logMessenger)
                .AddScoped<IClient, Client>()
                .AddDbContext<DataContext>(options => options.UseSqlite($"Data Source={Environment.CurrentDirectory}/players.db"))
                .AddScoped<IRepositoryCollection, RepositoryCollection>()
                .BuildServiceProvider();

            Log.Information("Service collection built.");

            discord.UseInteractivity(new InteractivityConfiguration
            {
                PaginationBehaviour = PaginationBehaviour.Ignore,
                PollBehaviour = PollBehaviour.KeepEmojis,
                Timeout = TimeSpan.FromMinutes(2)
            });

            // Use CommandsNext package for command management
            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { settings.CommandPrefix },
                Services = dependencies
            });

            // Register all commands in the assembly
            commands.RegisterCommands(Assembly.GetExecutingAssembly());

            // Register events and required services
            discord.MessageReactionAdded += (dc, e) =>
            {
                if (e.User.IsBot) return Task.CompletedTask;

                _ = Task.Run(async () =>
                {
                    await Handler_Message_ReactionAdd.ExecuteAsync(messageTracker, dc, e, dependencies.GetService<IRepositoryCollection>());
                });

                return Task.CompletedTask;
            };

            Log.Information("Commands and events registered.");

            // Wait for events
            await discord.ConnectAsync();

            Log.Information("Finished startup. Ready to receive commands.");

            await Task.Delay(-1);
        }
    }
}