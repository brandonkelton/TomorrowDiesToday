using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Database.DTOs
{
    public class SquadListUpdateDTO : IUpdateDTO
    {
        public string GameId { get; set; }
    }
}
