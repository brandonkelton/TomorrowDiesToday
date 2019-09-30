using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Database.DTOs
{
    public class SquadDTO
    {
        public string GameId { get; set; }

        public string PlayerId { get; set; }

        public string SquadId { get; set; }

        public int[] Data { get; set; }
    }
}
