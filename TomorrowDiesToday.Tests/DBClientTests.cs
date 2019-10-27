using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using TomorrowDiesToday.Services.Database;
using TomorrowDiesToday.Services.Database.DTOs;
using Xunit;

namespace TomorrowDiesToday.Tests
{
    public class DBClientTests
    {
        private IContainer Container;

        private Mock<IAmazonDynamoDB> _mockClient = new Mock<IAmazonDynamoDB>();
        private Mock<IDynamoDBContext> _mockContext = new Mock<IDynamoDBContext>();

        public DBClientTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DynamoClient>().As<IDBClient>().InstancePerLifetimeScope();
            builder.RegisterInstance(_mockClient.Object).As<IAmazonDynamoDB>().SingleInstance();
            builder.RegisterInstance(_mockContext.Object).As<IDynamoDBContext>().SingleInstance();
            Container = builder.Build();
        }

        [Fact]
        public async Task GameExistsIsTrue()
        {
            var checkingGameId = "1234";
            var returnedGameId = "1234";
            var returnedGameDTO = new GameDTO { GameId = returnedGameId };

            _mockContext.Setup(c => c.LoadAsync<GameDTO>(checkingGameId, default)).Returns(Task.FromResult(returnedGameDTO));

            var client = Container.Resolve<IDBClient>();
            var result = await client.GameExists(checkingGameId);

            Assert.True(result);
        }

        [Fact]
        public async Task GameExistsIsFalse()
        {
            var checkingGameId = "1234";
            GameDTO returnedGameDTO = null;

            _mockContext.Setup(c => c.LoadAsync<GameDTO>(checkingGameId, default)).Returns(Task.FromResult(returnedGameDTO));

            var client = Container.Resolve<IDBClient>();
            var result = await client.GameExists(checkingGameId);

            Assert.False(result);
        }

        [Fact]
        public async Task PlayerExistsIsTrue()
        {
            var checkingGameId = "1234";
            var returnedGameId = "1234";
            var checkingPlayerId = "TestPlayer";
            var returnedPlayerId = "TestPlayer";
            PlayerDTO returnedPlayerDTO = new PlayerDTO { GameId = returnedGameId, PlayerId = returnedPlayerId };

            _mockContext.Setup(c => c.LoadAsync<PlayerDTO>(checkingGameId, checkingPlayerId, default)).Returns(Task.FromResult(returnedPlayerDTO));

            var client = Container.Resolve<IDBClient>();
            var result = await client.PlayerExists(checkingGameId, checkingPlayerId);

            Assert.True(result);
        }

        [Fact]
        public async Task PlayerExistsIsFalse()
        {
            var checkingGameId = "1234";
            var checkingPlayerId = "TestPlayer";
            PlayerDTO returnedPlayerDTO = null;

            _mockContext.Setup(c => c.LoadAsync<PlayerDTO>(checkingGameId, checkingPlayerId, default)).Returns(Task.FromResult(returnedPlayerDTO));

            var client = Container.Resolve<IDBClient>();
            var result = await client.PlayerExists(checkingGameId, checkingPlayerId);

            Assert.False(result);
        }

        [Fact]
        public async Task RequestPlayerListReturnsList()
        {
            var checkingGameId = "1234";
            var mockResults = new Mock<MockAsyncSearch<PlayerDTO>>();
            mockResults.Setup(c => c.GetRemainingAsync()).Returns(checkingPlayerList);
            _mockContext.SetReturnsDefault<AsyncSearch<PlayerDTO>>(mockResults);
            _mockContext.Setup(c => c.QueryAsync<PlayerDTO>(checkingGameId, default)).Returns(() => mockResults.Object);
        }
    }
}
