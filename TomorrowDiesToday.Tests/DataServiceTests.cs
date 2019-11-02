using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Autofac;
using TomorrowDiesToday.Services.Data;
using TomorrowDiesToday.Services.Data.Models;
using TomorrowDiesToday.Services.Database;
using TomorrowDiesToday.Models;
using Xunit;
using TomorrowDiesToday.Services.Game;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using TomorrowDiesToday.Services.Database.DTOs;

namespace TomorrowDiesToday.Tests
{
    public class DataServiceTests
    {
        public static IContainer Container;

        private Mock<IDBClient> _mockClient = new Mock<IDBClient>();
        private Mock<IGameService> _mockGameService = new Mock<IGameService>();
        private Mock<IDataService<GameModel, GameRequest>> _mockGameDataService = new Mock<IDataService<GameModel, GameRequest>>();
        private Mock<IDataService<PlayerModel, PlayerRequest>> _mockPlayerDataService = new Mock<IDataService<PlayerModel, PlayerRequest>>();

        public DataServiceTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DynamoClient>().As<IDBClient>().InstancePerLifetimeScope();
            builder.RegisterType<GameService>().As<IGameService>().InstancePerLifetimeScope();
            builder.RegisterType<GameDataService>().As<IDataService<GameModel, GameRequest>>().InstancePerLifetimeScope();
            builder.RegisterType<PlayerDataService>().As<IDataService<PlayerModel, PlayerRequest>>().InstancePerLifetimeScope();
            builder.RegisterInstance(_mockClient.Object).As<IDBClient>().SingleInstance();
            builder.RegisterInstance(_mockGameService.Object).As<IGameService>().SingleInstance();
            builder.RegisterInstance(_mockGameDataService.Object).As<IDataService<GameModel, GameRequest>>().SingleInstance();
            builder.RegisterInstance(_mockPlayerDataService.Object).As<IDataService<PlayerModel, PlayerRequest>>().SingleInstance();
            Container = builder.Build();
        }

        [Fact]
        public async Task GameConfigureTable()
        {
            var gameDataService = Container.Resolve<IDataService<GameModel, GameRequest>>();
            await gameDataService.ConfigureTable();
            Assert.True(true); // pass if no exceptions thrown
        }

        [Fact]
        public async Task GameCreate()
        {
            var gameDataService = Container.Resolve<IDataService<GameModel, GameRequest>>();
            var gameId = "TestGame";
            await gameDataService.Create(gameId);
            Assert.True(true); // pass if no exceptions thrown
        }

        [Fact]
        public async Task GameExistsIsTrue()
        {
            var gameId = "TestGame";
            _mockClient.Setup(c => c.GameExists(gameId)).Returns(Task.FromResult(true));
            var gameDataService = Container.Resolve<IDataService<GameModel, GameRequest>>();
            var result = await gameDataService.Exists(gameId);
            Assert.True(result);
        }

        [Fact]
        public async Task GameExistsIsFalse()
        {
            var gameId = "TestGame";
            _mockClient.Setup(c => c.GameExists(gameId)).Returns(Task.FromResult(false));
            var gameDataService = Container.Resolve<IDataService<GameModel, GameRequest>>();
            var result = await gameDataService.Exists(gameId);
            Assert.False(result);
        }

        [Fact]
        public async Task GameUpdate()
        {
            var gameDataService = Container.Resolve<IDataService<GameModel, GameRequest>>();
            var gameModel = new GameModel
            {
                GameId = "TestGame",
                MyPlayer = new PlayerModel { PlayerId = "TestGame" },
                OtherPlayers = new List<PlayerModel>()
            };
            await gameDataService.Update(gameModel);
            Assert.True(true); // pass if no exceptions thrown
        }

        [Fact]
        public async Task GameRequestUpdate()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task PlayerConfigureTable()
        {
            var playerDataService = Container.Resolve<IDataService<PlayerModel, PlayerRequest>>();
            await playerDataService.ConfigureTable();
            Assert.True(true); // pass if no exceptions thrown
        }

        [Fact]
        public async Task PlayerCreate()
        {
            var playerDataService = Container.Resolve<IDataService<GameModel, GameRequest>>();
            _mockGameService.Setup(x => x.GameId).Returns("TestGame");
            var playerId = "TestPlayer";
            await playerDataService.Create(playerId);
            Assert.True(true); // pass if no exceptions thrown
        }

        [Fact]
        public async Task PlayerExistsIsTrue()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task PlayerExistsIsFalse()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task PlayerUpdate()
        {
            var playerDataService = Container.Resolve<IDataService<PlayerModel, PlayerRequest>>();
            var playerModel = new PlayerModel
            {
                PlayerId = "TestPlayer",
                Squads = new List<SquadModel>()
            };
            await playerDataService.Update(playerModel);
            Assert.True(true); // pass if no exceptions thrown
        }

        [Fact]
        public async Task PlayerRequestUpdate()
        {
            throw new NotImplementedException();
        }
    }
}
