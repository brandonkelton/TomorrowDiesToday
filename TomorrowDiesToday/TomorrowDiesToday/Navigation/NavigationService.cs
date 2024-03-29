﻿using System;
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
        public NavigationPage Navigation { get; private set; }

        public NavigationService()
        {
            var rootPage = new RootPage();
            Navigation = new NavigationPage(rootPage);
        }

        public async Task NavigateTo<T>() where T : Page
        {
            var page = IoC.Container.Resolve<T>();
            await Navigation.PushAsync(page);
        }

        public async Task OnInitAsync()
        {
            var localStorage = IoC.Container.Resolve<ILocalStorageService>();

            if (await localStorage.GetGameId() != null)
            {
                await NavigateTo<ResumeGamePage>();
            }
            else
            {
                await NavigateTo<StartPage>();
            }
        }
    }
}
