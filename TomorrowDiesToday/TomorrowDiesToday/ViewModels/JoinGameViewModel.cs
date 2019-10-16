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

namespace TomorrowDiesToday.ViewModels
{
    class JoinGameViewModel : BaseViewModel, IJoinGameViewModel
    {
        private IGameService _gameService;
        private IDataService<GameModel, GameRequest> _gameDataService;
        //public ICommand CreateGameCommand { get; private set; }
        public ICommand SetIsJoiningGameCommand { get; private set; }
        public ICommand JoinGameCommand { get; private set; }
        public ICommand NextStepCommand { get; private set; }
        //public ICommand CreatePlayerCommand { get; private set; }
        //public ICommand RefreshPlayerListCommand { get; private set; }

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

        private bool _isJoiningGame;
        public bool IsJoiningGame
        {
            get => _isJoiningGame;
            set => SetProperty(ref _isJoiningGame, value);
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
    }
}
