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

        public async Task Update(GameModel model)
        {
            var squadDTOs = new List<SquadDTO>();
            foreach (Squad squad in model.MyPlayer.Squads)
            {
                var squadDTO = new SquadDTO
                {
                    Id = squad.Id,
                    Count = squad.Count
                };
                squadDTOs.Add(squadDTO);
            }
            var player = new PlayerDTO
            {
                GameId = _game.GameId,
                PlayerId = model.MyPlayer.PlayerId,
                Squads = squadDTOs
            };

            await _client.Update(player);
        }
    }
}
