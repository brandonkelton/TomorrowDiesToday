using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TomorrowDiesToday.Models
{
    public class PlayerModel : IModel
    {
        public string GameId { get; set; }

        public string PlayerId { get; set; }

        public string PlayerName { get; set; }

        public ArmamentType PlayerType { get; set; }

        public List<SquadModel> Squads { get; set; } = new List<SquadModel>();
    }
}
