using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Services.Communication;

namespace TomorrowDiesToday.Services
{
    public static class StartupServices
    {
        private static IContainer _container;

        public static void Configure()
        {
            RegisterServices();
        }

        public static IDataService GetDataService()
        {
            return _container.Resolve<IDataService>();
        }

        private static void RegisterServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Communicator>().As<ICommunicator>();
            builder.RegisterType<DataService>().As<IDataService>();
            builder.RegisterType<Pipeline>().As<IPipeline>();

            _container = builder.Build();
        }
    }

    
}
