using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data;
using TomorrowDiesToday.Services.Data.Models;
using TomorrowDiesToday.Services.Database;
using TomorrowDiesToday.Services.Database.DTOs;

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
            //_builder.RegisterType<SquadDataTransferService>().As<IDataTransferService<SquadRequestDTO, SquadResponseDTO, SquadUpdateRequestDTO>>().SingleInstance();
            //_builder.RegisterType<SquadListDataTransferService>().As<IDataTransferService<SquadListRequestDTO, SquadListResponseDTO, SquadListUpdateDTO>>().SingleInstance();
            _builder.RegisterType<SquadDataService>().As<IDataService<SquadModel, PlayerRequest>>().SingleInstance();
        }
    }
}
