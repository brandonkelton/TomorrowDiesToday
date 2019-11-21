﻿using Autofac;
using Moq;
using System;
using System.Collections.Generic;
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
        private Mock<ISquadService> _mockSquadService = new Mock<ISquadService>();


        public SquadServiceTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<IGameService>().As<IGameService>().InstancePerLifetimeScope();
            builder.RegisterType<ISquadService>().As<ISquadService>().InstancePerLifetimeScope();
            builder.RegisterInstance(_mockGameService.Object).As<IGameService>().SingleInstance();
            builder.RegisterInstance(_mockSquadService.Object).As<ISquadService>().SingleInstance();
            Container = builder.Build();
        }

        [Fact]
        public void CalculateSquadStats()
        {
            var squadService = new SquadService();

            var inputSquadModel = new SquadModel
            {
                PlayerId = "0",
                SquadId = "0-1"
            };
            var playerArmament = new Armament(ArmamentType.ArchibaldKluge, new ArmamentStats(0, 1, 3, 1));
            playerArmament.SetCount(1);
            inputSquadModel.Armaments.Add(playerArmament);

            var result = squadService.CalculateSquadStats(inputSquadModel);

            var combatResult = result.Stats.Combat.Value;
            var stealthResult = result.Stats.Stealth.Value;
            var cunningResult = result.Stats.Cunning.Value;
            var diplomacyResult = result.Stats.Diplomacy.Value;

            Assert.True(combatResult == 0 && stealthResult == 1 && cunningResult == 3 && diplomacyResult == 1);
        }
    }
}
