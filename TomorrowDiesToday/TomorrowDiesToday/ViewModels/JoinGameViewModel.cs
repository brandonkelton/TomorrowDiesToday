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
    public sealed class JoinGameViewModel : BaseViewModel
    {
        private IGameService _gameService;
        private INavigationService _navService;

        public ICommand JoinGameCommand { get; private set; }

        public JoinGameViewModel(IGameService gameService, INavigationService navService)
        {
            Title = "Tomorrow Dies Today";

            _gameService = gameService;
            _navService = navService;

            JoinGameCommand = new Command(async () => await JoinGame());
        }

        private string _gameId;
        public string GameId
        {
            get => _gameId;
            set => SetProperty(ref _gameId, value);
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
            set => SetProperty(ref _isLoadingData, value);
        }

        private async Task JoinGame()
        {
            if (! await _gameService.JoinGame(GameId))
            {
                InvalidGameId = true;
                IsLoadingData = false;
                return;
            }
            IsLoadingData = false;
            InvalidGameId = false;

            await _navService.NavigateTo<SelectCharacterPage>();
        }
    }
}
