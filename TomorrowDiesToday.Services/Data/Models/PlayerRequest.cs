using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models.Enums;

namespace TomorrowDiesToday.Services.Data.Models
{
    public class PlayerRequest : IDataRequest
    {
        public string GameId { get; set; }
        public ArmamentType PlayerId { get; set; } = ArmamentType.None;
    }
}
