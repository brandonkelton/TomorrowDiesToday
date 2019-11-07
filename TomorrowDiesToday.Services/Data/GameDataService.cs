using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Database;
using TomorrowDiesToday.Services.Database.DTOs;
using TomorrowDiesToday.Services.Data.Models;

namespace TomorrowDiesToday.Services.Data
{
    public class GameDataService : IDataService<GameModel, GameRequest>
    {
        public IObservable<GameModel> DataReceived => _update;
        public IObservable<List<GameModel>> DataDictReceived => _updateDict;

        private readonly ReplaySubject<GameModel> _update = new ReplaySubject<GameModel>(1);
        private readonly ReplaySubject<List<GameModel>> _updateDict = new ReplaySubject<List<GameModel>>(1);
        private IDBClient _client;

        public GameDataService(IDBClient client)
        {
            _client = client;
        }

        public async Task ConfigureTable()
        {
            await _client.InitializeGameTable();
        }

        public async Task Create(GameRequest request)
        {
            await _client.CreateGame(request.GameId);
        }

        public async Task<bool> Exists(GameRequest request)
        {
            bool result = await _client.GameExists(request.GameId);
            return result;
        }

        public async Task RequestUpdate(GameRequest request)
        {
            var gameDTO = await _client.RequestGame(request.GameId);
            var gameModel = GameToModel(gameDTO);
            _update.OnNext(gameModel);
        }

        public async Task Update(GameModel gameModel)
        {
            if (gameModel.Tiles.Count > 0)
            {
                var gameDTO = GameToDTO(gameModel);
                await _client.UpdateGame(gameDTO);
            }
        }

        private GameDTO GameToDTO(GameModel gameModel)
        {
            var tileDTOs = new List<TileDTO>();
            foreach (TileModel tile in gameModel.Tiles)
            {
                var tileDTO = new TileDTO
                {
                    TileId = tile.TileId
                };
                tileDTOs.Add(tileDTO);
            }
            var gameDTO = new GameDTO
            {
                GameId = gameModel.GameId,
                Tiles = tileDTOs
            };
            return gameDTO;
        }

        private GameModel GameToModel(GameDTO gameDTO)
        {
            var gameModel = new GameModel();

            foreach (TileDTO tileDTO in gameDTO.Tiles)
            {
                var tileModel = new TileModel
                {
                    TileId = tileDTO.TileId
                };
                gameModel.Tiles.Add(tileModel);
            }

            return gameModel;
        }
    }
}
