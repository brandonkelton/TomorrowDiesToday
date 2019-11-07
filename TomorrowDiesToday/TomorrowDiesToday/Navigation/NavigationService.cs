using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Autofac;
using TomorrowDiesToday.Models.Templates;
using TomorrowDiesToday.Views;

namespace TomorrowDiesToday.Navigation
{
    public class NavigationService : INavigationService, IOnInitAsync
    {
        private NavigationPage _navigationPage { get; set; }

        public NavigationService(NavigationPage navigationPage)
        {
            _navigationPage = navigationPage;
        }

        public async Task OnInitAsync()
        {
            await NavigateTo<StartPage>();
        }

        public async Task NavigateTo<T>() where T : Page
        {
            var page = IoC.Container.Resolve<T>();
            await _navigationPage.PushAsync(page);
        }
    }
}
