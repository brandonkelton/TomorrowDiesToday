using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    public class PlayerModel : IModel
    {
        public string PlayerId { get; set; }

        public List<Squad> Squads { get; set; } = new List<Squad>();
    }
}
