using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Game;
using Xamarin.Forms;

namespace TomorrowDiesToday.ViewModels
{


    public class MainPageViewModel : BaseViewModel, IMainPageViewModel
    {
        #region Observable Collections
        public ObservableCollection<PlayerModel> Players { get; private set; } = new ObservableCollection<PlayerModel>();

        public ObservableCollection<PlayerModel> ThisPlayer { get; private set; } = new ObservableCollection<PlayerModel>();

        public ObservableCollection<object> Items { get; }
        #endregion

        #region ICommands
        public ICommand RefreshPlayerListCommand { get; private set; }

        public ICommand ToggleSelectedSquadCommand { get; private set; }

        public ICommand SelectSquad1Command { get; private set; }

        public ICommand SelectSquad2Command { get; private set; }

        public ICommand SelectSquad3Command { get; private set; }
        #endregion

        #region Services
        private IGameService _gameService;
        private IPlayerService _playerService;
        private ISquadService _squadService;
        #endregion

        #region Subscriptions
        private IDisposable _gameSubscription = null;
        private IDisposable _playerSubscription = null;
        private IDisposable _playerListSubscription = null;
        #endregion

        #region Properties

        private string _gameId;
        public string GameId
        {
            get => _gameId;
            set => SetProperty(ref _gameId, value);
        }

        private bool _squad1Selected;
        public bool Squad1Selected
        {
            get => _squad1Selected;
            set => SetProperty(ref _squad1Selected, value);
        }

        private bool _squad2Selected;
        public bool Squad2Selected
        {
            get => _squad2Selected;
            set => SetProperty(ref _squad2Selected, value);
        }

        private bool _squad3Selected;
        public bool Squad3Selected
        {
            get => _squad3Selected;
            set => SetProperty(ref _squad3Selected, value);
        }

        private bool _squad1Selectable;
        public bool Squad1Selectable
        {
            get => _squad1Selectable;
            set => SetProperty(ref _squad1Selectable, value);
        }

        private bool _squad2Selectable;
        public bool Squad2Selectable
        {
            get => _squad2Selectable;
            set => SetProperty(ref _squad2Selectable, value);
        }

        private bool _squad3Selectable;
        public bool Squad3Selectable
        {
            get => _squad3Selectable;
            set => SetProperty(ref _squad3Selectable, value);
        }

        #endregion

        #region Constructor
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

            Squad1Selected = true;
            Squad2Selected = false;
            Squad3Selected = false;
            Squad1Selectable = !Squad1Selected;
            Squad2Selectable = !Squad2Selected;
            Squad3Selectable = !Squad3Selected;
        }
        #endregion

        private void ConfigureCommands()
        {
            RefreshPlayerListCommand = new Command(async () => await RefreshPlayers());
            ToggleSelectedSquadCommand = new Command<string>(squadId => ToggleSelectedSquad(squadId));
            SelectSquad1Command = new Command(() => SelectSquad(1));
            SelectSquad2Command = new Command(() => SelectSquad(2));
            SelectSquad3Command = new Command(() => SelectSquad(3));
        }

        private void SelectSquad(int squad)
        {
            switch (squad)
            {
                case 1:
                    Squad1Selected = true;
                    Squad2Selected = false;
                    Squad3Selected = false;
                    break;
                case 2:
                    Squad1Selected = false;
                    Squad2Selected = true;
                    Squad3Selected = false;
                    break;
                case 3:
                    Squad1Selected = false;
                    Squad2Selected = false;
                    Squad3Selected = true;
                    break;
                default:
                    break;
            }

            Squad1Selectable = !Squad1Selected;
            Squad2Selectable = !Squad2Selected;
            Squad3Selectable = !Squad3Selected;
        }

        private void SubscribeToUpdates()
        {
            _gameSubscription = _gameService.ThisGameUpdate.Subscribe(gameModel =>
            {
                GameId = gameModel.GameId;
            });

            _playerSubscription = _playerService.ThisPlayerUpdate.Subscribe(playerModel =>
            {
                List<SquadModel> sortedSquads = playerModel.Squads.OrderBy(squad => squad.SquadId).ToList();
                playerModel.Squads = sortedSquads;
                ThisPlayer.Clear();
                ThisPlayer.Add(playerModel);
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
            List<SquadModel> squads = Players.SelectMany(player => player.Squads).ToList();
            squads.AddRange(ThisPlayer[0].Squads);
            SquadModel squadModel = squads.Where(squad => squad.SquadId == squadId).FirstOrDefault();
            _squadService.ToggleSelected(squadModel);            
        }
        #endregion

        public void Dispose()
        {
            if (_gameSubscription != null) _gameSubscription.Dispose();
            if (_playerSubscription != null) _playerSubscription.Dispose();
            if (_playerListSubscription != null) _playerListSubscription.Dispose();
        }

    }
}
