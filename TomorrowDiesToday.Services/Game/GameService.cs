using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Data;
using TomorrowDiesToday.Services.Data.Models;

namespace TomorrowDiesToday.Services.Game
{
    public class GameService : IGameService, IDisposable
    {
        #region Properties

        // Observables
        public IObservable<string> ErrorMessage => _errorMessage;
        public IObservable<GameModel> ThisGame => _thisGame;

        private readonly ReplaySubject<string> _errorMessage = new ReplaySubject<string>(1);
        private readonly ReplaySubject<GameModel> _thisGame = new ReplaySubject<GameModel>(1);

        private IDisposable _dataSubscription = null;

        // Requred Service(s)
        private IGameState _gameState;
        private IDataService<GameModel, GameRequest> _gameDataService;
        private IPlayerService _playerService;

        // Miscellaneous
        private static Random _random = new Random();

        #endregion

        #region Constructor

        public GameService(IGameState gameState, IDataService<GameModel, GameRequest> gameDataService, IPlayerService playerService)
        {
            _gameState = gameState;
            _gameDataService = gameDataService;
            _playerService = playerService;

            SubscribeToUpdates();
        }

        #endregion

        #region Public Methods

        public async Task CreateGame()
        {
            bool gameExists = true;
            string gameId = "";
            while (gameExists)
            {
                gameId = GenerateGameId();
                var request = new GameRequest { GameId = gameId };
                gameExists = await _gameDataService.Exists(request);
            }
            _gameState.SetGame(new GameModel
            {
                GameId = gameId,
                Players = new List<PlayerModel>(),
                Tiles = new List<TileModel>(),
                SelectedSquadStats = new SquadStats()
            });
            await _gameDataService.Create(_gameState.Game);
            _thisGame.OnNext(_gameState.Game);
        }

        public async Task SendGame()
        {
            var request = new GameRequest { GameId = _gameState.Game.GameId };
            if (!await _gameDataService.Exists(request))
            {
                await _gameDataService.Create(_gameState.Game);
            } else
            {
                await _gameDataService.Update(_gameState.Game);
            }
        }

        public void PushGame()
        {
            _thisGame.OnNext(_gameState.Game);
            _playerService.PushCurrentPlayer();
        }

        public async Task<bool> JoinGame(string gameId)
        {
            GameRequest request = new GameRequest { GameId = gameId };
            if (await _gameDataService.Exists(request))
            {
                _gameState.SetGame(new GameModel
                {
                    GameId = gameId,
                    Players = new List<PlayerModel>(),
                    Tiles = new List<TileModel>(),
                    SelectedSquadStats = new SquadStats()
                });
                _thisGame.OnNext(_gameState.Game);
                return true;
            }
            else
            {
                _errorMessage.OnNext("Game doesn't exist!");
                return false;
            }
        }

        public async Task RequestGameUpdate()
        {
            var request = new GameRequest { GameId = _gameState.Game.GameId };
            await _gameDataService.RequestUpdate(request);
        }

        #endregion

        #region Helper Methods

        private string GenerateGameId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            string gameId = new string(Enumerable.Repeat(chars, chars.Length)
              .Select(s => s[_random.Next(s.Length)]).Take(6).ToArray());

            return gameId;
        }

        private void SubscribeToUpdates()
        {
            _dataSubscription = _gameDataService.DataReceived.Subscribe(game =>
            {
                _thisGame.OnNext(game);
            });
        }

        public void Dispose()
        {
            if (_dataSubscription != null) _dataSubscription.Dispose();
        }

        #endregion
    }
}
