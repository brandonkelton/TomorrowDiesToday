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
    public class SquadListDataTransferService : IDataTransferService<SquadListRequestDTO, SquadListResponseDTO, SquadListUpdateDTO>
    {
        public event EventHandler<SquadListResponseDTO> ResponseReceived;
        private IDBClient _client;

        public SquadListDataTransferService(IDBClient client)
        {
            _client = client;
            _client.SquadsResponseReceived += OnSquadsResponseReceived;
        }

        public async Task Request(SquadListRequestDTO requestDTO)
        {
            await _client.RequestSquads(requestDTO.GameId, requestDTO.MyPlayerId);
        }

        public async Task Update(SquadListUpdateDTO updateDTO)
        {
            throw new NotImplementedException("SquadListDataTransferService does not allow updates");
        }

        private SquadListResponseDTO ConvertSquadResponse(List<Dictionary<string, AttributeValue>> squads)
        {
            var squadList = new SquadListResponseDTO();

            for (int i = 0; i < squads.Count; i++)
            {
                var squadModel = new SquadResponseDTO();

                foreach (KeyValuePair<string, AttributeValue> entry in squads[i])
                {
                    if (entry.Key == "SquadId")
                    {
                        squadModel.SquadId = entry.Value.S;
                    }
                    else if (entry.Key == "GameId")
                    {
                        squadList.GameId = entry.Value.S;
                        squadModel.GameId = entry.Value.S;
                    }
                    else if (entry.Key == "PlayerId")
                    {
                        squadModel.PlayerId = entry.Value.S;
                    }
                    else if (entry.Key == "PlayerData")
                    {
                        // squadModel.Data = Array.ConvertAll(entry.Value.S.Split(','), s => int.Parse(s));
                        squadModel.SquadData = entry.Value.S;
                    }
                }
                squadList.Items.Add(squadModel);
            }
            return squadList;
        }

        private void OnSquadsResponseReceived(object sender, List<Dictionary<string, AttributeValue>> squads)
        {
            ResponseReceived?.Invoke(this, ConvertSquadResponse(squads));
        }
    }
}
