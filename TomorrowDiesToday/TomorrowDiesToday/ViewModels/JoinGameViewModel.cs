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
using TomorrowDiesToday.Navigation;
using TomorrowDiesToday.Views;

namespace TomorrowDiesToday.ViewModels
{
    public class JoinGameViewModel : BaseViewModel, IJoinGameViewModel
    {
        public string Title => "Tomorrow Dies Today (Prototype)";
        private IGameService _gameService;
        private IDataService<GameModel, GameRequest> _gameDataService;
        private INavigationService _navService;

        public ICommand JoinGameCommand { get; private set; }

        public JoinGameViewModel(IGameService gameService, IDataService<GameModel, GameRequest> gameDataService, INavigationService navService)
        {
            _gameService = gameService;
            _gameDataService = gameDataService;
            _navService = navService;

            JoinGameCommand = new Command(async () => await JoinGame());
        }

        private string _gameId;
        public string GameId
        {
            get => _gameId;
            set
            {
                InvalidGameId = false;
                SetProperty(ref _gameId, value);
                _gameService.GameId = value;
            }
        }

        private bool _invalidGameId;
        public bool InvalidGameId
        {
            get => _invalidGameId;
            private set => SetProperty(ref _invalidGameId, value);
        }

        private bool _isLoadingData;
        public bool IsLoadingData
        {
            get => _isLoadingData;
            private set => SetProperty(ref _isLoadingData, value);
        }

        private async Task JoinGame()
        {
            IsLoadingData = true;
            var gameExists = await _gameDataService.Exists(GameId);
            IsLoadingData = false;

            if (!gameExists)
            {
                InvalidGameId = true;
                return;
            }

            await _navService.NavigateTo<SelectCharacterPage>();
        }
    }
}
