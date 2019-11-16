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
        public ObservableCollection<SquadModel> Squads { get; private set; } = new ObservableCollection<SquadModel>();

        public ICommand RefreshPlayerListCommand { get; private set; }

        private string _gameId;
        public string GameId
        {
            get => _gameId;
            set => SetProperty(ref _gameId, value);
        }

        private IGameService _gameService;
        private IPlayerService _playerService;
        private ISquadService _squadService;

        private IDisposable _gameSubscription = null;
        private IDisposable _playerListSubscription = null;
        private IDisposable _playerSquadsSubscription = null;

        public MainPageViewModel(IGameService gameService, IPlayerService playerService, ISquadService squadService)
        {
            _gameService = gameService;
            _playerService = playerService;
            _squadService = squadService;

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
            if (_playerSquadsSubscription != null) _playerSquadsSubscription.Dispose();
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

            _playerListSubscription = _playerService.OtherPlayersUpdate.Subscribe(playerModels =>
            {
                Players.Clear();
                Squads.Clear();
                playerModels.ForEach(playerModel => Players.Add(playerModel));
                playerModels.ForEach(playerModel => playerModel.Squads.ForEach(squadModel => Squads.Add(squadModel)));
            });

        }
    }
}
