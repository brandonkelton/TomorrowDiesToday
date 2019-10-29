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
        public IObservable<PlayerModel> DataReceived => _update;
        public IObservable<List<PlayerModel>> DataListReceived => _updateList;

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

        public async Task Create(string playerId)
        {
            var existingPlayer = await Exists(playerId);
            if(existingPlayer)
            {
                // TODO: create app specific exception and handle at a higher level
                // ex:  PlayerExistsException(playerId)
                throw new Exception("Player already exists!");
            }

            await _client.CreatePlayer(_game.GameId, playerId);
        }

        public async Task<bool> Exists(string playerId)
        {
            return await _client.PlayerExists(_game.GameId, playerId);
        }

        public async Task RequestUpdate(PlayerRequest request)
        {
            if (request.PlayerId != null)
            {
                var playerDTO = await _client.RequestPlayer(_game.GameId, request.PlayerId);
                var playerModel = PlayerToModel(playerDTO);

                _update.OnNext(playerModel);
            }
            else
            {
                var playerDTOs = await _client.RequestPlayerList(_game.GameId);
                var playerModels = new List<PlayerModel>();
                foreach (PlayerDTO playerDTO in playerDTOs)
                {
                    playerModels.Add(PlayerToModel(playerDTO));
                }
                _updateList.OnNext(playerModels);
            }
        }

        public async Task Update(PlayerModel playerModel)
        {
            var playerDTO = PlayerToDTO(playerModel);
            await _client.UpdatePlayer(playerDTO);
        }

        private PlayerDTO PlayerToDTO(PlayerModel playerModel)
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

        private PlayerModel PlayerToModel(PlayerDTO playerDTO)
        {
            var squadModels = new List<SquadModel>();
            foreach (SquadDTO squadDTO in playerDTO.Squads)
            {
                var squadModel = new SquadModel
                {
                    SquadId = squadDTO.SquadId,
                    Data = squadDTO.Data,
                    Stats = squadDTO.Stats
                };
                squadModels.Add(squadModel);
            }
            var playerModel = new PlayerModel
            {
                PlayerId = playerDTO.PlayerId,
                Squads = squadModels
            };
            return playerModel;
        }
    }
}
