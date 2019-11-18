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
using TomorrowDiesToday.Services.LocalStorage;

namespace TomorrowDiesToday.ViewModels
{
    public class ResumeGameViewModel : BaseViewModel, IResumeGameViewModel, IOnInitAsync
    {
        public string Title => "Tomorrow Dies Today";
        private IGameService _gameService;
        private INavigationService _navService;
        private ILocalStorageService _localStorage;

        public ICommand YesCommand { get; private set; }
        public ICommand NoCommand { get; private set; }

        public ResumeGameViewModel(IGameService gameService, INavigationService navService, ILocalStorageService localStorage)
        {
            _gameService = gameService;
            _navService = navService;
            _localStorage = localStorage;

            YesCommand = new Command(async () => await LoadGame());
            NoCommand = new Command(async () => await GoToStartPage());
        }

        public async Task OnInitAsync()
        {
            GameId = await _localStorage.GetGameId();
        }

        private string _gameId;
        public string GameId
        {
            get => _gameId;
            private set => SetProperty(ref _gameId, value);
        }

        private async Task LoadGame()
        {
            _localStorage.LoadGame();
            // TODO: should call out to server to locally refresh game and/or create game if it's been fully deleted
            await _navService.NavigateTo<MainPage>();
        }

        private async Task GoToStartPage()
        {
            await _navService.NavigateTo<StartPage>();
        }
    }
}
