using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RugbyRoyale.Discord.App;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Discord.Context;
using RugbyRoyale.Discord.Repositories;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.Model;
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

        private DiscordClient discord;
        private MessageTracker messageTracker;

        private ServiceProvider dependencies;

        private static void Main(string[] args)
        {
            new Program().MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private async Task MainAsync(string[] args)
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

            dependencies = new ServiceCollection()
                .AddSingleton(settings)
                .AddSingleton(coordinator)
                .AddScoped<IClient, Client>()
                .AddDbContext<DataContext>(options => options.UseSqlite($"Data Source={Environment.CurrentDirectory}/players.db"))
                .AddScoped<IPlayerRepository, PlayerRepository>()
                .AddScoped<ILeagueRepository, LeagueRepository>()
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

            messageTracker = MessageTracker.GetMessageTracker();
            messageTracker.Initialise();

            // Register events and required services
            discord.MessageReactionAdded += Message_ReactionAdd;

            // Wait for events
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        private async Task Message_ReactionAdd(MessageReactionAddEventArgs e)
        {
            if (messageTracker.CheckMessageIsCurrentLeagueAdvert(e.Message))
            {
                Guid leagueID = messageTracker.GetAdvertisedLeagueID(e.Message);

                ILeagueRepository leagueRepo = dependencies.GetService<ILeagueRepository>();
                League league = await leagueRepo.GetAsync(leagueID);

                // Check if league is full
                ILeagueUserRepository leagueUserRepo = dependencies.GetService<ILeagueUserRepository>();
                if (await leagueUserRepo.CountAsync(leagueID) == league.Size)
                {
                    await e.Channel.SendMessageAsync($"Sorry {e.User.Mention}, that competition is full. 😥");
                }

                // Add user to league
                var leagueUser = new LeagueUser()
                {
                    LeagueID = leagueID,
                    UserID = e.User.Id.ToString()
                };
                await leagueUserRepo.SaveAsync(leagueUser);

                // Update message
                DiscordEmbed embed = e.Message.Embeds.FirstOrDefault();
                if (embed == null)
                {
                    // Something went wrong. As a backup, send a message confirming the user has been added to the league
                    await e.Channel.SendMessageAsync($"{e.User.Mention} has joined {league.Name_Long}.");
                    return;
                }

                DiscordMember member = await e.Guild.GetMemberAsync(e.User.Id);

                int lastField = embed.Fields.Count - 1;
                string userList = embed.Fields[lastField].Value;
                if (userList == "-")
                {
                    userList = member.Nickname;
                }
                else
                {
                    userList += ", " + member.Nickname;
                }

                var editedAdvert = new DiscordEmbedBuilder(embed)
                    .RemoveFieldAt(lastField)
                    .AddField(embed.Fields[lastField].Name, userList);
                Optional<DiscordEmbed> optionalEmbed = new Optional<DiscordEmbed>(editedAdvert);

                await e.Message.ModifyAsync(embed: optionalEmbed);
            }
        }
    }
}