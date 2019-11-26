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
using TomorrowDiesToday.Services.Database.DTOs;

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
            var gameId = "ABCDEF";
            var playerId = "0";
            var playerModel = new PlayerModel
            {
                GameId = gameId,
                PlayerId = playerId
            };
            await playerDataService.Create(playerModel);
            Assert.True(true); // pass if no exceptions thrown
        }

        [Fact]
        public async Task ExistsIsFalse()
        {
            var gameId = "ABCDEF";
            var playerId = "0";
            var playerRequest = new PlayerRequest
            {
                GameId = gameId,
                PlayerId = playerId
            };
            _mockClient.Setup(c => c.PlayerExists(gameId, playerId)).Returns(Task.FromResult(false));
            var playerDataService = Container.Resolve<IDataService<PlayerModel, PlayerRequest>>();
            var result = await playerDataService.Exists(playerRequest);
            Assert.False(result);
        }

        [Fact]
        public async Task ExistsIsTrue()
        {
            var gameId = "ABCDEF";
            var playerId = "0";
            var playerRequest = new PlayerRequest
            {
                GameId = gameId,
                PlayerId = playerId
            };
            _mockClient.Setup(c => c.PlayerExists(gameId, playerId)).Returns(Task.FromResult(true));
            var playerDataService = Container.Resolve<IDataService<PlayerModel, PlayerRequest>>();
            var result = await playerDataService.Exists(playerRequest);
            Assert.True(result);
        }

        [Fact]
        public async Task RequestUpdate()
        {
            var gameId = "ABCDEF";
            var playerId = "0";
            var singlePlayerRequest = new PlayerRequest
            {
                GameId = gameId,
                PlayerId = playerId
            };
            var allPlayerRequest = new PlayerRequest { GameId = gameId };
            var playerDTO = new PlayerDTO
            {
                GameId = gameId,
                PlayerId = playerId,
                Squads = new List<SquadDTO>()
            };
            var playerDTOs = new List<PlayerDTO> { playerDTO };

            _mockClient.Setup(c => c.RequestPlayer(gameId, playerId)).Returns(Task.FromResult(playerDTO));
            _mockClient.Setup(c => c.RequestPlayerList(gameId)).Returns(Task.FromResult(playerDTOs));
            var playerDataService = Container.Resolve<IDataService<PlayerModel, PlayerRequest>>();
            
            await playerDataService.RequestUpdate(singlePlayerRequest);
            await playerDataService.RequestUpdate(allPlayerRequest);

            Assert.True(true); // pass if no exceptions thrown
        }

        [Fact]
        public async Task Update()
        {
            var playerDataService = Container.Resolve<IDataService<PlayerModel, PlayerRequest>>();
            var playerModel = new PlayerModel
            {
                GameId = "ABCDEF",
                PlayerId = "0",
                Squads = new List<SquadModel>()
            };
            await playerDataService.Update(playerModel);
            Assert.True(true); // pass if no exceptions thrown
        }
    }
}
