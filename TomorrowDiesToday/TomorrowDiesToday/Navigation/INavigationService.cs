using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TomorrowDiesToday.Navigation
{
    public interface INavigationService
    {
        NavigationPage Navigation { get; }

        Task NavigateTo<T>() where T : Page;
    }
}
