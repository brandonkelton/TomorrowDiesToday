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
    public class GameDataServiceTests
    {
        public static IContainer Container;

        private Mock<IDBClient> _mockClient = new Mock<IDBClient>();
        private Mock<IDynamoDBContext> _mockContext = new Mock<IDynamoDBContext>();
        private Mock<IGameService> _mockGameService = new Mock<IGameService>();
        private Mock<IDataService<GameModel, GameRequest>> _mockGameDataService = new Mock<IDataService<GameModel, GameRequest>>();
        
        public GameDataServiceTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<GameDataService>().As<IDataService<GameModel, GameRequest>>().InstancePerLifetimeScope();
            builder.RegisterInstance(_mockClient.Object).As<IDBClient>().SingleInstance();
            builder.RegisterInstance(_mockGameService.Object).As<IGameService>().SingleInstance();
            Container = builder.Build();
        }

        [Fact]
        public async Task ConfigureTable()
        {
            var gameDataService = Container.Resolve<IDataService<GameModel, GameRequest>>();
            await gameDataService.ConfigureTable();
            Assert.True(true); // pass if no exceptions thrown
        }

        [Fact]
        public async Task Create()
        {
            var gameDataService = Container.Resolve<IDataService<GameModel, GameRequest>>();
            var gameId = "TestGame";
            await gameDataService.Create(gameId);
            Assert.True(true); // pass if no exceptions thrown
        }

        [Fact]
        public async Task ExistsIsTrue()
        {
            var gameId = "TestGame";
            _mockClient.Setup(c => c.GameExists(gameId)).Returns(Task.FromResult(true));
            var gameDataService = Container.Resolve<IDataService<GameModel, GameRequest>>();
            var result = await gameDataService.Exists(gameId);
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsIsFalse()
        {
            var gameId = "TestGame";
            _mockClient.Setup(c => c.GameExists(gameId)).Returns(Task.FromResult(false));
            var gameDataService = Container.Resolve<IDataService<GameModel, GameRequest>>();
            var result = await gameDataService.Exists(gameId);
            Assert.False(result);
        }

        [Fact]
        public async Task Update()
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
        public async Task RequestUpdate()
        {
            throw new NotImplementedException();
        }
    }
}
