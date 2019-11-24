using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private INavigationService _navService;
        private ILocalStorageService _localStorage;

        public ICommand YesCommand { get; private set; }
        public ICommand NoCommand { get; private set; }

        public ResumeGameViewModel(INavigationService navService, ILocalStorageService localStorage)
        {
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
            await _localStorage.LoadGame();
            await _navService.NavigateTo<MainPage>();
        }

        private async Task GoToStartPage()
        {
            await _localStorage.DeleteGame();
            await _navService.NavigateTo<StartPage>();
        }
    }
}
