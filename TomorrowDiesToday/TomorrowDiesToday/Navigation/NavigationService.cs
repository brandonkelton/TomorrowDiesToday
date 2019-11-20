using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Autofac;
using TomorrowDiesToday.Models.Templates;
using TomorrowDiesToday.Views;
using TomorrowDiesToday.Services.LocalStorage;

namespace TomorrowDiesToday.Navigation
{
    public class NavigationService : INavigationService, IOnInitAsync
    {
        private ILocalStorageService _localStorage;

        public NavigationPage Navigation { get; private set; }

        public NavigationService(ILocalStorageService localStorage)
        { // circular dependency between navservice and startpage init
            var startPage = IoC.Container.Resolve<StartPage>();
            Navigation = new NavigationPage(startPage);
            _localStorage = localStorage;
        }

        public async Task NavigateTo<T>() where T : Page
        {
            var page = IoC.Container.Resolve<T>();
            await Navigation.PushAsync(page).ConfigureAwait(true);
        }

        public async Task OnInitAsync()
        {
            //if (await _localStorage.GetGameExists().ConfigureAwait(true))
            //{
            //    await NavigateTo<ResumeGamePage>().ConfigureAwait(true);
            //}
            //else
            //{
            //    await NavigateTo<StartPage>().ConfigureAwait(true);
            //}
        }
    }
}
