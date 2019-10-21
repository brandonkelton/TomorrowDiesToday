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
using TomorrowDiesToday.Navigation;
using TomorrowDiesToday.Views;
using Xamarin.Forms;

namespace TomorrowDiesToday.ViewModels
{
    public class SelectCharacterViewModel : BaseViewModel, ISelectCharacterViewModel
    {
        private INavigationService _navigationService;
        private IGameService _gameService;
        private IDataService<GameModel, GameRequest> _gameDataService;
        private IDataService<PlayerModel, PlayerRequest> _playerDataService;
        private IDisposable _playerListSubscription = null;

        public ObservableCollection<PlayerModel> Players { get; private set; } = new ObservableCollection<PlayerModel>();

        public ICommand NextStepCommand { get; private set; }
        public ICommand CreatePlayerCommand { get; private set; }

        private string _gameId;
        public string GameId
        {
            get => _gameId;
            set
            {
                SetProperty(ref _gameId, value);
                _gameService.GameId = value;
            }
        }

        private bool _playerExists;
        public bool PlayerExists
        {
            get => _playerExists;
            set => SetProperty(ref _playerExists, value);
        }

        private bool _isLoadingData;
        public bool IsLoadingData
        {
            get => _isLoadingData;
            set => SetProperty(ref _isLoadingData, value);
        }

        private string _playerAlreadySelected;
        public string PlayerAlreadySelected
        {
            get => _playerAlreadySelected;
            set => SetProperty(ref _playerAlreadySelected, value);
        }

        public SelectCharacterViewModel(INavigationService navigationService, IGameService gameService, IDataService<GameModel, GameRequest> gameDataService, IDataService<PlayerModel, PlayerRequest> playerDataService)
        {
            _navigationService = navigationService;
            _gameService = gameService;
            _gameDataService = gameDataService;
            _playerDataService = playerDataService;

            //IsWaitingForSelection = true;
            ConfigureCommands();
            SubscribeToUpdates();
        }

        private void ConfigureCommands()
        {
            NextStepCommand = new Command(async () => await NextStep());
            CreatePlayerCommand = new Command<string>(async playerId => await CreatePlayer(playerId));
        }

        private async Task NextStep()
        {
            _navigationService.NavigateTo<WaitForPlayersPage>();
        }

        private void SubscribeToUpdates()
        {
            _playerListSubscription = _playerDataService.DataListReceived.Subscribe(list =>
            {
                Players.Clear();
                list.ForEach(item => Players.Add(item));
            });
        }

        private void RefreshPlayers()
        {
            _playerDataService.RequestUpdate(new PlayerRequest());
        }

        private async Task CreatePlayer(string playerId)
        {
            IsLoadingData = true;
            PlayerExists = false;

            if (!await _playerDataService.Exists(playerId))
            {
                await _playerDataService.Create(playerId);
                IsLoadingData = false;
                _gameService.PlayerId = playerId;
                //CurrentPlayer = $"You are {playerId}";
                //IsSelectingPlayers = false;
                //IsWaitingForPlayers = true;
                return;
            }

            PlayerAlreadySelected = $"{playerId} Has Already Been Selected";
            PlayerExists = true;
        }
    }
}
