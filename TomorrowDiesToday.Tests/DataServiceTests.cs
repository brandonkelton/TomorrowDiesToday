using System;
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

namespace TomorrowDiesToday.Tests
{
    public class DataServiceTests
    {
        private static ContainerBuilder _builder = new ContainerBuilder();
        public static IContainer Container;
        private static bool _initialized = false;
        private static Random _random = new Random();
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        

        public DataServiceTests()
        {
            if (!_initialized)
            {
                // RegisterAndConfigureDB();
                RegisterAndConfigureDBLocal();
                RegisterServices();
                Container = _builder.Build();
                _initialized = true;
            }
        }

        /// <summary>
        /// Use this instead of the local DB functionality if you don't want to test against an actual DB.
        /// However, the current mocks throw an error as none of the methods are implemented, so some sort of very
        /// basic implementation should be set up for all methods... as there are quite a few, so maybe use local for now.  ;)
        /// </summary>
        private void RegisterAndConfigureDB()
        {
            _builder.RegisterType<MockAmazonDynamoDB>().As<IAmazonDynamoDB>().SingleInstance();
            _builder.RegisterType<MockDynamoDBContext>().As<IDynamoDBContext>().SingleInstance();
        }

        /// <summary>
        /// Use this instead of the regular mock configuration if you want to really test against your local DB
        /// </summary>
        private void RegisterAndConfigureDBLocal()
        {
            var config = new AmazonDynamoDBConfig
            {
                // Replace localhost with server IP to connect with DynamoDB-local on remote server
                ServiceURL = "http://localhost:8000/"
            };

            // Client ID is set in DynamoDB-local shell, http://localhost:8000/shell
            var client = new AmazonDynamoDBClient("TomorrowDiesToday", "fakeSecretKey", config);
            _builder.Register(c => client).As<IAmazonDynamoDB>().SingleInstance();
            _builder.Register(c => new DynamoDBContext(client)).As<IDynamoDBContext>().SingleInstance();
        }

        private void RegisterServices()
        {
            _builder.RegisterType<DynamoClient>().As<IDBClient>().SingleInstance();
            _builder.RegisterType<GameService>().As<IGameService>().SingleInstance();
            _builder.RegisterType<GameDataService>().As<IDataService<GameModel, GameRequest>>().SingleInstance();
            _builder.RegisterType<PlayerDataService>().As<IDataService<PlayerModel, PlayerRequest>>().SingleInstance();
        }

        [Fact]
        public async Task ConfigureTables()
        {
            var gameDataService = Container.Resolve<IDataService<GameModel, GameRequest>>();
            await gameDataService.ConfigureTable();
            Assert.True(true);
        }

        [Fact]
        public async Task CreateGame()
        {

            var gameService = Container.Resolve<IGameService>();
            var gameDataService = Container.Resolve<IDataService<GameModel, GameRequest>>();
            var gameId = gameService.GenerateGameId();
            await gameDataService.Create(gameId);
            Assert.True(true);
        }
    }
}
