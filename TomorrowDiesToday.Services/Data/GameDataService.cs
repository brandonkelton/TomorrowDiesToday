using System;
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

        public IObservable<GameModel> DataReceived => _update;
        public IObservable<List<GameModel>> DataListReceived => _updateList;

        public async Task Create(string id)
        {
            await _client.CreateGame(id);
        }

        public async Task<bool> Exists(string id)
        {
            return await _client.GameExists(_game.GameId);
        }

        public async Task RequestUpdate(GameRequest request)
        {
            await _client.RequestPlayerList(_game.GameId);
        }

        public async Task Update(GameModel gameModel)
        {
            if (gameModel.MyPlayer?.Squads != null)
            {
                var playerDTO = PlayerToDTO(gameModel.MyPlayer);
                await _client.Update(playerDTO);
            }
        }

        public PlayerDTO PlayerToDTO(PlayerModel playerModel)
        {
            var squadDTOs = new List<SquadDTO>();
            foreach (SquadModel squadModel in playerModel.Squads)
            {
                var squadDTO = new SquadDTO
                {
                    SquadId = squadModel.SquadId,
                    Data = squadModel.Data,
                    Stats = squadModel.Stats
                };
                squadDTOs.Add(squadDTO);
            }
            var playerDTO = new PlayerDTO
            {
                GameId = _game.GameId,
                PlayerId = playerModel.PlayerId,
                Squads = squadDTOs
            };
            return playerDTO;
        }
    }
}
