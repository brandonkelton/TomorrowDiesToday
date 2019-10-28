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
        Task<bool> GameExists(string gameId);

        Task<bool> PlayerExists(string gameId, string playerId);

        Task CreateGame(string gameId);

        Task CreatePlayer(string gameId, string playerId);

        Task DeleteGame(string gameId, string playerId);

        Task<GameDTO> RequestGame(string gameId);

        Task<PlayerDTO> RequestPlayer(string gameId, string playerId);

        Task<List<PlayerDTO>> RequestPlayerList(string gameId);

        Task UpdateGame(GameDTO game);

        Task UpdatePlayer(PlayerDTO player);

        //Temporary
        Task InitializeGameTable();

        //Temporary
        Task InitializePlayerTable();
    }
}
