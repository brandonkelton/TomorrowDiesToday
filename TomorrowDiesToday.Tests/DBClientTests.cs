using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Autofac;
using System;
using TomorrowDiesToday.Services.Database;
using Xunit;

namespace TomorrowDiesToday.Tests
{
    public class DBClientTests
    {
        public static IContainer Container { get; private set; }
        private static readonly ContainerBuilder _builder = new ContainerBuilder();

        public DBClientTests()
        {
            // RegisterAndConfigureDB();
            RegisterAndConfigureDBLocal();
            RegisterServices();
            Container = _builder.Build();
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
            IAmazonDynamoDB client = new AmazonDynamoDBClient("TomorrowDiesToday", "fakeSecretKey", config);
            _builder.Register(c => client).As<IAmazonDynamoDB>().SingleInstance();
            _builder.Register(c => new DynamoDBContext(client)).As<IDynamoDBContext>().SingleInstance();
        }

        private void RegisterServices()
        {
            _builder.RegisterType<DynamoClient>().As<IDBClient>().SingleInstance();
        }

        [Fact]
        public void Test1()
        {
            var db = Container.Resolve<IDBClient>();
            
        }
    }
}
