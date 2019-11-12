using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using TomorrowDiesToday.Models;

namespace TomorrowDiesToday.Services.Database.DTOs
{
    /// <summary>
    /// This is implemented as a List item within the Players table.
    /// </summary>
    public class SquadDTO
    {
        public string SquadId { get; set; }

        public List<Armament> Armaments { get; set; } = new List<Armament>();

        public List<Stat> Stats { get; set; } = new List<Stat>();
    }
}
