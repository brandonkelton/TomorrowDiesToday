using Autofac;
using System;
using System.Threading.Tasks;
using TomorrowDiesToday.Navigation;
using TomorrowDiesToday.Services;
using TomorrowDiesToday.Services.LocalStorage;
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
            var navigationService = IoC.Container.Resolve<INavigationService>();
            MainPage = navigationService.Navigation;
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void CleanUp()
        {
            IoC.Destroy();
            base.CleanUp();
        }
    }
}
