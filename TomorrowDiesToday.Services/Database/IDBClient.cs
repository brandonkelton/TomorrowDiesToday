using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomorrowDiesToday.Models.Enums;
using TomorrowDiesToday.Services.Database.DTOs;

namespace TomorrowDiesToday.Services.Database
{
    public interface IDBClient
    {
        Task<bool> GameExists(string gameId);

        Task<bool> PlayerExists(string gameId, ArmamentType playerId);

        Task DeleteGame(string gameId, ArmamentType playerId);

        Task<GameDTO> RequestGame(string gameId);

        Task<PlayerDTO> RequestPlayer(string gameId, ArmamentType playerId);

        Task<List<PlayerDTO>> RequestPlayerList(string gameId);

        Task UpdateGame(GameDTO game);

        Task UpdatePlayer(PlayerDTO player);

        //Temporary
        Task InitializeGameTable();

        //Temporary
        Task InitializePlayerTable();
    }
}
