using Autofac;
using System;
using TomorrowDiesToday.Navigation;
using TomorrowDiesToday.Services;
using TomorrowDiesToday.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace TomorrowDiesToday
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            IoC.Initialize();
            MainPage = IoC.Container.Resolve<NavigationPage>();
            
            // Hack to activate nav service - I need to work this out
            IoC.Container.Resolve<INavigationService>();
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

        protected override void CleanUp()
        {
            base.CleanUp();
            IoC.Destroy();
        }
    }
}
