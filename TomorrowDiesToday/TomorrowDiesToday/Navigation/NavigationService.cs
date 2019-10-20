using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TomorrowDiesToday.Navigation
{
    public class NavigationService : INavigationService
    {
        private INavigation Navigation { get; set; }

        public NavigationService(INavigation navgationPage)
        {
            Navigation = navgationPage;
        }


        public async Task NavigateTo<T>() where T: Page
        {
            var view = IoC.Container.Resolve<T>();
            await Navigation.PushAsync(view);
        }
    }
}
