using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Autofac;
using Autofac.Extras.FakeItEasy;
using Autofac.Extras.Moq;
using FakeItEasy;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private ContainerBuilder Builder = new ContainerBuilder();

        private Mock<IAmazonDynamoDB> _mockClient = new Mock<IAmazonDynamoDB>();
        private Mock<IDynamoDBContext> _mockContext = new Mock<IDynamoDBContext>();

        public DBClientTests()
        {
            Builder.RegisterType<DynamoClient>().As<IDBClient>().InstancePerLifetimeScope();
            Builder.RegisterInstance(_mockClient.Object).As<IAmazonDynamoDB>().SingleInstance();
            Builder.RegisterInstance(_mockContext.Object).As<IDynamoDBContext>().SingleInstance();
            //Builder.RegisterInstance((AsyncSearch<PlayerDTO>)System.Runtime.Serialization.FormatterServices
            //      .GetUninitializedObject(typeof(AsyncSearch<PlayerDTO>))).As<AsyncSearch<PlayerDTO>>().SingleInstance();

            Container = Builder.Build();
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
        public async Task RequestPlayerListReturnsEmptyList()
        {
            var checkingGameId = "1234";
            var checkingPlayerList = new List<PlayerDTO>()
            {
                new PlayerDTO { GameId = "1234", PlayerId = "Player1" },
                new PlayerDTO { GameId = "1234", PlayerId = "Player2" },
                new PlayerDTO { GameId = "1234", PlayerId = "Player3" }
            };

            using (var fake = new AutoFake())
            {
                //var fakeSearch = fake.Provide(Container.Resolve<AsyncSearch<PlayerDTO>>());

                //A.CallTo(() => fake.Resolve<AsyncSearch<PlayerDTO>>().GetRemainingAsync(default)).Returns(Task.FromResult(checkingPlayerList));
                //var fake = Fake.
                //var searchInstance = fake.Resolve<AsyncSearch<PlayerDTO>>();
                // var searchInstance = Container.Resolve<AsyncSearch<PlayerDTO>>();
                var fakeSearch = new Fake<AsyncSearch<PlayerDTO>>(x => x.)
                A.CallTo(() => fake.Resolve<AsyncSearch<PlayerDTO>>().GetRemainingAsync(default)).Returns(Task.FromResult(checkingPlayerList));
                A.CallTo(() => fake.Resolve<IDynamoDBContext>().QueryAsync<PlayerDTO>(checkingGameId, default)).Returns(fake.Resolve<AsyncSearch<PlayerDTO>>());

                var client = fake.Resolve<IDBClient>();
                var result = await client.RequestPlayerList(checkingGameId);

                Assert.True(result.Count == checkingPlayerList.Count);
            }
        }
    }
}
