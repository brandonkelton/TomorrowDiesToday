using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Data.Models
{
    public class PlayerRequest : IDataRequest
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }
    }
}
