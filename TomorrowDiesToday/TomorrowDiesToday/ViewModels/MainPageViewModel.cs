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
        private IDisposable _playerDictSubscription = null;

        public MainPageViewModel(IGameService gameService, IPlayerService playerService)
        {
            _gameService = gameService;
            _playerService = playerService;

            Items = new ObservableCollection<object>
            {
                new {Title="First", ComReq=5, SteReq=6,CunReq=1,DipReq=2 },
                new {Title="second", ComReq=5, SteReq=6,CunReq=1,DipReq=2 },
                new {Title="third", ComReq=5, SteReq=6,CunReq=1,DipReq=2}
            };
            ConfigureCommands();
            SubscribeToUpdates();
        }

        public void Dispose()
        {
            if (_gameSubscription != null) _gameSubscription.Dispose();
            if (_playerDictSubscription != null) _playerDictSubscription.Dispose();
        }

        private void ConfigureCommands()
        {
            //NextStepCommand = new Command(() => NextAfterGameCreated());
            RefreshPlayerListCommand = new Command(() => RefreshPlayers());
        }

        private void RefreshPlayers()
        {
            _playerService.RequestPlayersUpdate();
        }

        private void SubscribeToUpdates()
        {
            _gameSubscription = _gameService.ThisGame.Subscribe(gameModel =>
            {
                GameId = gameModel.GameId;
            });
            _playerDictSubscription = _playerService.OtherPlayersUpdate.Subscribe(playerModels =>
            {
                Players.Clear();
                playerModels.ForEach(item => Players.Add(item));
            });
        }
    }
}
