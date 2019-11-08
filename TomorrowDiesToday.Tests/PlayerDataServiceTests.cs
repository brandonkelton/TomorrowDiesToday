using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Autofac;
using TomorrowDiesToday.Services.Data;
using TomorrowDiesToday.Services.Data.Models;
using TomorrowDiesToday.Services.Database;
using TomorrowDiesToday.Models;
using Xunit;
using TomorrowDiesToday.Services.Game;
using System.Threading.Tasks;
using Moq;

namespace TomorrowDiesToday.Tests
{
    public class PlayerDataServiceTests
    {
        public static IContainer Container;

        private Mock<IDBClient> _mockClient = new Mock<IDBClient>();
        private Mock<IDynamoDBContext> _mockContext = new Mock<IDynamoDBContext>();
        private Mock<IGameService> _mockGameService = new Mock<IGameService>();
        private Mock<IDataService<PlayerModel, PlayerRequest>> _mockPlayerDataService = new Mock<IDataService<PlayerModel, PlayerRequest>>();

        public PlayerDataServiceTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<PlayerDataService>().As<IDataService<PlayerModel, PlayerRequest>>().InstancePerLifetimeScope();
            builder.RegisterInstance(_mockClient.Object).As<IDBClient>().SingleInstance();
            builder.RegisterInstance(_mockGameService.Object).As<IGameService>().SingleInstance();
            Container = builder.Build();
        }

        [Fact]
        public async Task ConfigureTable()
        {
            var playerDataService = Container.Resolve<IDataService<PlayerModel, PlayerRequest>>();
            await playerDataService.ConfigureTable();
            Assert.True(true); // pass if no exceptions thrown
        }

        [Fact]
        public async Task Create()
        {
            var playerDataService = Container.Resolve<IDataService<PlayerModel, PlayerRequest>>();
            var gameId = "TestGame";
            var playerId = "TestPlayer";
            var playerRequest = new PlayerRequest
            {
                GameId = gameId,
                PlayerId = playerId
            };
            await playerDataService.Create(playerRequest);
            Assert.True(true); // pass if no exceptions thrown
        }

        [Fact]
        public async Task ExistsIsTrue()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task ExistsIsFalse()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Update()
        {
            var playerDataService = Container.Resolve<IDataService<PlayerModel, PlayerRequest>>();
            var playerModel = new PlayerModel
            {
                GameId = "TestGame",
                PlayerId = "TestPlayer",
                Squads = new Dictionary<string, SquadModel>()
            };
            await playerDataService.Update(playerModel);
            Assert.True(true); // pass if no exceptions thrown
        }

        [Fact]
        public async Task RequestUpdate()
        {
            throw new NotImplementedException();
        }
    }
}
