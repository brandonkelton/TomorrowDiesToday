using System;
using System.Collections.Generic;

namespace TomorrowDiesToday.Models
{
    public class GameModel : IModel
    {
        public string GameId { get; set; }

        public PlayerModel ThisPlayer { get; set; }

        public List<PlayerModel> OtherPlayers { get; set; } = new List<PlayerModel>();

        public List<TileModel> Tiles { get; set; } = new List<TileModel>();
    }
}
