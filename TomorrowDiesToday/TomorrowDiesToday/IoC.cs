using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data;
using TomorrowDiesToday.Services.Data.Models;
using TomorrowDiesToday.Services.Database;
using TomorrowDiesToday.Services.Game;
using TomorrowDiesToday.Templates;
using TomorrowDiesToday.ViewModels;
using TomorrowDiesToday.Views;
using TomorrowDiesToday.Navigation;
using Xamarin.Forms.Internals;

namespace TomorrowDiesToday
{
    public static class IoC
    {
        public static IContainer Container { get; private set; }

        // Temporary variable until we have something better
        private static bool _altConfig = false;

        public static void Initialize()
        {
            var builder = new ContainerBuilder();

            RegisterServices(builder);
            RegisterAndConfigureDB(builder);
            RegisterViewModels(builder);
            RegisterViews(builder);
            
            Container = builder.Build();

            var registeredComponents = Container.ComponentRegistry.Registrations;
            foreach (var c in registeredComponents)
            {
                c.Activated += ComponentActivated;
            }

            DependencyResolver.ResolveUsing(type => Container.IsRegistered(type) ? Container.Resolve(type) : null);
        }

        public static void Destroy()
        {
            Container.Dispose();
        }

        private static async void ComponentActivated(object sender, ActivatedEventArgs<object> e)
        {
            if (e.Instance is IOnInit)
            {
                (e.Instance as IOnInit).OnInit();
            }

            if (e.Instance is IOnInitAsync)
            {
                await (e.Instance as IOnInitAsync).OnInitAsync();
            }
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<DynamoClient>().As<IDBClient>().SingleInstance();
            builder.RegisterType<GameService>().As<IGameService>().SingleInstance();
            builder.RegisterType<GameDataService>().As<IDataService<GameModel, GameRequest>>().SingleInstance();
            builder.RegisterType<PlayerDataService>().As<IDataService<PlayerModel, PlayerRequest>>().SingleInstance();
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
        }

        private static void RegisterViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<MainPageViewModel>().As<IMainPageViewModel>().SingleInstance();
            builder.RegisterType<StartPageViewModel>().As<IStartPageViewModel>().SingleInstance();
            builder.RegisterType<CreateGameViewModel>().As<ICreateGameViewModel>().SingleInstance();
            builder.RegisterType<JoinGameViewModel>().As<IJoinGameViewModel>().SingleInstance();
            builder.RegisterType<SelectCharacterViewModel>().As<ISelectCharacterViewModel>().SingleInstance();
            builder.RegisterType<WaitForPlayersViewModel>().As<IWaitForPlayersViewModel>().SingleInstance();
        }

        private static void RegisterViews(ContainerBuilder builder)
        {
            builder.RegisterType<MainPage>().SingleInstance();
            builder.RegisterType<StartPage>().SingleInstance();
            builder.RegisterType<CreateGamePage>().SingleInstance();
            builder.RegisterType<JoinGamePage>().SingleInstance();
            builder.RegisterType<SelectCharacterPage>().SingleInstance();
            builder.RegisterType<WaitForPlayersPage>().SingleInstance();
        }

        private static void RegisterAndConfigureDB(ContainerBuilder builder)
        {
            if (_altConfig)
            { // Use DynamoDB-local
                var config = new AmazonDynamoDBConfig
                {
                    // Replace localhost with server IP to connect with DynamoDB-local on remote server
                    ServiceURL = "http://localhost:8000/"
                };

                // Client ID is set in DynamoDB-local shell, http://localhost:8000/shell
                builder.RegisterType<AmazonDynamoDBClient>().OnPreparing(args =>
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
                builder.RegisterType<AmazonDynamoDBClient>().OnPreparing(args =>
                {
                    var credentialsParam = new NamedParameter("credentials", credentials);
                    var regionParam = new NamedParameter("region", RegionEndpoint.USEast2);
                    args.Parameters = new[] { credentialsParam, regionParam };
                }).As<IAmazonDynamoDB>().SingleInstance();
            }

            builder.RegisterType<DynamoDBContext>().As<IDynamoDBContext>().SingleInstance();
        }
    }
}