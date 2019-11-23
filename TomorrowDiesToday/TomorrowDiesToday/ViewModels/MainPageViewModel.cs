using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data;
using TomorrowDiesToday.Services.Game;
using Xamarin.Forms;

namespace TomorrowDiesToday.ViewModels
{
    public class MainPageViewModel : BaseViewModel, IMainPageViewModel
    {
        public ObservableCollection<PlayerModel> Players { get; private set; } = new ObservableCollection<PlayerModel>();

        public ObservableCollection<object> Items { get; }

        public ICommand RefreshPlayerListCommand { get; private set; }

        private string _gameId;
        public string GameId
        {
            get => _gameId;
            set => SetProperty(ref _gameId, value);
        }

        private IGameService _gameService;
        private IPlayerService _playerService;

        private IDisposable _gameSubscription = null;
        private IDisposable _playerListSubscription = null;

        public MainPageViewModel(IGameService gameService, IPlayerService playerService)
        {
            _gameService = gameService;
            _playerService = playerService;

            Items = new ObservableCollection<object>
            {
                new {Title="First"},
                new {Title="second"},
                new {Title="third"}
            };
            ConfigureCommands();
            SubscribeToUpdates();
        }

        public void Dispose()
        {
            if (_gameSubscription != null) _gameSubscription.Dispose();
            if (_playerListSubscription != null) _playerListSubscription.Dispose();
        }

        private void ConfigureCommands()
        {
            RefreshPlayerListCommand = new Command(async () => await RefreshPlayers());
        }

        private async Task RefreshPlayers()
        {
            await _playerService.RequestPlayersUpdate();
        }

        private void SubscribeToUpdates()
        {
            _gameSubscription = _gameService.ThisGame.Subscribe(gameModel =>
            {
                GameId = gameModel.GameId;
            });

            _playerListSubscription = _playerService.OtherPlayersUpdate.Subscribe(playerModels =>
            {
                Players.Clear();
                playerModels.ForEach(playerModel =>
                {
                    Players.Add(playerModel);
                });
            });

        }
    }
}
