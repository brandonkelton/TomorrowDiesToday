using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.Models
{
    public class SquadModel : IModel
    {
        public DateTime Timestamp => throw new NotImplementedException();

        public string GameId { get; set; }
        public string PlayerId { get; set; }
        public string SquadId { get; set; }

        public int[] Data = new int[10];

        public bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
