using System;
using System.Collections.Generic;

namespace TomorrowDiesToday.Models
{
    public class GameModel : IModel
    {
        public string GameId { get; set; }

        public PlayerModel ThisPlayer { get; set; }

        public Dictionary<string, PlayerModel> OtherPlayers { get; set; }

        public Dictionary<string, TileModel> Tiles { get; set; }
    }
}
