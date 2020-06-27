using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.GameEngine.Modules
{
    internal class PositionsGeneration
    {
        public static Player AddPositions(Player player, Position position, Random random)
        {
            player.AddPrimaryPosition(position);
            switch (position)
            {
                case Position.Prop:
                    if (random.NextDouble() < 0.005)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.Hooker, random);
                    }
                    break;

                case Position.Hooker:
                    if (random.NextDouble() < 0.04)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.Prop, random);
                    }
                    // If also a prop, unlikely they will be the kind of player who will also play in the back row
                    else
                    {
                        if (random.NextDouble() < 0.06)
                        {
                            player = AddToPrimaryOrSecondaryPositions(player, Position.Flanker, random, 0.45);
                        }
                        if (random.NextDouble() < 0.025)
                        {
                            player = AddToPrimaryOrSecondaryPositions(player, Position.Number8, random, 0.2);
                        }
                    }
                    break;

                case Position.Lock:
                    if (random.NextDouble() < 0.33)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.Flanker, random, 0.67);
                    }
                    if (random.NextDouble() < 0.09)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.Number8, random, 0.67);
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
                    if (random.NextDouble() < 0.02)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.ScrumHalf, random, 0.67);
                    }
                    // If also a scrum half, unlikely they will be capable of other positions
                    else
                    {
                        if (random.NextDouble() < 0.28)
                        {
                            player = AddToPrimaryOrSecondaryPositions(player, Position.Centre, random);
                        }
                        if (random.NextDouble() < 0.33)
                        {
                            player = AddToPrimaryOrSecondaryPositions(player, Position.FullBack, random, 0.4);
                        }
                        if (random.NextDouble() < 0.035)
                        {
                            player = AddToPrimaryOrSecondaryPositions(player, Position.Wing, random, 0.1);
                        }
                    }
                    break;

                case Position.Centre:
                    if (random.NextDouble() < 0.3)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.Wing, random, 0.6);
                    }
                    if (random.NextDouble() < 0.125)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.FullBack, random, 0.6);
                    }
                    if (random.NextDouble() < 0.12)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.FlyHalf, random, 0.6);
                    }
                    if (random.NextDouble() < 0.0075)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.Flanker, random, 0.33);
                    }
                    break;

                case Position.Wing:
                    //TODO
                    break;

                case Position.FullBack:
                    if (random.NextDouble() < 0.5)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.Wing, random, 0.75);
                    }
                    if (random.NextDouble() < 0.275)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.FlyHalf, random, 0.4);
                    }
                    if (random.NextDouble() < 0.25)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.Centre, random, 0.45);
                    }
                    if (random.NextDouble() < 0.0075)
                    {
                        player = AddToPrimaryOrSecondaryPositions(player, Position.ScrumHalf, random);
                    }
                    break;
            }

            return player;
        }

        private static Player AddToPrimaryOrSecondaryPositions(Player player, Position position, Random random, double primaryChance = 0.5)
        {
            List<Position> newPositions;
            if (random.NextDouble() < primaryChance)
            {
                // Check primary positions full
                if (player.Positions_Primary.Count == 2)
                {
                    // 50% chance of overwriting a random primary position
                    if (random.NextDouble() < 0.5)
                    {
                        // First move a random primary position to secondary positions
                        int overwriteIndex = random.Next(0, 2);
                        // If secondary positions full, randomly overwrite one
                        if (player.Positions_Secondary.Count == 2)
                        {
                            newPositions = player.Positions_Secondary;
                            newPositions[random.Next(0, 2)] = player.Positions_Primary[overwriteIndex];
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
                            newPositions[random.Next(0, 2)] = position;
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
                    if (random.NextDouble() < 0.5)
                    {
                        newPositions = player.Positions_Secondary;
                        newPositions[random.Next(0, 2)] = position;
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
    }
}
