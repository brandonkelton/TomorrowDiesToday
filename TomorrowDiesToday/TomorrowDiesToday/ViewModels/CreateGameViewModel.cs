using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Game;
using TomorrowDiesToday.Services.Data;
using TomorrowDiesToday.Services.Data.Models;
using Xamarin.Forms;
using TomorrowDiesToday.Models.Templates;
using TomorrowDiesToday.Navigation;
using TomorrowDiesToday.Views;
using TomorrowDiesToday.Services.LocalStorage;

namespace TomorrowDiesToday.ViewModels
{
    public sealed class CreateGameViewModel : BaseViewModel, IOnInitAsync, IDisposable
    {
        private IGameService _gameService;
        private INavigationService _navService;
        private IDisposable _gameSubscription = null;

        public ICommand NextStepCommand { get; private set; }

        public CreateGameViewModel(IGameService gameService, INavigationService navService)
        {
            Title = "Tomorrow Dies Today";

            _gameService = gameService;
            _navService = navService;

            NextStepCommand = new Command(async () => await GoToCharacterPage());

            SubscribeToUpdates();
        }

        public void Dispose()
        {
            if (_gameSubscription != null) _gameSubscription.Dispose();
        }

        public async Task OnInitAsync()
        {
            await CreateGame();
        }

        private bool _isLoadingData;
        public bool IsLoadingData
        {
            get => _isLoadingData;
            private set => SetProperty(ref _isLoadingData, value);
        }

        private string _gameId;
        public string GameId
        {
            get => _gameId;
            private set => SetProperty(ref _gameId, value);
        }

        private bool _gameCreated;
        public bool GameCreated
        {
            get => _gameCreated;
            private set => SetProperty(ref _gameCreated, value);
        }

        private async Task CreateGame()
        {
            IsLoadingData = true;

            await _gameService.CreateGame();
            GameCreated = true;

            IsLoadingData = false;
        }

        private async Task GoToCharacterPage()
        {
            await _navService.NavigateTo<SelectCharacterPage>();
        }

        private void SubscribeToUpdates()
        {
            _gameSubscription = _gameService.ThisGame.Subscribe(thisGame =>
            {
                GameId = thisGame.GameId;
            });
        }
    }
}
