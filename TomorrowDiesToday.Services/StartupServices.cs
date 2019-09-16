using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Services.Communication;
using TomorrowDiesToday.Services.Communication.PipelineServices;
using TomorrowDiesToday.Services.Store;

namespace TomorrowDiesToday.Services
{
    public static class StartupServices
    {
        private static IContainer _container;

        public static void Configure()
        {
            RegisterServices();
            ConfigurePipeline();
        }

        public static IDataService GetDataService()
        {
            return _container.Resolve<IDataService>();
        }

        public static IDataStore GetDataStore()
        {
            return _container.Resolve<IDataStore>();
        }

        private static void RegisterServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Communicator>().As<ICommunicator>();
            builder.RegisterType<DataService>().As<IDataService>();
            builder.RegisterType<Pipeline>().As<IPipeline>();
            builder.RegisterType<DataStore>().As<IDataStore>();

            _container = builder.Build();
        }

        /// <summary>
        /// When adding to the pipeline, the order matters.
        /// </summary>
        private static void ConfigurePipeline()
        {
            var pipeline = _container.Resolve<IPipeline>();

            // Add "In" pipeline services
            pipeline.AddService(PipelineDirection.In, new ModelValueValidator());

            //Add "Out" pipeline services
            pipeline.AddService(PipelineDirection.Out, new ModelValueValidator());
        }
    }

    
}
