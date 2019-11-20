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
    public class OtherPlayersModel : ObservableCollection<SquadModel> {
        public string PlayerName { get; set; }
        public string PlayerId { get; set; }
        private bool IsVisible = true;
        public ICommand Toggle { get; private set; }
        public OtherPlayersModel(){
            Toggle = new Command(() => ToggleSquads());
        }
        private void ToggleSquads()
        {
            IsVisible = !IsVisible;
        }
    }
    public class MainPageViewModel : BaseViewModel, IMainPageViewModel
    {
        public ObservableCollection<PlayerModel> Players { get; private set; } = new ObservableCollection<PlayerModel>();
        public ObservableCollection<object> Items { get; }
        public ObservableCollection<SquadModel> Squads { get; private set; } = new ObservableCollection<SquadModel>();
        public ObservableCollection<OtherPlayersModel> OtherPlayers { get; set; }

        public ICommand RefreshPlayerListCommand { get; private set; }

        private string _gameId;
        public string GameId
        {
            get => _gameId;
            set => SetProperty(ref _gameId, value);
        }

        private IGameService _gameService;
        private IPlayerService _playerService;
        //private ISquadService _squadService;

        private IDisposable _gameSubscription = null;
        private IDisposable _playerListSubscription = null;
        //private IDisposable _playerSquadsSubscription = null;

        public MainPageViewModel(IGameService gameService, IPlayerService playerService)
        {
            _gameService = gameService;
            _playerService = playerService;

            OtherPlayers = new ObservableCollection<OtherPlayersModel>();

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
                //playerModels.ForEach(playerModel => Players.Add(playerModel));
                playerModels.ForEach(playerModel => playerModel.Squads.ForEach(squadModel => Squads.Add(squadModel)));
                playerModels.ForEach(playerModel => OtherPlayers.Add(new OtherPlayersModel() { PlayerId=playerModel.PlayerId, PlayerName=playerModel.PlayerName}));
                foreach (OtherPlayersModel other in OtherPlayers) {
                    foreach(SquadModel squad in Squads)
                    {
                        string squadId = squad.SquadId.Substring(0, 1);
                        if(other.PlayerId == squadId)
                        {
                            other.Add(squad);
                        }
                    }
                }
            });

        }
    }
}
