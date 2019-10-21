using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TomorrowDiesToday.Views;
using Xamarin.Forms;
using TomorrowDiesToday.Navigation;

namespace TomorrowDiesToday.ViewModels
{
    public class StartPageViewModel : BaseViewModel, IStartPageViewModel
    {
        private INavigationService _navigationService;
        public ICommand CreateGameCommand { get; private set; }
        public ICommand JoinGameCommand { get; private set; }

        public StartPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            ConfigureCommands();
        }

        private void ConfigureCommands()
        {
            CreateGameCommand = new Command(async () => await CreateGame());
            JoinGameCommand = new Command(async () => await JoinGame());
        }

        private async Task CreateGame()
        {
            await _navigationService.NavigateTo<CreateGamePage>();
        }

        private async Task JoinGame()
        {
            await _navigationService.NavigateTo<JoinGamePage>();
        }

        /*
        private IGameService _gameService;
        private IDataService<GameModel, GameRequest> _gameDataService;
        private IDataService<PlayerModel, PlayerRequest> _playerDataService;
        private IDisposable _playerListSubscription = null;

        public ObservableCollection<PlayerModel> Players { get; private set; } = new ObservableCollection<PlayerModel>();

        public ICommand CreateGameCommand { get; private set; }
        public ICommand SetIsJoiningGameCommand { get; private set; }
        public ICommand JoinGameCommand { get; private set; }
        public ICommand NextStepCommand { get; private set; }
        public ICommand CreatePlayerCommand { get; private set; }
        public ICommand RefreshPlayerListCommand { get; private set; }
        //public ICommand ConfigureTableCommand { get; private set; }
        //public ICommand EncryptCommand { get; private set; }

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

        private bool _isLoadingData;
        public bool IsLoadingData
        {
            get => _isLoadingData;
            set => SetProperty(ref _isLoadingData, value);
        }

        //private string _text;
        //public string Text
        //{
        //    get => _text;
        //    set => SetProperty(ref _text, value);
        //}

        private string _playerAlreadySelected;
        public string PlayerAlreadySelected
        {
            get => _playerAlreadySelected;
            set => SetProperty(ref _playerAlreadySelected, value);
        }

        public StartPageViewModel(IGameService gameService, IDataService<GameModel, GameRequest> gameDataService, IDataService<PlayerModel, PlayerRequest> playerDataService)
        {
            _gameService = gameService;
            _gameDataService = gameDataService;
            _playerDataService = playerDataService;

            IsWaitingForSelection = true;
            ConfigureCommands();
            SubscribeToUpdates();
        }

        private void ConfigureCommands()
        {
            CreateGameCommand = new Command(async () => await CreateGame());
            SetIsJoiningGameCommand = new Command(() => StartJoiningGame());
            JoinGameCommand = new Command(async () => await JoinGame());
            NextStepCommand = new Command(() => NextAfterGameCreated());
            CreatePlayerCommand = new Command<string>(async playerId => await CreatePlayer(playerId));
            RefreshPlayerListCommand = new Command(() => RefreshPlayers());
            // ConfigureTableCommand = new Command(async () => await ConfigureTable());
            //EncryptCommand = new Command(() => EncryptText());
        }

        //private void EncryptText()
        //{
        //    var encryptedText = TDTCredentials.EncryptString(Text);
        //    Text = encryptedText;
        //}

        private void NextAfterGameCreated()
        {
            IsCreatingGame = false;
            IsCreatingOrJoiningGame = false;
            IsSelectingPlayers = true;
        }

        private void SubscribeToUpdates()
        {
            _playerListSubscription = _playerDataService.DataListReceived.Subscribe(list =>
            {
                Players.Clear();
                list.ForEach(item => Players.Add(item));
            });
        }

        //private async Task ConfigureTable()
        //{
        //    await _gameDataService.ConfigureTable();
        //    await _playerDataService.ConfigureTable();
        //}

        private void RefreshPlayers()
        {
            _playerDataService.RequestUpdate(new PlayerRequest());
        }

        private async Task CreatePlayer(string playerId)
        {
            IsLoadingData = true;
            PlayerExists = false;

            if (!await _playerDataService.Exists(playerId))
            {
                await _playerDataService.Create(playerId);
                IsLoadingData = false;
                _gameService.PlayerId = playerId;
                CurrentPlayer = $"You are {playerId}";
                IsSelectingPlayers = false;
                IsWaitingForPlayers = true;
                return;
            }

            PlayerAlreadySelected = $"{playerId} Has Already Been Selected";
            PlayerExists = true;
        }

        private void StartJoiningGame()
        {
            IsWaitingForSelection = false;
            IsJoiningGame = true;
            IsCreatingOrJoiningGame = true;
        }

        private async Task CreateGame()
        {
            IsLoadingData = true;
            IsWaitingForSelection = false;
            IsCreatingGame = true;
            IsCreatingOrJoiningGame = true;

            while (!GameCreated)
            {
                var gameId = _gameService.GenerateGameId();
                _gameService.GameId = gameId;
                if (!await _gameDataService.Exists(gameId))
                {
                    await _gameDataService.Create(gameId);
                    IsLoadingData = false;
                    GameId = gameId;
                    GameCreated = true;
                }
            }
        }

        private async Task JoinGame()
        {
            if (await _gameDataService.Exists(GameId))
            {
                InvalidGameId = false;
                GameJoined = true;
                IsJoiningGame = false;
                IsCreatingOrJoiningGame = false;
                IsSelectingPlayers = true;
                return;
            }

            GameJoined = false;
            InvalidGameId = true;
        }

        public void Dispose()
        {
            if (_playerListSubscription != null) _playerListSubscription.Dispose();
        }
        */
    }
}
