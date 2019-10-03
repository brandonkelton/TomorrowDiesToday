using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Data.Models
{
    public class PlayerListRequest : IDataRequest
    {
        public string GameId { get; set; }
    }
}
