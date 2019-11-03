using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    public class PlayerModel : IModel
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }

        // { SquadId => SquadModel }
        public Dictionary<string, SquadModel> Squads { get; set; } = new Dictionary<string, SquadModel>();
    }
}
