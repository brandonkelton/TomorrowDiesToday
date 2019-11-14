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
using TomorrowDiesToday.Services.Game;

namespace TomorrowDiesToday.ViewModels
{
    public class StartPageViewModel : BaseViewModel, IStartPageViewModel
    {
        public string Title => "Tomorrow Dies Today (Prototype)";
        private INavigationService _navigationService;
        public ICommand CreateGameCommand { get; private set; }
        public ICommand JoinGameCommand { get; private set; }

        private IGameService _gameService;
        private INavigationService _navService;

        private bool _isLoadingData;
        public bool IsLoadingData
        {
            get => _isLoadingData;
            private set => SetProperty(ref _isLoadingData, value);
        }

        private bool _gameCreated;
        public bool GameCreated
        {
            get => _gameCreated;
            private set => SetProperty(ref _gameCreated, value);
        }

        public StartPageViewModel(INavigationService navigationService, IGameService gameService)
        {
            _navigationService = navigationService;
            _gameService = gameService;
            CreateGameCommand = new Command(async () => await CreateGame());
            JoinGameCommand = new Command(async () => await JoinGame());
        }


        private async Task CreateGame()
        {
            GameCreated = false;
            IsLoadingData = true;
            while (!GameCreated)
            {
                await _gameService.CreateGame();
                GameCreated = true;
            }
            IsLoadingData = false;
            await _navigationService.NavigateTo<CreateGamePage>();
        }

        private async Task JoinGame()
        {
            await _navigationService.NavigateTo<JoinGamePage>();
        }
    }
}
