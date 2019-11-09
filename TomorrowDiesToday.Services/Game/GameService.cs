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
    public class GameService : IGameService
    {
        private GameModel _game { get; set; }
        public GameModel Game => _game;

        private readonly ReplaySubject<string> _errorMessage = new ReplaySubject<string>(1);
        public IObservable<string> GameErrorMessage => _errorMessage;

        private readonly ReplaySubject<GameModel> _thisGame
            = new ReplaySubject<GameModel>(1);
        public IObservable<GameModel> ThisGame => _thisGame;

        private IDataService<GameModel, GameRequest> _gameDataService;
        private static Random _random = new Random();

        public GameService(IDataService<GameModel, GameRequest> gameDataService)
        {
            _gameDataService = gameDataService;
        }

        public async Task CreateGame()
        {
            bool gameExists = true;
            string gameId = "";
            GameRequest request = new GameRequest();
            while (gameExists)
            {
                gameId = GenerateGameId();
                request = new GameRequest { GameId = gameId };
                gameExists = await _gameDataService.Exists(request);
            }
            await _gameDataService.Create(request);
            _game = new GameModel
            {
                GameId = gameId,
                ThisPlayer = new PlayerModel(),
                OtherPlayers = new Dictionary<string, PlayerModel>(),
                ActiveTiles = new Dictionary<string, TileModel>(),
                AllTiles = new Dictionary<string, TileModel>(),
                SelectedSquads = new Dictionary<string, SquadModel>()
            };
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
                    ThisPlayer = new PlayerModel(),
                    OtherPlayers = new Dictionary<string, PlayerModel>(),
                    ActiveTiles = new Dictionary<string, TileModel>(),
                    AllTiles = new Dictionary<string, TileModel>(),
                    SelectedSquads = new Dictionary<string, SquadModel>()
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

        private string GenerateGameId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            string gameId = new string(Enumerable.Repeat(chars, chars.Length)
              .Select(s => s[_random.Next(s.Length)]).Take(6).ToArray());

            return gameId;
        }
    }
}
