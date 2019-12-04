using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TomorrowDiesToday.Models.Enums;

namespace TomorrowDiesToday.Models
{
    public class PlayerModel : IModel
    {
        public string GameId { get; set; }

        public ArmamentType PlayerId { get; set; } = ArmamentType.None;

        public string PlayerName => PlayerId.ToDescription();

        public List<SquadModel> Squads { get; set; } = new List<SquadModel>();
    }
}
