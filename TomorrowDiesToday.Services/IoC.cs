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

        public static void Initialize()
        {
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
    }
}
