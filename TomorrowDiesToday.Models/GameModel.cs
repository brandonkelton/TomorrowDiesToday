using System;
using System.Collections.Generic;
using TomorrowDiesToday.Models.Enums;

namespace TomorrowDiesToday.Models
{
    public class GameModel : IModel
    {
        public string GameId { get; set; }

        public ArmamentType PlayerId { get; set; }

        public List<PlayerModel> Players { get; set; }

        public List<TileModel> Tiles { get; set; }

        public SquadStats SelectedSquadStats { get; set; }
    }
}
