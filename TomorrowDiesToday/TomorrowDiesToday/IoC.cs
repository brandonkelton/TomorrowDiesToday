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
using TomorrowDiesToday.Services.Game;
using TomorrowDiesToday.ViewModels;
using Xamarin.Forms.Internals;

namespace TomorrowDiesToday
{
    public static class IoC
    {
        public static IContainer Container { get; private set; }
        private static readonly ContainerBuilder _builder = new ContainerBuilder();

        // Temporary variable until we have something better
        private static bool _altConfig = false;

        public static void Initialize()
        {
            DependencyResolver.ResolveUsing(type => Container.IsRegistered(type) ? Container.Resolve(type) : null);

            RegisterServices();
            RegisterAndConfigureDB();
            RegisterViewModels();

            Container = _builder.Build();
        }

        private static void RegisterServices()
        {
            _builder.RegisterType<DynamoClient>().As<IDBClient>().SingleInstance();
            _builder.RegisterType<GameService>().As<IGameService>().SingleInstance();
            _builder.RegisterType<GameDataService>().As<IDataService<GameModel, GameRequest>>().SingleInstance();
            _builder.RegisterType<PlayerDataService>().As<IDataService<PlayerModel, PlayerRequest>>().SingleInstance();
        }

        private static void RegisterViewModels()
        {
            _builder.RegisterType<MainPageViewModel>().As<IMainPageViewModel>().SingleInstance();
            _builder.RegisterType<StartPageViewModel>().As<IStartPageViewModel>().SingleInstance();
            _builder.RegisterType<CreateGameViewModel>().As<ICreateGameViewModel>().SingleInstance();
            _builder.RegisterType<JoinGameViewModel>().As<IJoinGameViewModel>().SingleInstance();
            _builder.RegisterType<SelectCharacterViewModel>().As<ISelectCharacterViewModel>().SingleInstance();
            _builder.RegisterType<WaitForPlayersViewModel>().As<IWaitForPlayersViewModel>().SingleInstance();
        }

        private static void RegisterAndConfigureDB()
        {
            if (_altConfig)
            { // Use DynamoDB-local
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
                    args.Parameters = new[] { accessKeyIdParam, accessKeyParam, clientConfig };
                }).As<IAmazonDynamoDB>().SingleInstance();
            }
            else
            { // Use AWS DynamoDB
                var credentials = new TDTCredentials();
                _builder.RegisterType<AmazonDynamoDBClient>().OnPreparing(args =>
                {
                    var credentialsParam = new NamedParameter("credentials", credentials);
                    var regionParam = new NamedParameter("region", RegionEndpoint.USEast2);
                    args.Parameters = new[] { credentialsParam, regionParam };
                }).As<IAmazonDynamoDB>().SingleInstance();
            }

            _builder.RegisterType<DynamoDBContext>().As<IDynamoDBContext>().SingleInstance();
        }
    }
}
