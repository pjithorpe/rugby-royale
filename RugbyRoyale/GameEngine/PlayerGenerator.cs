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
        private int baseRating;
        private Nationality nat;
        private Random rand;

        public PlayerGenerator(int basePlayerRating, Nationality nationality)
        {
            baseRating = basePlayerRating;
            nat = nationality;

            rand = new Random();
        }

        public async Task<Player> GeneratePlayer(Position position)
        {
            var player = new Player();

            player = await NameGeneration.GenerateRandomName(player, nat);
            player = PositionsGeneration.AddPositions(player, position, rand);
            player = StatsGeneration.GenerateStats(player, baseRating);

            player.Focus = player.CalculateFocus();

            return player;
        }

        public async Task<Teamsheet> GenerateTeamsheet()
        {
            return new Teamsheet()
            {
                LooseheadProp = await GeneratePlayer(Position.Prop),
                Hooker = await GeneratePlayer(Position.Hooker),
                TightheadProp = await GeneratePlayer(Position.Prop),
                Number4Lock = await GeneratePlayer(Position.Lock),
                Number5Lock = await GeneratePlayer(Position.Lock),
                BlindsideFlanker = await GeneratePlayer(Position.Flanker),
                OpensideFlanker = await GeneratePlayer(Position.Flanker),
                Number8 = await GeneratePlayer(Position.Number8),
                ScrumHalf = await GeneratePlayer(Position.ScrumHalf),
                FlyHalf = await GeneratePlayer(Position.FlyHalf),
                InsideCentre = await GeneratePlayer(Position.Centre),
                OutsideCentre = await GeneratePlayer(Position.Centre),
                LeftWing = await GeneratePlayer(Position.Wing),
                RightWing = await GeneratePlayer(Position.Wing),
                FullBack = await GeneratePlayer(Position.FullBack),
            };
        }
    }
}