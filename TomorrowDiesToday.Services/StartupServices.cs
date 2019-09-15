using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Services.Communication;
using TomorrowDiesToday.Services.Communication.PipelineServices;

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

        private static void RegisterServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Communicator>().As<ICommunicator>();
            builder.RegisterType<DataService>().As<IDataService>();
            builder.RegisterType<Pipeline>().As<IPipeline>();

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
