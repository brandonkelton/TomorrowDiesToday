using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TomorrowDiesToday.Views;
using Xamarin.Forms;
using TomorrowDiesToday.Navigation;

namespace TomorrowDiesToday.ViewModels
{
    public class StartPageViewModel : BaseViewModel, IStartPageViewModel
    {
        private INavigationService _navigationService;
        public ICommand CreateGameCommand { get; private set; }
        public ICommand JoinGameCommand { get; private set; }

        public StartPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            CreateGameCommand = new Command(async () => await CreateGame());
            JoinGameCommand = new Command(async () => await JoinGame());
        }

        private async Task CreateGame()
        {
            await _navigationService.NavigateTo<CreateGamePage>();
        }

        private async Task JoinGame()
        {
            await _navigationService.NavigateTo<JoinGamePage>();
        }
    }
}
