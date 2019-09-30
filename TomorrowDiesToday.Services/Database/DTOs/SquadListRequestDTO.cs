using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Database.DTOs
{
    public class SquadListRequestDTO : IRequestDTO
    {
        public string GameId { get; set; }

        public string MyPlayerId { get; set; }
    }
}
