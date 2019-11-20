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

        public string PlayerName => ((ArmamentType) int.Parse(PlayerId)).ToDescription();

        public List<SquadModel> Squads { get; set; } = new List<SquadModel>();
    }
}
