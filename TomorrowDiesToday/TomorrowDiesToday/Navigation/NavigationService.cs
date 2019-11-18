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
    public class NavigationService : INavigationService, IAfterInitAsync
    {
        private NavigationPage _navigationPage;
        private ILocalStorageService _localStorage;

        public NavigationService(NavigationPage navigationPage, ILocalStorageService localStorage)
        {
            _navigationPage = navigationPage;
            _localStorage = localStorage;
        }

        public async Task AfterInitAsync()
        {
            //await NavigateTo<StartPage>();
            //if (await _localStorage.GameExists())
            //{
            //    await NavigateTo<ResumeGamePage>();
            //}
            //else
            //{
            //    await NavigateTo<StartPage>();
            //}
        }

        public async Task NavigateTo<T>() where T : Page
        {
            var page = IoC.Container.Resolve<T>();
            await _navigationPage.PushAsync(page);
        }
    }
}
