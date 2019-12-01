using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Data.Models
{
    public class GameRequest : IDataRequest
    {
        public string GameId { get; set; }
    }
}
