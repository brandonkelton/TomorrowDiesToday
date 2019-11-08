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

namespace TomorrowDiesToday.ViewModels
{
    public class SelectCharacterViewModel : BaseViewModel, ISelectCharacterViewModel
    {
        public string Title => "Tomorrow Dies Today (Prototype)";
        private IGameService _gameService;
        private IDataService<PlayerModel, PlayerRequest> _playerDataService;
        private INavigationService _navService;

        public ICommand SelectPlayerCommand { get; private set; }

        public SelectCharacterViewModel(IGameService gameService, IDataService<PlayerModel, PlayerRequest> playerDataService, INavigationService navService)
        {
            _gameService = gameService;
            _playerDataService = playerDataService;
            _navService = navService;

            SelectPlayerCommand = new Command<string>(async playerId => await SelectPlayer(playerId));
        }

        public string GameId => _gameService.GameId;

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

        private async Task SelectPlayer(string playerId)
        {
            PlayerAlreadySelected = String.Empty;
            PlayerExists = false;

            IsLoadingData = true;
            PlayerExists = await _playerDataService.Exists(playerId);
            IsLoadingData = false;

            if (PlayerExists)
            {
                PlayerAlreadySelected = $"{playerId} Has Already Been Selected";
                return;
            }

            IsLoadingData = true;
            await _playerDataService.Create(playerId);
            IsLoadingData = false;
            _gameService.PlayerId = playerId;

            await _navService.NavigateTo<MainPage>();
        }
    }
}
