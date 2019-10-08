using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data;
using TomorrowDiesToday.Services.Data.Models;
using TomorrowDiesToday.Services.Game;
using TomorrowDiesToday.ViewModels;
using Xamarin.Forms.Internals;

namespace TomorrowDiesToday
{
    public static class IoC
    {
        public static IContainer Container { get; private set; }
        private static readonly ContainerBuilder _builder = new ContainerBuilder();

        public static void Initialize()
        {
            DependencyResolver.ResolveUsing(type => Container.IsRegistered(type) ? Container.Resolve(type) : null);

            Services.IoC.Initialize();
            RegisterServices();
            RegisterViewModels();

            Container = _builder.Build();
        }

        private static void RegisterServices()
        {
            _builder.Register(c => Services.IoC.Container.Resolve<IDataService<GameModel, GameRequest>>()).As<IDataService<GameModel, GameRequest>>();
            _builder.Register(c => Services.IoC.Container.Resolve<IDataService<PlayerModel, PlayerRequest>>()).As<IDataService<PlayerModel, PlayerRequest>>();
            _builder.Register(c => Services.IoC.Container.Resolve<IGameService>()).As<IGameService>();
        }

        private static void RegisterViewModels()
        {
            _builder.RegisterType<MainPageViewModel>().As<IMainPageViewModel>().SingleInstance();
            _builder.RegisterType<StartPageViewModel>().As<IStartPageViewModel>().SingleInstance();
        }
    }
}
