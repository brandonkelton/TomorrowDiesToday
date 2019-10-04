using System;
using System.Collections.Generic;

namespace TomorrowDiesToday.Models
{
    public class GameModel : IModel
    {
        public PlayerModel MyPlayer { get; set; }

        public List<PlayerModel> OtherPlayers { get; set; }
    }
}
