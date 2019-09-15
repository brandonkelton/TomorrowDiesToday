using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Services;
using TomorrowDiesToday.Services.Communication;

namespace TomorrowDiesToday
{
    public static class Startup
    {
        private static IContainer _container;

        public static void Configure()
        {
            StartupServices.Configure();
            RegisterServices();
        }

        private static void RegisterServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(StartupServices.GetDataService());

            _container = builder.Build();
        }
    }
}
