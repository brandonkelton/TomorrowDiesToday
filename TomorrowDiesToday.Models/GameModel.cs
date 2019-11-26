using System;
using System.Collections.Generic;

namespace TomorrowDiesToday.Models
{
    public class GameModel : IModel
    {
        public string GameId { get; set; }

        public string PlayerId { get; set; }

        public ArmamentType PlayerType { get; set; }

        public List<PlayerModel> Players { get; set; }

        public List<TileModel> Tiles { get; set; }

        public SquadStats SelectedSquadStats { get; set; }
    }
}
