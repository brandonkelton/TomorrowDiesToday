using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    interface ISquad
    {
        Dictionary<string, int> SquadData { get; set; }
        Dictionary<string, int> SquadStats { get; set; }
    }
}
