using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data;
using TomorrowDiesToday.Services.Data.Models;
using TomorrowDiesToday.Services.Database;

namespace TomorrowDiesToday.Services
{
    public static class IoC
    {
        public static IContainer Container { get; private set; }
        private static readonly ContainerBuilder _builder = new ContainerBuilder();

        // Temporary variable until we have something better
        private static bool _altConfig = true;

        public static void Initialize()
        {
            RegisterAndConfigureDB();
            RegisterServices();

            Container = _builder.Build();
        }

        private static void RegisterServices()
        {
            _builder.RegisterType<DynamoClient>().As<IDBClient>().SingleInstance();
            _builder.RegisterType<GameDataService>().As<IDataService<GameModel, GameRequest>>().SingleInstance();
            _builder.RegisterType<PlayerDataService>().As<IDataService<PlayerModel, PlayerRequest>>().SingleInstance();
            _builder.RegisterType<PlayerDataService>().As<IDataService<PlayerModel, PlayerRequest>>().SingleInstance();
        }

        private static void RegisterAndConfigureDB()
        {
            IAmazonDynamoDB client;
            if (_altConfig)
            { // Use DynamoDB-local
                var config = new AmazonDynamoDBConfig
                {
                    // Replace localhost with server IP to connect with DynamoDB-local on remote server
                    ServiceURL = "http://localhost:8000/"
                };

                // Client ID is set in DynamoDB-local shell, http://localhost:8000/shell
                client = new AmazonDynamoDBClient("TomorrowDiesToday", "fakeSecretKey", config);
                _builder.Register(c => client).As<IAmazonDynamoDB>().SingleInstance();
            }
            else
            { // Use AWS DynamoDB
                var credentials = new TDTCredentials();
                client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast2);
                _builder.Register(c => client).As<IAmazonDynamoDB>().SingleInstance();
            }

            _builder.Register(c => new DynamoDBContext(client)).As<IDynamoDBContext>().SingleInstance();
        }
    }
}
