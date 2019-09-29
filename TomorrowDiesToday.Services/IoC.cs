using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data;
using TomorrowDiesToday.Services.Data.Models;

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
            _builder.RegisterType<SquadDataService>().As<IDataService<SquadModel, SquadRequest>>().SingleInstance();
        }
    }
}
