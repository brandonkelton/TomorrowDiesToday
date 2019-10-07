using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data;
using TomorrowDiesToday.Services.Data.Models;
using Xamarin.Forms;

namespace TomorrowDiesToday.ViewModels
{
    public class StartPageViewModel : BaseViewModel, IStartPageViewModel
    {
        private IDataService<GameModel, GameRequest> _gameDataService;
        private IDataService<PlayerModel, PlayerRequest> _playerDataService;
        private static Random _random = new Random();

        public ObservableCollection<PlayerModel> Players { get; private set; } = new ObservableCollection<PlayerModel>();

        public ICommand CreateGameCommand { get; private set; }
        public ICommand SetIsJoiningGameCommand { get; private set; }
        public ICommand JoinGameCommand { get; private set; }
        public ICommand NextStepCommand { get; private set; }
        public ICommand CreatePlayerCommand { get; private set; }
        public ICommand RefreshPlayerListCommand { get; private set; }

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

        private bool _isWaitingForSelection;
        public bool IsWaitingForSelection
        {
            get => _isWaitingForSelection;
            set => SetProperty(ref _isWaitingForSelection, value);
        }

        private bool _isCreatingOrJoiningGame;
        public bool IsCreatingOrJoiningGame
        {
            get => _isCreatingOrJoiningGame;
            set => SetProperty(ref _isCreatingOrJoiningGame, value);
        }

        private bool _isCreatingGame;
        public bool IsCreatingGame
        {
            get => _isCreatingGame;
            set => SetProperty(ref _isCreatingGame, value);
        }

        private bool _isJoiningGame;
        public bool IsJoiningGame
        {
            get => _isJoiningGame;
            set => SetProperty(ref _isJoiningGame, value);
        }

        private bool _gameCreated;
        public bool GameCreated
        {
            get => _gameCreated;
            set => SetProperty(ref _gameCreated, value);
        }

        private bool _gameJoined;
        public bool GameJoined
        {
            get => _gameJoined;
            set => SetProperty(ref _gameJoined, value);
        }

        private bool _isSelectingPlayers;
        public bool IsSelectingPlayers
        {
            get => _isSelectingPlayers;
            set => SetProperty(ref _isSelectingPlayers, value);
        }

        private bool _invalidGameId;
        public bool InvalidGameId
        {
            get => _invalidGameId;
            set => SetProperty(ref _invalidGameId, value);
        }

        private bool _playerExists;
        public bool PlayerExists
        {
            get => _playerExists;
            set => SetProperty(ref _playerExists, value);
        }

        private bool _isWaitingForPlayers;
        public bool IsWaitingForPlayers
        {
            get => _isWaitingForPlayers;
            set => SetProperty(ref _isWaitingForPlayers, value);
        }

        public StartPageViewModel(IDataService<GameModel, GameRequest> gameDataService, IDataService<PlayerModel, PlayerRequest> playerDataService)
        {
            _gameDataService = gameDataService;
            _playerDataService = playerDataService;

            ConfigureCommands();
        }

        private void ConfigureCommands()
        {
            CreateGameCommand = new Command(async () => await CreateGame());
            SetIsJoiningGameCommand = new Command(() => StartJoiningGame());
            JoinGameCommand = new Command(async () => await JoinGame());
            NextStepCommand = new Command(() => NextStep());
            CreatePlayerCommand = new Command<string>(async playerId => await CreatePlayer(playerId));
            RefreshPlayerListCommand = new Command(() => RefreshPlayers());
        }

        private void RefreshPlayers()
        {
            _playerDataService.RequestUpdate(new PlayerRequest { AsList = true });
        }

        private async Task CreatePlayer(string playerId)
        {
            if (!await _playerDataService.Exists(playerId))
            {
                await _playerDataService.Create(playerId);
                CurrentPlayer = $"You are {playerId}";
                IsWaitingForSelection = false;
                IsWaitingForPlayers = true;
                return;
            }

            PlayerExists = true;
        }

        private void NextStep()
        {
            IsSelectingPlayers = true;
        }

        private void StartJoiningGame()
        {
            IsWaitingForSelection = false;
            IsJoiningGame = true;
            IsCreatingGame = false;
            IsCreatingOrJoiningGame = true;
        }

        private async Task CreateGame()
        {
            IsWaitingForSelection = false;
            IsCreatingGame = true;
            IsJoiningGame = false;
            IsCreatingOrJoiningGame = true;

            while (!GameCreated)
            {
                var gameId = GenerateGameId();
                if (!await _gameDataService.Exists(gameId))
                {
                    await _gameDataService.Create(gameId);
                    GameId = gameId;
                    GameCreated = true;
                }
            }
        }

        // Maybe should be moved to service (GameStartService? GameService?)
        private string GenerateGameId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, chars.Length)
              .Select(s => s[_random.Next(s.Length)]).Take(6).ToArray());
        }

        private async Task JoinGame()
        {
            if (await _gameDataService.Exists(GameId))
            {
                InvalidGameId = false;
                await _gameDataService.Update(new GameModel { GameId = GameId });
                GameJoined = true;
                NextStep();
                return;
            }

            GameJoined = false;
            InvalidGameId = true;
        }
    }
}
