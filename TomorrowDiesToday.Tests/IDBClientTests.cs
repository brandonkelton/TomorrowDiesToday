using Autofac;
using System;
using TomorrowDiesToday.Services.Database;
using Xunit;

namespace TomorrowDiesToday.Tests
{
    public class IDBClientTests
    {
        public static IContainer Container { get; private set; }
        private static readonly ContainerBuilder _builder = new ContainerBuilder();

        //[OnStart]
        //public void SetupTests()
        //{
        //    RegisterServices();
        //    Container = _builder.Build();
        //}

        private void RegisterServices()
        {

            _builder.RegisterType<DynamoClient>().As<IDBClient>().SingleInstance();
        }

        [Fact]
        public void Test1()
        {
            var db = Container.Resolve<IDBClient>();
            
        }
    }
}
