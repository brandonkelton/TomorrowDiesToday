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
    public class WaitForPlayersViewModel : BaseViewModel, IWaitForPlayersViewModel, IDisposable
    {
        private IGameService _gameService;
        private IPlayerService _playerService;
        private INavigationService _navService;
        private IDisposable _gameSubscription = null;
        private IDisposable _playerDictSubscription = null;

        public ObservableCollection<PlayerModel> Players { get; private set; } = new ObservableCollection<PlayerModel>();

        public ICommand RefreshPlayerListCommand { get; private set; }
        public ICommand ContinueCommand { get; private set; }

        private string _gameId;
        public string GameId
        {
            get => _gameId;
            set => SetProperty(ref _gameId, value);
        }

        private string _currentPlayer;
        public string CurrentPlayer
        {
            get => _currentPlayer;
            set => SetProperty(ref _currentPlayer, value);
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

        public WaitForPlayersViewModel(IGameService gameService, IPlayerService playerService, INavigationService navService)
        {
            _gameService = gameService;
            _playerService = playerService;
            _navService = navService;

            //IsWaitingForSelection = true;
            ConfigureCommands();
            SubscribeToUpdates();
        }

        private void ConfigureCommands()
        {
            //NextStepCommand = new Command(() => NextAfterGameCreated());
            RefreshPlayerListCommand = new Command(() => RefreshPlayers());
            ContinueCommand = new Command(async () => await Continue());
        }

        private void SubscribeToUpdates()
        {
            _gameSubscription = _gameService.ThisGame.Subscribe(gameModel =>
            {
                GameId = gameModel.GameId;
                var playerArmamentType = ((ArmamentType) int.Parse(gameModel.PlayerId));
                CurrentPlayer = playerArmamentType.ToDescription();
            });
            _playerDictSubscription = _playerService.OtherPlayersUpdate.Subscribe(playerModels =>
            {
                Players = new ObservableCollection<PlayerModel>(playerModels);
            });
        }

        private async Task Continue()
        {
            await _navService.NavigateTo<MainPage>();
        }

        private void RefreshPlayers()
        {
            _playerService.RequestPlayersUpdate();
        }

        public void Dispose()
        {
            if (_gameSubscription != null) _gameSubscription.Dispose();
            if (_playerDictSubscription != null) _playerDictSubscription.Dispose();
        }
    }
}
