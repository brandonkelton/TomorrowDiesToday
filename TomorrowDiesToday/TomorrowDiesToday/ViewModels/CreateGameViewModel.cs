﻿using System;
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
    public class CreateGameViewModel : BaseViewModel, ICreateGameViewModel
    {
        private IGameService _gameService;
        private IDataService<GameModel, GameRequest> _gameDataService;

        public ICommand CreateGameCommand { get; private set; }
        public ICommand NextStepCommand { get; private set; }

        private bool _isLoadingData;
        public bool IsLoadingData
        {
            get => _isLoadingData;
            set => SetProperty(ref _isLoadingData, value);
        }

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

        private bool _gameCreated;
        public bool GameCreated
        {
            get => _gameCreated;
            set => SetProperty(ref _gameCreated, value);
        }

        public CreateGameViewModel(IGameService gameService, IDataService<GameModel, GameRequest> gameDataService, IDataService<PlayerModel, PlayerRequest> playerDataService)
        {
            _gameService = gameService;
            _gameDataService = gameDataService;
            //_playerDataService = playerDataService;

            //IsWaitingForSelection = true;
            ConfigureCommands();
            //SubscribeToUpdates();
        }

        private void ConfigureCommands()
        {
            CreateGameCommand = new Command(async () => await CreateGame());
            //SetIsJoiningGameCommand = new Command(() => StartJoiningGame());
            //JoinGameCommand = new Command(async () => await JoinGame());
            //NextStepCommand = new Command(() => NextAfterGameCreated());
            //CreatePlayerCommand = new Command<string>(async playerId => await CreatePlayer(playerId));
            //RefreshPlayerListCommand = new Command(() => RefreshPlayers());
            // ConfigureTableCommand = new Command(async () => await ConfigureTable());
            //EncryptCommand = new Command(() => EncryptText());
        }

        private async Task CreateGame()
        {
            IsLoadingData = true;
            //IsWaitingForSelection = false;
            //IsCreatingGame = true;
            //IsCreatingOrJoiningGame = true;

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
    }
}