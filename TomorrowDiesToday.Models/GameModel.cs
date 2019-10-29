using System;
using System.Collections.Generic;

namespace TomorrowDiesToday.Models
{
    public class GameModel : IModel
    {
        public string GameId { get; set; }

        public PlayerModel MyPlayer { get; set; }

        public List<PlayerModel> OtherPlayers { get; set; }

        public List<TileModel> Tiles { get; set; }
    }
}
