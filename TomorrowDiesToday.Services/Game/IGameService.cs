using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Game
{
    public interface IGameService
    {
        string GameId { get; set; }
        string PlayerId { get; set; }

        string GenerateGameId();

        bool SuccessCheck(Dictionary<string, int> tileStats, Dictionary<string, int> squadStats);

        Dictionary<string, int> TileLookup(string tileName, Boolean flipped, int alerts);

        bool ValidateSquad(Dictionary<string, int> squadData);
    }
}
