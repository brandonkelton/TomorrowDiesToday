using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models;
using System.Threading.Tasks;

namespace TomorrowDiesToday.Services.Navigation
{
    public interface INavigationService
    {

        void PresentAsMainPage(ViewModelBase viewModel);

        void PresentAsNavigatableMainPage(ViewModelBase viewModel);

        Task NavigateTo(ViewModelBase viewModel);

        Task NavigateBack();

        Task NavigateBackToRoot();
    }
}
