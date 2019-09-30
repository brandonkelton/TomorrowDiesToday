using System;
using System.Collections.Generic;

namespace TomorrowDiesToday.Models
{
    public class GameModel : IModel
    {
        public DateTime Timestamp { get; private set; }

        public string GameId { get; set; }

        public string MyPlayerId { get; set; }

        public List<SquadModel> MySquads { get; set; }

        // Other Players: PlayerId -> List of Squads
        public Dictionary<string, List<SquadModel>>  OtherPlayers { get; set; }

    }
}
