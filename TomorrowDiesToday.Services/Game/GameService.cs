using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TomorrowDiesToday.Services.Game
{
    public class GameService : IGameService
    {
        private static Random _random = new Random();

        public string GameId { get; set; }

        public string PlayerId { get; set; }

        public string GenerateGameId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, chars.Length)
              .Select(s => s[_random.Next(s.Length)]).Take(6).ToArray());
        }
    }
}
