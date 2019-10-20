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
        private Mock<IDynamoDBContext> _mockContext = new Mock<IDynamoDBContext>();
        private Mock<IGameService> _mockGameService = new Mock<IGameService>();
        private Mock<IDataService<GameModel, GameRequest>> _mockGameDataService = new Mock<IDataService<GameModel, GameRequest>>();
        private Mock<IDataService<PlayerModel, PlayerRequest>> _mockPlayerDataService = new Mock<IDataService<PlayerModel, PlayerRequest>>();
        private static Random _random = new Random();
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public DataServiceTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DynamoClient>().As<IDBClient>().InstancePerLifetimeScope();
            builder.RegisterType<GameService>().As<IGameService>().InstancePerLifetimeScope();
            builder.RegisterType<GameDataService>().As<IDataService<GameModel, GameRequest>>().InstancePerLifetimeScope();
            builder.RegisterType<PlayerDataService>().As<IDataService<PlayerModel, PlayerRequest>>().InstancePerLifetimeScope();
            builder.RegisterInstance(_mockClient.Object).As<IDBClient>().SingleInstance();
            builder.RegisterInstance(_mockContext.Object).As<IDynamoDBContext>().SingleInstance();
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
            Assert.True(true);
        }

        [Fact]
        public async Task GameCreate()
        {

            var gameService = Container.Resolve<IGameService>();
            var gameDataService = Container.Resolve<IDataService<GameModel, GameRequest>>();
            await gameDataService.ConfigureTable();
            var gameId = gameService.GenerateGameId();
            await gameDataService.Create(gameId);
            Assert.True(true);
        }

        [Fact]
        public async Task GameExistsIsTrue()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task GameExistsIsFalse()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task GameUpdate()
        {
            var gameDataService = Container.Resolve<IDataService<GameModel, GameRequest>>();

            var gameModel = new GameModel
            {
                GameId = "Test",
                MyPlayer = new PlayerModel { PlayerId = "Test" },
                OtherPlayers = new List<PlayerModel>()
            };

            await gameDataService.Update(gameModel);
            Assert.True(true);
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
            Assert.True(true);
        }

        [Fact]
        public async Task PlayerCreate()
        {
            var gameService = Container.Resolve<IGameService>();
            var playerDataService = Container.Resolve<IDataService<GameModel, GameRequest>>();
            var gameId = gameService.GenerateGameId();
            var playerId = "Test";
            await playerDataService.Create(playerId);
            Assert.True(true);
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
                PlayerId = "Test",
                Squads = new List<SquadModel>()
            };

            await playerDataService.Update(playerModel);
            Assert.True(true);
        }

        [Fact]
        public async Task PlayerRequestUpdate()
        {
            throw new NotImplementedException();
        }
    }
}
