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
using TomorrowDiesToday.Navigation;
using TomorrowDiesToday.Views;
using Xamarin.Forms;

namespace TomorrowDiesToday.ViewModels
{
    public class JoinGameViewModel : BaseViewModel, IJoinGameViewModel
    {
        private INavigationService _navigationService;
        private IGameService _gameService;
        private IDataService<GameModel, GameRequest> _gameDataService;
        public ICommand SetIsJoiningGameCommand { get; private set; }
        public ICommand JoinGameCommand { get; private set; }
        public ICommand NextStepCommand { get; private set; }

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

        private bool _gameJoined;
        public bool GameJoined
        {
            get => _gameJoined;
            set => SetProperty(ref _gameJoined, value);
        }

        private bool _invalidGameId;
        public bool InvalidGameId
        {
            get => _invalidGameId;
            set => SetProperty(ref _invalidGameId, value);
        }

        private bool _isLoadingData;
        public bool IsLoadingData
        {
            get => _isLoadingData;
            set => SetProperty(ref _isLoadingData, value);
        }

        public JoinGameViewModel(INavigationService navigationService, IGameService gameService, IDataService<GameModel, GameRequest> gameDataService)
        {
            _navigationService = navigationService;
            _gameService = gameService;
            _gameDataService = gameDataService;

            //IsWaitingForSelection = true;
            //ConfigureCommands();
            //SubscribeToUpdates();
        }

        private void ConfigureCommands()
        {
            //CreateGameCommand = new Command(async () => await CreateGame());
            JoinGameCommand = new Command(async () => await JoinGame());
            //NextStepCommand = new Command(() => NextAfterGameCreated());
        }

        private async Task JoinGame()
        {
            if (await _gameDataService.Exists(GameId))
            {
                InvalidGameId = false;
                GameJoined = true;
                //IsJoiningGame = false;
                //IsCreatingOrJoiningGame = false;
                //IsSelectingPlayers = true;
                //return;
                await _navigationService.NavigateTo<SelectCharacterPage>();
            }

            GameJoined = false;
            InvalidGameId = true;
        }
    }
}
