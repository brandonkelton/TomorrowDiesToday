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
        Task<bool> Exists(string gameId);

        Task Start(string gameId);

        Task CreatePlayer(string gameId, string playerId);

        Task DeleteGame(string gameId, string playerId);

        Task<PlayerDTO> RequestPlayer(string gameId, string playerId);

        Task<List<PlayerDTO>> RequestPlayerList(string gameId);

        //Temporary
        Task InitializeGameTable();

        //Temporary
        Task InitializePlayerTable();
    }
}
