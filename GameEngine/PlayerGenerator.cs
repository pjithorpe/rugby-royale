using Redzen.Numerics.Distributions.Double;
using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.Model;
using RugbyRoyale.GameEngine.Modules;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.GameEngine
{
    public class PlayerGenerator
    {
        private double baseRating;
        private double stdDev = Configuration.PLAYER_GEN_STD_DEV;
        private Random rand;

        public PlayerGenerator(double basePlayerRating)
        {
            baseRating = basePlayerRating;

            rand = new Random();
        }

        public async Task<Player> GeneratePlayer(Position position)
        {
            var player = new Player()
            {
                Positions_Primary = { position },
            };

            player = await GenerateName(player);
            player = AddExtraPositions(player);
            player = GenerateStats(player);

            player.Focus = player.CalculateFocus();

            return player;
        }

        private async Task<Player> GenerateName(Player player)
        {
            // TODO: nationality

            return await NameGeneration.GenerateRandomName(player, Nationality.French);
        }

        private Player AddExtraPositions(Player player)
        {
            Position position = player.Positions_Primary[0];
            switch (position)
            {
                case Position.Prop:
                    if (rand.NextDouble() < 0.005)
                    {
                        return AddToPrimaryOrSecondaryPositions(player, Position.Hooker);
                    }
                    break;

                case Position.Hooker:
                    if (rand.NextDouble() < 0.04)
                    {
                        return AddToPrimaryOrSecondaryPositions(player, Position.Prop);
                    }
                    // If also a prop, unlikely they will be the kind of player who will also play in the back row
                    else
                    {
                        if (rand.NextDouble() < 0.06)
                        {
                            return AddToPrimaryOrSecondaryPositions(player, Position.Flanker, 0.45);
                        }
                        if (rand.NextDouble() < 0.025)
                        {
                            return AddToPrimaryOrSecondaryPositions(player, Position.Number8, 0.2);
                        }
                    }
                    break;

                case Position.Lock:
                    if (rand.NextDouble() < 0.33)
                    {
                        return AddToPrimaryOrSecondaryPositions(player, Position.Flanker, 0.67);
                    }
                    if (rand.NextDouble() < 0.09)
                    {
                        return AddToPrimaryOrSecondaryPositions(player, Position.Number8, 0.67);
                    }
                    break;

                case Position.Flanker:
                    //TODO
                    break;

                case Position.Number8:
                    //TODO
                    break;

                case Position.ScrumHalf:
                    //TODO
                    break;

                case Position.FlyHalf:
                    if (rand.NextDouble() < 0.02)
                    {
                        return AddToPrimaryOrSecondaryPositions(player, Position.ScrumHalf, 0.67);
                    }
                    // If also a scrum half, unlikely they will be capable of other positions
                    else
                    {
                        if (rand.NextDouble() < 0.28)
                        {
                            return AddToPrimaryOrSecondaryPositions(player, Position.Centre);
                        }
                        if (rand.NextDouble() < 0.33)
                        {
                            return AddToPrimaryOrSecondaryPositions(player, Position.FullBack, 0.4);
                        }
                        if (rand.NextDouble() < 0.035)
                        {
                            return AddToPrimaryOrSecondaryPositions(player, Position.Wing, 0.1);
                        }
                    }
                    break;

                case Position.Centre:
                    //TODO
                    break;

                case Position.Wing:
                    //TODO
                    break;

                case Position.FullBack:
                    //TODO
                    break;
            }

            return player;
        }

        private Player AddToPrimaryOrSecondaryPositions(Player player, Position position, double primaryChance = 0.5)
        {
            if (rand.NextDouble() < primaryChance)
            {
                player.Positions_Primary.Add(position);
            }
            else
            {
                player.Positions_Secondary.Add(position);
            }

            return player;
        }

        private Player GenerateStats(Player player)
        {
            // Fast normal distribution sampling algorithm
            var distribution = new ZigguratGaussianSampler(baseRating, stdDev);

            player.Attack = Convert.ToInt32(distribution.Sample());
            player.Defence = Convert.ToInt32(distribution.Sample());
            player.Physicality = Convert.ToInt32(distribution.Sample());
            player.Stamina = Convert.ToInt32(distribution.Sample());
            player.Handling = Convert.ToInt32(distribution.Sample());
            player.Kicking = Convert.ToInt32(distribution.Sample());

            // Check if player is overpowered
            if (player.TotalStats() > (baseRating + stdDev) * 6)
            {
                return GenerateStats(player);
            }
            // or underpowered
            else if (player.TotalStats() < (baseRating - stdDev) * 6)
            {
                return GenerateStats(player);
            }

            return player;
        }
    }
}