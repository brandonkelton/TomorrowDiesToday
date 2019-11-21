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
        {
            _localStorage = localStorage;

            var rootPage = new RootPage();
            Navigation = new NavigationPage(rootPage);
        }

        public async Task NavigateTo<T>() where T : Page
        {
            var page = IoC.Container.Resolve<T>();
            await Navigation.PushAsync(page).ConfigureAwait(true);
        }

        public async Task OnInitAsync()
        {
            if (await _localStorage.GetGameExists().ConfigureAwait(true))
            {
                await NavigateTo<ResumeGamePage>().ConfigureAwait(true);
            }
            else
            {
                await NavigateTo<StartPage>().ConfigureAwait(true);
            }
        }
    }
}
