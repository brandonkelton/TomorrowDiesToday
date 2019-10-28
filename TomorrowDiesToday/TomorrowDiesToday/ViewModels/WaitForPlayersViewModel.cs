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

namespace TomorrowDiesToday.ViewModels
{
    public class WaitForPlayersViewModel : BaseViewModel, IWaitForPlayersViewModel, IDisposable
    {
        private IGameService _gameService;
        private IDataService<GameModel, GameRequest> _gameDataService;
        private IDataService<PlayerModel, PlayerRequest> _playerDataService;
        private IDisposable _playerListSubscription = null;

        public ObservableCollection<PlayerModel> Players { get; private set; } = new ObservableCollection<PlayerModel>();

        public ICommand RefreshPlayerListCommand { get; private set; }
        public ICommand NextStepCommand { get; private set; }

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

        private string _currentPlayer;
        public string CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                SetProperty(ref _currentPlayer, value);
                _gameService.PlayerId = value;
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

        public WaitForPlayersViewModel(IGameService gameService, IDataService<GameModel, GameRequest> gameDataService, IDataService<PlayerModel, PlayerRequest> playerDataService)
        {
            _gameService = gameService;
            _gameDataService = gameDataService;
            _playerDataService = playerDataService;

            //IsWaitingForSelection = true;
            ConfigureCommands();
            SubscribeToUpdates();
        }

        private void ConfigureCommands()
        {
            //NextStepCommand = new Command(() => NextAfterGameCreated());
            RefreshPlayerListCommand = new Command(() => RefreshPlayers());
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

        public void Dispose()
        {
            if (_playerListSubscription != null) _playerListSubscription.Dispose();
        }
    }
}
