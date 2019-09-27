using Autofac;
using System;
using TomorrowDiesToday.Services;
using TomorrowDiesToday.Services.Communication;
using TomorrowDiesToday.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace TomorrowDiesToday
{
    public partial class App : Application
    {
        public static IContainer Container { get; private set; }
        private static readonly ContainerBuilder _builder = new ContainerBuilder();

        public App()
        {
            InitializeComponent();

            //DependencyResolver.ResolveUsing(type => Container.IsRegistered(type) ? Container.Resolve(type) : null);
            //RegisterServices();
            //Container = _builder.Build();
            MainPage = new MainPage();
        }

        private static void RegisterServices()
        {
            _builder.RegisterInstance(StartupServices.GetDataStore());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
