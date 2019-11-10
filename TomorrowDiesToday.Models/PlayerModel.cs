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

        // { SquadId => SquadModel }
        public Dictionary<string, SquadModel> Squads { get; set; } = new Dictionary<string, SquadModel>();

        public List<SquadModel> SquadList => Squads.Select(s => s.Value).ToList();
    }
}
