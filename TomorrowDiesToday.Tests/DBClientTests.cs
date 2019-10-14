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
            RegisterAndConfigureDB();
            // RegisterAndConfigureDBLocal();
            RegisterServices();
            Container = _builder.Build();
        }

        /// <summary>
        /// This should be called when running general tests. (Default)
        /// </summary>
        private void RegisterAndConfigureDB()
        {
            _builder.RegisterType<MockAmazonDynamoDB>().As<IAmazonDynamoDB>().SingleInstance();
            _builder.RegisterType<MockDynamoDBContext>().As<IDynamoDBContext>().SingleInstance();
        }

        /// <summary>
        /// If you need to test actual DB functionality, call this method instead of RegisterAndConfigureDB(). 
        /// You will need a local copy of DynamoDB installed with the KeyID and SecretAccessKey specified 
        /// in the client initialization within this method.
        /// </summary>
        private void RegisterAndConfigureDBLocal()
        {
            var config = new AmazonDynamoDBConfig
            {
                // Replace localhost with server IP to connect with DynamoDB-local on remote server
                ServiceURL = "http://localhost:8000/"
            };

            // Client ID is set in DynamoDB-local shell, http://localhost:8000/shell
            _builder.RegisterType<AmazonDynamoDBClient>().OnPreparing(args =>
            {
                var accessKeyIdParam = new NamedParameter("awsAccessKeyId", "TomorrowDiesToday");
                var accessKeyParam = new NamedParameter("awsSecretAccessKey", "fakeSecretKey");
                var clientConfig = new NamedParameter("clientConfig", config);
                args.Parameters = new []{ accessKeyIdParam, accessKeyParam, clientConfig };
            }).As<IAmazonDynamoDB>().SingleInstance();
            _builder.RegisterType<DynamoDBContext>().As<IDynamoDBContext>().SingleInstance();
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
