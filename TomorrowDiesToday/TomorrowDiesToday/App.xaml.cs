using Autofac;
using System;
using TomorrowDiesToday.Services;
using TomorrowDiesToday.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using TomorrowDiesToday.ViewModels;
using TomorrowDiesToday.Services.Navigation;

namespace TomorrowDiesToday
{
    public partial class App : Application, IHaveMainPage
    {
        public App()
        {
            InitializeComponent();
            IoC.Initialize();
            MainPage = IoC.Container.Resolve<INavigationPage>();
            //MainPage = new JoinGamePage();
            var navigator = new NavigationService(this, new ViewLocator());

            var rootViewModel = new StartPageViewModel();

            navigator.PresentAsNavigatableMainPage(rootViewModel);
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
