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
    public class CreateGameViewModel : BaseViewModel, ICreateGameViewModel, IDisposable
    {
        public string Title => "Tomorrow Dies Today";
        private IGameService _gameService;
        private INavigationService _navService;
        private IDisposable _gameSubscription = null;

        public ICommand NextStepCommand { get; private set; }

        public CreateGameViewModel(IGameService gameService, INavigationService navService)
        {
            _gameService = gameService;
            _navService = navService;

            NextStepCommand = new Command(async () => await GoToCharacterPage());

            SubscribeToUpdates();
        }

        public void Dispose()
        {
            if (_gameSubscription != null) _gameSubscription.Dispose();
        }

        private string _gameId;
        public string GameId
        {
            get => _gameId;
            private set => SetProperty(ref _gameId, value);
        }

        private async Task GoToCharacterPage()
        {
            await _navService.NavigateTo<SelectCharacterPage>();
        }

        private void SubscribeToUpdates()
        {
            _gameSubscription = _gameService.ThisGame.Subscribe(thisGame =>
            {
                GameId = thisGame.GameId;
            });
        }
    }
}
