using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Services.Database.DTOs;

namespace TomorrowDiesToday.Services.Database
{
    public interface IDBClient
    {
        event EventHandler<List<Dictionary<string, AttributeValue>>> SquadResponseReceived;
        event EventHandler<List<Dictionary<string, AttributeValue>>> SquadsResponseReceived;

        Task RequestSquads(string gameId, string playerId);

        Task RequestSquad(SquadRequestDTO squadDTO);

        Task SendSquad(SquadUpdateDTO squadDTO);
    }
}
