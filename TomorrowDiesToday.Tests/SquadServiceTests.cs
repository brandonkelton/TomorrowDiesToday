using Autofac;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Game;
using Xunit;

namespace TomorrowDiesToday.Tests
{
    public class SquadServiceTests
    {
        public static IContainer Container;

        private Mock<IGameService> _mockGameService = new Mock<IGameService>();

        public SquadServiceTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SquadService>().As<ISquadService>().InstancePerLifetimeScope();
            builder.RegisterInstance(_mockGameService.Object).As<IGameService>().SingleInstance();
            Container = builder.Build();
        }

        [Fact]
        public void CalculateSquadStats()
        {
            var squadService = Container.Resolve<ISquadService>();

            var inputSquadModel = new SquadModel();
            var playerArmament = new Armament(ArmamentType.ArchibaldKluge, new ArmamentStats(0, 1, 3, 1));
            playerArmament.SetCount(1);
            inputSquadModel.Armaments.Add(playerArmament);

            squadService.CalculateSquadStats(inputSquadModel);

            var combatResult = inputSquadModel.Stats.Combat.Value;
            var stealthResult = inputSquadModel.Stats.Stealth.Value;
            var cunningResult = inputSquadModel.Stats.Cunning.Value;
            var diplomacyResult = inputSquadModel.Stats.Diplomacy.Value;

            Assert.True(combatResult == 0 && stealthResult == 1 && cunningResult == 3 && diplomacyResult == 1);
        }

        [Fact]
        public void DecrementAbilityCount()
        {
            var squadService = Container.Resolve<ISquadService>();

            var abilityType = ArmamentType.UgoCombat;
            var squadModel = new SquadModel();

            var ability = squadModel.Abilities.Where(a => a.ArmamentType == ArmamentType.UgoCombat).FirstOrDefault();
            ability.SetCount(1);

            squadService.DecrementAbilityCount(abilityType, squadModel);

            Assert.True(ability.Count == 0);
        }

        [Fact]
        public void DecrementArmamentCount()
        {
            var squadService = Container.Resolve<ISquadService>();

            var armamentType = ArmamentType.Thief;
            var squadModel = new SquadModel();

            var armament = squadModel.Armaments.Where(a => a.ArmamentType == ArmamentType.Thief).FirstOrDefault();
            armament.SetCount(1);

            squadService.DecrementArmamentCount(armamentType, squadModel);

            Assert.True(armament.Count == 0);
        }

        [Fact]
        public void DecrementItemCount()
        {
            var squadService = Container.Resolve<ISquadService>();

            var itemType = ArmamentType.ExplosiveRounds;
            var squadModel = new SquadModel();

            var item = squadModel.Items.Where(a => a.ArmamentType == ArmamentType.ExplosiveRounds).FirstOrDefault();
            item.SetCount(1);

            squadService.DecrementItemCount(itemType, squadModel);

            Assert.True(item.Count == 0);
        }

        [Fact]
        public void IncrementAbilityCount()
        {
            var abilityType = ArmamentType.UgoStealth;
            var ugoPlayerId = ((int)ArmamentType.UgoDottore).ToString();
            var squadModel = new SquadModel
            {
                PlayerId = ugoPlayerId,
                SquadId = "TEST"
            };
            var game = new GameModel 
            { 
                GameId = "ABCDEF",
                PlayerId = ugoPlayerId,
                PlayerType = ArmamentType.UgoDottore,
                Players = new List<PlayerModel>
                { 
                   new PlayerModel
                   {
                       PlayerId = ugoPlayerId,
                       PlayerName = ArmamentType.UgoDottore.ToDescription(),
                       PlayerType = ArmamentType.UgoDottore,
                       Squads = new List<SquadModel>
                       {
                           squadModel,
                           new SquadModel
                           {
                               PlayerId = ugoPlayerId,
                               SquadId = "OTHER"
                           }
                       }
                   }
                
                }
            };
            
            var otherSquad = game.Players.Select(p => p.Squads.Where(s => s.SquadId == "OTHER").FirstOrDefault()).FirstOrDefault();
            var otherSquadAbility = otherSquad.Abilities.Where(a => a.ArmamentType == abilityType).FirstOrDefault();
            otherSquadAbility.SetCount(1);
            var ability = squadModel.Abilities.Where(a => a.ArmamentType == abilityType).FirstOrDefault();

            _mockGameService.Setup(c => c.Game).Returns(game);
            var squadService = Container.Resolve<ISquadService>();
            squadService.IncrementAbilityCount(abilityType, squadModel);

            Assert.True(ability.Count == 1 && otherSquadAbility.Count == 0);
        }

