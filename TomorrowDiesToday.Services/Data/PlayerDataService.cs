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
        public IObservable<PlayerModel> DataReceived => _dataReceived;
        public IObservable<List<PlayerModel>> DataListReceived => _dataListReceived;

        private readonly ReplaySubject<PlayerModel> _dataReceived = new ReplaySubject<PlayerModel>(1);
        private readonly ReplaySubject<List<PlayerModel>> _dataListReceived = new ReplaySubject<List<PlayerModel>>(1);
        private IDBClient _client;

        public PlayerDataService(IDBClient client)
        {
            _client = client;
        }

        public async Task ConfigureTable()
        {
            await _client.InitializePlayerTable();
        }

        public async Task Create(PlayerModel playerModel)
        {
            var playerDTO = PlayerToDTO(playerModel);
            await _client.UpdatePlayer(playerDTO);
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

                _dataReceived.OnNext(playerModel);
            }
            else
            {
                var playerDTOs = await _client.RequestPlayerList(request.GameId);
                var playerModels = new List<PlayerModel>();
                foreach (PlayerDTO playerDTO in playerDTOs)
                {
                    PlayerModel playerModel = PlayerToModel(playerDTO);
                    playerModel.GameId = request.GameId;
                    playerModels.Add(playerModel);
                }
                _dataListReceived.OnNext(playerModels);
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
                    Armaments = squadModel.Armaments,
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
            var squadModels = new List<SquadModel>();
            foreach (SquadDTO squadDTO in playerDTO.Squads)
            {
                var squadModel = new SquadModel
                {
                    PlayerId = playerDTO.PlayerId,
                    SquadId = squadDTO.SquadId,
                    Armaments = squadDTO.Armaments,
                    Stats = squadDTO.Stats
                };
                squadModels.Add(squadModel);
            }
            var playerArmamentType = ((ArmamentType)int.Parse(playerDTO.PlayerId));
            var playerModel = new PlayerModel
            {
                PlayerId = playerDTO.PlayerId,
                PlayerName = playerArmamentType.ToDescription(),
                PlayerType = playerArmamentType,
                Squads = squadModels
            };
            return playerModel;
        }
    }
}
