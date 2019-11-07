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
        public IObservable<Dictionary<string, PlayerModel>> DataDictReceived => _updateDict;

        private readonly ReplaySubject<PlayerModel> _update = new ReplaySubject<PlayerModel>(1);
        private readonly ReplaySubject<Dictionary<string, PlayerModel>> _updateDict = new ReplaySubject<Dictionary<string, PlayerModel>>(1);
        private IDBClient _client;

        public PlayerDataService(IDBClient client)
        {
            _client = client;
        }

        public async Task ConfigureTable()
        {
            await _client.InitializePlayerTable();
        }

        public async Task Create(PlayerRequest request)
        {
            await _client.CreatePlayer(request.GameId, request.PlayerId);
        }

        public async Task<bool> Exists(PlayerRequest request)
        {
            return await _client.PlayerExists(request.GameId, request.PlayerId);
        }

        public async Task RequestUpdate(PlayerRequest request)
        {
            if (request.PlayerId != null)
            {
                PlayerDTO playerDTO = await _client.RequestPlayer(request.GameId, request.PlayerId);
                PlayerModel playerModel = PlayerToModel(playerDTO);

                _update.OnNext(playerModel);
            }
            else
            {
                var playerDTOs = await _client.RequestPlayerList(request.GameId);
                var playerModels = new Dictionary<string, PlayerModel>();
                foreach (PlayerDTO playerDTO in playerDTOs)
                {
                    PlayerModel playerModel = PlayerToModel(playerDTO);
                    playerModel.GameId = request.GameId;
                    playerModels.Add(playerDTO.PlayerId, playerModel);
                }
                _updateDict.OnNext(playerModels);
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
            foreach (SquadModel squad in playerModel.Squads)
            {
                var squadDTO = new SquadDTO
                {
                    SquadId = squad.SquadId,
                    Data = squadModel.Data,
                    Stats = squadModel.Stats
                };
                squadDTOs.Add(squadDTO);
            }
            var playerDTO = new PlayerDTO
            {
                GameId = playerModel.GameId,
                PlayerId = playerModel.PlayerId,
                Squads = squadDTOs
            };
            return playerDTO;
        }

        private PlayerModel PlayerToModel(PlayerDTO playerDTO)
        {
            var squadModels = new Dictionary<string, SquadModel>();
            foreach (SquadDTO squadDTO in playerDTO.Squads)
            {
                var squadModel = new SquadModel
                {
                    SquadId = squadDTO.SquadId,
                    Data = squadDTO.Data,
                    Stats = squadDTO.Stats
                };
                squadModels.Add(squadModel.SquadId, squadModel);
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
