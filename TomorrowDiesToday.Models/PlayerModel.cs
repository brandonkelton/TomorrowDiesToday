using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    public class PlayerModel : IModel
    {
        public string PlayerId { get; set; }

        public List<SquadModel> Squads { get; set; } = new List<SquadModel>();
    }
}
