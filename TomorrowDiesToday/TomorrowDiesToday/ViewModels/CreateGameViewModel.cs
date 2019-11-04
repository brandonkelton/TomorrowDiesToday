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
using Xamarin.Forms;
using TomorrowDiesToday.Models.Templates;
using TomorrowDiesToday.Navigation;
using TomorrowDiesToday.Views;

namespace TomorrowDiesToday.ViewModels
{
    public class CreateGameViewModel : BaseViewModel, ICreateGameViewModel, IOnInitAsync
    {
        public string Title => "Tomorrow Dies Today";
        private IGameService _gameService;
        private IDataService<GameModel, GameRequest> _gameDataService;
        private INavigationService _navService;

        public ICommand NextStepCommand { get; private set; }

        public CreateGameViewModel(IGameService gameService, IDataService<GameModel, GameRequest> gameDataService, INavigationService navService)
        {
            _gameService = gameService;
            _gameDataService = gameDataService;
            _navService = navService;

            NextStepCommand = new Command(async () => await GoToCharacterPage());
        }

        public async Task OnInitAsync()
        {
            await CreateGame();
        }

        private bool _isLoadingData;
        public bool IsLoadingData
        {
            get => _isLoadingData;
            private set => SetProperty(ref _isLoadingData, value);
        }

        public string GameId
        {
            get => _gameService.GameId;
            private set
            {
                _gameService.GameId = value;
                OnPropertyChanged(nameof(GameId));
            }
        }

        private bool _gameCreated;
        public bool GameCreated
        {
            get => _gameCreated;
            private set => SetProperty(ref _gameCreated, value);
        }

        private async Task CreateGame()
        {
            IsLoadingData = true;

            while (!GameCreated)
            {
                GameId = _gameService.GenerateGameId();
                var gameExists = await _gameDataService.Exists(GameId);
                
                if (!gameExists)
                {
                    await _gameDataService.Create(GameId);
                    GameCreated = true;
                }
            }

            IsLoadingData = false;
        }

        private async Task GoToCharacterPage()
        {
            await _navService.NavigateTo<SelectCharacterPage>();
        }
    }
}
