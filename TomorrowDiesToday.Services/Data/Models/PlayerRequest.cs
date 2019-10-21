using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Services.Data.Models
{
    public class PlayerRequest : IDataRequest
    {
        public string PlayerId { get; set; }
    }
}