        [Fact]
        public void IncrementArmamentCount()
        {
            var playerArmamentType = ArmamentType.ArchibaldKluge;
            var otherArmamentType = ArmamentType.Hacker;
            var playerId = ((int)playerArmamentType).ToString();
            var thisSquad = new SquadModel
            {
                PlayerId = playerId,
                SquadId = "TEST"
            };
            thisSquad.Armaments.Add(new Armament(playerArmamentType, new ArmamentStats(0, 1, 3, 1)));
            var game = new GameModel
            {
                GameId = "ABCDEF",
                PlayerId = playerId,
                PlayerType = playerArmamentType,
                Players = new List<PlayerModel>
                {
                   new PlayerModel
                   {
                       PlayerId = playerId,
                       PlayerName = playerArmamentType.ToDescription(),
                       PlayerType = playerArmamentType,
                       Squads = new List<SquadModel>
                       {
                           thisSquad,
                           new SquadModel
                           {
                               PlayerId = playerId,
                               SquadId = "OTHER"
                           }
                       }
                   }

                }
            };
            var otherSquad = game.Players.Select(p => p.Squads.Where(s => s.SquadId == "OTHER").FirstOrDefault()).FirstOrDefault();
            otherSquad.Armaments.Add(new Armament(playerArmamentType, new ArmamentStats(0, 1, 3, 1)));
            var otherSquadPlayerArmament = otherSquad.Armaments.Where(a => a.ArmamentType == playerArmamentType).FirstOrDefault();
            var otherSquadHackerArmament = otherSquad.Armaments.Where(a => a.ArmamentType == otherArmamentType).FirstOrDefault();
            var thisSquadPlayerArmament = thisSquad.Armaments.Where(a => a.ArmamentType == playerArmamentType).FirstOrDefault();
            var thisSquadHackerArmament = thisSquad.Armaments.Where(a => a.ArmamentType == otherArmamentType).FirstOrDefault();
            otherSquadPlayerArmament.SetCount(1);
            otherSquadHackerArmament.SetCount(1);

            _mockGameService.Setup(c => c.Game).Returns(game);
            var squadService = Container.Resolve<ISquadService>();
            squadService.IncrementArmamentCount(playerArmamentType, thisSquad);
            squadService.IncrementArmamentCount(otherArmamentType, thisSquad);

            var successfulPlayerArmamentIncrement = thisSquadPlayerArmament.Count == 1 && otherSquadPlayerArmament.Count == 0;
            var successfulOtherArmamentIncrement = thisSquadHackerArmament.Count == 1 && otherSquadHackerArmament.Count == 1;

            Assert.True(successfulPlayerArmamentIncrement && successfulOtherArmamentIncrement);
        }

        [Fact]
        public void IncrementItemCount()
        {
            var playerArmamentType = ArmamentType.ArchibaldKluge;
            var explosiveRounds = ArmamentType.ExplosiveRounds;
            var hypnoticSpray = ArmamentType.HypnoticSpray;
            var playerId = ((int)playerArmamentType).ToString();
            var thisSquad = new SquadModel
            {
                PlayerId = playerId,
                SquadId = "TEST"
            };
            var game = new GameModel
            {
                GameId = "ABCDEF",
                PlayerId = playerId,
                PlayerType = playerArmamentType,
                Players = new List<PlayerModel>
                {
                   new PlayerModel
                   {
                       PlayerId = playerId,
                       PlayerName = playerArmamentType.ToDescription(),
                       PlayerType = playerArmamentType,
                       Squads = new List<SquadModel>
                       {
                           thisSquad,
                           new SquadModel
                           {
                               PlayerId = playerId,
                               SquadId = "OTHER"
                           }
                       }
                   }

                }
            };

            var otherSquad = game.Players.Select(p => p.Squads.Where(s => s.SquadId == "OTHER").FirstOrDefault()).FirstOrDefault();
            var otherSquadExplosiveRounds = otherSquad.Items.Where(a => a.ArmamentType == explosiveRounds).FirstOrDefault();
            var otherSquadHypnoticSpray = otherSquad.Items.Where(a => a.ArmamentType == hypnoticSpray).FirstOrDefault();
            otherSquadExplosiveRounds.SetCount(1);
            otherSquadHypnoticSpray.SetCount(1);
            var thisSquadExplosiveRounds = thisSquad.Items.Where(a => a.ArmamentType == explosiveRounds).FirstOrDefault();
            var thisSquadHypnoticSpray = thisSquad.Items.Where(a => a.ArmamentType == hypnoticSpray).FirstOrDefault();

            _mockGameService.Setup(c => c.Game).Returns(game);
            var squadService = Container.Resolve<ISquadService>();
            squadService.IncrementItemCount(explosiveRounds, thisSquad);
            squadService.IncrementItemCount(hypnoticSpray, thisSquad);

            var successfulExplosiveRoundsIncrement = thisSquadExplosiveRounds.Count == 1 && otherSquadExplosiveRounds.Count == 0;
            var successfulHypnoticSprayIncrement = thisSquadHypnoticSpray.Count == 1 && otherSquadHypnoticSpray.Count == 0;

            Assert.True(successfulExplosiveRoundsIncrement && successfulHypnoticSprayIncrement);
        }

        [Fact]
        public void ToggleSelected()
        {
            var startSelected = new SquadModel
            {
                IsSelected = true,
                Stats = new SquadStats(1, 1, 1, 1)
            };
            var startNotSelected = new SquadModel
            {
                IsSelected = false,
                Stats = new SquadStats(2, 2, 2, 2)
            };
            var game = new GameModel
            {
                GameId = "ABCDEF",
                Players = new List<PlayerModel>
                {
                   new PlayerModel
                   {
                       Squads = new List<SquadModel>
                       {
                           startSelected,
                           startNotSelected
                       }
                   }
                },
                SelectedSquadStats = new SquadStats()
            };

            _mockGameService.Setup(c => c.Game).Returns(game);
            var squadService = Container.Resolve<ISquadService>();
            squadService.ToggleSelected(startSelected);
            squadService.ToggleSelected(startNotSelected);

            Assert.True(!startSelected.IsSelected && startNotSelected.IsSelected);
        }

    }
}
