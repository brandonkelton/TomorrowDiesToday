using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Services.Communication;

namespace TomorrowDiesToday.Services
{
    public class Startup
    {
        public void Configure()
        {
            RegisterServices();
        }

        private void RegisterServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Communicator>().As<ICommunicator>();
            builder.RegisterType<DataService>().As<IDataService>();
            builder.RegisterType<Pipeline>().As<IPipeline>();
        }
    }

    
}
