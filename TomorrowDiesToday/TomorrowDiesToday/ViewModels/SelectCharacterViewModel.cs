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
using TomorrowDiesToday.Navigation;
using TomorrowDiesToday.Views;
using TomorrowDiesToday.Models.Enums;

namespace TomorrowDiesToday.ViewModels
{
    public class SelectCharacterViewModel : BaseViewModel, ISelectCharacterViewModel, IDisposable
    {
        public string Title => "Tomorrow Dies Today (Prototype)";
        private IGameService _gameService;
        private IPlayerService _playerService;
        private INavigationService _navService;
        private IDisposable _gameSubscription = null;

        public ICommand SelectPlayerCommand { get; private set; }

        public SelectCharacterViewModel(IGameService gameService, IPlayerService playerService, INavigationService navService)
        {
            _gameService = gameService;
            _playerService = playerService;
            _navService = navService;

            SelectPlayerCommand = new Command<string>(async playerId => await SelectPlayer(playerId));
            SubscribeToUpdates();
        }

        private string _gameId;
        public string GameId
        {
            get => _gameId;
            private set => SetProperty(ref _gameId, value);
        }

        private bool _playerExists;
        public bool PlayerExists
        {
            get => _playerExists;
            private set => SetProperty(ref _playerExists, value);
        }

        private bool _isLoadingData;
        public bool IsLoadingData
        {
            get => _isLoadingData;
            private set => SetProperty(ref _isLoadingData, value);
        }

        private string _playerAlreadySelected;
        public string PlayerAlreadySelected
        {
            get => _playerAlreadySelected;
            private set => SetProperty(ref _playerAlreadySelected, value);
        }

        public void Dispose()
        {
            if (_gameSubscription != null) _gameSubscription.Dispose();
        }

        private async Task SelectPlayer(string playerId)
        {
            PlayerAlreadySelected = String.Empty;
            PlayerExists = false;
            IsLoadingData = true;
            if (! await _playerService.ChoosePlayer(playerId))
            {
                PlayerAlreadySelected = $"{playerId} Has Already Been Sdelected";
                IsLoadingData = false;
                return;
            }
            IsLoadingData = false;

            await _navService.NavigateTo<MainPage>();
        }

        private void SubscribeToUpdates()
        {
            _gameSubscription = _gameService.ThisGame.Subscribe(thisGame =>
            {
                _gameId = thisGame.GameId;
            });
        }
    }
}
