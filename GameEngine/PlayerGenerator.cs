using Redzen.Numerics.Distributions.Double;
using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.Model;
using RugbyRoyale.GameEngine.Modules;
using System;
using System.Collections.Generic;
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
            var player = new Player();

            player = await GenerateName(player);
            player = AddPositions(player, position);
            player = GenerateStats(player);

            player.Focus = player.CalculateFocus();

            return player;
        }

        private async Task<Player> GenerateName(Player player)
        {
            // TODO: nationality

            return await NameGeneration.GenerateRandomName(player, Nationality.French);
        }

        private Player AddPositions(Player player, Position position)
        {
            player.AddPrimaryPosition(position);
            switch (position)
            {
                case Position.Prop:
                    if (rand.NextDouble() < 0.005)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.Hooker);
                    }
                    break;

                case Position.Hooker:
                    if (rand.NextDouble() < 0.04)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.Prop);
                    }
                    // If also a prop, unlikely they will be the kind of player who will also play in the back row
                    else
                    {
                        if (rand.NextDouble() < 0.06)
                        {
                            player = AddToPrimaryOrSecondaryPositions(player, Position.Flanker, 0.45);
                        }
                        if (rand.NextDouble() < 0.025)
                        {
                            player = AddToPrimaryOrSecondaryPositions(player, Position.Number8, 0.2);
                        }
                    }
                    break;

                case Position.Lock:
                    if (rand.NextDouble() < 0.33)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.Flanker, 0.67);
                    }
                    if (rand.NextDouble() < 0.09)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.Number8, 0.67);
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
                        player = AddToPrimaryOrSecondaryPositions(player, Position.ScrumHalf, 0.67);
                    }
                    // If also a scrum half, unlikely they will be capable of other positions
                    else
                    {
                        if (rand.NextDouble() < 0.28)
                        {
                            player = AddToPrimaryOrSecondaryPositions(player, Position.Centre);
                        }
                        if (rand.NextDouble() < 0.33)
                        {
                            player = AddToPrimaryOrSecondaryPositions(player, Position.FullBack, 0.4);
                        }
                        if (rand.NextDouble() < 0.035)
                        {
                            player = AddToPrimaryOrSecondaryPositions(player, Position.Wing, 0.1);
                        }
                    }
                    break;

                case Position.Centre:
                    if (rand.NextDouble() < 0.3)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.Wing, 0.6);
                    }
                    if (rand.NextDouble() < 0.125)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.FullBack, 0.6);
                    }
                    if (rand.NextDouble() < 0.12)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.FlyHalf, 0.6);
                    }
                    if (rand.NextDouble() < 0.0075)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.Flanker, 0.33);
                    }
                    break;

                case Position.Wing:
                    //TODO
                    break;

                case Position.FullBack:
                    if (rand.NextDouble() < 0.5)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.Wing, 0.75);
                    }
                    if (rand.NextDouble() < 0.275)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.FlyHalf, 0.4);
                    }
                    if (rand.NextDouble() < 0.25)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.Centre, 0.45);
                    }
                    if (rand.NextDouble() < 0.0075)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.ScrumHalf);
                    }
                    break;
            }

            return player;
        }

        private Player AddToPrimaryOrSecondaryPositions(Player player, Position position, double primaryChance = 0.5)
        {
            List<Position> newPositions;
            if (rand.NextDouble() < primaryChance)
            {
                // Check primary positions full
                if (player.Positions_Primary.Count == 2)
                {
                    // 50% chance of overwriting a random primary position
                    if (rand.NextDouble() < 0.5)
                    {
                        // First move a random primary position to secondary positions
                        int overwriteIndex = rand.Next(0, 2);
                        // If secondary positions full, randomly overwrite one
                        if (player.Positions_Secondary.Count == 2)
                        {
                            newPositions = player.Positions_Secondary;
                            newPositions[rand.Next(0, 2)] = player.Positions_Primary[overwriteIndex];
                            player.Positions_Secondary = newPositions;
                        }
                        else
                        {
                            player.AddSecondaryPosition(player.Positions_Primary[overwriteIndex]);
                        }

                        // Now overwrite primary with new position
                        newPositions = player.Positions_Primary;
                        newPositions[overwriteIndex] = position;
                        player.Positions_Primary = newPositions;
                    }
                    // otherwise add to secondary positions
                    else
                    {
                        // If secondary positions also full, randomly discard one and add this
                        if (player.Positions_Secondary.Count == 2)
                        {
                            newPositions = player.Positions_Secondary;
                            newPositions[rand.Next(0, 2)] = position;
                            player.Positions_Secondary = newPositions;
                        }
                        else
                        {
                            player.AddSecondaryPosition(position);
                        }
                    }
                }
                else
                {
                    player.AddPrimaryPosition(position);
                }
            }
            else
            {
                // Check secondary positions full
                if (player.Positions_Secondary.Count == 2)
                {
                    // 50% chance of overwriting a random secondary position
                    if (rand.NextDouble() < 0.5)
                    {
                        newPositions = player.Positions_Secondary;
                        newPositions[rand.Next(0, 2)] = position;
                        player.Positions_Secondary = newPositions;
                    }
                }
                else
                {
                    player.AddSecondaryPosition(position);
                }
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