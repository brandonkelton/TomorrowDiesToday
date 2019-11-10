using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    public class PlayerModel : IModel
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }

        public string PlayerName { get; set; }

        // { SquadId => SquadModel }
        public List<SquadModel> Squads { get; set; } = new List<SquadModel>();

        public int Points { get; set; }
    }
}
