﻿using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Database;
using TomorrowDiesToday.Services.Database.DTOs;
using TomorrowDiesToday.Services.Data.Models;
using TomorrowDiesToday.Services.Game;

namespace TomorrowDiesToday.Services.Data
{
    public class GameDataService : IDataService<GameModel, GameRequest>
    {
        public IObservable<GameModel> DataReceived => _update;
        public IObservable<List<GameModel>> DataListReceived => _updateList;

        private readonly ReplaySubject<GameModel> _update = new ReplaySubject<GameModel>(1);
        private readonly ReplaySubject<List<GameModel>> _updateList = new ReplaySubject<List<GameModel>>(1);
        private IDBClient _client;
        private IGameService _game;

        public GameDataService(IDBClient client, IGameService game)
        {
            _client = client;
            _game = game;
        }

        public async Task ConfigureTable()
        {
            await _client.InitializeGameTable();
        }

        public async Task Create(string gameId)
        {
            await _client.CreateGame(gameId);
        }

        public async Task<bool> Exists(string gameId)
        {
            return await _client.GameExists(gameId);
        }

        public async Task RequestUpdate(GameRequest request)
        {
            var gameDTO = await _client.RequestGame(_game.GameId);
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
            foreach (KeyValuePair<string, TileModel> tile in gameModel.Tiles)
            {
                string tileId = tile.Key;
                TileModel tileModel = tile.Value;
                var tileDTO = new TileDTO
                {
                    TileId = tileModel.TileId
                };
                tileDTOs.Add(tileDTO);
            }
            var gameDTO = new GameDTO
            {
                GameId = _game.GameId,
                Tiles = tileDTOs
            };
            return gameDTO;
        }

        private GameModel GameToModel(GameDTO gameDTO)
        {
            var tileModels = new Dictionary<string, TileModel>();
            foreach (TileDTO tileDTO in gameDTO.Tiles)
            {
                var tileModel = new TileModel
                {
                    TileId = tileDTO.TileId
                };
                tileModels.Add(tileModel.TileId, tileModel);
            }
            var gameModel = new GameModel
            {
                Tiles = tileModels
            };
            return gameModel;
        }
    }
}
