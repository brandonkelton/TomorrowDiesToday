using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Game;
using TomorrowDiesToday.Services.LocalStorage;
using Xamarin.Forms;

namespace TomorrowDiesToday.ViewModels
{
    public sealed class MainPageViewModel : BaseViewModel
    {
        #region Observable Collections
        public ObservableCollection<PlayerModel> Players { get; private set; } = new ObservableCollection<PlayerModel>();
        public ObservableCollection<object> Items { get; }
        #endregion

        #region ICommands
        public ICommand RefreshPlayerListCommand { get; private set; }
        public ICommand ToggleSelectedSquadCommand { get; private set; }
        public ICommand SyncGameCommand { get; private set; }
        public ICommand ExitGameCommand { get; private set; }
        #endregion

        #region Services
        private IGameService _gameService;
        private IPlayerService _playerService;
        private ISquadService _squadService;
        private ILocalStorageService _storageService;
        #endregion

        #region Subscriptions
        private IDisposable _gameSubscription = null;
        private IDisposable _playerListSubscription = null;
        #endregion

        #region Properties
        private string _gameId;
        public string GameId
        {
            get => _gameId;
            set => SetProperty(ref _gameId, value);
        }
        #endregion

        #region Constructor
        public MainPageViewModel(IGameService gameService, IPlayerService playerService, ISquadService squadService, ILocalStorageService storageService)
        {
            _gameService = gameService;
            _playerService = playerService;
            _squadService = squadService;
            _storageService = storageService;

            Items = new ObservableCollection<object>
            {
                new {Title="First"},
                new {Title="second"},
                new {Title="third"}
            };
            ConfigureCommands();
            SubscribeToUpdates();
        }
        #endregion

        private void ConfigureCommands()
        {
            RefreshPlayerListCommand = new Command(async () => await RefreshPlayers());
            ToggleSelectedSquadCommand = new Command<string>(squadId => ToggleSelectedSquad(squadId));
            SyncGameCommand = new Command(async () => await SyncGame());
            ExitGameCommand = new Command(() => ExitGame());
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
                    List<SquadModel> sortedSquads = playerModel.Squads.OrderBy(squad => squad.SquadId).ToList();
                    playerModel.Squads = sortedSquads;
                    Players.Add(playerModel);
                });
            });
        }

        #region Tasks
        private async Task RefreshPlayers()
        {
            await _playerService.RequestPlayersUpdate();
        }

        private void ToggleSelectedSquad(string squadId)
        {
            SquadModel squadModel = Players.SelectMany(player => player.Squads.Where(squad => squad.SquadId == squadId)).FirstOrDefault();
            _squadService.ToggleSelected(squadModel);            
        }

        private async Task SyncGame()
        {
            await _gameService.SendGame();
            await _playerService.SendThisPlayer();

            await _gameService.RequestGameUpdate();
            await _playerService.RequestPlayersUpdate();

            await _storageService.SaveGame();
        }

        private void ExitGame()
        {
            Process.GetCurrentProcess().Kill();
        }
        #endregion

        public void Dispose()
        {
            if (_gameSubscription != null) _gameSubscription.Dispose();
            if (_playerListSubscription != null) _playerListSubscription.Dispose();
        }

    }
}
