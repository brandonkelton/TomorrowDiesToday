using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data;
using Xamarin.Forms;
using TomorrowDiesToday.Navigation;
using TomorrowDiesToday.Services.Game;

namespace TomorrowDiesToday.ViewModels
{
    public class MainPageViewModel : BaseViewModel, IMainPageViewModel, IDisposable
    {
        private IGameService _gameService;
        private INavigationService _navService;
        private IDisposable _gameSubscription = null;
        private IDisposable _playerDictSubscription = null;

        public ObservableCollection<PlayerModel> Players { get; private set; } = new ObservableCollection<PlayerModel>();

        public MainPageViewModel(INavigationService navigationService, IGameService gameService)
        {
            _navService = navigationService;
            _gameService = gameService;
            _gameService.RequestPlayersUpdate();
            SubscribeToUpdates();           

            Items = new ObservableCollection<object>
            {
                new {Title="First"},
                new {Title="second"},
                new {Title="third"}
            };
        }
        public ObservableCollection<object> Items { get; }

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

        private void SubscribeToUpdates()
        {
            _gameSubscription = _gameService.ThisGame.Subscribe(gameModel =>
            {
                GameId = gameModel.GameId;
                CurrentPlayer = gameModel.ThisPlayer.PlayerName;
            });
            _playerDictSubscription = _gameService.OtherPlayers.Subscribe(dict =>
            {
                Players.Clear();
                foreach (KeyValuePair<string, PlayerModel> player in dict)
                {
                    Players.Add(player.Value);
                }
            });
        }

        public void Dispose()
        {
            if (_gameSubscription != null) _gameSubscription.Dispose();
            if (_playerDictSubscription != null) _playerDictSubscription.Dispose();
        }
    }
}
