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

        // Main GameModel instance
        public GameModel Game => _game;
        private GameModel _game { get; set; }

        // Requred Service(s)
        private IDataService<GameModel, GameRequest> _gameDataService;

        // Miscellaneous
        private static Random _random = new Random();

        #endregion

        #region Constructor

        public GameService(IDataService<GameModel, GameRequest> gameDataService)
        {
            _gameDataService = gameDataService;
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
            _game = new GameModel
            {
                GameId = gameId,
                Players = new List<PlayerModel>(),
                Tiles = new List<TileModel>(),
                SelectedSquadStats = new SquadStats()
            };
            await _gameDataService.Create(_game);
            _thisGame.OnNext(Game);
        }

        public async Task SendGame()
        {
            await _gameDataService.Update(_game);
        }

        public void SetGame(GameModel game)
        {
            _game = game;
            _thisGame.OnNext(Game);
        }

        public async Task<bool> JoinGame(string gameId)
        {
            GameRequest request = new GameRequest { GameId = gameId };
            if (await _gameDataService.Exists(request))
            {
                _game = new GameModel
                {
                    GameId = gameId,
                    Players = new List<PlayerModel>(),
                    Tiles = new List<TileModel>(),
                    SelectedSquadStats = new SquadStats()
                };
                _thisGame.OnNext(Game);
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
            var request = new GameRequest { GameId = Game.GameId };
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
