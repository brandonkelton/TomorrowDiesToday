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
    public class PlayerDataService : IDataService<PlayerModel, PlayerRequest>
    {
        private readonly ReplaySubject<PlayerModel> _update = new ReplaySubject<PlayerModel>(1);
        private readonly ReplaySubject<List<PlayerModel>> _updateList = new ReplaySubject<List<PlayerModel>>(1);
        private IDBClient _client;
        private IGameService _game;

        public PlayerDataService(IDBClient client, IGameService game)
        {
            _client = client;
            _game = game;
        }

        public async Task ConfigureTable()
        {
            await _client.InitializePlayerTable();
        }

        public IObservable<PlayerModel> DataReceived => _update;
        public IObservable<List<PlayerModel>> DataListReceived => _updateList;

        public async Task Create(string id)
        {
            var existingPlayer = await Exists(id);
            if(existingPlayer)
            {
                // TODO: create app specific exception and handle at a higher level
                // ex:  PlayerExistsException(playerId)
                throw new Exception("Player already exists!");
            }
            await _client.CreatePlayer(_game.GameId, id);
        }

        public async Task<bool> Exists(string id)
        {
            return await _client.PlayerExists(_game.GameId, id);
        }

        public async Task RequestUpdate(PlayerRequest request)
        {
            if (request.Id != null)
            {
                var playerDTO = await _client.RequestPlayer(_game.GameId, request.Id);
                var squadModels = new List<Squad>();
                foreach (SquadDTO squadDTO in playerDTO.Squads)
                {
                    var model = new Squad
                    {
                        Id = squadDTO.Id,
                        Count = squadDTO.Count
                    };
                    squadModels.Add(model);
                }

                var playerModel = new PlayerModel
                {
                    PlayerId = playerDTO.PlayerId,
                    Squads = squadModels
                };

                _update.OnNext(playerModel);
            }
            else
            {
                var playerDTOs = await _client.RequestPlayerList(_game.GameId);
                var playerModels = new List<PlayerModel>();
                foreach(PlayerDTO playerDTO in playerDTOs)
                {
                    var squadModels = new List<Squad>();
                    foreach (SquadDTO dto in playerDTO.Squads)
                    {
                        var model = new Squad
                        {
                            Id = dto.Id,
                            Count = dto.Count
                        };
                        squadModels.Add(model);
                    }

                    var playerModel = new PlayerModel
                    {
                        PlayerId = playerDTO.PlayerId,
                        Squads = squadModels
                    };
                    playerModels.Add(playerModel);
                }
                
                _updateList.OnNext(playerModels);
            }
        }

        public async Task Update(PlayerModel model)
        {
            var squadDTOs = new List<SquadDTO>();
            foreach(Squad squad in model.Squads)
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
                PlayerId = model.PlayerId,
                Squads = squadDTOs
            };

            await _client.Update(player);
        }
    }
}
