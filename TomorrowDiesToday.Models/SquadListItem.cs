using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    public class SquadListItem
    {
        public bool IsSelected { get; set; }

        public SquadModel Squad { get; set; }
    }
}
