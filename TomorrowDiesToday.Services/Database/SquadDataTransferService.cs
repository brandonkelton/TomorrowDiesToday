using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Services.Database.DTOs;
using Amazon.DynamoDBv2.Model;

namespace TomorrowDiesToday.Services.Database
{
    /// <summary>
    /// This service will receive models and update-requests from the DataService, and then send the corresponding DTO
    /// to the DynamoClient.  For update requests the service waits for DynamoClient to return a DTO and then returns
    /// the corresponding model to the DataService.
    /// </summary>
    public class SquadDataTransferService : IDataTransferService<SquadRequestDTO, SquadResponseDTO, SquadUpdateDTO>
    {
        public event EventHandler<SquadResponseDTO> ResponseReceived;
        private IDBClient _client;

        public SquadDataTransferService(IDBClient client)
        {
            _client = client;
            _client.SquadResponseReceived += OnSquadResponseReceived;
        }

        public async Task Request(SquadRequestDTO requestDTO)
        {
            await _client.RequestSquad(requestDTO);
        }

        public async Task Update(SquadUpdateDTO updateDTO)
        {
            //var squadDTO = new SquadUpdateDTO
            //{
            //    SquadId = squadModel.SquadId,
            //    GameId = squadModel.GameId,
            //    PlayerId = squadModel.PlayerId,
            //    SquadData = string.Join(",", squadModel.Data)
            //};

            await _client.SendSquad(updateDTO);
        }

        private SquadResponseDTO ConvertSquadResponse(List<Dictionary<string, AttributeValue>> squads)
        {
            var squadModel = new SquadResponseDTO();

            foreach (KeyValuePair<string, AttributeValue> entry in squads[0])
            {
                if (entry.Key == "SquadId")
                {
                    squadModel.SquadId = entry.Value.S;
                }
                else if (entry.Key == "GameId")
                {
                    squadModel.GameId = entry.Value.S;
                }
                else if (entry.Key == "PlayerId")
                {
                    squadModel.PlayerId = entry.Value.S;
                }
                else if (entry.Key == "PlayerData")
                {
                    squadModel.SquadData = entry.Value.S;
                }
            }

            return squadModel;
        }

        private void OnSquadResponseReceived(object sender, List<Dictionary<string, AttributeValue>> squads)
        {
            ResponseReceived?.Invoke(this, ConvertSquadResponse(squads));
        }
    }
}
